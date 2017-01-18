
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
using Android.Views.InputMethods;
using Android.Support.V4.View;
using System.Threading;
using System.Security.AccessControl;
using BoaBeePCL;
using BoaBeeLogic;

namespace Leadbox
{
	public class FragmentClassifyScreen : Android.Support.V4.App.Fragment
	{
		public ViewPager _viewPager { get; set; }

		View _rootview;
		Animation fadeIn;
		Animation fadeOut;
		RelativeLayout popUpconract;
        public LinearLayout QuestionLinear;
		ListView listitems;
        List<DBlocalContact> localcontacts;

		
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		//There is no questionnaire avialable. Go to the Classification tabin your online dashboard to add questionnaires.
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            localcontacts = CheckActiveContacts(localcontacts);
			_rootview = inflater.Inflate (Resource.Layout.Activity_classify_screen , container, false);
            var btnExit = _rootview.FindViewById<TextView> (Resource.Id.cancelButton);
            var btnSync = _rootview.FindViewById<TextView> (Resource.Id.ShowContacts);
			var textQuestionsNull = _rootview.FindViewById<TextView> (Resource.Id.textQuestionIsNull);
			var listQuestions = _rootview.FindViewById<ListView> (Resource.Id.listQuestions);
			var list = DBLocalDataStore.GetInstance ().GetLocalQuestions (DBLocalDataStore.GetInstance ().GetSelectedQuestionPosition());
			popUpconract = _rootview.FindViewById<RelativeLayout> (Resource.Id.popRelative);
			var linearForTap = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayoutForTap);
			textQuestionsNull.Visibility = ViewStates.Gone;
            var titleText = _rootview.FindViewById<TextView>(Resource.Id.textView1);
            var infotext = _rootview.FindViewById<TextView>(Resource.Id.InformationText);
            
			listitems = _rootview.FindViewById<ListView> (Resource.Id.listViewApps);
			QuestionLinear = _rootview.FindViewById<LinearLayout> (Resource.Id.QuestionLinear);
            btnSync.SetTypeface(font, TypefaceStyle.Normal);
            btnExit.SetTypeface(font, TypefaceStyle.Normal);
            titleText.SetTypeface(font, TypefaceStyle.Normal);
            infotext.SetTypeface(font, TypefaceStyle.Normal);
            

			if (list.Count == 0) {
				textQuestionsNull.Text = string.Format ("There is no questionnaire avialable. \nGo to the Classification tab in your \nonline dashboard to add questionnaires.");
				textQuestionsNull.SetTypeface (font, TypefaceStyle.Normal);
				textQuestionsNull.Visibility = ViewStates.Visible;
			} else {
				var adapter = new QuestionsListAdapter(Activity, list);
				listQuestions.ItemsCanFocus = true;
				listQuestions.Focusable = false;
				listQuestions.FocusableInTouchMode = false;

//				((ScrollView)QuestionLinear.Parent).Touch += (sender, e) => {
//					InputMethodManager inputMethodManager = Activity.GetSystemService (Context.InputMethodService) as InputMethodManager;
//					inputMethodManager.HideSoftInputFromWindow (((ScrollView)QuestionLinear.Parent).WindowToken, HideSoftInputFlags.None);
//				};
				//listQuestions.Adapter = adapter;

				for (int i = 0; i < list.Count; i++) {
					QuestionLinear.AddView(adapter.GetView (i, null, QuestionLinear));
				}

				QuestionLinear.Drag += (sender, e) => {
					Toast.MakeText (Activity, "Drag", ToastLength.Long).Show ();
				};

				QuestionLinear.Touch += (sender, e) => {
					//Toast.MakeText (Activity, "Touch", ToastLength.Long).Show ();
					InputMethodManager inputMethodManager = Activity.GetSystemService (Context.InputMethodService) as InputMethodManager;
					inputMethodManager.HideSoftInputFromWindow (QuestionLinear.WindowToken, HideSoftInputFlags.None);
				};

				listQuestions.ScrollStateChanged += (sender, e) => {
					InputMethodManager inputMethodManager = Activity.GetSystemService (Context.InputMethodService) as InputMethodManager;
					inputMethodManager.HideSoftInputFromWindow (listQuestions.WindowToken, HideSoftInputFlags.None);
				};
			}


		

			linearForTap.Click += (object sender, EventArgs e) => {
				fadeOut = new AlphaAnimation (1f, 0f);
				fadeOut.Duration = 400;
				popUpconract.Visibility = ViewStates.Gone;
				popUpconract.StartAnimation(fadeOut);
			};

