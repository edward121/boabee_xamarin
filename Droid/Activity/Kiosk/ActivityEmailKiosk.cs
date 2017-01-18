
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
using Android.Graphics;
using System.Text.RegularExpressions;
using Android.Content.PM;
using BoaBeePCL;
using Android.Text.Method;
using System.Threading;

namespace Leadbox
{
    [Activity(Label = "ActivityEmailKiosk",
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/ActivityTheme",
        WindowSoftInputMode = SoftInput.AdjustResize)]

    public class ActivityEmailKiosk : Activity
    {
        Context context;
        public System.Timers.Timer _timer;
        int _countSeconds;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.input_email_layout);
            //          localcontact = DBLocalDataStore.GetInstance().GetLocalContacts();
            EditText email = FindViewById<EditText>(Resource.Id.editText1);
            var titeltest = FindViewById<TextView>(Resource.Id.textView1);
            var nextbutton = FindViewById<TextView>(Resource.Id.textNext);
            var imageview2 = FindViewById<TextView>(Resource.Id.textCancel);
            var textTitle = FindViewById<TextView>(Resource.Id.textView2);
            var fon = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            var questionFontSize = DBLocalDataStore.GetInstance().GetQuestionFontSize();
            var answerFontSize = DBLocalDataStore.GetInstance().GetAnswerFontSize();
            var questinoFontColor = DBLocalDataStore.GetInstance().GetQuestionFontColor();
            var questinoBackgroundColor = DBLocalDataStore.GetInstance().GetQuestionBackgroundColor();
            var answerFontColor = DBLocalDataStore.GetInstance().GetAnswerFontColor();
            var answerBackgroundColor = DBLocalDataStore.GetInstance().GetAnswerBackgroundColor();
            var settingsKiosk = DBLocalDataStore.GetInstance().GetKioskSettings();
            email.TextChanged += (sender, e) =>
            {
                timer();
            };
            context = this;
            timer();
            if (settingsKiosk == null)
            {
                textTitle.Text = "Welcome!";
            }
            else {
                if (settingsKiosk.kioskTitle == null)
                {
                    textTitle.Text = "Welcome!";
                }
                else {
                    textTitle.Text = settingsKiosk.kioskTitle;
                }
            }

            fon.Visibility = ViewStates.Visible;
            fon.SetBackgroundColor(Color.White);
            email.SetTextColor(Color.Rgb(0xED, 0xCD, 0x00));

            email.SetTypeface(font, TypefaceStyle.Normal);
            titeltest.SetTypeface(font, TypefaceStyle.Normal);
            nextbutton.SetTypeface(font, TypefaceStyle.Normal);
            imageview2.SetTypeface(font, TypefaceStyle.Normal);
            textTitle.SetTypeface(font, TypefaceStyle.Normal);

            if (questinoBackgroundColor == null)
            {
                fon.SetBackgroundColor(Color.Rgb(0xff, 0xff, 0xff));
            }
            else {
                var tempForColors = Color.Rgb((Int32)questinoBackgroundColor.redByte, (Int32)questinoBackgroundColor.greenByte, (Int32)questinoBackgroundColor.blueByte);
                fon.SetBackgroundColor(tempForColors);
            }
            if (questinoFontColor == null)
            {
                titeltest.SetTextColor(Color.Rgb(0x00, 0x00, 0x00));
            }
            else {
                var tempForColors = Color.Rgb((Int32)questinoFontColor.redByte, (Int32)questinoFontColor.greenByte, (Int32)questinoFontColor.blueByte);
                titeltest.SetTextColor(tempForColors);
            }


            if (answerBackgroundColor == null)
            {
                email.SetBackgroundColor(Color.Rgb(0xff, 0xff, 0xff));
            }
            else {
                var tempForColors = Color.Rgb((Int32)answerBackgroundColor.redByte, (Int32)answerBackgroundColor.greenByte, (Int32)answerBackgroundColor.blueByte);
                email.SetBackgroundColor(tempForColors);
            }
            if (answerFontColor == null)
            {
                email.SetTextColor(Color.Rgb(0xED, 0xCD, 0x00));
            }
            else {
                var tempForColors = Color.Rgb((Int32)answerFontColor.redByte, (Int32)answerFontColor.greenByte, (Int32)answerFontColor.blueByte);
                email.SetTextColor(tempForColors);
            }

            if (questionFontSize == null)
            {
                titeltest.TextSize = 18;
            }
            else {
                titeltest.TextSize = questionFontSize.size;
            }
            if (answerFontSize == null)
            {
                email.TextSize = 18;
            }
            else {
                email.TextSize = answerFontSize.size;
            }
            nextbutton.Click += delegate
            {
                bool checkemail = isEmail(email.Text);
                if (checkemail == true)
                {
                    var _activityKioskMain = new Intent(this, typeof(ActivityKioskMain));
                    _activityKioskMain.PutExtra("InfoKioskMain", new[] { email.Text });
                    StartActivity(_activityKioskMain);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Email is incorrect.", ToastLength.Short).Show();
                }
            };
            imageview2.Click += delegate
            {
                StartActivity(new Intent(this, typeof(ActivityTouchKiosk)));
                Finish();
            };

        }
        private bool isEmail(string email)
        {
            var regexPatter = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$";

            if (Regex.Match(email, regexPatter).Success)
                return true;

            return false;
        }
        public override bool DispatchTouchEvent(MotionEvent even)
        {
            if (even.Action == MotionEventActions.Down)
            {
                ((ActivityEmailKiosk)context).timer();
            }
            return base.DispatchTouchEvent(even);
        }

        public void createdialogclose()
        {
            int ActivityNow = 1;
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            FragmentCloseKiosok dialogclose = new FragmentCloseKiosok(this, _timer, ActivityNow);
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
            _countSeconds = 15;
            _timer.Enabled = true;
        }

    }

}

