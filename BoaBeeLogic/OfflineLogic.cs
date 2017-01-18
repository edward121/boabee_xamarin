using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BoaBeePCL;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;

namespace BoaBeeLogic
{
    public static class OfflineLogic
    {
        public delegate void StatusLookup(string message);

        private static bool isBarcodeValid(string barcode)
        {
            if ((barcode.Contains("http://") && (barcode.IndexOf("http://") == 0)) ||
                        (barcode.Contains("https://") && (barcode.IndexOf("https://") == 0)) ||
                        (barcode.Contains("ftp://") && (barcode.IndexOf("ftp://") == 0)) ||
                        (barcode.Contains("www.") && (barcode.IndexOf("www.") == 0)) ||
                        (barcode.Contains("MAILTO:") && (barcode.IndexOf("MAILTO:") == 0)) ||
                        (barcode.Contains("MATMSG:") && (barcode.IndexOf("MATMSG:") == 0)) ||
                        (barcode.Contains("tel:") && (barcode.IndexOf("tel:") == 0)) ||
                        (barcode.Contains("TEL:") && (barcode.IndexOf("TEL:") == 0)) ||
                        (barcode.Contains("SMSTO:") && (barcode.IndexOf("SMSTO:") == 0)) ||
                        (barcode.Contains("GEO:") && (barcode.IndexOf("GEO:") == 0)) ||
                        (barcode.Contains("BEGIN:VEVENT") && (barcode.IndexOf("BEGIN:VEVENT") == 0)) ||
                        (barcode.Contains("WIFI:") && (barcode.IndexOf("WIFI:") == 0)))
            {
                return false;
            }
            return true;
        }

        public static async Task<DBlocalContact> didScanBarcode(string barcode_, string symbology, StatusLookup callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("Callback must not be null to handle result");
            }

            var barcode = barcode_.Trim();

            DBlocalContact temporaryContact = null;
            if (symbology == "QR")
            {
                if (!OfflineLogic.isBarcodeValid(barcode))
                {
                    throw new InvalidOperationException("The QR code cannot be used by BoaBee because it is not a contact or an ID.");
                }
                else if (barcode.Contains("BEGIN:VCARD"))
                {
                    //VCard
                    temporaryContact = OfflineLogic.parseAsVCard(barcode);
                }
                else if (barcode.StartsWith("[[") && barcode.EndsWith("]]"))
                {
                    //Artexis
                    temporaryContact = OfflineLogic.parseAsArtexis(barcode);
                }
                else
                {
                    //unknown format
                    temporaryContact = new DBlocalContact();
                    temporaryContact.uid = barcode;
                }
            }
            else
            {
                //unknown format
                temporaryContact = new DBlocalContact();
                temporaryContact.uid = barcode;
            }
            DBAppSettings appSettings = DBLocalDataStore.GetInstance().GetAppSettings();
            if (appSettings == null)
            {
                appSettings = new DBAppSettings();
                appSettings.instantContactCheck = true;
                DBLocalDataStore.GetInstance().SetAppSettings(appSettings);
            }

