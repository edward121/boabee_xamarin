
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
using Android.Views.InputMethods;
using Android.Graphics;
using System.Text.RegularExpressions;
using Android.Text.Method;
using BoaBeePCL;
using BoaBeeLogic;

namespace Leadbox
{
    [Activity(Label = "ActivityKioskMain", ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/ActivityThemeS",
        WindowSoftInputMode = SoftInput.AdjustResize)]
    public class ActivityKioskMain : Activity, ViewTreeObserver.IOnScrollChangedListener
    {
        public static List<QuestionAndAnswer> listQuestionAndAnswer;

        public static QuestionAndAnswer tempQuestionAndAnswer = new QuestionAndAnswer();
        int idcontact;
        int _countSeconds;
        public string name;
        bool check = false;
        public string _email;
        public string nameQuestion;
        public System.Timers.Timer _timer;
        public bool checkContacOntWrite = false;
        private List<DBOverviewQuestionAnswer> _list_for_changes;
        public List<DBOverviewQuestionAnswer> oldQuestionAnswer;
        //List<DBOverviewContacts> overviewContacts ;
        public DBOverviewContacts customer;
        List<DBlocalContact> locaLcontact;
        DBlocalContact localcontacts;
        TextView previous;
        TextView finish;
        DBContactToServer OldContactToServer;
        Context context;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.KioskMain);
            idcontact = 0;
            string[] strMass = Intent.GetStringArrayExtra("InfoKioskMain");
            _email = strMass[0];
            try
            {
                DBlocalContact oldlocalcontact = DBLocalDataStore.GetInstance().GetLocalContacts().Find(s => s.uid == _email);
                if (oldlocalcontact != null)
                {
                    OldContactToServer = new DBContactToServer(oldlocalcontact);
                }
            }
            catch { }
            ActivityFinishKiosk.uid = _email;
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

            var _localcontact = DBLocalDataStore.GetInstance().GetLocalContacts();
            var list = DBLocalDataStore.GetInstance().GetLocalQuestions((DBLocalDataStore.GetInstance().GetSelectedQuestionPosition()));
            var questionFontSize = DBLocalDataStore.GetInstance().GetQuestionFontSize();


            var checkAnswer = DBLocalDataStore.GetInstance().GetOverwievAnswers(-1,"server");

            var answerFontSize = DBLocalDataStore.GetInstance().GetAnswerFontSize();
            var questinoFontColor = DBLocalDataStore.GetInstance().GetQuestionFontColor();
            var questinoBackgroundColor = DBLocalDataStore.GetInstance().GetQuestionBackgroundColor();
            var answerFontColor = DBLocalDataStore.GetInstance().GetAnswerFontColor();
            var answerBackgroundColor = DBLocalDataStore.GetInstance().GetAnswerBackgroundColor();
            var settingsKiosk = DBLocalDataStore.GetInstance().GetKioskSettings();
            RelativeLayout relativeLayout = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            LinearLayout questionLinear = FindViewById<LinearLayout>(Resource.Id.QuestionLinear);
            previous = FindViewById<TextView>(Resource.Id.textCancel);
            finish = FindViewById<TextView>(Resource.Id.textNext);
            var textTitle = FindViewById<TextView>(Resource.Id.textView1);
            ScrollView scroll = FindViewById<ScrollView>(Resource.Id.scrollView1);
            var fon = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            textTitle.SetTypeface(font, TypefaceStyle.Normal);
            finish.SetTypeface(font, TypefaceStyle.Normal);
            previous.SetTypeface(font, TypefaceStyle.Normal);
            context = this;
            timer();
            relativeLayout.Touch += delegate
            {
                timer();
            };
            fon.Visibility = ViewStates.Visible;

            if (settingsKiosk == null)
            {
                textTitle.Text = "Welcome!";
            }
            else {
                if (settingsKiosk.kioskTitle == null || settingsKiosk.badgePrinting == false)
                {
                    textTitle.Text = "Welcome!";
                }
                else {
                    textTitle.Text = settingsKiosk.kioskTitle;
                }
            }
            if (questinoBackgroundColor == null)
            {
                fon.SetBackgroundColor(Color.White);
            }
            else {
                var tempForColors = Color.Rgb((Int32)questinoBackgroundColor.redByte, (Int32)questinoBackgroundColor.greenByte, (Int32)questinoBackgroundColor.blueByte);
                //  colorPickerQuestionBackgroundColor.Color = tempForColors;

                fon.SetBackgroundColor(tempForColors);
            }

