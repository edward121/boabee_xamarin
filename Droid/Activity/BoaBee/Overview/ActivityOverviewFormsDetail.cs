
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
using System.Globalization;
using BoaBeePCL;
using Android.Views.InputMethods;
using BoaBeeLogic;

namespace Leadbox
{
    [Activity(Label = "ActivityOverviewFormsSelected",
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/ActivityThemeS",
        WindowSoftInputMode = SoftInput.AdjustResize)]
    public class ActivityOverviewFormsDetail : Activity
    {
        private List<Answer> list_default_files;
        private List<DBOverviewQuestionAnswer> _list_for_changes;
        private List<string> listOldAnswer = new List<string>();
        public static AnsweredForm forms;
        public static contactData data;
        public static int RequestId;
        public static int syncID;
        private DBlocalContact customer;
        public static bool isSend = false;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var ID = Intent.GetIntExtra("id_form_overview", 0);
            SetContentView(Resource.Layout.OverviewFormLayoutDetail);
            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            customer = data.contact;


            list_default_files = DBLocalDataStore.GetInstance().GetLocalAnswers();
            _list_for_changes = new List<DBOverviewQuestionAnswer>();

            //_list_for_changes = DBLocalDataStore.GetInstance ().GetOverwievAnswers (customer.session, "");

            var textTop = FindViewById<TextView>(Resource.Id.textViewTitle);
            //			var list_answers = FindViewById<ListView>(Resource.Id.listFormsAnswers);
            var buttonCancel = FindViewById<TextView>(Resource.Id.buttonClose);
            var textC = FindViewById<TextView>(Resource.Id.textCustomer);
            var QuestionLinear = FindViewById<LinearLayout>(Resource.Id.listFormsAnswers);
            //			var questionSelect = DBLocalDataStore.GetInstance().GetSelectedQuestionPosition();
            //			list_answers.Adapter = new OverviewAnswersListAdapter (this, _list);
            //add items into listview
            var list = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
            //			SaveAndLoad.tempAnswer.GetRange(0,list.Count);
            for (int i = 0; i < list.Count(); i++)
            {
                DBOverviewQuestionAnswer overview = new DBOverviewQuestionAnswer();
               // overview.datetime = Convert.ToDateTime(forms.startdate);
                overview.name_question = list[i].name;
                overview.type_question = list[i].type;
                overview.type_answer = list[i].type;
                overview.question = list[i].question;
                overview.required = list[i].required;
                overview.answer = "";
                for (int n = 0; n < forms.answers.Count(); n++)
                {
                    overview.answer = forms.answers[n].name == list[i].name ? forms.answers[n].answer : overview.answer;
                }

                _list_for_changes.Add(overview);
                listOldAnswer.Add(overview.answer);

            }


            for (int i = 0; i < list.Count; i++)
            {
                var qs = list[i];
                var ans = _list_for_changes.Find(s => s.question == qs.question);

                SaveAndLoad.tempAnswer.Add(new DBOverviewQuestionAnswer
                {
                    question = qs.question,
                    required = qs.required,
                    name_question = qs.name,
                    answer = ans != null ? ans.answer : "",
                    type_question = qs.type,
                    datetime = _list_for_changes[0].datetime,
                    status = _list_for_changes[0].status,
                    session = _list_for_changes[0].session
                });
            }
            var adapter = new QuestionsListAdapter(this, list, _list_for_changes);
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
                InputMethodManager inputMethodManager = this.GetSystemService(Context.InputMethodService) as InputMethodManager;
                inputMethodManager.HideSoftInputFromWindow(QuestionLinear.WindowToken, HideSoftInputFlags.None);
            };

            //====

            textTop.SetTypeface(font, TypefaceStyle.Normal);
            textC.SetTypeface(font, TypefaceStyle.Normal);

            var full_description = "";
            if (!string.IsNullOrEmpty(customer.uid))
            {
                if (!string.IsNullOrEmpty(customer.company))
                    textC.Text = customer.firstname + " " + customer.lastname + " (" + customer.company + ")";
                else
                    textC.Text = customer.firstname + " " + customer.lastname;
            }
            else {
                textC.Text = customer.uid;
            }

