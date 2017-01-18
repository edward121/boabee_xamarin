
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
using Android.Views.InputMethods;
using BoaBeePCL;
using Newtonsoft.Json;

namespace Leadbox
{
	[Activity(Label = "ActivityOverviewContacts", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme", WindowSoftInputMode = SoftInput.StateHidden)]			
	public class ActivityOverviewContacts : Activity
	{
        //private static List<contactData> contactForOvervie(List<contactData> listcontact)
        //{
        //    List<contactData> contacts = new List<contactData>();
        //    bool finded;
        //    for (int i = 0; i < listcontact.Count; i++)
        //    {
        //        finded = false;
        //        for (int n = i + 1; n < listcontact.Count; n++)
        //        {
        //            if (listcontact[i].contact.uid == listcontact[n].contact.uid)
        //            {
        //                finded = true;
        //            }
        //        }
        //        if (!finded)
        //        {
        //            contacts.Add(listcontact[i]);
        //        }
        //    }
        //    return contacts;
        //}
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OverviewContactLayout);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

            var txtCount = FindViewById<TextView>(Resource.Id.textselectapp);
            var txtFilter = FindViewById<EditText>(Resource.Id.textFilter);
            var _list_contacts = FindViewById<ListView>(Resource.Id.listViewApps);
            var btnClose = FindViewById<TextView>(Resource.Id.buttonCancel);
            txtCount.SetTypeface(font, TypefaceStyle.Normal);
            txtFilter.SetTypeface(font, TypefaceStyle.Normal);
            btnClose.SetTypeface(font, TypefaceStyle.Normal);

            var list_localcontact = DBLocalDataStore.GetInstance().GetLocalContacts();
            List<contactData> list_contacts_uid = new List<contactData>();
            List<DBSyncRequest> SyncRequest = new List<DBSyncRequest>();
            try
            {
                SyncRequest = DBLocalDataStore.GetInstance().getSyncRequests();
            }
            catch { }
            //for (int i = SyncRequest.Count - 1; i >= 0; i--)
            //{
            //    var receivedData = JsonConvert.DeserializeObject<BoaBeePCL.SyncContext>(SyncRequest[i].serializedSyncContext);
            //    if (receivedData.contacts != null)
            //    {
            //        if (receivedData.contacts.Count != 0)
            //        {
            //            for (int n = receivedData.contacts.Count - 1; n >= 0; n--)
            //            {
            //                DBlocalContact contact = list_localcontact.Find(s => s.uid == receivedData.contacts[n].uid);
            //                string date = receivedData.orders[0].created;
            //                contactData contactdate = new contactData(contact, date);
            //                list_contacts_uid.Add(contactdate);
            //        }
            //    }
            //    }
            //}
            var contactsInRequest = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.useInRequest).ToList();
            for (int i = 0; i < contactsInRequest.Count; i++)
            {
                list_contacts_uid.Add(new contactData(contactsInRequest[i], ""));
            }
            var _prof = list_contacts_uid;



			txtCount.Text = list_contacts_uid.Count + " contact met";
			_list_contacts.Adapter = new OverviewContactsListAdapter (this, _prof, false);

			txtFilter.Click += (sender, e) => {
				ShowKeyboard(txtFilter);
			};

			txtFilter.TextChanged += (sender, e) => {
					_prof = list_contacts_uid.Where(s => 
                    (s.contact.firstname + s.contact.lastname + s.contact.company).ToUpper()
					.Contains(txtFilter.Text.ToUpper())).ToList();

				_list_contacts.Adapter = new OverviewContactsListAdapter (this, _prof, false);
			};

			_list_contacts.ItemClick += (sender, e) => {
				//_prof[e.Position];
                if (!string.IsNullOrEmpty(_prof[e.Position].contact.uid)){
					var _activity = new Intent(this, typeof(ActivityOverviewContactsDetail));
                    _activity.PutExtra("id_customer_overview", _prof[e.Position].contact.Id);
					StartActivity(_activity);
				}
			};

			btnClose.Click += (sender, e) => {
				HideKeyboard(txtFilter);
				StartActivity(typeof(ActivityHomescreen));
				Finish();
			};

		}

		public void ShowKeyboard(View pView) {
			pView.RequestFocus();

			InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
			inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
			inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
		}

		public void HideKeyboard(View pView) {
			InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
			inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
		}

		public override void OnBackPressed ()
		{
			base.OnBackPressed ();
			var txtFilter = FindViewById<EditText> (Resource.Id.textFilter);
			HideKeyboard(txtFilter);
			StartActivity(typeof(ActivityHomescreen));
			Finish();
		}

	}
}