            previous.Visibility = ViewStates.Gone;
            finish.Visibility = ViewStates.Gone;
          
            scroll.ViewTreeObserver.AddOnScrollChangedListener(this);
            foreach (var item in _localcontact)
            {
                if (item.email == _email)
                {
                    check = true;
                    idcontact = item.Id;
                    localcontacts = DBLocalDataStore.GetInstance().GetLocalContactsById(idcontact);
                }
            }
            if (localcontacts==null) 
            {
                localcontacts = new DBlocalContact();
            }
            listQuestionAndAnswer = new List<QuestionAndAnswer>();
            locaLcontact = new List<DBlocalContact>();
            locaLcontact.Add(localcontacts);
            foreach (var item in list)
            {
                QuestionAndAnswer _questionAndAnswer = new QuestionAndAnswer();
                _questionAndAnswer.question = item.question;
                _questionAndAnswer.answer = "";
                _questionAndAnswer.type = item.type;
                _questionAndAnswer.options = item.options;
                _questionAndAnswer.nameQuestion = item.name;
                _questionAndAnswer.maxLength = item.maxLength;
                listQuestionAndAnswer.Add(_questionAndAnswer);
            }

            // заполнение данных о контакте в статическую переменную 
              foreach (var items in listQuestionAndAnswer)
                {
                checkIfThere(items.nameQuestion,items);
                    if (items.nameQuestion.ToLower() == nameQuestion.ToLower())
                    {
                        items.answer = name;
                    }
                    //else
                    //{
                    //    var check = listQuestionAndAnswer.Any(s => s.question.ToLower() == nameQuestion.ToLower());
                    //    if (check == false)
                    //    {
                    //        QuestionAndAnswer _questionAndAnswer = new QuestionAndAnswer();
                    //        _questionAndAnswer.question = nameQuestion;
                    //        _questionAndAnswer.answer = name;
                    //        _questionAndAnswer.type = "string";
                    //        _questionAndAnswer.maxLength = 255;
                    //    }

                    //}
                    name = "";
                    nameQuestion = "";
                }
            //if (localcontacts == null)
            //{
            //    SaveAndLoad.tempContact = new DBlocalContact();
            //}
            //else
            //{
            //    SaveAndLoad.tempContact = new DBlocalContact();
            //    SaveAndLoad.tempContact.firstName = localcontacts.firstName;
            //    SaveAndLoad.tempContact.lastName = localcontacts.lastName;
            //    SaveAndLoad.tempContact.email = localcontacts.email;
            //    SaveAndLoad.tempContact.company = localcontacts.company;
            //    SaveAndLoad.tempContact.phone = localcontacts.phone;
            //    SaveAndLoad.tempContact.source = localcontacts.source;
            //    SaveAndLoad.tempContact.barcode = localcontacts.barcode;
            //    SaveAndLoad.tempContact.jobTitle = localcontacts.jobTitle;
            //    SaveAndLoad.tempContact.dateTime = localcontacts.dateTime;


            //}

            // отрисовка адаптера
            var adapter = new QuestionsListAdapterForKiosk(this, listQuestionAndAnswer, localcontacts, questionFontSize, answerFontSize,
                                                   questinoFontColor, questinoBackgroundColor,
                                                   answerFontColor, answerBackgroundColor);
            for (int i = 0; i < list.Count; i++)
            {
                questionLinear.AddView(adapter.GetView(i, null, questionLinear));
            }
            questionLinear.Click += delegate
            {
                timer();
            };