            //check_valid (customer, textC);
            var _status_send = "";
            if (!isSend)
            {
                _status_send = "only stored locally";
            }
            else {
                _status_send = "safely stored in the cloud";
            }
            string[] forstr1 = data.date.Split('T');
            string[] forstr2 = data.date.Split('T');
            forstr2 = forstr2[1].Split('+');
            var str1 = string.Format("{0:yyyy-MM-dd}", forstr1[0]);
            var str2 = string.Format("{0:H:mm:ss}", forstr2[0]);

            full_description = textC.Text + "\n" + forstr1[0] + " " + forstr2[0];
            full_description = full_description + "\n" + _status_send;

            textC.Text = full_description;


            //textC.Text = "";

            buttonCancel.SetTypeface(font, TypefaceStyle.Normal);

            buttonCancel.Click += (object sender, EventArgs e) =>
            {
                bool flag_changes_was = false;
                for (int i = 0; i < _list_for_changes.Count; i++)
                {
                    if (listOldAnswer[i] != _list_for_changes[i].answer)
                    {
                        if (_list_for_changes[i].answer == "select a value" && listOldAnswer[i] == "")
                        {
                        }
                        else { 
                            flag_changes_was = true;
                        }
                    }

                }

                if (!flag_changes_was)
                {
                    Finish();
                    return;
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                    AlertDialog ad = builder.Create();
                    ad.SetMessage("Save changes?");
                    ad.SetTitle("Warning:");
                    ad.SetButton("No", (s, ev) =>
                    {
                        Finish();
                    });
                    ad.SetButton2("Yes", (s, ev) =>
                    {
                        Console.WriteLine("changes answers are saved!");
                        checkAnswers(_list_for_changes);
                    });
                    ad.Show();
                }

                InputMethodManager inputMethodManager = this.GetSystemService(Context.InputMethodService) as InputMethodManager;
                inputMethodManager.HideSoftInputFromWindow(buttonCancel.WindowToken, HideSoftInputFlags.None);
            };

        }

        void checkAnswers(List<DBOverviewQuestionAnswer> contact)
        {
            var oldAnswer = DBLocalDataStore.GetInstance().GetOverwievAnswers(customer.Id, "");
            List<Answer> answers = new List<Answer>();
            for (int i = 0; i < contact.Count; i++)
            {
                Answer answer = new Answer();
                answer.name = contact[i].question;
                answer.answer = contact[i].answer.ToString();
                answer.type = contact[i].type_answer;
                answers.Add(answer);

            }

            OfflineLogic.updateForms(customer.uid,answers, RequestId);
            Finish();
    }
			//oldAnswer.Clear();
			//SaveAndLoad.tempAnswer.Clear();
			//_list_for_changes.Clear();
			//list_default_files.Clear();
//			foreach (var answer in _list_for_changes)
//			{
//				if (answer.answer != "" && answer.answer != "select a value")
//				{
//					answer.status = "update";
//					DBLocalDataStore.GetInstance().AddOverwievAnswer(answer);
//				}
//			}
		

        void check_valid(DBlocalContact contact, TextView _text)
		{
            Console.WriteLine ("Barcod every customer = " + contact.uid);
            if (string.IsNullOrEmpty (contact.lastname) &&
                string.IsNullOrEmpty (contact.firstname) &&
				string.IsNullOrEmpty (contact.company)) {
                if (!string.IsNullOrEmpty (contact.uid)) {
					_text.Text = "badge "+contact.uid;
					Console.WriteLine (contact.uid);
					return;
				}
			}

            if (string.IsNullOrEmpty (contact.lastname) &&
                string.IsNullOrEmpty (contact.firstname)) {
				if (!string.IsNullOrEmpty (contact.uid)) {
					_text.Text = "badge "+contact.uid;
					Console.WriteLine (contact.uid);
					return;
				}
			}

		}

	}
}

//_list_for_changes.ForEach(s=>{
//	list_default_files.ForEach(a=>{
//		if (s.question == a.question)
//		{
//			Console.WriteLine("s = " + s.answer + "\na = " + a.answer);
//			if (s.answer != a.answer)
//			{
//				Console.WriteLine("s.answer != a.answer");
//				flag_changes_was = true;
//			}
//		}
//		else if (s.question != a.question)
//		{
//		}
//	});
//	Console.WriteLine("s = " + s.answer);
//	if (s.answer != "" && s.answer != "select a value")
//	{
//		Console.WriteLine("s.answer != && s.answer ");
//		flag_changes_was = true;
//	}
//});

