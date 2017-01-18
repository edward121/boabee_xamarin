
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;
using Android.Views.InputMethods;
using System.Text.RegularExpressions;
using BoaBeePCL;
using BoaBeeLogic;

namespace Leadbox
{
    [Activity(Label = "ActivityEditScannedContact", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]
    public class ActivityEditScannedContact : Activity
    {

        protected override void OnResume()
        {
            base.OnResume();
        }

        public static string MessageText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string _uuid = Intent.GetStringExtra("id_customer");
            bool multi = Intent.GetBooleanExtra("Multi_flag", false);

            SetContentView(Resource.Layout.Edit_Scanned_contact_screen);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

            var customer = new DBlocalContact();


            customer = DBLocalDataStore.GetInstance().GetLocalContactsByUID(_uuid);


            var textTop = FindViewById<TextView>(Resource.Id.TitleScannedContact);
            var firstName = FindViewById<EditText>(Resource.Id.firstNameScanned);
            var lastName = FindViewById<EditText>(Resource.Id.lastNameScanned);
            var email = FindViewById<EditText>(Resource.Id.emailFieldScanned);
            var company = FindViewById<EditText>(Resource.Id.companyScanned);
            var phone = FindViewById<EditText>(Resource.Id.phoneScanned);
            var burcode = FindViewById<EditText>(Resource.Id.textBarcodeScanned);
            var job = FindViewById<EditText>(Resource.Id.jobScanned);
            var street = FindViewById<EditText>(Resource.Id.streetScanned);
            var zip = FindViewById<EditText>(Resource.Id.zipScanned);
            var city = FindViewById<EditText>(Resource.Id.cityScanned);
            var country = FindViewById<EditText>(Resource.Id.countryScanned);

            var buttonNext = FindViewById<TextView>(Resource.Id.buttonNext);
            var cancelButton = FindViewById<TextView>(Resource.Id.canselButton);


            var textfirstName = FindViewById<TextView>(Resource.Id.textfirstNameScanned);
            var textlastName = FindViewById<TextView>(Resource.Id.textlastNameScanned);
            var textemail = FindViewById<TextView>(Resource.Id.textemailFieldScanned);
            var texttextcompany = FindViewById<TextView>(Resource.Id.textcompanyScanned);
            var textphone = FindViewById<TextView>(Resource.Id.textphoneScanned);
            var textbarcode = FindViewById<TextView>(Resource.Id.textIdScanned);
            var texttextjob = FindViewById<TextView>(Resource.Id.textjobScanned);
            var textStreet = FindViewById<TextView>(Resource.Id.textStreetScanned);
            var textZip = FindViewById<TextView>(Resource.Id.textZipScanned);
            var textCity = FindViewById<TextView>(Resource.Id.textCityScanned);
            var textCountry = FindViewById<TextView>(Resource.Id.textCountryScanned);
            var textMessage = FindViewById<TextView>(Resource.Id.TextMessage);

            firstName.Enabled = false;
            lastName.Enabled = false;
            email.Enabled = false;
            company.Enabled = false;
            phone.Enabled = false;
            job.Enabled = false;
            street.Enabled = false;
            zip.Enabled = false;
            city.Enabled = false;
            country.Enabled = false;

            textMessage.SetTypeface(font, TypefaceStyle.Normal);
            textTop.SetTypeface(font, TypefaceStyle.Normal);
            firstName.SetTypeface(font, TypefaceStyle.Normal);
            lastName.SetTypeface(font, TypefaceStyle.Normal);
            email.SetTypeface(font, TypefaceStyle.Normal);
            company.SetTypeface(font, TypefaceStyle.Normal);
            phone.SetTypeface(font, TypefaceStyle.Normal);
            job.SetTypeface(font, TypefaceStyle.Normal);
            street.SetTypeface(font, TypefaceStyle.Normal);
            zip.SetTypeface(font, TypefaceStyle.Normal);
            city.SetTypeface(font, TypefaceStyle.Normal);
            country.SetTypeface(font, TypefaceStyle.Normal);
            buttonNext.SetTypeface(font, TypefaceStyle.Normal);

            burcode.SetTypeface(font, TypefaceStyle.Normal);

            textfirstName.SetTypeface(font, TypefaceStyle.Normal);
            textlastName.SetTypeface(font, TypefaceStyle.Normal);
            textemail.SetTypeface(font, TypefaceStyle.Normal);
            texttextcompany.SetTypeface(font, TypefaceStyle.Normal);
            textphone.SetTypeface(font, TypefaceStyle.Normal);
            textbarcode.SetTypeface(font, TypefaceStyle.Normal);
            texttextjob.SetTypeface(font, TypefaceStyle.Normal);
            textStreet.SetTypeface(font, TypefaceStyle.Normal);
            textZip.SetTypeface(font, TypefaceStyle.Normal);
            textCity.SetTypeface(font, TypefaceStyle.Normal);
            textCountry.SetTypeface(font, TypefaceStyle.Normal);

            //firstName.Focusable = true;
            burcode.Text = customer.uid;
            firstName.Text = customer.firstname;
            lastName.Text = customer.lastname;

            email.Text = customer.email;
            company.Text = customer.company;
            phone.Text = customer.phone;
            job.Text = customer.jobtitle;
            street.Text = customer.street;
            zip.Text = customer.zip;
            city.Text = customer.city;
            country.Text = customer.country;

            if (!string.IsNullOrWhiteSpace(email.Text) ||
               !string.IsNullOrWhiteSpace(company.Text) ||
               !string.IsNullOrWhiteSpace(job.Text) ||
               !string.IsNullOrWhiteSpace(street.Text) ||
               !string.IsNullOrWhiteSpace(zip.Text) ||
               !string.IsNullOrWhiteSpace(city.Text) ||
               !string.IsNullOrWhiteSpace(country.Text))
            {
                MessageText = "These are the details we have found for you. Feel free to complete the rest below.";
            }
            else {
                MessageText = "The check for additional contact details can't be executed at this moment. More details will be added to your report if available.";
            }

            textMessage.Text = MessageText;

            HideKeyboard(firstName);
            HideKeyboard(lastName);
            HideKeyboard(email);
            HideKeyboard(company);
            HideKeyboard(phone);
            HideKeyboard(job);
            HideKeyboard(street);
            HideKeyboard(zip);
            HideKeyboard(city);
            HideKeyboard(country);



            //if (customer.source == "manual" || customer.source == "server") {
            Console.WriteLine("manual editing");
            firstName.Enabled = true;
            lastName.Enabled = true;
            email.Enabled = true;
            company.Enabled = true;
            phone.Enabled = true;
            job.Enabled = true;
            street.Enabled = true;
            zip.Enabled = true;
            city.Enabled = true;
            country.Enabled = true;
            ////} else {
            //  popupDialog ("A contact with this email address already exists.\nYou can not overwrite existing contacts.");
            //  firstName.Enabled = false;
            //  lastName.Enabled = false;
            //  email.Enabled = false;
            //  company.Enabled = false;
            //  phone.Enabled = false;
            //  job.Enabled = false;
            //  street.Enabled = false;
            //  zip.Enabled = false;
            //  city.Enabled = false;
            //  country.Enabled = false;
            //}
            cancelButton.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                AlertDialog ad = builder.Create();
                ad.SetMessage("Your current work will be lost.");
                ad.SetTitle("Warning:");
                ad.SetButton("Cancel", (s, ev) =>
                {
                });
                ad.SetButton2("Ok", (s, ev) =>
                {
                    SaveAndLoad.GetInstance().DeleteFile();
                    OfflineLogic.ClearDataSelected();
                    this.Finish();
                });

                ad.Show();
            };


            buttonNext.Click += (object sender, EventArgs e) =>
            {
                if (ValidateForm(firstName, lastName))
                {
                    HideKeyboard(firstName);
                    HideKeyboard(lastName);
                    HideKeyboard(email);
                    HideKeyboard(company);
                    HideKeyboard(job);
                    HideKeyboard(phone);
                    HideKeyboard(street);
                    HideKeyboard(zip);
                    HideKeyboard(city);
                    HideKeyboard(country);
                    //if (customer.source == "manual" || customer.source == "server") {

                    var _list = DBLocalDataStore.GetInstance().GetLocalContacts();
                    var _t = _list.Where(s => s.email == email.Text).ToList();

                    var t = _t.Where(s => s.Id != customer.Id).ToList().Count;


                    customer.firstname = firstName.Text;
                    customer.lastname = lastName.Text;
                    customer.email = email.Text;
                    customer.company = company.Text;
                    customer.phone = phone.Text;
                    customer.jobtitle = job.Text;
                    customer.street = street.Text;
                    customer.zip = zip.Text;
                    customer.city = city.Text;
                    customer.country = country.Text;
                    customer.activeContact = true;
                    customer = ActivityHomescreen.ReturnNull(customer);
                    BoaBeeLogic.OfflineLogic.createOrUpdateContact(customer);
                        //var contactsToSend = DBLocalDataStore.GetInstance().GetContactsToServer().Where(c => c.uid == ContactToServer.uid).ToList();
                        //if (contactsToSend.Count == 0)
                        //{
                        //    ContactToServer = ContactToServer.NewContact(ContactToServer);
                        //    DBLocalDataStore.GetInstance().AddContactToServer(ContactToServer);
                        //}
                        //else {
                        //    ContactToServer = new DBContactToServer();
                        //    ContactToServer = ContactToServer.NewContact(ContactToServer);
                        //    contactsToSend[0] &= ContactToServer;
                        //    DBLocalDataStore.GetInstance().UpdateContactToServer(contactsToSend[0]);
                        //}
                    

                    if (multi)
                        Finish();
                    else {
                        if (DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition()).Count != 0)
                        {
                            StartActivity(typeof(ActivityIdentifyClassifyShare));
                            Finish();
                        }
                        else {
                            if (DBLocalDataStore.GetInstance().GetAllLocalFiles().Count != 0)
                            {
                                StartActivity(typeof(ActivityIdentifyClassifyShare));
                                Finish();
                            }
                            else {
                                DBLocalDataStore.GetInstance().resetAnswers();
                                var answer2 = SaveAndLoad.GetInstance().GetAllAnswers();
                                var listques = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
                                if (DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition()).Count != 0)
                                {
                                    for (int i = 0; i < answer2.Count; i++)
                                    {
                                        DBAnswer answer = new DBAnswer();
                                        answer.question = listques[i].question;
                                        answer.answer = answer2[i] == "_,___" ? "" : answer2[i];
                                        answer.Id = i + 1;
                                        DBLocalDataStore.GetInstance().updateAnswer(answer);
                                    }
                                }
                                OfflineLogic.prepareSync();
                                SaveAndLoad.GetInstance().DeleteFile();
                                OfflineLogic.ClearDataSelected();
                                StartActivity(new Intent(this, typeof(ActivityHomescreen)));
                                Finish();
                            }
                        }
                    }


                };
            };
        }

        

        /// <summary>
        /// Popups the dialog.
        /// </summary>
        /// <param name="alertMassege">Alert massege.</param>
        public void popupDialog(string alertMassege)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
            AlertDialog ad = builder.Create();
            ad.SetMessage(alertMassege);
            ad.SetTitle("Warning:");
            ad.SetCancelable(false);
            ad.SetCanceledOnTouchOutside(false);
            ad.SetButton("Ok", (s, ev) =>
            {
                Window.SetSoftInputMode(SoftInput.StateHidden);
                //this.OnBackPressed();
            });
            ad.Show();
        }

        private bool ValidateForm(EditText firstname, EditText lastname)
        {
            //if (string.IsNullOrWhiteSpace(firstname.Text))
            //{
            //    Toast.MakeText(this, "First name is missing.", ToastLength.Short).Show();
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(lastname.Text))
            //{
            //    Toast.MakeText(this, "Last name is missing.", ToastLength.Short).Show();
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// Hides the keyboard.
        /// </summary>
        /// <param name="pView">P view.</param>
        public void HideKeyboard(View pView)
        {
            InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
            Window.SetSoftInputMode(SoftInput.StateHidden);
        }

        /// <Docs>Called when the activity has detected the user's press of the back
        ///  key.</Docs>
        /// <para tool="javadoc-to-mdoc">Called when the activity has detected the user's press of the back
        ///  key. The default implementation simply finishes the current activity,
        ///  but you can override this to do whatever you want.</para>
        /// <format type="text/html">[Android Documentation]</format>
        /// <since version="Added in API level 5"></since>
        /// <summary>
        /// Raises the back pressed event.
        /// </summary>
        public override void OnBackPressed()
        {

        }

    }
}