            string message = string.Empty;
            if (appSettings.instantContactCheck)
            {
                var localContact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(temporaryContact.uid);
                if (localContact != null)
                {
                    temporaryContact &= localContact;
                    temporaryContact.Id = localContact.Id;
                }

                if (await Reachability.isConnected())
                {
                    try
                    {
                        int timeout = 10000;
                        var timeoutCancellationTokenSource = new CancellationTokenSource();
                        var timeoutTask = Task.Delay(timeout, timeoutCancellationTokenSource.Token);

                        var task = NetworkRequests.contactLookup(temporaryContact.uid);

                        DBlocalContact serverContact = null;
                        if (await Task.WhenAny(task, timeoutTask) == task)
                        {
                            // Task completed within timeout.
                            // Consider that the task may have faulted or been canceled.
                            // We re-await the task so that any exceptions/cancellation is rethrown.

                            timeoutCancellationTokenSource.Cancel();

                            serverContact = await task;
                            if (serverContact != null)
                            {
                                if (serverContact.hasOnlyUID() || serverContact.hasOnlyUIDAndName())
                                {
                                    message = "Check for additional contact details can't be executed at this moment. More details will be added to your report if available.";
                                }
                                else
                                {
                                    message = "These are the details we have found for you. Feel free to complete the rest below.";
                                }
                            }
                            else
                            {
                                message = "Check for additional contact details can't be executed at this moment. More details will be added to your report if available.";
                            }
                            Console.Error.WriteLine("Server has contact: {0}", serverContact != null);
                        }
                        else
                        {
                            // timeout/cancellation logic
                            message = "Check for additional contact details can't be executed at this moment. More details will be added to your report if available.";
                            Console.Error.WriteLine("Timed out");
                        }

                        temporaryContact *= serverContact;
                    }
                    catch (Exception e)
                    {
                        message = "Check for additional contact details can't be executed at this moment. More details will be added to your report if available.";
                        Console.Error.WriteLine("contactLookup exception: {0}", e.Message);
                    }
                }
                else
                {
                    message = "Check for additional contact details can't be executed at this moment. More details will be added to your report if available.";
                }
            }
            else
            {
                message = "Instant contact checking is turned off";
            }
            callback(message);
            return temporaryContact;
        }

        private static DBlocalContact parseAsArtexis(string barcodeString)
        {
            //format example: "[["barcode","5160017139930041470033",0],["Name","Gert Stalmans",1]]"

            var parts = barcodeString.Trim('[', ']').Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);

            string barcode = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;

            foreach (var part in parts)
            {
                string partCopy = new string(part.ToCharArray());
                partCopy = partCopy.Replace("\"", null);
                var subParts = partCopy.Split(',');
                if (subParts[0].ToLower().Equals("barcode"))
                {
                    barcode = subParts[1];
                    continue;
                }
                if (subParts[0].ToLower().Equals("name"))
                {
                    string namePart = subParts[1];

                    if (namePart.Contains(" "))
                    {
                        var nameSplit = namePart.Split(' ').ToList();
                        firstName = nameSplit.ElementAt(0);
                        nameSplit.RemoveAt(0);
                        lastName = string.Join(" ", nameSplit);
                    }
                    else
                    {
                        firstName = namePart;
                    }

                    continue;
                }
            }

