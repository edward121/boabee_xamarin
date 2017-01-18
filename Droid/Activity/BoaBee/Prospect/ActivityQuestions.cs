
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

namespace Leadbox
{
    [Activity(Label = "ActivityQuestions")]
    public class ActivityQuestions : Activity
    {
        TextView btnShowcontactPopup;
        Animation fadeIn;
        Animation fadeOut;
        RelativeLayout popUpconract;
        public LinearLayout QuestionLinear;
        ListView listitems;
        List<DBlocalContact> localcontacts;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_classify_screen);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            localcontacts = CheckActiveContacts(localcontacts);
            var btnExit = FindViewById<ImageView>(Resource.Id.buttonExit);
            var btnSync = FindViewById<ImageView>(Resource.Id.btnSync);
            var textQuestionsNull = FindViewById<TextView>(Resource.Id.textQuestionIsNull);
            var listQuestions = FindViewById<ListView>(Resource.Id.listQuestions);
            var list = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
            btnShowcontactPopup = FindViewById<TextView>(Resource.Id.ShowContacts);
            popUpconract = FindViewById<RelativeLayout>(Resource.Id.popRelative);
            var linearForTap = FindViewById<LinearLayout>(Resource.Id.linearLayoutForTap);
            textQuestionsNull.Visibility = ViewStates.Gone;
            listitems = FindViewById<ListView>(Resource.Id.listViewApps);
            QuestionLinear = FindViewById<LinearLayout>(Resource.Id.QuestionLinear);

            if (list.Count == 0)
            {
                textQuestionsNull.Text = string.Format("There is no questionnaire avialable. \nGo to the Classification tab in your \nonline dashboard to add questionnaires.");
                textQuestionsNull.SetTypeface(font, TypefaceStyle.Normal);
                textQuestionsNull.Visibility = ViewStates.Visible;
            }
            else {
                var adapter = new QuestionsListAdapter(this, list);
                listQuestions.ItemsCanFocus = true;
                listQuestions.Focusable = false;
                listQuestions.FocusableInTouchMode = false;

                //              ((ScrollView)QuestionLinear.Parent).Touch += (sender, e) => {
                //                  InputMethodManager inputMethodManager = Activity.GetSystemService (Context.InputMethodService) as InputMethodManager;
                //                  inputMethodManager.HideSoftInputFromWindow (((ScrollView)QuestionLinear.Parent).WindowToken, HideSoftInputFlags.None);
                //              };
                //listQuestions.Adapter = adapter;

                for (int i = 0; i < list.Count; i++)
                {
                    QuestionLinear.AddView(adapter.GetView(i, null, QuestionLinear));
                }

                QuestionLinear.Drag += (sender, e) =>
                {
                    Toast.MakeText(this, "Drag", ToastLength.Long).Show();
                };

                QuestionLinear.Touch += (sender, e) =>
                {
                    //Toast.MakeText (Activity, "Touch", ToastLength.Long).Show ();
                    InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
                    inputMethodManager.HideSoftInputFromWindow(QuestionLinear.WindowToken, HideSoftInputFlags.None);
                };

                listQuestions.ScrollStateChanged += (sender, e) =>
                {
                    InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
                    inputMethodManager.HideSoftInputFromWindow(listQuestions.WindowToken, HideSoftInputFlags.None);
                };
            }


            btnShowcontactPopup.SetTypeface(font, TypefaceStyle.Normal);

            linearForTap.Click += (object sender, EventArgs e) =>
            {
                fadeOut = new AlphaAnimation(1f, 0f);
                fadeOut.Duration = 400;
                popUpconract.Visibility = ViewStates.Gone;
                popUpconract.StartAnimation(fadeOut);
            };

            btnExit.Click += (object sender, EventArgs e) =>
            {
                //InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
                //inputMethodManager.HideSoftInputFromWindow(ActivityQuestions.WindowToken, HideSoftInputFlags.None);

                //AlertDialog.Builder builder = new AlertDialog.Builder( Resource.Style.TransparentProgressDialog);
                //AlertDialog ad = builder.Create();
                //ad.SetMessage("Your current work will be lost.");
                //ad.SetTitle("Warning:");
                //ad.SetButton("Cancel", (s, ev) =>
                //{

                //});
                //ad.SetButton2("Ok", (s, ev) =>
                //{
                //    StartActivity(new Intent(this, typeof(ActivityHomescreen)));
                //    SaveAndLoad.GetInstance().DeleteFile();
                //    DBLocalDataStore.GetInstance().ClearAllContactsPopup();
                //    DBLocalDataStore.GetInstance().ClearAllFilesPopup();
                //    this.Finish();
                //});

                //ad.Show();


            };

            btnSync.Click += (object sender, EventArgs e) =>
            {
                InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
                inputMethodManager.HideSoftInputFromWindow(listQuestions.WindowToken, HideSoftInputFlags.None);
                Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);


                return;
               

