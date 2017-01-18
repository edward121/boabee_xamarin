
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using BoaBeePCL;
using Android.Text;
using System.Runtime.InteropServices;
using BoaBeeLogic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Android.Support.V4.App;
using Android;
using Android.Views.Animations;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Preferences;

namespace Leadbox
{
	[Activity (Label = "ActivityHomescreen", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityHomescreen : Activity
	{
		ConnectManager.NetworkState _state;
		BSCustomers bs = new BSCustomers ();
		BSAnswersUpdate answers_update = new BSAnswersUpdate();
		Dialog alertDialog;
        ImageView btnSettings;
        public bool animationStart = false;
        public System.Timers.Timer timer = new System.Timers.Timer();
        public  Drawable iconRed;


		protected override void OnResume ()
		{
			base.OnResume ();
			try {
				_state = ConnectManager.GetInstance ().stateNet ();
				Console.WriteLine ("state net = " + _state.ToString ());
				var btnSettings = FindViewById<ImageView> (Resource.Id.btnsettings);
				var list_share = DBLocalDataStore.GetInstance ().GetOverwievFiles (-1, "new").Count;
				var list_answers = DBLocalDataStore.GetInstance ().GetOverwievAnswers (-1, "new").Count;
				var list_contacts = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "new").Count;
				var list_answers_update = DBLocalDataStore.GetInstance ().GetOverwievAnswers (-1, "update").Count;
				//if ((list_share + list_answers + list_contacts) == 0) {
				//	btnSettings.SetImageResource (Resource.Drawable.settingsIP);
				//} else {
				//	btnSettings.SetImageResource (Resource.Drawable.red_cloud_button);
				//	if (!bs.IsAlive) {
				//		bs.SetContext (this);
				//		bs.SetStopped (false);
				//		bs.SetStatusLog (SendCompleted);
				//		bs.Start ();
				//	}
				//}

				//if ((list_share + list_answers + list_contacts + list_answers_update) == 0) {
				//	btnSettings.SetImageResource (Resource.Drawable.settingsIP);
				//} else {
				//	if (list_answers_update != 0) {
				//		btnSettings.SetImageResource (Resource.Drawable.settingsIP);
				//		if (!answers_update.IsAlive) {
				//			answers_update.SetContext (this);
				//			answers_update.SetStopped (false);
				//			answers_update.SetStatusLog (SendCompleted);
				//			answers_update.Start ();
				//		}
				//	}
				//}
			} catch 
			{
			}
            if (btnSettings.Background == iconRed)
                animationStart = true;
            else
                animationStart = false;
            _state = ConnectManager.GetInstance().stateNet();
             try
                {
                    var syncContext = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList(); ;
                    if (syncContext.Count > 0)
                    {
                        var btnSettings = FindViewById<ImageView>(Resource.Id.btnsettings);
                        btnSettings.SetImageResource(Resource.Drawable.settings_button);
                        animationStart = true;
                        //timer = new System.Timers.Timer();
                        timer.Interval = 2500;
                        timer.Elapsed += delegate
                            {
                                if (animationStart)
                                {
                                    RunOnUiThread(() =>
                                    {
                                        scaleImage(btnSettings, 1f, 1.2f, 1f, 1.2f, 50f, 50f);
                                    });
                                }
                                else {
                                    timer.Stop();
                                }
                            };
                    timer.Start();
                    if (_state == ConnectManager.NetworkState.ConnectedWifi || _state == ConnectManager.NetworkState.ConnectedData)
                    {
                        Task.Run(async () =>
                        {
                            int _countSeconds = 60;
                        System.Timers.Timer _timer;
                        _timer = new System.Timers.Timer();
                        _timer.Interval = 1000;
                        _timer.Elapsed += delegate
                            {
                                _countSeconds--;
                        };
                            _timer.Start();
                                #warning to send data to server remove 'true'!!!!
                                await NetworkRequests.SyncDataServer();
                            RunOnUiThread(() =>
                            {
                                if (!DBLocalDataStore.GetInstance().GetLocalUserInfo().invalidPassword)
                                    btnSettings.SetImageResource(Resource.Drawable.settingsIP);
                                    scaleImage(btnSettings, 1f, 1.2f, 1f, 1.2f, 50f, 50f);
                                    _timer.Stop();
                                    animationStart = false;
                            });
                        });
                    }
                    else {
                        System.Threading.Thread th = new System.Threading.Thread(() =>
                        {
                            

                                          
                            int _countSeconds = 60;
                            System.Timers.Timer _timer;
                            _timer = new System.Timers.Timer();
                            _timer.Interval = 1000;
                            _timer.Elapsed += delegate {
                                _countSeconds--;
                                if (_countSeconds < 5)
                                {
                                    _timer.Stop();
                                    _state = ConnectManager.GetInstance().stateNet();
                                    if (_state == ConnectManager.NetworkState.ConnectedWifi || _state == ConnectManager.NetworkState.ConnectedData)
                                    {
                                        Task.Run(async () =>
                                        {
                                            #warning to send data to server remove 'true'!!!!
                                            await NetworkRequests.SyncDataServer();
                                            RunOnUiThread(() =>
                                            {
                                                    if(!DBLocalDataStore.GetInstance().GetLocalUserInfo().invalidPassword)
                                                    btnSettings.SetImageResource(Resource.Drawable.settingsIP);
                                                    scaleImage(btnSettings, 1f, 1.2f, 1f, 1.2f, 50f, 50f);
                                                    animationStart = false;
                                            });
                                            _timer.Dispose();
                                        });
                                    }
                                    else {
                                        _countSeconds = 60;
                                        _timer.Start();
                                    }
                                }

                            };
                            _timer.Enabled = true;
                        });
                        th.Start();
                    
                    }
                        
                        
                    }
                }
                catch { 
                
                }
           
            var countcontacts = FindViewById<TextView>(Resource.Id.countcontacts);
            var countemails = FindViewById<TextView>(Resource.Id.countemails);
            var countsheets = FindViewById<TextView>(Resource.Id.countsheets);

            var counts = DBLocalDataStore.GetInstance().GetCountHomeScreen();

            countcontacts.Text = counts.countContacts.ToString();
            countemails.Text = counts.countShare.ToString();
            countsheets.Text = counts.countQuestion.ToString();

            if (DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList().Count > 0)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                AlertDialog ad = builder.Create();
                ad.SetMessage("You didn’t complete last work. Continue?");
                ad.SetTitle("Warning:");
                ad.SetButton("Continue existing.", (s, ev) =>
                {
                    ActivityIdentifyClassifyShare.afterCrash = true;
                    var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                    var somePref = prefs.GetInt("ScrennDestroy", 0);
                    if (somePref != 0)
                    {
                        ActivityIdentifyClassifyShare.afterCrash = true;
                        StartActivity(typeof(ActivityIdentifyClassifyShare));
                    }
                    else { 
                        StartActivity(typeof(ActivityBadgeScanning));
                    }
                });
                ad.Show();
            }
        }
        public static List<DBlocalContact> contactForOvervie(List<DBlocalContact> listcontact)
        {
            List<DBlocalContact> contacts = new List<DBlocalContact>();
            bool finded;
            for (int i = 0; i < listcontact.Count; i++)
            {
                finded = false;
                for (int n = i + 1; n < listcontact.Count; n++)
                {
                    if (listcontact[i].uid == listcontact[n].uid)
                    {
                        finded = true;
                    }
                }
                if (!finded)
                {
                    contacts.Add(listcontact[i]);
                }
            }
            return contacts;
        }
		protected override void OnCreate (Bundle savedInstanceState)
		{
			NotificationManager _notify = GetSystemService ("notification") as NotificationManager;
			_notify.CancelAll ();

			base.OnCreate (savedInstanceState);
           
            var AppInfo = DBLocalDataStore.GetInstance().GetAppInfo();
			var list_temp_answers = SaveAndLoad.GetInstance().GetAllAnswers();
			list_temp_answers.RemoveAll(s=>s == "select a value" || s == "_,___");
			bool checka = false;
			bool check = false;
			check = Intent.GetBooleanExtra("InfoPin",checka);
            var activeContacts = FragmentClassifyScreen.CheckActiveContacts(DBLocalDataStore.GetInstance().GetLocalContacts());
            var dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/");
            if (!dir.Exists())
            {
                Directory.CreateDirectory(dir.ToString());
                dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ExportDataBaseBoaBee/");
                Directory.CreateDirectory(dir.ToString());
                dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ImportDataBaseBoabee/");
                Directory.CreateDirectory(dir.ToString());
            }
			//bool check = Convert.ToBoolean(checka);
			//			if (check != null)
			//			{
			//if (AppInfo.appType != "kiosk") {
			//	if (check == true) {
   //                 if (activeContacts.Count > 0) {
			//			AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
			//			AlertDialog ad = builder.Create ();
			//			ad.SetCancelable (false);
			//			ad.SetCanceledOnTouchOutside (false);
			//			ad.SetMessage ("You still have ongoing work. Lets continue!");
			//			ad.SetTitle ("Warning:");
			//			ad.SetButton ("Yes", (s, ev) => {
			//				StartActivity (typeof (ActivityIdentifyClassifyShare));
			//				Finish ();
			//			});
			//			//ad.SetButton2 ("No, delete it", (s, ev) => {
			//			//	DBLocalDataStore.GetInstance ().ClearAllContactsPopup ();
			//			//	DBLocalDataStore.GetInstance ().ClearAllFilesPopup ();
			//			//	SaveAndLoad.GetInstance ().DeleteFile ();//!!!
			//			//});
			//			ad.Show ();
			//		}

			//	}
			//}
//			}
//			if ((popup_contacts + popup_files + list_temp_answers.Count) > 0)
//			{
//				alertDialog.Show ();
//
//			}
			SetContentView (Resource.Layout.Homescreen_layout);
			var sp = DBLocalDataStore.GetInstance ().GetSelectProfile ();
			if (sp == null) {
				AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetCancelable (false);
				ad.SetCanceledOnTouchOutside (false);
				ad.SetMessage ("App not selected.");
				ad.SetTitle ("Error:");
				ad.SetButton ("Ok",(s, ev) => {
					StartActivity(typeof(ActivitySelectApp));
					Finish();
				});
				ad.Show ();

			}

			if (DBLocalDataStore.GetInstance().GetLocalFormDefinitions().Count > 0 && 
				string.IsNullOrEmpty (DBLocalDataStore.GetInstance ().GetSelectedQuestionPosition ())) {
				AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetCancelable (false);
				ad.SetCanceledOnTouchOutside (false);
				ad.SetMessage ("Questions form not selected.");
				ad.SetTitle ("Select question:");
				ad.SetButton ("Select form questions",(s, ev) => {
					StartActivity(typeof(ActivitySelectQuestion));
					Finish();
				});
				ad.Show ();
			}

//			List<DBfileTO> _list_only_files = DBLocalDataStore.GetInstance ().GetOnlyLocalFilesWithOutlocalPath ();
//
//
//			if (_list_only_files.Count != 0){
//				AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
//				AlertDialog ad = builder.Create ();
//				ad.SetCancelable (false);
//				ad.SetCanceledOnTouchOutside (false);
//				ad.SetMessage ("The last download of files was interrupted. Please try again");
//				ad.SetTitle ("Retry process:");
//				ad.SetButton ("Retry",(s, ev) => {
//					StartActivity(typeof(ActivityReceivingInformation));
//					Finish();
//				});
//				ad.Show ();
//			}
            iconRed = ContextCompat.GetDrawable(Application, Resource.Drawable.settings_button);
            Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            //var textLogo = FindViewById<TextView>(Resource.Id.textLogo);
            var gotoIdetify = FindViewById<TextView> (Resource.Id.buttonIdetify);
            //var switchButton = FindViewById<Button> (Resource.Id.buttonSwitch);
            //var myworkButton = FindViewById<Button> (Resource.Id.buttonMyWork);
            var selectedApp = FindViewById<TextView> (Resource.Id.selectedApp);
            var contacts = FindViewById<TextView> (Resource.Id.contactmet);
            var emails = FindViewById<TextView> (Resource.Id.emailsent);
            var sheets = FindViewById<TextView> (Resource.Id.infosheets);

            var countcontacts = FindViewById<TextView> (Resource.Id.countcontacts);
            var countemails = FindViewById<TextView> (Resource.Id.countemails);
            var countsheets = FindViewById<TextView> (Resource.Id.countsheets);

            btnSettings = FindViewById<ImageView> (Resource.Id.btnsettings);

            //textLogo.SetTypeface (font, TypefaceStyle.Normal);
            selectedApp.SetTypeface (font, TypefaceStyle.Normal);
            contacts.SetTypeface (font, TypefaceStyle.Normal);
            emails.SetTypeface (font, TypefaceStyle.Normal);
            sheets.SetTypeface (font, TypefaceStyle.Normal);
            countcontacts.SetTypeface (font, TypefaceStyle.Normal);
            countemails.SetTypeface (font, TypefaceStyle.Normal);
            countsheets.SetTypeface (font, TypefaceStyle.Normal);
            gotoIdetify.SetTypeface (font, TypefaceStyle.Normal);


            if(AppInfo.appType == "kiosk")
            {
                gotoIdetify.Text = "start kiosk";
            }
            else
            {
                gotoIdetify.Text = "add prospect";
            }
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == Permission.Denied)
            {
                int MY_PERMISSIONS_REQUEST_Camera = 101;
                ActivityCompat.RequestPermissions(this,
                                                  new String[] { Manifest.Permission.Camera },
                                                  MY_PERMISSIONS_REQUEST_Camera);
            }

            selectedApp.Text = "";
            if (sp != null)
                selectedApp.Text = sp.displayName.ToUpper();
            gotoIdetify.Click += (object sender, EventArgs e) => {
                if(AppInfo.appType == "kiosk")
                {
                    StartActivity(typeof(ActivityTouchKiosk));
                    Finish();
                }
                else
                {
                    ActivityBadgeScanning.fromHomeScreen = true;
                    StartActivity(typeof(ActivityBadgeScanning));
                }
                    
            

			};

			contacts.Text = string.Format("contacts\nmet");
			emails.Text = string.Format("emails\nsent");
			sheets.Text = string.Format("info\nsheets");

			var list_contacts = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "");
			var count_contacts = new List<DBOverviewContacts> ();
			//var count_share = list_contacts.Where (s=>s.isfiles == true).ToList ();
			//var count_answer = list_contacts.Where (s=>s.isanswers == true).ToList ();

