
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Views.Animations;
using Android.Support.V4.View;
using System.Threading;
using BoaBeePCL;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;

namespace Leadbox
{
	public class FragmentIdentifyScreen : Android.Support.V4.App.Fragment
	{
		public ViewPager _viewPager{ get; set; }
		TextView btnShowcontactPopup;
		ListView listitems;
		Animation fadeIn;
		Animation fadeOut;
		RelativeLayout popUpconract;
        List<DBlocalContact> localcontacts;


		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}



		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            localcontacts = DBLocalDataStore.GetInstance ().GetLocalContacts ();
            localcontacts = FragmentClassifyScreen.CheckActiveContacts(localcontacts);
			var _rootview = inflater.Inflate (Resource.Layout.Activity_identify_screen, container, false);

			listitems = _rootview.FindViewById<ListView> (Resource.Id.listViewApps);
			var linearForTap = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayoutForTap);
			//listitems.Alpha = 0f;
			popUpconract = _rootview.FindViewById<RelativeLayout> (Resource.Id.popRelative);
			// Create your application here
			var buttonBadge = _rootview.FindViewById<RelativeLayout> (Resource.Id.badgeRelativ);
			var buttonManual = _rootview.FindViewById<RelativeLayout> (Resource.Id.manualRelativ);
			var buttonLookup = _rootview.FindViewById<RelativeLayout> (Resource.Id.lookupRelativ);
			var textBadge = _rootview.FindViewById<TextView> (Resource.Id.textBadge);
			var textManual = _rootview.FindViewById<TextView> (Resource.Id.textManual);
			var textLookup = _rootview.FindViewById<TextView> (Resource.Id.textLookup);
			var btnExit = _rootview.FindViewById<ImageView> (Resource.Id.buttonExit);
			var btnSync = _rootview.FindViewById<ImageView> (Resource.Id.btnSync);
			btnShowcontactPopup = _rootview.FindViewById<TextView> (Resource.Id.ShowContacts);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			textBadge.SetTypeface (font, TypefaceStyle.Normal);
			textManual.SetTypeface (font, TypefaceStyle.Normal);
			textLookup.SetTypeface (font, TypefaceStyle.Normal);
			btnShowcontactPopup.SetTypeface (font, TypefaceStyle.Normal);
			linearForTap.Click += (object sender, EventArgs e) => {
				fadeOut = new AlphaAnimation (1f, 0f);
				fadeOut.Duration = 400;
				popUpconract.Visibility = ViewStates.Gone;
				popUpconract.StartAnimation(fadeOut);
			};

			btnShowcontactPopup.Click += (object sender, EventArgs e) => {
                if (localcontacts.Count > 0){
					if (popUpconract.Visibility == ViewStates.Gone)
					{
						fadeIn = new AlphaAnimation (0f, 1f);
						fadeIn.Duration = 400;
						popUpconract.Visibility = ViewStates.Visible;
						popUpconract.StartAnimation(fadeIn);
					}
					else {
						fadeOut = new AlphaAnimation (1f, 0f);
						fadeOut.Duration = 400;
						popUpconract.Visibility = ViewStates.Gone;
						popUpconract.StartAnimation(fadeOut);
					}
				}
			};
			buttonBadge.Click += (sender, e) => {
                if (ActivityCompat.CheckSelfPermission(this.Activity, Manifest.Permission.Camera) == Permission.Granted)
                {
                    StartActivity(new Intent(Activity, typeof(ActivityBadgeScanning)));
                }
                else {
                    int MY_PERMISSIONS_REQUEST_Camera = 101;
                    ActivityCompat.RequestPermissions(this.Activity,
                    new String[] { Manifest.Permission.Camera },
                                                      MY_PERMISSIONS_REQUEST_Camera);
                }

              
			};
			buttonLookup.Click += (object sender, EventArgs e) => {
				StartActivity (new Intent (Activity, typeof(ActivitySelectContact)));
			};

            buttonManual.Click += delegate {
                StartActivity(new Intent(Activity, typeof(ActivityNewContactScreen)));
            };

			ConnectManager.LogHandler logh = (string message) => Activity.RunOnUiThread(() =>{
				AlertDialog.Builder builder = new AlertDialog.Builder (Activity, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetMessage (message);
				ad.SetTitle ("Alert");
				ad.SetButton ("Ok",(s, ev) => {

				});
				ad.Show ();

			});

			btnExit.Click += (object sender, EventArgs e) => 
			{
				AlertDialog.Builder builder = new AlertDialog.Builder (Activity, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetMessage ("Your current work will be lost.");
				ad.SetTitle ("Warning:");
				ad.SetButton ("Cancel",(s, ev) => {

				});
				ad.SetButton2("Ok",(s, ev) => {
					StartActivity (new Intent (Activity, typeof(ActivityHomescreen)));
					SaveAndLoad.GetInstance ().DeleteFile ();
					DBLocalDataStore.GetInstance().ClearAllContactsPopup();
					DBLocalDataStore.GetInstance().ClearAllFilesPopup();
					Activity.Finish();
				});
				ad.Show ();


			};

			ConnectManager.StatusDownload _stat = (success, message) => 
				Activity.RunOnUiThread (() => {
					Toast.MakeText (Activity, "Success", ToastLength.Short).Show ();
				});

			btnSync.Click += (object sender, EventArgs e) => {
				_viewPager.SetCurrentItem(1, true);
				return;
				if (DBLocalDataStore.GetInstance ().GetLocalContactsPopup ().Count == 0){
					Toast.MakeText (Activity, "Please select at least one contact", ToastLength.Short).Show ();
					return;
				}
				try{
					//DBLocalDataStore.GetInstance().ClearAllOverwievContacts();
					//DBLocalDataStore.GetInstance().ClearAllOverwievFiles();
					//return;

					var popupcontacts = DBLocalDataStore.GetInstance().GetLocalContactsPopup();
					var popuplinks = DBLocalDataStore.GetInstance().GetLocalFilesPopup();
					int session = DBLocalDataStore.GetInstance().GetLastSession() + 1;
					var date_now = DateTime.Now;

					var qa = DBLocalDataStore.GetInstance ().GetLocalQuestions (DBLocalDataStore.GetInstance ().GetSelectedQuestionPosition());
					if (qa.Count != 0){
						var aa = SaveAndLoad.GetInstance ().GetAllAnswers ();
						if (!check_required(aa, qa, session, date_now)){
							return;
						}
					}

					var answers = DBLocalDataStore.GetInstance().GetOverwievAnswers(session, "new");

					foreach(var ppc in popupcontacts){
						DBLocalDataStore.GetInstance().AddOverwievContact(new DBOverviewContacts{
							firstName = ppc.firstName,
							lastName = ppc.lastName,
							phone = ppc.phone,
							email = ppc.email,
							barcode = ppc.barcode,
							company = ppc.company,
							session = session,
							status = "new",
							street = ppc.street,
							zip = ppc.zip,
							city = ppc.city,
							country = ppc.country,
							datetime = date_now,
							isfiles = popuplinks.Count == 0 ? false : true,
							isanswers = answers.Count == 0 ? false : true
						});
					}

					foreach(var ppl in popuplinks){
						DBLocalDataStore.GetInstance().AddOverwievFile(new DBOverviewFileTO{
							name = ppl.name,
							fileType = ppl.fileType,
							folderUuid = ppl.folderUuid,
							md5 = ppl.md5,
							mimeType = ppl.mimeType,
							downloadUrl = ppl.downloadUrl,
							session = session,
							status = "new",
							datetime = date_now,
							uuid = ppl.uuid
						});
					}

//					var bs = new BSCustomers ();
//					bs.SetContext((Android.App.Activity)Activity);
//					bs.SetStartStop(false);

					Console.WriteLine("stop for test");
				}catch(Exception ex){
					Console.WriteLine(ex.Message);
					Toast.MakeText (Activity, ex.Message, ToastLength.Short).Show ();
					return;
				}

				StartActivity (new Intent (Activity, typeof(ActivitySelectQuestion)));
				SaveAndLoad.GetInstance ().DeleteFile ();
				DBLocalDataStore.GetInstance().ClearAllContactsPopup();
				DBLocalDataStore.GetInstance().ClearAllFilesPopup();
				Activity.Finish();

			};

			return _rootview;
		}

		bool check_required(List<string> str1, List<DBQuestion> qs, int _session, DateTime date_now)
		{
			//var que = new List<DBQuestion> ();
			//var changeque = new List<DBQuestion> ();
			List<bool> true_answer = new List<bool>();
			List<bool> false_answer = new List<bool>();
			List<DBOverviewQuestionAnswer> _list_answer = new List<DBOverviewQuestionAnswer>();
			List<DBQuestion> que = new List<DBQuestion> ();

			for (int i = 0; i < str1.Count; i++) {
				var s = str1 [i];
				if (!s.ToUpper ().Contains ("select a value".ToUpper ()) && !s.ToUpper ().Contains("_,___")) {
					false_answer.Add (false);
				}

				if (qs [i].required) {
					if (s.ToUpper ().Contains ("select a value".ToUpper ()) || s.ToUpper () == "_,___") {
						true_answer.Add (false);
						que.Add (qs[i]);
					} else {
						que.Add (null);
						true_answer.Add (true);
						_list_answer.Add (new DBOverviewQuestionAnswer{
							question = qs[i].question,
							required = qs[i].required,
							name_question = qs[i].name,
							answer = s,
							type_question = qs[i].type,
							datetime = date_now,
							status = "new",
							session = _session
						});
					}
				} else {
					que.Add (null);
					if (s.ToUpper ().Contains ("select a value".ToUpper ()) || s.ToUpper () == "_,___") {
						Console.WriteLine ("answer is empty and not required");
						//true_answer.Add (false);
					} else {
						//true_answer.Add (true);
						_list_answer.Add (new DBOverviewQuestionAnswer{
							question = qs[i].question,
							required = qs[i].required,
							name_question = qs[i].name,
							answer = s,
							type_question = qs[i].type,
							datetime = date_now,
							status = "new",
							session = _session
						});
					}
				}
			}

			if (false_answer.Count != 0) {
				true_answer = true_answer.Where (o => o == false).ToList();
				if (true_answer.Count != 0) {
					FragmentClassifyScreen __view = (FragmentClassifyScreen)_viewPager.Adapter.InstantiateItem(_viewPager, 1);

					var listQuestions = __view.View.FindViewById<ListView> (Resource.Id.listQuestions);
					listQuestions.Adapter = new QuestionsListAdapter(Activity, qs, que);
					_viewPager.SetCurrentItem(1, true);
					Toast.MakeText(Activity, "You did not complete all mandatory fields in the classify screen. Please correct.", ToastLength.Short).Show();
					return false;
				}

				//return true;
			} else {
				return true;
			}

			foreach (var la in _list_answer) {
				DBLocalDataStore.GetInstance ().AddOverwievAnswer (la);
			}
			return true;
		}

		void click_delete_popup(object sender, EventArgs e)
		{
            localcontacts = FragmentClassifyScreen.CheckActiveContacts(localcontacts);
			int id = 0;
			if (sender is ImageView)
			{
				//paymentId = Convert.ToString(((ImageButton) sender).Tag);
				id = Convert.ToInt32( ((ImageView) sender).Tag);
			}

            localcontacts[id].activeContact = false;
            DBLocalDataStore.GetInstance().UpdateLocalContact(localcontacts[id]);
            localcontacts = FragmentClassifyScreen.CheckActiveContacts(localcontacts);
			listitems.Adapter = new ContactsPopupListAdapter (Activity, localcontacts, click_delete_popup);
			btnShowcontactPopup.Text = localcontacts.Count + "";
			scaleView(btnShowcontactPopup, 1f, 1f, 1f, 1.2f, 0f, 50f);
		}

		public override void OnResume ()
		{
			base.OnResume ();
            localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            localcontacts = FragmentClassifyScreen.CheckActiveContacts(localcontacts);
			if (localcontacts != null) {
				listitems.Adapter = new ContactsPopupListAdapter (Activity, localcontacts, click_delete_popup);
				btnShowcontactPopup.Text = localcontacts.Count + "";
				//scaleView(countContacts, 1f, 1f, 1f, 1.2f, 0f, 50f);
			}
			if (popUpconract.Visibility == ViewStates.Visible) {
				fadeOut = new AlphaAnimation (1f, 0f);
				fadeOut.Duration = 400;
				popUpconract.Visibility = ViewStates.Gone;
				popUpconract.StartAnimation (fadeOut);
			}
		}
		public void scaleView(TextView v,  float startScalex, float endScalex, float startScaley, float endScaley, float pivotX,float pivotY) {
			Animation anim = new ScaleAnimation (
				startScalex, endScalex, 
				startScaley,endScaley,pivotX,pivotY);
			anim.Duration = 400;
			v.StartAnimation(anim);
			anim.AnimationEnd += (sender, e) =>
				{
					v.TextScaleX =1.2f;
					scaleView2(v);
				};
		}
		public void scaleView2(TextView v) {
			Animation anim2 = new ScaleAnimation (
				1f, 1f, 
				1.2f,1f,
				0f,50f);
			anim2.Duration = 400;
			v.StartAnimation(anim2);
			anim2.AnimationEnd += (sender, e) =>
				{
					v.TextScaleX =1f;
				};
		}
	}
}