            previous.Click += (object sender, EventArgs e) =>
                {
                    StartActivity(typeof(ActivityEmailKiosk));
                    Finish();
                };
            finish.Click += (object sender, EventArgs e) =>
            {
                foreach (var item in listQuestionAndAnswer)
                {
                    checkOnWriteContacs(item.nameQuestion,item.answer,item );
                }
                localcontacts.email = _email;
                localcontacts.uid = _email;
                localcontacts.activeContact = true;
                var oldAnswer = DBLocalDataStore.GetInstance().GetOverwievAnswers(-1, "");
                if (!check)
                    {
                    if (ValidateForm(_email, SaveAndLoad.tempContact.firstname, SaveAndLoad.tempContact.lastname))
                        {
                       
                            DBLocalDataStore.GetInstance().AddLocalContact(localcontacts);
                            DBContactToServer contacttoServer = new DBContactToServer(localcontacts,true);
                            DBLocalDataStore.GetInstance().AddContactToServer(contacttoServer);
                            List<DBAnswer> listanswers = new List<DBAnswer>();
                            DBLocalDataStore.GetInstance().resetAnswers();
                            for (int i = 0; i < listQuestionAndAnswer.Count; i++)
                            {
                                DBAnswer answer = new DBAnswer();
                                answer.question = listQuestionAndAnswer[i].question;
                                answer.answer = listQuestionAndAnswer[i].answer;
                                answer.Id = i + 1;
                                listanswers.Add(answer);
                                DBLocalDataStore.GetInstance().updateAnswer(listanswers[i]);
                            }
                            activeFiles();
                            listQuestionAndAnswer.Clear();
                            SaveAndLoad.tempAnswer.Clear();
                            StartActivity(typeof(ActivityFinishKiosk));
                            Finish();
                        }
                        else
                        {
                            Console.WriteLine("Invalid create new customer!");
                            Toast.MakeText(this, "E-mail or Last Name or First Name incorrect.", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                    if (ValidateForm(_email, SaveAndLoad.tempContact.firstname, SaveAndLoad.tempContact.lastname))
                    {
                        localcontacts.activeContact = true;
                        DBLocalDataStore.GetInstance().UpdateLocalContact(localcontacts);
                        OldContactToServer -= localcontacts;
                        DBLocalDataStore.GetInstance().AddContactToServer(OldContactToServer);
                        List<DBAnswer> listanswers = new List<DBAnswer>();
                        DBLocalDataStore.GetInstance().resetAnswers();
                        for (int i = 0; i < listQuestionAndAnswer.Count; i++)
                        {
                            DBAnswer answer = new DBAnswer();
                            answer.question = listQuestionAndAnswer[i].question;
                            answer.answer = listQuestionAndAnswer[i].answer;
                            answer.Id = i + 1;
                            listanswers.Add(answer);
                            DBLocalDataStore.GetInstance().updateAnswer(listanswers[i]);
                        }
                        activeFiles();
                        listQuestionAndAnswer.Clear();
                        SaveAndLoad.tempAnswer.Clear();
                        StartActivity(typeof(ActivityFinishKiosk));
                        Finish();
                        //foreach (var new_answer in SaveAndLoad.tempAnswer)
                        //{
                        //    //new_answer.status = "update";
                        //    //DBLocalDataStore.GetInstance().AddOverwievAnswer(new_answer);

                        //int count = 0;
                        //foreach (var old_answers in oldAnswer)
                        //{
                        //    if (old_answers.question == new_answer.question)
                        //    {
                        //        count++;
                        //        if (old_answers.answer != new_answer.answer)
                        //        {
                        //            old_answers.answer = new_answer.answer;
                        //            old_answers.status = "update";//update
                        //            DBLocalDataStore.GetInstance().UpdateOverwievAnswer(old_answers);
                        //        }
                        //        //else {
                        //        //  old_answers.answer = new_answer.answer;
                        //        //      old_answers.status = "new";//update
                        //        //      DBLocalDataStore.GetInstance ().UpdateOverwievAnswer (old_answers);
                        //        //  }
                        //        new_answer.status = "update";//update
                        //    }

                        //}

                        //if (!string.IsNullOrEmpty(new_answer.answer) && new_answer.answer != "select a value" && new_answer.status != "update")
                        //{
                        //    new_answer.status = "update";//update
                        //    DBLocalDataStore.GetInstance().AddOverwievAnswer(new_answer);
                        //}
                        //}
                    }
                    else
                    {
                        Console.WriteLine("Invalid create new customer!");
                        Toast.MakeText(this, "E-mail or Last Name or First Name incorrect.", ToastLength.Short).Show();
                    }
                    var _list_answ = DBLocalDataStore.GetInstance().GetOverwievAnswers(-1, "update");
                    //oldQuestionAnswer.Clear();
                        listQuestionAndAnswer.Clear();
                        SaveAndLoad.tempAnswer.Clear();

                        StartActivity(typeof(ActivityFinishKiosk));
                        Finish();
                    }

                //                  var localcontactss = DBLocalDataStore.GetInstance().GetLocalContacts();
                //                  foreach(var item in localcontactss)
                //                  {
                //                      if (item.email == _email)_timer
                //                      {
                //                          check = true;
                //                          emailContact =item.uuid;
                //                      }
                //
                //                  }
                if (_list_for_changes == null)
                    {
                    }
                    else
                    {

                        _list_for_changes.Clear();
                    }


                };
        }
        public void OnScrollChanged()
        {
            ScrollView MainLayout_ScrollView1 =
                ((ScrollView)FindViewById(Resource.Id.scrollView1));
            double scrollingSpace =
                MainLayout_ScrollView1.GetChildAt(0).Height - MainLayout_ScrollView1.Height;

            if (scrollingSpace <= MainLayout_ScrollView1.ScrollY) // Touched bottom
            {

                previous.Visibility = ViewStates.Visible;
                finish.Visibility = ViewStates.Visible;

            }
            else
            {
                previous.Visibility = ViewStates.Gone;
                finish.Visibility = ViewStates.Gone;    //Do the load more like things
            }
        }
        private bool ValidateForm(string user, string firstname, string lastname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
            {
                Toast.MakeText(this, "First name is missing.", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrWhiteSpace(lastname))
            {
                Toast.MakeText(this, "Last name is missing.", ToastLength.Short).Show();
                return false;
            }

            if (string.IsNullOrWhiteSpace(user))
            {
                Toast.MakeText(this, "Email is missing.", ToastLength.Short).Show();
                return false;
            }

            if (!isEmail(user))
            {
                Toast.MakeText(this, "Email is incorrect.", ToastLength.Short).Show();
                return false;
            }



            return true;

        }
        public void activeFiles()
        {
            var listfiles = DBLocalDataStore.GetInstance().GetAllLocalFiles();
            if (listfiles.Count != 0)
            {
                for (int i = 0; i < listfiles.Count; i++)
                {
                    if (listfiles[i].isDefault)
                    {
                        listfiles[i].activeFile = true;
                        DBLocalDataStore.GetInstance().UpdateLocalFile(listfiles[i]);
                    }
                }
            }
        
        }
     
        public void checkIfThere(string question,QuestionAndAnswer questionAndAnswer)
        {
            switch (question.ToLower())
            {
                case "firstname":
                    name = localcontacts.firstname;
                    nameQuestion = "firstName";

                    break;
                case "lastname":
                    name = localcontacts.lastname;
                    nameQuestion = "lastName";
                    //SaveAndLoad.tempContact.lastName = name;
                    break;
                case "email":
                    name = localcontacts.email;
                    nameQuestion = "email";
                    //SaveAndLoad.tempContact.email = name;
                    break;
                case "company":
                    name = localcontacts.company;
                    nameQuestion = "company";
                    //SaveAndLoad.tempContact.company = name;
                    break;
                case "mobile":
                    if (questionAndAnswer.question == "Cellphone")
                    {
                        name = localcontacts.phone;
                        nameQuestion = "mobile";
                    }
                    else if (questionAndAnswer.question == "mobile") 
                    {
                        name = localcontacts.mobile;
                        nameQuestion = "phone";
                    }
                    break;
                ///////////////////////////////////////////WARNING BADGE/////////////////////////////////////////////////////////////////       
                case "barcode":
                    name = localcontacts.uid;
                    nameQuestion = "barcode";
                    //SaveAndLoad.tempContact.barcode = name;
                    break;
                case "jobTitle":
                    name = localcontacts.jobtitle;
                    nameQuestion = "jobTitle";
                    //SaveAndLoad.tempContact.jobTitle = name;
                    break;
                case "externalReference":
                    name = localcontacts.externalReference;
                    nameQuestion = "externalReference";
                    //SaveAndLoad.tempContact.externalReference = name;
                    break;
                case "externalCompanyReference":
                    name = localcontacts.externalCompanyReference;
                    nameQuestion = "externalCompanyReference";
                    //SaveAndLoad.tempContact.externalCompanyReference = name;
                    break;
                case "prefix":
                    name = localcontacts.prefix;
                    nameQuestion = "prefix";
                    //SaveAndLoad.tempContact.prefix = name;
                    break;
               
                case "fax":
                    name = localcontacts.fax;
                    nameQuestion = "fax";
                    //SaveAndLoad.tempContact.fax = name;
                    break;
                case "vat":
                    name = localcontacts.vat;
                    nameQuestion = "vat";
                    //SaveAndLoad.tempContact.vat = name;
                    break;
                case "function":
                    name = localcontacts.function;
                    nameQuestion = "function";
                    //SaveAndLoad.tempContact.function = name;
                    break;
                case "level":
                    name = localcontacts.level;
                    nameQuestion = "level";
                    //SaveAndLoad.tempContact.level = name;
                    break;
                case "department":
                    name = localcontacts.department;
                    nameQuestion = "department";
                    //SaveAndLoad.tempContact.department = name;
                    break;
                case "street":
                    name = localcontacts.street;
                    nameQuestion = "street";
                    //SaveAndLoad.tempContact.street = name;
                    break;
                case "city":
                    name = localcontacts.city;
                    nameQuestion = "city";
                    //SaveAndLoad.tempContact.city = name;
                    break;
                case "zip":
                    name = localcontacts.zip;
                    nameQuestion = "zip";
                    //SaveAndLoad.tempContact.zip = name;
                    break;
                case "country":
                    name = localcontacts.country;
                    nameQuestion = "country";
                    //SaveAndLoad.tempContact.country = name;
                    break;

                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }
        public void checkOnWriteContacs(string question,string answer, QuestionAndAnswer questionAndAnswer)
        {
            switch (question.ToLower())
            {
                case "firstname":
                    localcontacts.firstname = answer;
                    break;
                case "lastname":
                    localcontacts.lastname = answer;
                    break;
                case "email":
                    localcontacts.email = answer;
                    break;
                case "company":
                    localcontacts.company = answer;
                    break;
                case "mobile":
                    if (questionAndAnswer.question == "Cellphone")
                    {
                        localcontacts.phone = answer;
                    }
                    else if (questionAndAnswer.question == "mobile")
                    {
                        localcontacts.mobile = answer;
                    }

                    break;
                ///////////////////////////////////////////WARNING BADGE/////////////////////////////////////////////////////////////////       
                case "barcode":
                    localcontacts.uid = answer;
                    break;
                case "jobTitle":
                    localcontacts.jobtitle = answer;
                    break;
                case "externalReference":
                    localcontacts.externalReference = answer;
                    break;
                case "externalCompanyReference":
                    localcontacts.externalCompanyReference = answer;
                    break;
                case "prefix":
                    localcontacts.prefix = answer;
                    break;
                //case "mobile":
                //    localcontacts.mobile = answer;
                //    break;
                case "fax":
                    localcontacts.fax = answer;
                    break;
                case "vat":
                    localcontacts.vat = answer;
                    break;
                case "function":
                    localcontacts.function = answer;
                    break;
                case "level":
                    localcontacts.level = answer;
                    break;
                case "department":
                    localcontacts.department = answer;
                    break;
                case "street":
                    localcontacts.street = answer;
                    break;
                case "city":
                    localcontacts.city = answer;
                    break;
                case "zip":
                    localcontacts.zip = answer;
                    break;
                case "country":
                    localcontacts.country = answer;
                    break;

                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }
        private bool isEmail(string email)
        {
            var regexPatter = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$";

            if (Regex.Match(email, regexPatter).Success)
                return true;

            return false;
        }
        //create timer and dialog for close
        public void createdialogclose()
        {
            int ActivityNow = 2;
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            FragmentCloseKiosok dialogclose = new FragmentCloseKiosok(context, _timer, ActivityNow);
            dialogclose.Show(transaction, "dialog close");
        }
        public void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            _countSeconds--;
            if (_countSeconds == 0)
            {
                createdialogclose();
            }
        }
        public void timer()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Close();
            }
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimedEvent;
            _countSeconds = 10;
            _timer.Enabled = true;
        }
        public override bool DispatchTouchEvent(MotionEvent even)
        {
            if (even.Action == MotionEventActions.Down)
            {
                ((ActivityKioskMain)context).timer();
            }
            return base.DispatchTouchEvent(even);
        }
        //end timer
    }
    public class QuestionAndAnswer
    {
        public string question;
        public string answer;
        public string type;
        public string options;
        public string nameQuestion;
        public int? maxLength;

    }
}