			foreach (var cont in list_contacts) {
				if (!string.IsNullOrEmpty (cont.email) && count_contacts.Where (s => s.email == cont.email).ToList ().Count == 0) {
					count_contacts.Add (cont);
				}
				if (string.IsNullOrEmpty (cont.email) && count_contacts.Where (s => s.barcode == cont.barcode).ToList ().Count == 0) {
					count_contacts.Add (cont);
				}
			}

//			foreach (var cont in count_share) {
//				if (!string.IsNullOrEmpty (cont.email) && count_contacts_share.Where (s => s.email == cont.email).ToList ().Count == 0) {
//					count_contacts_share.Add (cont);
//				}
//				if (string.IsNullOrEmpty (cont.email) && count_contacts_share.Where (s => s.barcode == cont.barcode).ToList ().Count == 0) {
//					count_contacts_share.Add (cont);
//				}
//			}
//
//			foreach (var cont in count_answer) {
//				if (!string.IsNullOrEmpty (cont.email) && count_contacts_answers.Where (s => s.email == cont.email).ToList ().Count == 0) {
//					count_contacts_answers.Add (cont);
//				}
//				if (string.IsNullOrEmpty (cont.email) && count_contacts_answers.Where (s => s.barcode == cont.barcode).ToList ().Count == 0) {
//					count_contacts_answers.Add (cont);
//				}
//			}


