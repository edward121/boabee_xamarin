
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoaBeeLogic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Scandit;
using Scandit.Interfaces;
using System.Threading;
using Android.Graphics;
using System.Text.RegularExpressions;
using BoaBeePCL;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System.Threading.Tasks;
using Android.Views.InputMethods;
using Android.Views.Animations;

namespace Leadbox
{
	[Activity (Label = "ActivityBadgeScanning", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityBadgeScanning : Activity, IScanditSDKListener, ActivityCompat.IOnRequestPermissionsResultCallback
	{
		private ScanditSDKAutoAdjustingBarcodePicker picker;
		public static string appKey = "npj2MAMeXf41wjXjkbrussTn/5lgnOcGB7OqWk6vAQA";
		public bool  flag_multi = false;
		bool flag_errors = false;
		private int MY_PERMISSIONS_REQUEST_CAMERA;
        public System.Timers.Timer _timer;
        bool OneContactScanned = false;
        public static bool fromHomeScreen = false;

		public void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			//base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            try{
			Console.WriteLine("MY_PERMISSIONS_REQUEST_READ_CONTACTS = " + MY_PERMISSIONS_REQUEST_CAMERA);
			switch (requestCode)
			{
				default :
					{
						// If request is cancelled, the result arrays are empty.
						if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
						{
							OPenScanCode();
							// permission was granted, yay! Do the
							// contacts-related task you need to do.

						}
						else
						{

							// permission denied, boo! Disable the
							// functionality that depends on this permission.
						}
						return;
						break;
					}

			}
            }
            catch { Finish(); }
		}

            

		protected override void OnCreate (Bundle bundle)
		{
            try
            {
                var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                var prefEditor = prefs.Edit();
                prefEditor.PutInt("ScrennDestroy", 0);
                prefEditor.Commit();

                flag_multi = false;
                base.OnCreate(bundle);
                if ((int)Build.VERSION.SdkInt < 23)
                {
                    OPenScanCode();
                    return;
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted)
                {

                    // Should we show an explanation?
                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this,
                        Manifest.Permission.Camera))
                    {

                        OPenScanCode();
                        // Show an expanation to the user *asynchronously* -- don't block
                        // this thread waiting for the user's response! After the user
                        // sees the explanation, try again to request the permission.

                    }
                    else {

                        // No explanation needed, we can request the permission.

                        ActivityCompat.RequestPermissions(this,
                        new[] { Manifest.Permission.Camera },
                        MY_PERMISSIONS_REQUEST_CAMERA);

                        // MY_PERMISSIONS_REQUEST_READ_CONTACTS is an
                        // app-defined int constant. The callback method gets the
                        // result of the request.
                    }
                }
                else
                {
                    OPenScanCode();
			}
            }catch{
                Finish();
            }

        }
        Dialog alertDialog;
        TextView CountContacts;
        TextView btnMulti;
        private void OPenScanCode()
        {
            //RequestWindowFeature (WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            var _rootview = LayoutInflater.Inflate(Resource.Layout.Activity_badge_scanning, null, true);
            // Setup the barcode scanner
            picker = new ScanditSDKAutoAdjustingBarcodePicker(this, appKey, ScanditSDK.CameraFacingBack);
            picker.OverlayView.AddListener(this);


            TextView btnExit = _rootview.FindViewById<TextView>(Resource.Id.cancelButton);
            TextView manualButton = _rootview.FindViewById<TextView>(Resource.Id.manualButton);
            TextView lookupButton = _rootview.FindViewById<TextView>(Resource.Id.lookupButton);
            CountContacts = _rootview.FindViewById<TextView>(Resource.Id.countContacts);
            btnMulti = _rootview.FindViewById<TextView>(Resource.Id.MultiButton);
            

            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            btnExit.SetTypeface(font, TypefaceStyle.Normal);
            manualButton.SetTypeface(font, TypefaceStyle.Normal);
            lookupButton.SetTypeface(font, TypefaceStyle.Normal);
            CountContacts.SetTypeface(font, TypefaceStyle.Normal);
            btnMulti.SetTypeface(font, TypefaceStyle.Normal);
            manualButton.Click += delegate
            {
                StartActivity(typeof(ActivityNewContactScreen));
                Finish();
            };
            lookupButton.Click += delegate
            {
                StartActivity(typeof(ActivitySelectContact));
                Finish();
            };
            btnMulti.Click += (sender, e) =>
            {
                if (!OneContactScanned)
                {
                    if (flag_multi)
                    {
                        flag_multi = false;
                        btnMulti.SetBackgroundResource(Resource.Drawable.icon_single);
                    }
                    else {
                        flag_multi = true;
                        btnMulti.SetBackgroundResource(Resource.Drawable.icon_multi);
                    }
                }
                else { 
                     StartActivity(typeof(ActivitySelectedContacts));
                }
            };
            if (DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList().Count > 0)
            {
                flag_multi = true;
                OneContactScanned = true;
                //btnMulti.Background = null;
                btnMulti.SetBackgroundResource(Resource.Drawable.count_contacts);
                
                btnMulti.Text = DBLocalDataStore.GetInstance().GetLocalContacts().Where(l => l.activeContact).ToList().Count.ToString();
            }
            fromHomeScreen = false;
            btnExit.Click += (object sender, EventArgs e) =>
            {
                InputMethodManager inputMethodManager = GetSystemService(Context.InputMethodService) as InputMethodManager;
                inputMethodManager.HideSoftInputFromWindow(_rootview.WindowToken, HideSoftInputFlags.None);

                AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                AlertDialog ad = builder.Create();
                ad.SetMessage("Your current work will be lost.");
                ad.SetTitle("Warning:");
                ad.SetButton("Cancel", (s, ev) =>
                {
                });
                ad.SetButton2("Ok", (s, ev) =>
                {
                    SaveAndLoad.GetInstance().DeleteFile();
                    OfflineLogic.ClearDataSelected();
                    this.Finish();
                });

                ad.Show();
            };
            CountContacts.Click +=delegate {
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
            };
            CountContacts.Visibility = ViewStates.Invisible;
            SetContentView(picker);
            picker.AddView(_rootview, Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
            Console.WriteLine("show");
            picker.StartScanning();
        }

		string[] array_problem_qr = new string[]{"http://", "https://", "ftp://", "www.", "MAILTO:",
			"MATMSG:", "tel:", "TEL:", "SMSTO:", "GEO:", "BEGIN:VEVENT", "WIFI:"};

       

        DBlocalContact contact;
        public async void DidScanBarcode(string barcode, string symbology)
        {
            try
            {
                Console.WriteLine("barcode scanned: {0}, '{1}'", symbology, barcode);

                // Call GC.Collect() before stopping the scanner as the garbage collector for some reason does not 
                // collect objects without references asap but waits for a long time until finally collecting them.
                GC.Collect();
                // stop the camera
                picker.StopScanning();
                await DialogLoading();
                Thread th = new Thread(async () =>
                {
                    contact = await OfflineLogic.didScanBarcode(barcode, symbology, (string message) =>
                {
                    ActivityEditScannedContact.MessageText = message;
                });
                    RunOnUiThread(() =>
                    {
                        contact = ActivityHomescreen.ReturnNull(contact);
                        BoaBeeLogic.OfflineLogic.createOrUpdateContact(contact);
                        alertDialog.Cancel();
                        contact.activeContact = true;
                        var appSetting = DBLocalDataStore.GetInstance().GetAppSettings();
                        if (flag_multi && appSetting.instantContactCheck)
                        {
                            var _activity = new Intent(Application.Context, typeof(ActivityEditScannedContact));
                            _activity.PutExtra("id_customer", contact.uid);
                            _activity.PutExtra("Multi_flag", true);
                            StartActivity(_activity);
                            OneContactScanned = true;
                            btnMulti.SetBackgroundResource(Resource.Drawable.count_contacts);
                            btnMulti.Text = DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList().Count.ToString();
                            picker.StartScanning();
                        }
                        else if (!flag_multi && appSetting.instantContactCheck)
                        {
                            var _activity = new Intent(Application.Context, typeof(ActivityEditScannedContact));
                            _activity.PutExtra("id_customer", contact.uid);
                            StartActivity(_activity);
                            this.Finish();
                        }
                        else if (!flag_multi && !appSetting.instantContactCheck)
                        {
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
                            Finish();

                        }
                        else if (flag_multi && !appSetting.instantContactCheck)
                        {
                            picker.StartScanning();
                            OneContactScanned = true;
                            btnMulti.SetBackgroundResource(Resource.Drawable.count_contacts);
                            btnMulti.Text = DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList().Count.ToString();
                            CountContacts.Visibility = ViewStates.Visible;
                        }
                    });

                });
                th.Start();
            }
            catch { Finish();}
        }

       
        public void DidCancel () {
			Console.WriteLine ("Cancel was pressed.");
			//Finish ();
		}

		public void DidManualSearch (string text) {
			Console.WriteLine ("Search was used.");
		}

		protected override void OnResume () {
            base.OnResume();
            if (DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList().Count > 0)
            {
                CountContacts.Visibility = ViewStates.Visible;
            }
            if (flag_multi && OneContactScanned)
            {
                btnMulti.Text = DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList().Count.ToString();
                btnMulti.SetBackgroundResource(Resource.Drawable.count_contacts);
                
            }
            if (DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList().Count == 0)
            {
                OneContactScanned = false;
                flag_multi = false;
                btnMulti.SetBackgroundResource(Resource.Drawable.icon_single);
                btnMulti.Text = "";
                CountContacts.Visibility = ViewStates.Invisible;
            }

            try
            {
                picker.StartScanning();
            }
            catch { Finish();}
		}

		protected override void OnPause () {
			// Call GC.Collect() before stopping the scanner as the garbage collector for some reason does not 
			// collect objects without references asap but waits for a long time until finally collecting them.
			GC.Collect ();
			//picker.StopScanning ();
			base.OnPause ();
		}

		public override void OnBackPressed () {
			//base.OnBackPressed ();
			//Finish ();
		}

		void IDisposable.Dispose ()
		{
			//throw new NotImplementedException ();
		}


		string substr(string tt)
		{
			//var t = tt.Substring (tt.IndexOf("\n"));
			//Console.WriteLine ("substr = " + t);

			var tv = tt.Remove (tt.IndexOf("\n"));
			Console.WriteLine ("substr tv = " + tv);
			return tv;
		}




        async Task popupDialog(string alertMassege)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
            AlertDialog ad = builder.Create();
            ad.SetMessage(alertMassege);
            ad.SetTitle("Alert:");
            ad.SetButton("Ok", (s, ev) =>
            {
                alertDialog.Dismiss();
                Finish();
            });
            ad.Show();
        }


		private bool isEmail(string email)
		{
			var regexPatter = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$";

			if(Regex.Match(email,regexPatter).Success)
				return true;

			return false;
		}
        public async Task DialogLoading()
        {
            alertDialog = new Dialog(this, Resource.Style.TransparentProgressDialog);
            alertDialog.SetCancelable(false);
            alertDialog.SetCanceledOnTouchOutside(false);
            alertDialog.SetContentView(Resource.Layout.custom_progressdialog); //SetView (progressLoading);
            alertDialog.Show();
        }
        AlertDialog.Builder alert;

        public void scaleView(TextView v, float startScalex, float endScalex, float startScaley, float endScaley, float pivotX, float pivotY)
        {
            Animation anim = new ScaleAnimation(
                startScalex, endScalex,
                startScaley, endScaley, pivotX, pivotY);
            anim.Duration = 400;
            v.StartAnimation(anim);
            anim.AnimationEnd += (sender, e) =>
            {
                v.TextScaleX = 1.1f;
                scaleView2(v);
            };
        }
        public void scaleView2(TextView v)
        {
            Animation anim2 = new ScaleAnimation(
                1f, 1f,
                1.1f, 1f,
                0f, 50f);
            anim2.Duration = 400;
            v.StartAnimation(anim2);
            anim2.AnimationEnd += (sender, e) =>
                {
                    v.TextScaleX = 1f;
                };
        }
       

    }
}