            if (string.IsNullOrWhiteSpace(barcode) || string.IsNullOrWhiteSpace(string.Format("{0} {1}", firstName, lastName)))
            {
                throw new InvalidOperationException("The QR code contains invalid information and can't be used.");
            }
            else
            {
                DBlocalContact contact = new DBlocalContact();
                contact.firstname = firstName;
                contact.lastname = string.IsNullOrWhiteSpace(lastName) ? null : lastName;
                contact.uid = barcode;
                return contact;
            }
        }

        private static DBlocalContact parseAsVCard(string barcode)
        {
            var QRContent = barcode.Split("\r\n".ToCharArray());

            DBlocalContact contact = new DBlocalContact();

            foreach (var _part in QRContent)
            {
                var part = string.Copy(_part);

                part = part.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

                if (part.Contains("N:") && part.IndexOf("N:") == 0)
                {
                    var t = part.Substring(part.IndexOf(":") + 1).Split(";".ToCharArray());

                    List<string> firstLastName = new List<string>();
                    foreach (var st in t)
                    {
                        if (!st.Contains(";") && !string.IsNullOrEmpty(st))
                        {
                            firstLastName.Add(st);
                        }

                    }
                    contact.lastname = firstLastName[0].Trim();
                    contact.firstname = firstLastName[1].Trim();
                    continue;
                }

                if (part.Contains("ORG") && part.IndexOf("ORG") == 0)
                {
                    contact.company = part.Substring(part.IndexOf(":") + 1);
                    continue;
                }
                if (part.Contains("EMAIL") && part.IndexOf("EMAIL") == 0)
                {
                    if (string.IsNullOrEmpty(contact.email))
                    {
                        contact.email = part.Substring(part.IndexOf(":") + 1);
                        contact.uid = contact.email;
                    }
                    continue;
                }
                if (part.Contains("TEL") && part.IndexOf("TEL") == 0)
                {
                    if (string.IsNullOrEmpty(contact.phone))
                    {
                        contact.phone = part.Substring(part.IndexOf(":") + 1);
                    }
                    continue;
                }
                if (part.Contains("ADR") && part.IndexOf("ADR") == 0)
                {
                    part = part.Substring(part.IndexOf(":") + 1);
                    List<string> adr_parts = part.Split(";".ToCharArray(), StringSplitOptions.None).ToList();
                    contact.city = adr_parts[3];
                    contact.street = adr_parts[2];
                    contact.zip = adr_parts[5];
                    contact.country = adr_parts[6];
                    continue;
                }
            }
            //contact.email = contact.email.Replace(" ", string.Empty);
            if (contact.email != null)
            {
                contact.email = contact.email.Trim();
            }
            if (contact.lastname != null)
            {
                contact.lastname = contact.lastname.Trim();
            }
            if (contact.firstname != null)
            {
                contact.firstname = contact.firstname.Trim();
            }

            if (!isEmail(contact.email))
            {
                throw new InvalidOperationException("The QR code contains invalid information and can't be used.");
            }
            if (string.IsNullOrWhiteSpace(contact.lastname))
            {
                throw new InvalidOperationException("The QR code contains invalid information and can't be used.");
            }
            if (string.IsNullOrWhiteSpace(contact.firstname))
            {
                throw new InvalidOperationException("The QR code contains invalid information and can't be used.");
            }

            return contact;
        }

        public static bool isEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            //var regexPatter = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$";
            var regexPatter = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            return Regex.IsMatch(email, regexPatter, RegexOptions.IgnoreCase);
        }

        public static bool validateContact(DBlocalContact contact, bool isNewContact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "Contact must not be null");
            }
            if (string.IsNullOrEmpty(contact.firstname))
            {
                throw new ArgumentException("First name is missing");
            }
            if (string.IsNullOrEmpty(contact.lastname))
            {
                throw new ArgumentException("Last name is missing");
            }
            if (string.IsNullOrEmpty(contact.email) && isNewContact)
            {
                throw new ArgumentException("Email is missing");
            }

            if (!OfflineLogic.isEmail(contact.email) && isNewContact)
            {
                throw new ArgumentException("Email is incorrect");
            }

            return true;
        }

        public static bool isAnswerValid(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer) || answer.Equals("select a value") || answer.Equals("") || answer.Equals("_,___"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool isQuestionRequired(DBQuestion question)
        {
            return question.required;
        }

        public static void createOrUpdateContact(DBlocalContact newContact)
        {
            DBlocalContact oldLocalContact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(newContact.uid);
            DBContactToServer existingChanges = DBLocalDataStore.GetInstance().GetContactsToServerByUID(newContact.uid);
            if (oldLocalContact != null)
            {
                if (existingChanges == null)
                {
                    existingChanges = new DBContactToServer(oldLocalContact);
                    existingChanges -= newContact;
                    DBLocalDataStore.GetInstance().AddContactToServer(existingChanges);
                }
                else
                {
                    DBContactToServer lastChanges = new DBContactToServer(oldLocalContact);
                    lastChanges -= newContact;
                    existingChanges &= lastChanges;
                    DBLocalDataStore.GetInstance().UpdateContactToServer(existingChanges);
                }
                newContact.Id = oldLocalContact.Id;
                newContact.activeContact = true;

                DBLocalDataStore.GetInstance().UpdateLocalContact(newContact);
                Console.WriteLine("updated contact: \n{0}", JsonConvert.SerializeObject(newContact));
            }
            else
            {
                existingChanges = new DBContactToServer(newContact, true);
                //existingChanges = existingChanges.newContact(existingChanges);
                DBLocalDataStore.GetInstance().AddContactToServer(existingChanges);
                newContact.activeContact = true;
                if (newContact.Id == 0)
                {
                    DBLocalDataStore.GetInstance().AddLocalContact(newContact);
                }
                else
                {
                    DBLocalDataStore.GetInstance().UpdateLocalContact(newContact);
                }
                Console.WriteLine("created contact: \n{0}", JsonConvert.SerializeObject(newContact));
            }
        }

        public static void updateForms(string uid, List<Answer> answers, int originalRequestID)
        {
            if (answers == null)
            {
                throw new ArgumentException("Answers must not be null");
            }

            var dateNow = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", DateTime.Now);
            //var answersToSend = DBLocalDataStore.GetInstance().getAnswers();
            var appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
            var formDefinition = DBLocalDataStore.GetInstance().GetLocalFormDefinitions().Find(s => s.uuid == DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
            var questions = DBLocalDataStore.GetInstance().GetLocalQuestions(formDefinition.uuid);

            UpdateForms scr = new UpdateForms();
            scr.forms = new List<AnsweredForm>();
            string profileName = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;

            var form = new AnsweredForm();
            form.contactUid = uid;
            form.startdate = dateNow;
            form.enddate = dateNow;
            form.name = formDefinition.objectName;
            form.user = new DeviceUser { username = user.username, profile = profileName };

            bool isAnyQuestionAnswered = answers.Count(s => OfflineLogic.isAnswerValid(s.answer)) > 0;
            bool isAnswersValid = true;
            List<Answer> tmpAnswers = new List<Answer>();

            for (int index = 0; (index < answers.Count) && isAnyQuestionAnswered; index++)
            {
                if (OfflineLogic.isAnswerValid(answers[index].answer))
                {
                    Answer answer = new Answer();
                    answer.answer = answers[index].answer;
                    answer.name = questions[index].name;
                    answer.type = questions[index].type;
                    tmpAnswers.Add(answer);
                }
                else
                {
                    if (OfflineLogic.isQuestionRequired(questions[index]))
                    {
                        isAnswersValid = false;
                    }
                }
            }
            if (isAnswersValid)
            {
                form.answers = tmpAnswers.ToArray();
                scr.forms.Add(form);

                string json = JsonConvert.SerializeObject(scr);
                DBSyncRequest syncRequest = new DBSyncRequest();
                syncRequest.serializedSyncContext = json;

                DBLocalDataStore.GetInstance().addSyncRequest(syncRequest);

                var originalSyncRequest = DBLocalDataStore.GetInstance().getSyncRequests().Find(r => r.Id == originalRequestID);
                var originalContext = JsonConvert.DeserializeObject<SyncContext>(originalSyncRequest.serializedSyncContext);
                originalContext.forms.RemoveAll(c => c.contactUid.Equals(uid));

                if (originalContext.contacts.isNullOrEmpty() && originalContext.forms.isNullOrEmpty() && originalContext.orders.isNullOrEmpty())
                {
                    DBLocalDataStore.GetInstance().deleteSyncRequest(originalSyncRequest);
                }
                else
                {
                    originalSyncRequest.serializedSyncContext = JsonConvert.SerializeObject(originalContext);
                    DBLocalDataStore.GetInstance().updateSyncReqest(originalSyncRequest);
                }
            }
            else
            {
                throw new InvalidOperationException("You did not complete all mandatory fields in the info screen. Please correct.");
            }
        }

        public static void updateForms(string uid)
        {
            var dateNow = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", DateTime.Now);
            var answersToSend = DBLocalDataStore.GetInstance().getAnswers();
            var appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
            var formDefinition = DBLocalDataStore.GetInstance().GetLocalFormDefinitions().Find(s => s.uuid == DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
            var questions = DBLocalDataStore.GetInstance().GetLocalQuestions(formDefinition.uuid);

            UpdateForms scr = new UpdateForms();
            scr.context = new RequestData();
            scr.forms = new List<AnsweredForm>();

            var context = scr.context;
            context.password = user.password;
            context.username = user.username;
            context.profile = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;
            context.tags = new string[] { user.tags };
            context.campaignReference = appInfo.campaignReference;

            var form = new AnsweredForm();
            List<Answer> tmpAnswers = new List<Answer>();
            form.contactUid = uid;
            form.startdate = dateNow;
            form.enddate = dateNow;
            form.name = formDefinition.objectName;
            form.user = new DeviceUser { username = user.username, profile = context.profile };
            form.answers = new Answer[] { };
            bool isAnyQuestionAnswered = answersToSend.Count(s => OfflineLogic.isAnswerValid(s.answer)) > 0;
            bool isAnswersValid = true;

            for (int index = 0; (index < answersToSend.Count) && isAnyQuestionAnswered; index++)
            {
                if (OfflineLogic.isAnswerValid(answersToSend[index].answer))
                {
                    Answer answer = new Answer();
                    answer.answer = answersToSend[index].answer;
                    answer.name = questions[index].name;
                    answer.type = questions[index].type;
                    tmpAnswers.Add(answer);
                }
                else
                {
                    if (OfflineLogic.isQuestionRequired(questions[index]))
                    {
                        isAnswersValid = false;
                    }
                }
            }
            if (isAnswersValid)
            {
                form.answers = tmpAnswers.ToArray();
                scr.forms.Add(form);
            }
            else
            {
                throw new InvalidOperationException("You did not complete all mandatory fields in the info screen. Please correct.");
            }

            string json = JsonConvert.SerializeObject(scr);
            DBSyncRequest syncRequest = new DBSyncRequest();
            syncRequest.serializedSyncContext = json;

            DBLocalDataStore.GetInstance().addSyncRequest(syncRequest);
            OfflineLogic.ClearDataSelected();
        }

        public static void prepareSync(bool isKiosk=false)
        {
            var dateNow = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", DateTime.Now);

            var activeLocalContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.activeContact).ToList();
            if (activeLocalContacts.Count == 0 && !isKiosk)
            {
                throw new InvalidOperationException("Please select at least one contact.");
            }

            var allactivefiles = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();

            var appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
            var contactsToSend = DBLocalDataStore.GetInstance().GetContactsToServer();
            var invalidContactsToSend = contactsToSend.Where(c => string.IsNullOrWhiteSpace(c.uid));

            foreach (var invalidItem in invalidContactsToSend)
            {
                DBLocalDataStore.GetInstance().RemoveContactToServer(invalidItem);
            }

            contactsToSend.RemoveAll(c => string.IsNullOrWhiteSpace(c.uid));

            var answersToSend = DBLocalDataStore.GetInstance().getAnswers();
            var formDefinition = DBLocalDataStore.GetInstance().GetLocalFormDefinitions().Find(s => s.uuid == DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
            var questions = DBLocalDataStore.GetInstance().GetLocalQuestions(formDefinition.uuid);
            List<DBOrder> orders = new List<DBOrder>();

            List<DBOrderLine> orderLines = new List<DBOrderLine>();

            allactivefiles.ForEach((f) =>
            {
                DBOrderLine orderLine = new DBOrderLine();
                orderLine.itemDescription = f.name;
                orderLine.item = f.uuid;
                orderLines.Add(orderLine);
            });

            activeLocalContacts.ForEach((lc) =>
            {
                if (string.IsNullOrWhiteSpace(lc.uid))
                {
                    return;
                }

                var serverContact = contactsToSend.Find(sc => sc.uid.Equals(lc.uid));
                if (serverContact == null)
                {
                    DBContactToServer newServerContact = new DBContactToServer(true);
                    newServerContact.uid = lc.uid;
                    DBLocalDataStore.GetInstance().AddContactToServer(newServerContact);
                    contactsToSend.Add(newServerContact);
                }

                DBOrder order = new DBOrder();
                order.contactUid = lc.uid;
                order.created = DateTime.Now.ToString("yyyy-MM-ddTH:mm:sszzz");
                order.creator = user.username;
                order.orderLine = orderLines;
                orders.Add(order);
            });
           

            SyncContext scr = new SyncContext();
            scr.context = new RequestData();
            scr.contacts = new List<CustomerType>();
            scr.forms = new List<AnsweredForm>();
            scr.orders = orders;

            contactsToSend.ForEach((c) =>
            {
                //scr.contacts.Add(new CustomerType(c));
                if (!string.IsNullOrWhiteSpace(c.uid))
                {
                    scr.contacts.Add(new CustomerType(c));
                }
            });

            bool isAnyQuestionAnswered = answersToSend.Count(s => OfflineLogic.isAnswerValid(s.answer)) > 0;
            string profileName = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;
            foreach (var contact in activeLocalContacts)
            {
                bool isAnswersValid = true;

                if (isAnyQuestionAnswered)
                {
                    var form = new AnsweredForm();
                    List<Answer> tmpAnswers = new List<Answer>();
                    form.contactUid = contact.uid;
                    form.startdate = dateNow;
                    form.enddate = dateNow;
                    form.name = formDefinition.objectName;
                    form.user = new DeviceUser { username = user.username, profile = profileName };
                    form.answers = new Answer[] { };

                    for (int index = 0; (index < answersToSend.Count) && isAnyQuestionAnswered; index++)
                    {
                        if (OfflineLogic.isAnswerValid(answersToSend[index].answer))
                        {
                            Answer answer = new Answer();
                            answer.answer = answersToSend[index].answer;
                            answer.name = questions[index].name;
                            answer.type = questions[index].type;
                            tmpAnswers.Add(answer);
                        }
                        else
                        {
                            if (OfflineLogic.isQuestionRequired(questions[index]))
                            {
                                isAnswersValid = false;
                            }
                        }
                    }
                    if (isAnswersValid)
                    {
                        form.answers = tmpAnswers.ToArray();
                        scr.forms.Add(form);
                    }
                    else
                    {
                        if(!isKiosk)
                            throw new InvalidOperationException("Not all mandatory questions are completed");
                    }
                }
            }
            var Counts = DBLocalDataStore.GetInstance().GetCountHomeScreen();
            for (int i = 0; i < scr.contacts.Count; i++)
            {
                activeLocalContacts[i].useInRequest = true;
                DBLocalDataStore.GetInstance().UpdateLocalContact(activeLocalContacts[i]);
            }
            if (scr.orders[0].orderLine.Count != 0)
            {
                Counts.countShare += scr.orders.Count;
            }
            Counts.countQuestion += scr.forms.Count;
            Counts.countContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.useInRequest).ToList().Count;
            DBLocalDataStore.GetInstance().SetCountHomeScreen(Counts);

            JsonSerializerSettings serializationSettings = new JsonSerializerSettings();
            serializationSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(scr, Formatting.Indented, serializationSettings);
            DBSyncRequest syncRequest = new DBSyncRequest();
            syncRequest.serializedSyncContext = json;

            Console.Error.WriteLine("Saving request: {0}", json);
            DBLocalDataStore.GetInstance().addSyncRequest(syncRequest);


           // DBLocalDataStore.GetInstance().SetCountHomeScreen(new DBHomeScreenCounts() { countContacts = ActivityHomescreen.counts[0], countShare = ActivityHomescreen.counts[1], countQuestion = ActivityHomescreen.counts[2] });


            
            if (!isKiosk)
                OfflineLogic.ClearDataSelected();
        }

        public static void ClearDataSelected(bool clearContactsToServer = true)
        {
            DBLocalDataStore ds = DBLocalDataStore.GetInstance();
            var allactivecontacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList();
            var allactivefiles = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();

            allactivecontacts.ForEach((c) =>
            {
                c.activeContact = false;
                ds.UpdateLocalContact(c);
            });
            allactivefiles.ForEach((f) =>
            {
                f.activeFile = false;
                ds.UpdateLocalFile(f);
            });

            if (clearContactsToServer)
            {
                ds.ClearAllContactsToServer();
            }

            ds.resetAnswers();
            //SaveAndLoad.files_selected.Clear();
        }
    }
}

