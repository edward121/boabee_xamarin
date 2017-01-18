
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
using Android.Views.InputMethods;
using BoaBeePCL;
using Newtonsoft.Json;

namespace Leadbox
{

    public class contactData
    {
        DBlocalContact dBlocalContact;

        public contactData(DBlocalContact contact,string date,int requestid = 1)
        {
            this.contact = contact;
            this.date = date;
            this.RequestId = requestid;
        }

        public DBlocalContact contact  { get; set; }
        public string date { get; set; }
        public int RequestId { get; set; }
    }
	[Activity(Label = "ActivityOverviewShares", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme", WindowSoftInputMode = SoftInput.StateHidden)]			
	public class ActivityOverviewShares : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.OverviewShareLayout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			var txtCountShare = FindViewById<TextView> (Resource.Id.textCountShare);
			var txtFilter = FindViewById<EditText> (Resource.Id.textFilter);
			var _list_view_share = FindViewById<ListView> (Resource.Id.listViewShare);
			var btnClose = FindViewById<TextView> (Resource.Id.buttonCancel);
            var list_localcontact = DBLocalDataStore.GetInstance().GetLocalContacts();
            List<contactData> list_contacts_uid = new List<contactData>();
            List<DBSyncRequest> SyncRequest = new List<DBSyncRequest>();
            List<List<DBOrderLine>> orderLine = new List<List<DBOrderLine>>();
            List<bool> isSend = new List<bool>();
            try
            {
                SyncRequest = DBLocalDataStore.GetInstance().getSyncRequests();
            }
            catch { }
            for (int i = SyncRequest.Count - 1; i >= 0; i--)
            {
                var receivedData = JsonConvert.DeserializeObject<BoaBeePCL.SyncContext>(SyncRequest[i].serializedSyncContext);
                try
                {
                    if (receivedData.orders[0].orderLine.Count != 0)
                    {
                        for (int n = receivedData.orders.Count - 1; n >= 0; n--)
                        {
                            list_contacts_uid.Add(new contactData(list_localcontact.Find(s => s.uid == receivedData.orders[n].contactUid), receivedData.orders[0].created));
                            orderLine.Add(receivedData.orders[n].orderLine);
                            isSend.Add(SyncRequest[i].isSent);
                        }
                    }
                }
                catch { }
            }
            var _prof = list_contacts_uid;

			txtCountShare.Text = list_contacts_uid.Count + " emails sent";

			txtFilter.SetTypeface (font, TypefaceStyle.Normal);
			txtCountShare.SetTypeface (font, TypefaceStyle.Normal);
			btnClose.SetTypeface (font, TypefaceStyle.Normal);
			_list_view_share.Adapter = new OverviewContactsListAdapter (this, _prof, true);

			_list_view_share.ItemClick += (sender, e) => {
                ActivityOverviewSharesDetail.files = orderLine[e.Position];
                ActivityOverviewSharesDetail.customer = _prof[e.Position];
                ActivityOverviewSharesDetail.isSend = isSend[e.Position];
				var _activity = new Intent(this, typeof(ActivityOverviewSharesDetail));
                _activity.PutExtra("id_select_contact_for_files", _prof[e.Position].contact.Id);

				StartActivity(_activity);
			};

			txtFilter.Click += (sender, e) => {
				ShowKeyboard(txtFilter);
			};

			txtFilter.TextChanged += (sender, e) => {
				_prof = list_contacts_uid.Where(s => 
                                                (s.contact.firstname + s.contact.lastname + s.contact.company).ToUpper()
					.Contains(txtFilter.Text.ToUpper())).ToList();

				_list_view_share.Adapter = new OverviewContactsListAdapter (this, _prof, true);
			};

			btnClose.Click += (sender, e) =>  {
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
		//	StartActivity(typeof(ActivityHomescreen));
			Finish();
		}

	}
}