			btnExit.Click += (object sender, EventArgs e) => 
			{
				InputMethodManager inputMethodManager = Activity.GetSystemService (Context.InputMethodService) as InputMethodManager;
				inputMethodManager.HideSoftInputFromWindow (_rootview.WindowToken, HideSoftInputFlags.None);

				AlertDialog.Builder builder = new AlertDialog.Builder (Activity, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetMessage ("Your current work will be lost.");
				ad.SetTitle ("Warning:");
				ad.SetButton ("Cancel",(s, ev) => {

				});
				ad.SetButton2("Ok",(s, ev) => {
					SaveAndLoad.GetInstance().DeleteFile();
                    OfflineLogic.ClearDataSelected();
					Activity.Finish();
				});

				ad.Show ();
			};

			btnSync.Click += (object sender, EventArgs e) => {
				InputMethodManager inputMethodManager = Activity.GetSystemService (Context.InputMethodService) as InputMethodManager;
				inputMethodManager.HideSoftInputFromWindow (listQuestions.WindowToken, HideSoftInputFlags.None);
				Activity.Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);

                if (DBLocalDataStore.GetInstance().GetAllLocalFiles().Count != 0)
                {
                    ActivityIdentifyClassifyShare.viewPager.Visibility = ViewStates.Gone;
                    ActivityIdentifyClassifyShare.viewPagerShare.Visibility = ViewStates.Visible;
                    var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutInt("ScrennDestroy", 2);
                    prefEditor.Commit();
                }
                else {
                    DBLocalDataStore.GetInstance().resetAnswers();
                    var answer2 = SaveAndLoad.GetInstance().GetAllAnswers();
                    var listques = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
                    for (int i = 0; i < answer2.Count; i++)
                    {
                        DBAnswer answer = new DBAnswer();
                        answer.question = listques[i].question;
                        answer.answer = answer2[i] == "_,___" ? "" : answer2[i];
                        answer.Id = i + 1;
                        DBLocalDataStore.GetInstance().updateAnswer(answer);
                    }
                    try
                    {
                        OfflineLogic.prepareSync();
                        SaveAndLoad.GetInstance().DeleteFile();
                        OfflineLogic.ClearDataSelected();
                        StartActivity(new Intent(Activity, typeof(ActivityHomescreen)));
                        Activity.Finish();
                    }
                    catch (InvalidOperationException ex){
                        Toast.MakeText(Activity, ex.Message , ToastLength.Long).Show();
                        return;
                    }
                }
				return;
			};
            
			return _rootview;
		}

		bool check_required(List<string> str1, List<DBQuestion> qs, int _session, DateTime date_now)
		{
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
					var listQuestions = _rootview.FindViewById<ListView> (Resource.Id.listQuestions);
					listQuestions.Adapter = new QuestionsListAdapter (Activity, qs, que);
					//_viewPager.SetCurrentItem(1, true);
					Toast.MakeText(Activity, "You did not complete all mandatory fields in the classify screen. Please correct.", ToastLength.Short).Show();
					return false;
				}
			} else {
				return true;
			}

			foreach (var la in _list_answer) {
				DBLocalDataStore.GetInstance ().AddOverwievAnswer (la);
			}

			var contacts = DBLocalDataStore.GetInstance().GetOverwievContacts(_session, "");
			foreach (var c in contacts) {
				c.isanswers = true;
				DBLocalDataStore.GetInstance ().UpdateOverwievContact (c);
			}

			//DBLocalDataStore.GetInstance ().AddLocalAnswers (_list_answer);
			return true;
		}

		void click_delete_popup(object sender, EventArgs e)
		{
			localcontacts = CheckActiveContacts(localcontacts);
            int id = 0;
            if (sender is ImageView)
            {
                //paymentId = Convert.ToString(((ImageButton) sender).Tag);
                id = Convert.ToInt32(((ImageView)sender).Tag);
            }

            localcontacts[id].activeContact = false;
            DBLocalDataStore.GetInstance().UpdateLocalContact(localcontacts[id]);
            localcontacts = CheckActiveContacts(localcontacts);
            listitems.Adapter = new ContactsPopupListAdapter(Activity, localcontacts, click_delete_popup);

		}

		public override void OnResume ()
		{
			base.OnResume();
            localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            localcontacts = CheckActiveContacts(localcontacts);
            if (localcontacts != null)
            {
                listitems.Adapter = new ContactsPopupListAdapter(Activity, localcontacts, click_delete_popup);
                //scaleView(countContacts, 1f, 1f, 1f, 1.2f, 0f, 50f);
            }
            if (popUpconract.Visibility == ViewStates.Visible)
            {
                fadeOut = new AlphaAnimation(1f, 0f);
                fadeOut.Duration = 400;
                popUpconract.Visibility = ViewStates.Gone;
                popUpconract.StartAnimation(fadeOut);
            }
		}
        public static List<DBlocalContact> CheckActiveContacts(List<DBlocalContact> localcontact)
        {
            List<DBlocalContact> ActiveContacts = new List<DBlocalContact>();
            for (int i = 0; i < localcontact.Count; i++)
            {
                if (localcontact[i].activeContact == true)
                {
                    ActiveContacts.Add(localcontact[i]);
                }
            }
            return ActiveContacts;
        }
	}
}

