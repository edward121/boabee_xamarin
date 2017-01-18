using System;
using System.IO;
using BoaBeePCL;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace BoaBeeLogic
{
    public static class NetworkRequests
    {
        public delegate void statusResponseList<T>(bool success, string messageTitle, string message, List<T> list);
        public delegate void StatusDownload(bool success, string message = "");
        public delegate void StatusDownloadFilesAndForms(bool success, string message = "", bool isKiosk = false);

        public static async Task getProfiles(DBUserLoginRequest user)
        {
            var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ServerURLs.profilesURL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.ServicePoint.ConnectionLimit = 3;
            httpWebRequest.Timeout = 30000;
            httpWebRequest.KeepAlive = false;

            var loginContext = new LoginContext(user);

            var jsonUser = JsonConvert.SerializeObject(loginContext);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonUser);
            }
            var response = await httpWebRequest.GetResponseAsync();

            string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();

            httpWebRequest.Abort();

            var exchangeInfo = JsonConvert.DeserializeObject<ExchengeInfo>(responseData);
            var localDataStore = DBLocalDataStore.GetInstance();

            var profilesList = new List<DBBasicAuthority>();
            localDataStore.ClearBasicAuthority();
            foreach (DBBasicAuthority profile in exchangeInfo.profiles)
            {
                profilesList.Add(profile);
                localDataStore.AddBasicAuthority(profile);
            }
        }

        public static async Task performAuth(DBUserLoginRequest user, statusResponseList<DBBasicAuthority> callback)
        {
            //throw new Exception("TEST EXCEPTION");

            if (callback == null)
            {
                throw new Exception("Callback must not be null");
            }

            var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ServerURLs.authURL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.ServicePoint.ConnectionLimit = 3;
            httpWebRequest.Timeout = 30000;
            httpWebRequest.KeepAlive = false;

            var loginContext = new LoginContext(user);

            var jsonUser = JsonConvert.SerializeObject(loginContext);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonUser);
            }

            var response = await httpWebRequest.GetResponseAsync();

            string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();

            httpWebRequest.Abort();

            var exchangeInfo = JsonConvert.DeserializeObject<ExchengeInfo>(responseData);

            if (!exchangeInfo.success)
            {
                callback(false, "Login failed", "Incorrect username/password detected, please try again", null);
                return;
            }

            if (exchangeInfo.profiles == null || exchangeInfo.profiles.Length == 0)
            {
                callback(false, "Warning:", "You currently have no app setups available, go to your dashboard and create one!", null);
                return;
            }

            var localDataStore = DBLocalDataStore.GetInstance();

            localDataStore.AddUserInfo(user);
            localDataStore.ClearBasicAuthority();

            var profilesList = new List<DBBasicAuthority>();
            foreach (DBBasicAuthority profile in exchangeInfo.profiles)
            {
                profilesList.Add(profile);
                localDataStore.AddBasicAuthority(profile);
            }
            callback(true, null, "OK", profilesList);
        }

        public static async Task GetContacts(StatusDownload loghandler)
        {
            SyncContext scr = new SyncContext();

            DBAppInfo appInfo = DBLocalDataStore.GetInstance().GetAppInfo();

            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
            scr.context = new RequestData();
            var output = scr.context;
            output.password = user.password;
            output.username = user.username;
            output.profile = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;
            output.tags = new string[] { user.tags };
            output.campaignReference = appInfo.campaignReference;

            var jsonUser = JsonConvert.SerializeObject(scr);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ServerURLs.contactsURLNew);
            try
            {
                //throw new Exception("TEST EXCEPTION");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 30000;
                httpWebRequest.KeepAlive = false;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonUser);
                }

                var response = await httpWebRequest.GetResponseAsync();

                NetworkRequests.GetContactsFromResponse(response, loghandler);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                httpWebRequest.Abort();
            }
        }

        public delegate void finishPrintDelegate(bool success);
        private static List<object> finishPrintDelegates = new List<object>();
        private static event finishPrintDelegate _onFinishPrint;
        public static event finishPrintDelegate onFinishPrint
        {
            add
            {
                if (!finishPrintDelegates.Contains(value))
                {
                    finishPrintDelegates.Add(value);
                    _onFinishPrint += value;
                }
                else
                {
                    //Console.Error.WriteLine("{0} is already subscribed to 'onFinishPrint'", value);
                }
            }
            remove
            {
                _onFinishPrint -= value;
                finishPrintDelegates.Remove(value);
            }
        }

        public static async Task sendBadge(DBlocalContact contact, string URL)
        {
            try
            {
                var uri = new Uri(URL);
                uri.ToString();
            }
            catch
            {
                throw new ArgumentException("Invalid webhook URL");
            }
            SyncContext scr = new SyncContext();
            scr.context = new RequestData();
            scr.contacts = new List<CustomerType>();
            scr.contacts.Add(new CustomerType(contact));

            var appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
            var context = scr.context;

            context.password = user.password;
            context.username = user.username;
            context.profile = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;
            context.tags = new string[] { user.tags };
            context.campaignReference = appInfo.campaignReference;

            string json = JsonConvert.SerializeObject(scr, Formatting.Indented);
            Console.WriteLine("Trying to send to webhook:\n{0}", json);

            //var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            var httpWebRequest = WebRequest.CreateHttp(URL);
            try
            {
                //throw new Exception("TEST EXCEPTION");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 30000;
                httpWebRequest.ServicePoint.ConnectionLimit = 3;
                httpWebRequest.KeepAlive = false;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                var response = await httpWebRequest.GetResponseAsync();

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    Console.WriteLine("Received from webhook:\n{0}", result);

                    var settings = new JsonSerializerSettings();
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    var receivedData = JsonConvert.DeserializeObject<CustomerSyncResult>(result, settings);
                    if (NetworkRequests._onFinishPrint != null)
                    {
                        NetworkRequests._onFinishPrint(receivedData.success);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Webhook exception: {0}", ex.Message);
                if (NetworkRequests._onFinishPrint != null)
                {
                    NetworkRequests._onFinishPrint(false);
                }
                //throw;
            }
            finally
            {
                httpWebRequest.Abort();
            }
        }

        public delegate void finishSyncDelegate();
        public delegate void beginSyncDelegate();
        public delegate void failSyncDelegate();
        public static event finishSyncDelegate onFinishSync;
        public static event beginSyncDelegate onBeginSync;
        public static event failSyncDelegate onFailSync;

        public static async Task SyncDataServer()
        {
            var SyncData = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();
            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
            List<CustomerType> receivedContacts = new List<CustomerType>();

            if (user.invalidPassword)
            {
                return;
            }

            if (SyncData.Count > 0 && NetworkRequests.onBeginSync != null)
            {
                NetworkRequests.onBeginSync();
            }

            var appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
            DBAppSettings appSettings = DBLocalDataStore.GetInstance().GetAppSettings();

            for (int i = 0; i < SyncData.Count; i++)
            {
                string jsonUser = SyncData[i].serializedSyncContext;
                var data = JsonConvert.DeserializeObject<SyncContext>(jsonUser);
               
                data.context = new RequestData();
                var context = data.context;
                context.password = user.password;
                //context.password = "WrongPassword";
                context.username = user.username;
                context.profile = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;
                context.tags = new string[] { user.tags };
                context.campaignReference = appInfo.campaignReference;

                JsonSerializerSettings serializationSettings = new JsonSerializerSettings();
                serializationSettings.DefaultValueHandling = DefaultValueHandling.Ignore;

                jsonUser = JsonConvert.SerializeObject(data, Formatting.Indented, serializationSettings);

                Console.WriteLine("Sending to sync server:\n{0}", jsonUser);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ServerURLs.contactsURLNew);
                try
                {
                    //throw new Exception("TEST EXCEPTION");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.Timeout = 30000;
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.PreAuthenticate = false;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsonUser);
                    }

                    var response = await httpWebRequest.GetResponseAsync();

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();

                        Console.Error.WriteLine("Received from sync server:\n{0}", result);

                        var receivedData = JsonConvert.DeserializeObject<CustomerSyncResult>(result);
                        if (receivedData.success)
                        {
                            SyncData[i].isSent = true;
                            DBLocalDataStore.GetInstance().updateSyncReqest(SyncData[i]);
                            if(appSettings.getSharedContacts)
                            {
                                receivedContacts.AddRange(receivedData.contacts);
                            }
                        }
                        else
                        {
                            user.invalidPassword = true;
                            DBLocalDataStore.GetInstance().AddUserInfo(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sync exception: {0}", ex.Message);
                    if (NetworkRequests.onFailSync != null)
                    {
                        onFailSync();
                    }
                    throw;
                }
                finally
                {
                    httpWebRequest.Abort();
                }
            }

            if (receivedContacts.Count > 0)
            {
                //CustomerTypeComparer comparer = new CustomerTypeComparer();
                //receivedContacts = receivedContacts.Distinct(comparer).ToList();

                NetworkRequests.UpdateContactsFromResponse(receivedContacts);

                if (NetworkRequests.onFinishSync != null)
                {
                    onFinishSync();
                }
            }
            else
            {
                if (NetworkRequests.onFailSync != null)
                {
                    onFailSync();
                }
            }
        }

        private static void UpdateContactsFromResponse(List<CustomerType> contactsList)
        {
            var localContacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            localContacts.RemoveAll((c) => string.IsNullOrWhiteSpace(c.uid));
            var serverContacts = contactsList;
            if (serverContacts != null)
            {
                foreach (var contact in serverContacts)
                {
                    if (string.IsNullOrWhiteSpace(contact.uid))
                    {
                        Console.WriteLine("Received contact {0} has no UID, ignoring", JsonConvert.SerializeObject(contact, Formatting.Indented));
                        continue;
                    }
                    var existingLocalContact = localContacts.Find(c => c.uid.Equals(contact.uid));
                    var newContact = new DBlocalContact(contact);
                    if (existingLocalContact != null)
                    {
                        existingLocalContact += newContact;
                        existingLocalContact.source = ContactSource.Server;
                        DBLocalDataStore.GetInstance().UpdateLocalContact(existingLocalContact);
                        Console.Error.WriteLine("Contact with UID {0} updated from server", contact.uid);
                    }
                    else
                    {
                        newContact.source = ContactSource.Server;
                        DBLocalDataStore.GetInstance().AddLocalContact(newContact);
                        Console.Error.WriteLine("Contact with UID {0} added", contact.uid);
                    }
                }
            }
        }

        private static void GetContactsFromResponse(WebResponse response, StatusDownload loghandler)
        {
            var httpResponse = response.GetResponseStream();
            using (var streamReader = new StreamReader(httpResponse))
            {
                var result = streamReader.ReadToEnd();

                var receivedData = JsonConvert.DeserializeObject<CustomerSyncResult>(result);
                if (receivedData.success)
                {
                    var contacts = receivedData.contacts;
                    DBLocalDataStore.GetInstance().ClearAllContacts();
                    foreach (var contact in contacts)
                    {
                        if (string.IsNullOrEmpty(contact.uid))
                        {
                            Console.WriteLine("Received contact {0} has no UID, ignoring", JsonConvert.SerializeObject(contact, Formatting.Indented));
                            continue;
                        }
                        var localContact = new DBlocalContact(contact);
                        localContact.source = ContactSource.Server;
                        DBLocalDataStore.GetInstance().AddLocalContact(localContact);
                        Console.Error.WriteLine("Contact with UID {0} added", contact.uid);
                    }
                    loghandler(true, "Contacts updated from server");
                }
                else
                {
                    loghandler(false, receivedData.error);
                }
            }
        }

        public static async Task GetFilesAndForms(StatusDownloadFilesAndForms loghandler)
        {
            RequestData requestData = new RequestData();
            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
            requestData.password = user.password;
            requestData.username = user.username;
            requestData.profile = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;
            requestData.tags = new string[] { user.tags };

            var jsonUser = JsonConvert.SerializeObject(requestData);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ServerURLs.filesAndFormsURL);
            try
            { 
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.ServicePoint.ConnectionLimit = 3;
                httpWebRequest.Timeout = 30000;
                httpWebRequest.KeepAlive = false;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonUser);
                }

                var response = await httpWebRequest.GetResponseAsync();

                NetworkRequests.GetFilesAndFormsFromResponse(response, loghandler);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                httpWebRequest.Abort();
            }

        }

        private static void GetFilesAndFormsFromResponse(WebResponse response, StatusDownloadFilesAndForms loghandler)
        {
            var httpResponse = response.GetResponseStream();
            using (var streamReader = new StreamReader(httpResponse))
            {
                var result = streamReader.ReadToEnd();

                var receivedData = JsonConvert.DeserializeObject<CampaignAppUpdateRequestResult>(result);
                if (receivedData.success)
                {
                    if (receivedData.defaultForm != null)
                    {
                        DBLocalDataStore.GetInstance().AddLocalFormDefinitions(new[] { receivedData.defaultForm }, receivedData.defaultForm.uuid);
                    }
                    else
                    {
                        DBLocalDataStore.GetInstance().AddLocalFormDefinitions(receivedData.forms);
                    }

                    DBLocalDataStore.GetInstance().ClearAllFiles();
                    DBLocalDataStore.GetInstance().ClearAllFolders();
                    DBLocalDataStore.GetInstance().AddLocalFolders(receivedData.folders);
                    DBAppInfo appInfo = new DBAppInfo();

                    appInfo.appType = (receivedData.appType != null) ? receivedData.appType.ToLower() : null;
                    appInfo.campaignReference = receivedData.campaignReference;

                    appInfo.welcomeImageURL = (receivedData.welcomeScreenImage != null) ? receivedData.welcomeScreenImage.downloadUrl : null;
                    appInfo.finishedImageURL = (receivedData.finishedScreenImage != null) ? receivedData.finishedScreenImage.downloadUrl : null;

                    appInfo.welcomeImageFileType = (receivedData.welcomeScreenImage != null) ? receivedData.welcomeScreenImage.fileType : null;
                    appInfo.finishedImageFileType = (receivedData.finishedScreenImage != null) ? receivedData.finishedScreenImage.fileType : null;

                    appInfo.welcomeImageLocalPath = null; 
                    appInfo.finishedImageLocalPath = null; 

                    appInfo.welcomeImageMD5 = (receivedData.welcomeScreenImage != null) ? receivedData.welcomeScreenImage.md5 : null;
                    appInfo.finishedImageMD5 = (receivedData.finishedScreenImage != null) ? receivedData.finishedScreenImage.md5 : null;

                    DBLocalDataStore.GetInstance().SetAppInfo(appInfo);

                    if (receivedData.folders.Length == 0)
                    {
                        DBLocalDataStore.GetInstance().ShowLocalFiles(true);
                    }
                    else
                    {
                        DBLocalDataStore.GetInstance().ShowLocalFiles(false);
                    }

                    loghandler(true, "Files and forms updated from server", appInfo.appType.ToLower().Equals("kiosk"));
                }
                else
                {
                    loghandler(false, receivedData.error, false);
                }
            }
        }

        public static async Task<DBlocalContact> contactLookup(string contactUID)
        {
            Console.Error.WriteLine("Lookup started at: {0:HH:mm:ss.ffff}", DateTime.Now);

            DBAppInfo appInfo = DBLocalDataStore.GetInstance().GetAppInfo();

            var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();

            LookupContext lookup = new LookupContext(contactUID);
            lookup.context = new RequestData();
            var output = lookup.context;
            output.password = user.password;
            output.username = user.username;
            output.profile = DBLocalDataStore.GetInstance().GetSelectProfile().shortName;
            output.tags = new string[] { user.tags };
            output.campaignReference = appInfo.campaignReference;

            var jsonUser = JsonConvert.SerializeObject(lookup);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ServerURLs.contactLookupURL);
            try
            {
                //throw new Exception("TEST EXCEPTION");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 30000;
                httpWebRequest.KeepAlive = false;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonUser);
                }

                var response = await httpWebRequest.GetResponseAsync(); 

                return NetworkRequests.getContactFromLookup(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                Console.Error.WriteLine("Lookup finished at: {0:HH:mm:ss.ffff}", DateTime.Now);
                httpWebRequest.Abort();
            }
        }

        private static DBlocalContact getContactFromLookup(WebResponse response)
        {
            var httpResponse = response.GetResponseStream();
            using (var streamReader = new StreamReader(httpResponse))
            {
                var result = streamReader.ReadToEnd();
                Console.Error.WriteLine("Lookup result: {0}", result);
                var receivedData = JsonConvert.DeserializeObject<LookupResult>(result);
                if (receivedData.success)
                {
                    if (receivedData.contact != null)
                    {
                        return new DBlocalContact(receivedData.contact);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new Exception(receivedData.error);
                }
            }
        }
    }
}

