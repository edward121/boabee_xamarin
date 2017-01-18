
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
	[Activity(Label = "ActivityOverviewForms", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme", WindowSoftInputMode = SoftInput.StateHidden)]			
	public class ActivityOverviewForms : Activity
	{Dialog alertDialog;

        public List<DBlocalContact> list_localcontact = DBLocalDataStore.GetInstance().GetLocalContacts();
        public List<contactData> list_contacts_uid = new List<contactData>();
        public List<DBSyncRequest> SyncRequest = new List<DBSyncRequest>();
        public List<AnsweredForm> forms = new List<AnsweredForm>();
        public List<bool> isSend = new List<bool>();
        public ListView _list_contacts;
        public List<contactData> _prof;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView (Resource.Layout.OverviewContactLayout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            List<int> SyncID = new List<int>();

			var txtCount = FindViewById<TextView> (Resource.Id.textselectapp);
			var txtFilter = FindViewById<EditText> (Resource.Id.textFilter);
			_list_contacts = FindViewById<ListView> (Resource.Id.listViewApps);
			var btnClose = FindViewById<TextView> (Resource.Id.buttonCancel);
			txtCount.SetTypeface (font, TypefaceStyle.Normal);
			txtFilter.SetTypeface (font, TypefaceStyle.Normal);
			btnClose.SetTypeface (font, TypefaceStyle.Normal);
            //var list_localcontact = DBLocalDataStore.GetInstance().GetLocalContacts();
            //List<contactData> list_contacts_uid = new List<contactData>();
            //List<DBSyncRequest> SyncRequest = new List<DBSyncRequest>();
            //List<AnsweredForm> forms = new List<AnsweredForm>();
            //List<bool> isSend = new List<bool>();
            try
            {
                SyncRequest = DBLocalDataStore.GetInstance().getSyncRequests();
            }
            catch { }
            for (int i = SyncRequest.Count-1; i >= 0; i--)
            {
                var receivedData = JsonConvert.DeserializeObject<BoaBeePCL.SyncContext>(SyncRequest[i].serializedSyncContext);
                if (receivedData.forms.Count != 0)
                {
                    for (int n = receivedData.forms.Count -1 ; n >= 0; n--)
                    {
                        list_contacts_uid.Add(new contactData(list_localcontact.Find(s => s.uid == receivedData.forms[n].contactUid), receivedData.forms[0].startdate, SyncRequest[i].Id));
                        forms.Add(receivedData.forms[n]);
                        isSend.Add(SyncRequest[i].isSent);
                    }
                }
            }

            _prof = list_contacts_uid;
			txtCount.Text = list_contacts_uid.Count + " info sheets";
			_list_contacts.Adapter = new OverviewContactsListAdapter (this, _prof, true);

			txtFilter.Click += (sender, e) => {
				ShowKeyboard(txtFilter);

			};

			txtFilter.TextChanged += (sender, e) => {
				_prof = list_contacts_uid.Where(s => 
                                                (s.contact.firstname + s.contact.lastname + s.contact.company).ToUpper()
					.Contains(txtFilter.Text.ToUpper())).ToList();

				_list_contacts.Adapter = new OverviewContactsListAdapter (this, _prof, true);
			};

			_list_contacts.ItemClick += (sender, e) => {
				var _activity = new Intent(this, typeof(ActivityOverviewFormsDetail));
                _activity.PutExtra("id_form_overview", _prof[e.Position].contact.uid);
                ActivityOverviewFormsDetail.forms = forms[e.Position];
                for (int i = 0; i < forms.Count; i++)
                {
                    if (forms[e.Position].contactUid == forms[i].contactUid)
                    {
                        if (Convert.ToDateTime(forms[i].enddate) > Convert.ToDateTime(ActivityOverviewFormsDetail.forms.enddate))
                        { 
                            ActivityOverviewFormsDetail.forms = forms[i];
                        }
                    }
                
                }
                ActivityOverviewFormsDetail.data = _prof[e.Position];
                ActivityOverviewFormsDetail.isSend = isSend[e.Position];
                ActivityOverviewFormsDetail.RequestId = _prof[e.Position].RequestId;
				StartActivity(_activity);
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

		protected override void OnResume()
		{
            
            base.OnResume();
            var SyncRequest = DBLocalDataStore.GetInstance().getSyncRequests();
            list_contacts_uid = new List<contactData>();
            for (int i = SyncRequest.Count - 1; i >= 0; i--)
            {
                var receivedData = JsonConvert.DeserializeObject<BoaBeePCL.SyncContext>(SyncRequest[i].serializedSyncContext);
                if (receivedData.forms.Count != 0)
                {
                    for (int n = receivedData.forms.Count - 1; n >= 0; n--)
                    {
                        list_contacts_uid.Add(new contactData(list_localcontact.Find(s => s.uid == receivedData.forms[n].contactUid), receivedData.forms[0].startdate, SyncRequest[i].Id));
                        forms.Add(receivedData.forms[n]);
                        isSend.Add(SyncRequest[i].isSent);
                    }
                }
            }

            _prof = list_contacts_uid;
            _list_contacts.Adapter = new OverviewContactsListAdapter(this, _prof, true);
			Window.SetSoftInputMode(SoftInput.StateHidden);
		}

	}
}