                try
                {

                    var popupcontacts = DBLocalDataStore.GetInstance().GetLocalContactsPopup();
                    var popuplinks = DBLocalDataStore.GetInstance().GetLocalFilesPopup();
                    int session = DBLocalDataStore.GetInstance().GetLastSession() + 1;
                    var date_now = DateTime.Now;

                    var qa = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
                    if (qa.Count != 0)
                    {
                        var aa = SaveAndLoad.GetInstance().GetAllAnswers();
                        if (!check_required(aa, qa, session, date_now))
                        {
                            return;
                        }
                    }

                    var answers = DBLocalDataStore.GetInstance().GetOverwievAnswers(session, "new");

                    foreach (var ppc in popupcontacts)
                    {
                        DBLocalDataStore.GetInstance().AddOverwievContact(new DBOverviewContacts
                        {
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
                        Console.WriteLine("add");
                    }

                    foreach (var ppl in popuplinks)
                    {
                        DBLocalDataStore.GetInstance().AddOverwievFile(new DBOverviewFileTO
                        {
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
                    Console.WriteLine("stop for test");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                    return;
                }

                StartActivity(new Intent(this, typeof(ActivityHomescreen)));
                SaveAndLoad.GetInstance().DeleteFile();
                DBLocalDataStore.GetInstance().ClearAllContactsPopup();
                DBLocalDataStore.GetInstance().ClearAllFilesPopup();
                this.Finish();

            };

            btnShowcontactPopup.Click += (object sender, EventArgs e) =>
            {
                if (localcontacts.Count > 0)
                {
                    if (popUpconract.Visibility == ViewStates.Gone)
                    {
                        fadeIn = new AlphaAnimation(0f, 1f);
                        fadeIn.Duration = 400;
                        popUpconract.Visibility = ViewStates.Visible;
                        popUpconract.StartAnimation(fadeIn);
                    }
                    else {
                        fadeOut = new AlphaAnimation(1f, 0f);
                        fadeOut.Duration = 400;
                        popUpconract.Visibility = ViewStates.Gone;
                        popUpconract.StartAnimation(fadeOut);
                    }
                }
            };

        }
        bool check_required(List<string> str1, List<DBQuestion> qs, int _session, DateTime date_now)
        {
            List<bool> true_answer = new List<bool>();
            List<bool> false_answer = new List<bool>();
            List<DBOverviewQuestionAnswer> _list_answer = new List<DBOverviewQuestionAnswer>();
            List<DBQuestion> que = new List<DBQuestion>();

            for (int i = 0; i < str1.Count; i++)
            {
                var s = str1[i];
                if (!s.ToUpper().Contains("select a value".ToUpper()) && !s.ToUpper().Contains("_,___"))
                {
                    false_answer.Add(false);
                }

                if (qs[i].required)
                {
                    if (s.ToUpper().Contains("select a value".ToUpper()) || s.ToUpper() == "_,___")
                    {
                        true_answer.Add(false);
                        que.Add(qs[i]);
                    }
                    else {
                        que.Add(null);
                        true_answer.Add(true);
                        _list_answer.Add(new DBOverviewQuestionAnswer
                        {
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
                else {
                    que.Add(null);
                    if (s.ToUpper().Contains("select a value".ToUpper()) || s.ToUpper() == "_,___")
                    {
                        Console.WriteLine("answer is empty and not required");
                        //true_answer.Add (false);
                    }
                    else {
                        //true_answer.Add (true);
                        _list_answer.Add(new DBOverviewQuestionAnswer
                        {
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

            if (false_answer.Count != 0)
            {
                true_answer = true_answer.Where(o => o == false).ToList();
                if (true_answer.Count != 0)
                {
                    var listQuestions = FindViewById<ListView>(Resource.Id.listQuestions);
                    listQuestions.Adapter = new QuestionsListAdapter(this, qs, que);
                    //_viewPager.SetCurrentItem(1, true);
                    Toast.MakeText(this, "You did not complete all mandatory fields in the classify screen. Please correct.", ToastLength.Short).Show();
                    return false;
                }
            }
            else {
                return true;
            }

            foreach (var la in _list_answer)
            {
                DBLocalDataStore.GetInstance().AddOverwievAnswer(la);
            }

            var contacts = DBLocalDataStore.GetInstance().GetOverwievContacts(_session, "");
            foreach (var c in contacts)
            {
                c.isanswers = true;
                DBLocalDataStore.GetInstance().UpdateOverwievContact(c);
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
            listitems.Adapter = new ContactsPopupListAdapter(this, localcontacts, click_delete_popup);
            btnShowcontactPopup.Text = localcontacts.Count + "";

        }

        //public override void OnResume()
        //{
        //    base.OnResume();
        //    localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
        //    localcontacts = CheckActiveContacts(localcontacts);
        //    if (localcontacts != null)
        //    {
        //        listitems.Adapter = new ContactsPopupListAdapter(Activity, localcontacts, click_delete_popup);
        //        btnShowcontactPopup.Text = localcontacts.Count + "";
        //        //scaleView(countContacts, 1f, 1f, 1f, 1.2f, 0f, 50f);
        //    }
        //    if (popUpconract.Visibility == ViewStates.Visible)
        //    {
        //        fadeOut = new AlphaAnimation(1f, 0f);
        //        fadeOut.Duration = 400;
        //        popUpconract.Visibility = ViewStates.Gone;
        //        popUpconract.StartAnimation(fadeOut);
        //    }

        //}
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
