
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
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Android.Views.InputMethods;
using Android.Content.Res;
using BoaBeePCL;
using BoaBeeLogic;

namespace Leadbox
{
	[Activity(Label = "ActivitySelectContact", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme", WindowSoftInputMode = SoftInput.StateAlwaysHidden)]			
	public class ActivitySelectContact : Activity
	{
		List<DBlocalContact> prof;
		List<DBlocalContact> _prof_search;
		EditText searchText;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView (Resource.Layout.SelectContactLayout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			var TextTop = FindViewById<TextView>(Resource.Id.textselectapp);

			var buttonCancel = FindViewById<TextView>(Resource.Id.buttonCancel);
			searchText = FindViewById<EditText> (Resource.Id.lookUpFilter);
			searchText.SetTypeface (font, TypefaceStyle.Normal);
			HideKeyboard(searchText);
			var temp_list = DBLocalDataStore.GetInstance ().GetLocalContacts();
			prof = temp_list.Where(s=>s.email != null).ToList();
            prof = prof.OrderBy (s => s.firstname + s.lastname).ToList();
			////var temp_cust_without_bc = temp_list.Where (s=> s.email == null && s.barcode != null).ToList ();
			//temp_cust_without_bc = temp_cust_without_bc.OrderBy (s=>s.barcode).ToList();

			//foreach (var cust in temp_cust_without_bc) {
			//	prof.Add (cust);
			//}

			_prof_search = prof;

			var listitems = FindViewById<ListView> (Resource.Id.listViewApps);

			listitems.Adapter = new ContactsListAdapter (this, _prof_search, ClickCustomerEdit, ClickCustomer);

			listitems.ScrollStateChanged += (sender, e) => 
				HideKeyboard(searchText);

			searchText.Click += (sender, e) => {
				ShowKeyboard(searchText);
			};

			TextTop.SetTypeface (font, TypefaceStyle.Normal);
			buttonCancel.SetTypeface (font, TypefaceStyle.Normal);

			buttonCancel.Click += (object sender, EventArgs e) => {
				//StartActivity(typeof(ActivityLoginForm));
				HideKeyboard(searchText);
				Finish();

                StartActivity(typeof(ActivityBadgeScanning));
			};
			searchText.TextChanged += (sender, e) => {
				_prof_search = prof.Where(s => 
                                          (s.firstname + " " + s.lastname + " (" + s.company + ")").ToUpper()
                .Contains(searchText.Text.ToUpper()) || s.uid != null && s.email == null && ("badge "+s.uid).Contains(searchText.Text)).ToList();



				listitems.Adapter = new ContactsListAdapter (this, _prof_search, ClickCustomerEdit, ClickCustomer);

			};

		}

		void ClickCustomerEdit(object sender, EventArgs e)
		{
			int id = 0;
			if (sender is TextView)
			{
				//paymentId = Convert.ToString(((ImageButton) sender).Tag);
				id = Convert.ToInt32( ((TextView) sender).Tag);
			}
			HideKeyboard(searchText);
			var _activity = new Intent(this, typeof(ActivityEditSelectedContact));
            if (string.IsNullOrEmpty (_prof_search [id].uid)) {
				_activity.PutExtra ("id_customer", _prof_search [id].Id);
				StartActivity (_activity);
			} else {
                _activity.PutExtra ("id_customer", _prof_search [id].uid);
				StartActivity (_activity);
			}
            Finish();

		}

		void ClickCustomer(object sender, EventArgs e)
		{
			int id = 0;
			if (sender is TextView)
			{
				id = Convert.ToInt32( ((TextView) sender).Tag);
			}

			if (_prof_search.Count == 0)
				_prof_search = prof;

			var lu = _prof_search[id];

            lu.activeContact = true;
            DBLocalDataStore.GetInstance().UpdateLocalContact(lu);
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
			HideKeyboard(searchText);
			//Finish();
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

		protected override void OnResume ()
		{
            base.OnResume();
			HideKeyboard(searchText);
			//Window.SetSoftInputMode (SoftInput.StateHidden);

            var temp_list = DBLocalDataStore.GetInstance ().GetLocalContacts();
			prof = temp_list.Where(s=>s.email != null).ToList();
            prof = prof.OrderBy (s => s.firstname + s.lastname).ToList();
            var temp_cust_without_bc = temp_list.Where (s=> s.email == null && s.uid != null).ToList ();
            temp_cust_without_bc = temp_cust_without_bc.OrderBy (s=>s.uid).ToList();

            foreach (var cust in temp_cust_without_bc) {
				prof.Add (cust);
			}

			//_prof_search = prof;
			_prof_search = prof.Where(s => 
                                      (s.firstname + " " + s.lastname + " (" + s.company + ")").ToUpper()
				.Contains(searchText.Text.ToUpper())).ToList();

			var listitems = FindViewById<ListView> (Resource.Id.listViewApps);
			listitems.Adapter = new ContactsListAdapter (this, _prof_search, ClickCustomerEdit, ClickCustomer);
		}

	}
}

