
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
	[Activity (Label = "ActivityOverviewContactsDetail", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme",  WindowSoftInputMode = SoftInput.StateHidden)]			
	public class ActivityOverviewContactsDetail : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			var ID = Intent.GetIntExtra ("id_customer_overview", 0);
			SetContentView(Resource.Layout.Overview_contact_detail);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

            //var _list_popup_contacts = DBLocalDataStore.GetInstance ().GetLocalContactsById(ID);

			var customer = DBLocalDataStore.GetInstance().GetLocalContactsById(ID);

			var textTop = FindViewById<TextView>(Resource.Id.textlogin);
			var firstName = FindViewById<EditText> (Resource.Id.firstName);
			var lastName = FindViewById<EditText> (Resource.Id.lastName);
			var email = FindViewById<EditText> (Resource.Id.emailField);
			var company = FindViewById<EditText> (Resource.Id.company);
			var phone = FindViewById<EditText> (Resource.Id.phone);
			var barcode = FindViewById<EditText> (Resource.Id.id);
			var job = FindViewById<EditText> (Resource.Id.job);
			var street = FindViewById<EditText> (Resource.Id.street);
			var zip = FindViewById<EditText> (Resource.Id.zip);
			var city = FindViewById<EditText> (Resource.Id.city);
			var country = FindViewById<EditText> (Resource.Id.country);

			var buttonCancel = FindViewById<TextView>(Resource.Id.buttonCancel);

			var textfirstName = FindViewById<TextView> (Resource.Id.textfirstName);
			var textlastName = FindViewById<TextView> (Resource.Id.textlastName);
			var textemail = FindViewById<TextView> (Resource.Id.textemailField);
			var texttextcompany = FindViewById<TextView> (Resource.Id.textcompany);
			var textphone = FindViewById<TextView> (Resource.Id.textphone);
			var textbarcode = FindViewById<TextView> (Resource.Id.textId);
			var texttextjob = FindViewById<TextView> (Resource.Id.textjob);
			var infoCustomer = FindViewById<TextView> (Resource.Id.infoCustomer);
			var textStreet = FindViewById<TextView> (Resource.Id.textStreet);
			var textZip = FindViewById<TextView> (Resource.Id.textZip);
			var textCity = FindViewById<TextView> (Resource.Id.textCity);
			var textCountry = FindViewById<TextView> (Resource.Id.textCountry);

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

			textTop.SetTypeface (font, TypefaceStyle.Normal);
			firstName.SetTypeface (font, TypefaceStyle.Normal);
			lastName.SetTypeface (font, TypefaceStyle.Normal);
			email.SetTypeface (font, TypefaceStyle.Normal);
			company.SetTypeface (font, TypefaceStyle.Normal);
			phone.SetTypeface (font, TypefaceStyle.Normal);
			barcode.SetTypeface (font, TypefaceStyle.Normal);
			job.SetTypeface (font, TypefaceStyle.Normal);
			street.SetTypeface (font, TypefaceStyle.Normal);
			zip.SetTypeface (font, TypefaceStyle.Normal);
			city.SetTypeface (font, TypefaceStyle.Normal);
			country.SetTypeface (font, TypefaceStyle.Normal);

			buttonCancel.SetTypeface (font, TypefaceStyle.Normal);

			textfirstName.SetTypeface (font, TypefaceStyle.Normal);
			textlastName.SetTypeface (font, TypefaceStyle.Normal);
			textemail.SetTypeface (font, TypefaceStyle.Normal);
			texttextcompany.SetTypeface (font, TypefaceStyle.Normal);
			textphone.SetTypeface (font, TypefaceStyle.Normal);
			textbarcode.SetTypeface (font, TypefaceStyle.Normal);
			texttextjob.SetTypeface (font, TypefaceStyle.Normal);
			infoCustomer.SetTypeface (font, TypefaceStyle.Normal);
			textStreet.SetTypeface (font, TypefaceStyle.Normal);
			textZip.SetTypeface (font, TypefaceStyle.Normal);
			textCity.SetTypeface (font, TypefaceStyle.Normal);
			textCountry.SetTypeface (font, TypefaceStyle.Normal);

            firstName.Text = customer.firstname;
            lastName.Text = customer.lastname;
			email.Text = customer.email;
			company.Text = customer.company;
			phone.Text = customer.phone;
            barcode.Text = customer.uid;
            job.Text = customer.jobtitle;
			street.Text = customer.street;
			zip.Text = customer.zip;
			city.Text = customer.city;
			country.Text = customer.country;

			infoCustomer.Text = "";

			

			buttonCancel.Click += (object sender, EventArgs e) => {
				Finish();
			};
		}

		public override void OnBackPressed ()
		{
			Finish();
		}

	}
}