			countcontacts.Click += (sender, e) => {
					StartActivity(typeof(ActivityOverviewContacts));
					Finish();
			};

			countemails.Click += (sender, e) => {
					StartActivity(typeof(ActivityOverviewShares));
					Finish();
			};

			countsheets.Click += (sender, e) => {
				StartActivity(typeof(ActivityOverviewForms));
					Finish();
			};

			btnSettings.Click += (sender, e) => {
				if(AppInfo.appType != "kiosk" )
				{
					StartActivity(typeof(ActivitySettingsScreen));
					Finish();
				}
				else
				{
					StartActivity(typeof(ActivitySettingsScreen));
					Finish();
				}
			};

//			var bs = new BSCustomers ();
//			bs.SetContext (this);
            if (btnSettings.Background == iconRed)
                animationStart = true;
            else
                animationStart = false;
		}

//		public override void OnBackPressed ()
//		{
//			//base.OnBackPressed ();
//		}

		public override void Finish ()
		{
			//bs.SetStopped (true);
			bs.stopped = true;
			bs.Dispose ();
			answers_update.stopped = true;
			answers_update.Dispose ();
			//bs.SetAllContactsToServer ();
			base.Finish ();
		}

		void SendCompleted(bool flag, string message)
		{
			RunOnUiThread (()=>{
				if (flag) 
				{
					OnResume ();
				} 
				else
				{
					Console.WriteLine(message);
				}
			});

		}
        public static DBlocalContact ReturnNull(DBlocalContact lc)
        { 
            lc.firstname = string.IsNullOrEmpty(lc.firstname) ? null : lc.firstname;
            lc.lastname = string.IsNullOrEmpty(lc.lastname) ? null : lc.lastname;
            lc.email = string.IsNullOrEmpty(lc.email) ? null : lc.email;
            lc.company = string.IsNullOrEmpty(lc.company) ? null : lc.company;
            lc.jobtitle = string.IsNullOrEmpty(lc.jobtitle) ? null : lc.jobtitle;
            lc.phone = string.IsNullOrEmpty(lc.phone) ? null : lc.phone;
            lc.street = string.IsNullOrEmpty(lc.street) ? null : lc.street;
            lc.zip = string.IsNullOrEmpty(lc.zip) ? null : lc.zip;
            lc.city = string.IsNullOrEmpty(lc.city) ? null : lc.city;
            lc.country = string.IsNullOrEmpty(lc.country) ? null : lc.country;

            return lc;
        }
       public void scaleImage(ImageView v, float startScalex, float endScalex, float startScaley, float endScaley, float pivotX, float pivotY)
        {
            Animation anim = new ScaleAnimation(
                startScalex, endScalex,
                startScaley, endScaley,
                pivotX, pivotY);

            anim.Duration = 1000;

            v.StartAnimation(anim);
            anim.AnimationEnd += (sender, e) =>
            {
                v.ScaleX = 1f;
                v.ScaleY = 1f;
                scaleImage2(v);

            };
        }
        public void scaleImage2(ImageView v)
        {//ImageView v,  float startScalex, float endScalex, float startScaley, float endScaley, float pivotX,float pivotY) {
            Animation anim2 = new ScaleAnimation(
                1.2f, 1f,
                1.2f, 1f,
                50f, 50f);

            //  scaleImage((ImageView)sender, 1f, 1.3f, 1f, 1.3f, 20f, 10f);
            anim2.Duration = 1000;

            v.StartAnimation(anim2);
            anim2.AnimationEnd += (sender, e) =>
            {
                v.ScaleX = 1f;
                v.ScaleY = 1f;
                //listfiles.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file, list_popup_temp);

            };
        }
        public override void OnBackPressed()
        {
            
        }
	}
}
