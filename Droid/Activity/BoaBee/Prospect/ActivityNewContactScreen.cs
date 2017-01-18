
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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Java.Lang;
using Android.Views.InputMethods;
using BoaBeePCL;
using BoaBeeLogic;

namespace Leadbox
{
	[Activity (Label = "ActivityNewContactScreen", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme", WindowSoftInputMode = SoftInput.StateHidden)]			
	public class ActivityNewContactScreen : Activity
	{
        List<DBlocalContact> localcontacts;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

				SetContentView (Resource.Layout.New_contact_screen);

				Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

				var textTop = FindViewById<TextView>(Resource.Id.textlogin);
				var firstName = FindViewById<EditText> (Resource.Id.firstName);
				var lastName = FindViewById<EditText> (Resource.Id.lastName);
				var email = FindViewById<EditText> (Resource.Id.emailField);
				var company = FindViewById<EditText> (Resource.Id.company);
		     	var phone = FindViewById<EditText> (Resource.Id.phone);
			    var job = FindViewById<EditText> (Resource.Id.job);
				var street = FindViewById<EditText> (Resource.Id.street);
				var zip = FindViewById<EditText> (Resource.Id.zip);
				var city = FindViewById<EditText> (Resource.Id.city);
				var country = FindViewById<EditText> (Resource.Id.country);
				var buttonCancel = FindViewById<TextView>(Resource.Id.buttonCancel);
				var buttonCreate = FindViewById<TextView>(Resource.Id.buttonCreate);

				var txtfirstName = FindViewById<TextView>(Resource.Id.textfirstname);
				var txtlastName = FindViewById<TextView>(Resource.Id.textlastName);
				var txtemail = FindViewById<TextView>(Resource.Id.textemailField);
				var txtcompany = FindViewById<TextView>(Resource.Id.textcompany);
			    var txtphone = FindViewById<TextView>(Resource.Id.textphone);
			    var txtjob= FindViewById<TextView>(Resource.Id.textjob);
				var txtzip = FindViewById<TextView> (Resource.Id.textZip);
				var txtcity = FindViewById<TextView> (Resource.Id.textCity);
				var txtcountry = FindViewById<TextView> (Resource.Id.textCountry);
				var txtstreet = FindViewById<TextView> (Resource.Id.textStreet);

				txtfirstName.SetTypeface (font, TypefaceStyle.Normal);
				txtlastName.SetTypeface (font, TypefaceStyle.Normal);
				txtemail.SetTypeface (font, TypefaceStyle.Normal);
				txtcompany.SetTypeface (font, TypefaceStyle.Normal);
				txtphone.SetTypeface (font, TypefaceStyle.Normal);
			    txtjob.SetTypeface (font, TypefaceStyle.Normal);
				txtzip.SetTypeface (font, TypefaceStyle.Normal);
				txtcity.SetTypeface (font, TypefaceStyle.Normal);
				txtcountry.SetTypeface (font, TypefaceStyle.Normal);
				txtstreet.SetTypeface (font, TypefaceStyle.Normal);

				textTop.SetTypeface (font, TypefaceStyle.Normal);
				firstName.SetTypeface (font, TypefaceStyle.Normal);
				lastName.SetTypeface (font, TypefaceStyle.Normal);
				email.SetTypeface (font, TypefaceStyle.Normal);
				company.SetTypeface (font, TypefaceStyle.Normal);
			    job.SetTypeface (font, TypefaceStyle.Normal);
				phone.SetTypeface (font, TypefaceStyle.Normal);
				street.SetTypeface (font, TypefaceStyle.Normal);
				zip.SetTypeface (font, TypefaceStyle.Normal);
				city.SetTypeface (font, TypefaceStyle.Normal);
				country.SetTypeface (font, TypefaceStyle.Normal);
				buttonCancel.SetTypeface (font, TypefaceStyle.Normal);
				buttonCreate.SetTypeface (font, TypefaceStyle.Normal);

				buttonCancel.Click += (object sender, EventArgs e) => {
					HideKeyboard(firstName);
					HideKeyboard(lastName);
					HideKeyboard(email);
					HideKeyboard(company);
				    HideKeyboard(phone);
				    HideKeyboard(job);
					HideKeyboard (zip);
					HideKeyboard (street);
					HideKeyboard (city);
					HideKeyboard (country);
					Finish();
                   
                    StartActivity(typeof(ActivityBadgeScanning));
				};

				buttonCreate.Click += (object sender, EventArgs e) => {
					HideKeyboard(firstName);
					HideKeyboard(lastName);
					HideKeyboard(email);
					HideKeyboard(company);
					HideKeyboard(phone);
				    HideKeyboard(job);
					HideKeyboard (zip);
					HideKeyboard (street);
					HideKeyboard (city);
					HideKeyboard (country);
					if(ValidateForm(email, firstName, lastName))
					{
                        var lc = new DBlocalContact();
                        lc.firstname = firstName.Text;
                        lc.lastname = lastName.Text;
                        lc.email = email.Text;
                        lc.company = company.Text;
                        lc.jobtitle = company.Text;
                        lc.phone = phone.Text;
                        lc.street = street.Text;
                        lc.zip = zip.Text;
                        lc.city = city.Text;
                        lc.country = country.Text;
                        lc.activeContact = true;
                        lc.uid = lc.email;
                        
                        lc = ActivityHomescreen.ReturnNull(lc);
                        BoaBeeLogic.OfflineLogic.createOrUpdateContact(lc);
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
                        Finish();
					}
					else
					{
						Console.WriteLine("Invalid create new customer!");
						//Toast.MakeText(this, "E-mail or Last Name or First Name incorrect.", ToastLength.Short).Show();
					}
				};
		}

		public override void Finish ()
		{
			Console.WriteLine ("Finish activity_new_customer");
			base.Finish ();
		}

		void ShowAlert(string message, DBpopupContact lc)
		{
			var firstName = FindViewById<EditText> (Resource.Id.firstName);
			var lastName = FindViewById<EditText> (Resource.Id.lastName);
			var email = FindViewById<EditText> (Resource.Id.emailField);
			var company = FindViewById<EditText> (Resource.Id.company);
			var phone = FindViewById<EditText> (Resource.Id.phone);
			var job = FindViewById<EditText> (Resource.Id.job);
			var zip = FindViewById<EditText> (Resource.Id.zip);
			var street = FindViewById<EditText> (Resource.Id.street);
			var city = FindViewById<EditText> (Resource.Id.city);
			var country = FindViewById<EditText> (Resource.Id.country);

			HideKeyboard(firstName);
			HideKeyboard(lastName);
			HideKeyboard(email);
			HideKeyboard(company);
			HideKeyboard(phone);
			HideKeyboard(job);
			HideKeyboard (zip);
			HideKeyboard (street);
			HideKeyboard (city);
			HideKeyboard (country);
			AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
			AlertDialog ad = builder.Create ();
			ad.SetCancelable (false);
			ad.SetCanceledOnTouchOutside (false);
			ad.SetMessage (message);
			ad.SetTitle ("Checked customer:");
			ad.SetButton ("UPDATE & CONTINUE".ToUpper(),(s, ev) => {
				if (DBLocalDataStore.GetInstance ().GetLocalContactsContainsPopup(lc) == 0)
					DBLocalDataStore.GetInstance().AddLocalContactPopup(lc);

				var list_contact = DBLocalDataStore.GetInstance ().GetLocalContacts();
				var exist_costomer = list_contact.FirstOrDefault(o=>o.email == lc.email);

                exist_costomer.firstname = lc.firstName;
                exist_costomer.lastname = lc.lastName;
				exist_costomer.company = lc.company;
				exist_costomer.phone = lc.phone;
				exist_costomer.email = lc.email;
                exist_costomer.jobtitle = lc.jobTitle;
				exist_costomer.zip = lc.zip;
				exist_costomer.street = lc.street;
				exist_costomer.city = lc.city;
				exist_costomer.country = lc.country;


				DBLocalDataStore.GetInstance().UpdateLocalContact(exist_costomer);

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
                };
			});
			ad.SetButton2 ("CANCEL".ToUpper(), (s, ev) => {
				
			});
			ad.Show ();
		}

		private bool ValidateForm(EditText user, EditText firstname,  EditText lastname)
        {

            if (string.IsNullOrWhiteSpace(firstname.Text))
            {
                Toast.MakeText(this, "First name is missing.", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrWhiteSpace(lastname.Text))
            {
                Toast.MakeText(this, "Last name is missing.", ToastLength.Short).Show();
                return false;
            }

			if (string.IsNullOrWhiteSpace (user.Text)) {
				Toast.MakeText(this, "Email is missing.", ToastLength.Short).Show();
				return false;
			}

			if (!isEmail (user.Text)) {
				Toast.MakeText(this, "Email is incorrect.", ToastLength.Short).Show();
				return false;
			}
			return true;

		}

		private bool isEmail(string email)
		{
			var regexPatter = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$";

			if(Regex.Match(email,regexPatter).Success)
				return true;

			return false;
		}
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StartActivity(typeof(ActivityBadgeScanning));
        }

		public void HideKeyboard(View pView) {
			InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
			inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
			Window.SetSoftInputMode (SoftInput.StateHidden);
		}

	}
}

