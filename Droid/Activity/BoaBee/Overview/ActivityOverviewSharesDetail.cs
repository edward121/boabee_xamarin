
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
using Java.Net;
using Dalvik.SystemInterop;
using BoaBeePCL;

namespace Leadbox
{
	[Activity(Label = "ActivityOverviewSharesDetail", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityOverviewSharesDetail : Activity
	{
		private int _id;
        public static List<DBOrderLine> files;
        public static contactData customer;
        public static bool isSend = false;
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			_id = Intent.GetIntExtra ("id_select_contact_for_files", 0);
			SetContentView (Resource.Layout.OverviewShareDetailLayout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			var txtUpText = FindViewById<TextView> (Resource.Id.textCountShare);
			var _list_view_share = FindViewById<ListView> (Resource.Id.listViewShare);
			var btnClose = FindViewById<TextView> (Resource.Id.buttonCancel);
			var nameContact = FindViewById<TextView> (Resource.Id.contactName);

            //var customer = DBLocalDataStore.GetInstance ().GetLocalContactsById (_id);
			//var customer = _list_popup_contacts.Single(o=>o.uuid == _id);
			//var list_contacts_mail = new List<DBOverviewContacts>();
			/*
			if (!string.IsNullOrEmpty (customer.email))
				list_contacts_mail = _list_popup_contacts.Where (s => s.email == customer.email).ToList ();
			else {
				list_contacts_mail = _list_popup_contacts.Where (s => s.barcode == customer.barcode).ToList ();
			}

			if (!string.IsNullOrEmpty (customer.email)) {
				if (!string.IsNullOrEmpty (customer.company))
					nameContact.Text = customer.firstName + " " + customer.lastName + " (" + customer.company + ")";
				else
					nameContact.Text = customer.firstName + " " + customer.lastName;
			} else {
				nameContact.Text = customer.barcode;
			}*/
            

            var _list = files;
			/*foreach (var cust in list_contacts_mail) {
				var temp_list = DBLocalDataStore.GetInstance ().GetOverwievFiles (_id, "");
				//temp_list = temp_list.OrderBy (s=>s.name).ToList();
				foreach (var temp in temp_list) {
					_list.Add (temp);
				}
			}*/

			var _status_send = "";
            if(!isSend){
				_status_send = "only stored locally";
			} else {
				_status_send = "safely stored in the cloud";
			}

			var full_description = "";
            if (!string.IsNullOrEmpty (customer.contact.firstname)) {
				if (!string.IsNullOrEmpty (customer.contact.company))
                    full_description = customer.contact.firstname + " " + customer.contact.lastname + " (" + customer.contact.company + ")";
				else
                    full_description = customer.contact.firstname + " " + customer.contact.lastname;
			} else {
                full_description = "uid"+" "+customer.contact.uid;
			}
            string[] forstr1 = customer.date.Split('T');
            string[] forstr2 = customer.date.Split('T');
            forstr2 = forstr2[1].Split('+');
            forstr1[0].Replace('-', '.');

            full_description = full_description + "\n" + forstr1[0] + " " + forstr2[0];
			full_description = full_description + "\n" + _status_send;

			nameContact.Text = full_description;


			txtUpText.SetTypeface (font, TypefaceStyle.Normal);
			nameContact.SetTypeface (font, TypefaceStyle.Normal);
			btnClose.SetTypeface (font, TypefaceStyle.Normal);
			_list_view_share.Adapter = new OverviewSharesDetailListAdapter (this, _list);


			btnClose.Click += (sender, e) =>  Finish();
		}
	}
}

