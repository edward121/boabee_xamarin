
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

namespace Leadbox
{
	[Activity (Label = "ActivityEditSelectedContact", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityEditSelectedContact : Activity
	{
        
		protected override void OnResume ()
		{
			base.OnResume();
        }



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            int ID = Intent.GetIntExtra("id_customer", -1);
            string _uuid = Intent.GetStringExtra("id_customer");
            bool EditScan = Intent.GetBooleanExtra("EditScan", false);


            SetContentView(Resource.Layout.Edit_contact_screen);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

            var customer = new DBlocalContact();

            if (string.IsNullOrEmpty(_uuid))
            {
                customer = DBLocalDataStore.GetInstance().GetLocalContactsById(ID);
            }
            else {
                customer = DBLocalDataStore.GetInstance().GetLocalContactsByUID(_uuid);
            }

            var textTop = FindViewById<TextView>(Resource.Id.textlogin);
            var firstName = FindViewById<EditText>(Resource.Id.firstName);
            var lastName = FindViewById<EditText>(Resource.Id.lastName);
            var email = FindViewById<EditText>(Resource.Id.emailField);
            var company = FindViewById<EditText>(Resource.Id.company);
            var phone = FindViewById<EditText>(Resource.Id.phone);
            var burcode = FindViewById<EditText>(Resource.Id.textBarcode);
            var job = FindViewById<EditText>(Resource.Id.job);
            var street = FindViewById<EditText>(Resource.Id.street);
            var zip = FindViewById<EditText>(Resource.Id.zip);
            var city = FindViewById<EditText>(Resource.Id.city);
            var country = FindViewById<EditText>(Resource.Id.country);

            var buttonCancel = FindViewById<TextView>(Resource.Id.buttonCancel);
            var buttonUpdate = FindViewById<TextView>(Resource.Id.buttonCreate);

            var textfirstName = FindViewById<TextView>(Resource.Id.textfirstName);
            var textlastName = FindViewById<TextView>(Resource.Id.textlastName);
            var textemail = FindViewById<TextView>(Resource.Id.textemailField);
            var texttextcompany = FindViewById<TextView>(Resource.Id.textcompany);
            var textphone = FindViewById<TextView>(Resource.Id.textphone);
            var textbarcode = FindViewById<TextView>(Resource.Id.textId);
            var texttextjob = FindViewById<TextView>(Resource.Id.textjob);
            var textStreet = FindViewById<TextView>(Resource.Id.textStreet);
            var textZip = FindViewById<TextView>(Resource.Id.textZip);
            var textCity = FindViewById<TextView>(Resource.Id.textCity);
            var textCountry = FindViewById<TextView>(Resource.Id.textCountry);

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
            buttonCancel.SetTypeface(font, TypefaceStyle.Normal);
            buttonUpdate.SetTypeface(font, TypefaceStyle.Normal);
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
            firstName.Text = customer.firstname;
            lastName.Text = customer.lastname;
            email.Text = customer.email;
            company.Text = customer.company;
            phone.Text = customer.phone;
            burcode.Text = customer.uid;
            job.Text = customer.jobtitle;
            street.Text = customer.street;
            zip.Text = customer.zip;
            city.Text = customer.city;
            country.Text = customer.country;

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

            if (EditScan == true)
            {
                buttonCancel.Visibility = ViewStates.Invisible;
                textTop.Text = "EDIT SCANNED CONTACT";
                buttonUpdate.Text = "Next";
            }


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
            //	popupDialog ("A contact with this email address already exists.\nYou can not overwrite existing contacts.");
            //	firstName.Enabled = false;
            //	lastName.Enabled = false;
            //	email.Enabled = false;
            //	company.Enabled = false;
            //	phone.Enabled = false;
            //	job.Enabled = false;
            //	street.Enabled = false;
            //	zip.Enabled = false;
            //	city.Enabled = false;
            //	country.Enabled = false;
            //}

            buttonCancel.Click += (object sender, EventArgs e) =>
            {
                Window.SetSoftInputMode(SoftInput.StateHidden);
                Finish();
            };

            buttonUpdate.Click += (object sender, EventArgs e) =>
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
                        //customer.source = "manual";
                        customer.street = street.Text;
                        customer.zip = zip.Text;
                        customer.city = city.Text;
                        customer.country = country.Text;
                        customer.activeContact = true;
                        customer = ActivityHomescreen.ReturnNull(customer);

                        BoaBeeLogic.OfflineLogic.createOrUpdateContact(customer);

						
						var _listPopup = DBLocalDataStore.GetInstance ().GetLocalContactsPopup ();
						var _tPopup = _listPopup.Where (s => s.email == email.Text).ToList ();
						var _customer=_tPopup.FirstOrDefault ();
						if (_customer == null) {
							_customer = new DBpopupContact ();
						}
							_customer.firstName = firstName.Text;
							_customer.lastName = lastName.Text;
							_customer.email = email.Text;
							_customer.company = company.Text;
							_customer.phone = phone.Text;
							_customer.jobTitle = job.Text;
							_customer.source = "manual";
							_customer.street = street.Text;
							_customer.zip = zip.Text;
							_customer.city = city.Text;
							_customer.country = country.Text;
                            

							DBLocalDataStore.GetInstance ().UpdatePopupContact(_customer);
                            Finish ();
                        

                    } else {
                        Console.WriteLine("Error adding customer");
                        //Toast.MakeText(this, "E-mail, Last Name or First Name incorrect.", ToastLength.Short).Show();
                    }
                //}
			};
        

		}

		/// <summary>
		/// Popups the dialog.
		/// </summary>
		/// <param name="alertMassege">Alert massege.</param>
		public void popupDialog(string alertMassege)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
			AlertDialog ad = builder.Create ();
			ad.SetMessage (alertMassege);
			ad.SetTitle ("Warning:");
			ad.SetCancelable (false);
			ad.SetCanceledOnTouchOutside (false);
			ad.SetButton ("Ok",(s, ev) => {
				Window.SetSoftInputMode(SoftInput.StateHidden);
				//this.OnBackPressed();
			});
			ad.Show ();
		}

		private bool ValidateForm(EditText firstname, EditText lastname)
		{
			if (string.IsNullOrWhiteSpace (firstname.Text)) {
				Toast.MakeText(this, "First name is missing.", ToastLength.Short).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace (lastname.Text)) {
				Toast.MakeText(this, "Last name is missing.", ToastLength.Short).Show();
				return false;
			}
			return true;
		}

		/// <summary>
		/// Hides the keyboard.
		/// </summary>
		/// <param name="pView">P view.</param>
		public void HideKeyboard(View pView) {
			InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
			inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
			Window.SetSoftInputMode (SoftInput.StateHidden);
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
		public override void OnBackPressed ()
		{
			
		}

	}
}

