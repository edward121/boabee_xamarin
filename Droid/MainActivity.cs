using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Graphics;
using System.Threading;
using System.Collections.Generic;
using BoaBeePCL;
using Android.App;
using FireBase;
using BoaBeeLogic;
using System.Threading.Tasks;
using System.IO;
using Android.Support.V4.App;
using Android;
using System.Net;
using Newtonsoft.Json;
using System.Linq;

namespace Leadbox
{
	[Activity (Label = "BoaBee", 
		Theme = "@style/ActivityTheme", 
		MainLauncher = true, 
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
        public static Typeface font;
        public static bool usedImportDB = false;
       
        public ConnectManager.NetworkState _state;

		protected override void OnCreate (Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = System.IO.Path.Combine(libraryPath, "database.dbx");
            var dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/");
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Denied && ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
            {
                int ReadExternalStorage = 102;
                ActivityCompat.RequestPermissions(this,
                                                  new String[] { Manifest.Permission.ReadExternalStorage },
                                                  ReadExternalStorage);
                int WriteExternalStorage = 103;
                ActivityCompat.RequestPermissions(this,
                                                  new String[] { Manifest.Permission.WriteExternalStorage },
                                                  WriteExternalStorage);
            }
            //if (!dir.Exists())
            //{
            //    Directory.CreateDirectory(dir.ToString());
            //    dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ExportDataBaseBoaBee/");
            //    Directory.CreateDirectory(dir.ToString());
            //    dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ImportDataBaseBoabee/");
            //    Directory.CreateDirectory(dir.ToString());
            //}


            dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ImportDataBaseBoabee/" + "database.dbx");
            //dbPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "DataBaseBoaBee/"+"database.dbx";
            if (!dir.Exists())
            {
                libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                dbPath = System.IO.Path.Combine(libraryPath, "database.dbx");
                DBLocalDataStore.GetInstance().SetPath(dbPath);
            }
            else {
                DBLocalDataStore.GetInstance().SetPath(dir.ToString());
                usedImportDB = true;
            }



            alert = new AlertDialog.Builder(this);
            // Set our view from the "main" layout resource  @drawable/homescreen_background

            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            _state = ConnectManager.GetInstance().stateNet();
            try
            {
                if (_state == ConnectManager.NetworkState.ConnectedWifi || _state == ConnectManager.NetworkState.ConnectedData)
                {
                    CheckVersionFireBase();
                }
                else {
                    startApp();
                }
            }
            catch { startApp(); }
        }




        void startApp()
		{
			var infoUser = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
            DBAppSettings appSettings = new DBAppSettings();
            appSettings = DBLocalDataStore.GetInstance().GetAppSettings();
            string versionNow = this.PackageManager.GetPackageInfo(this.PackageName, 0).VersionName;
            if (DBLocalDataStore.GetInstance().getVersion() == null)
            {
                DBVersion version = new DBVersion();
                version.VersionDataBase = versionNow;
                DBLocalDataStore.GetInstance().addDBVersion(version);
            }
            else {
                if (DBLocalDataStore.GetInstance().getVersion()[0].VersionDataBase != versionNow)
                { 
                    string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    string dbPath = System.IO.Path.Combine(libraryPath, "database.dbx");
                    File.Delete(dbPath);
                    DBLocalDataStore.GetInstance().SetPath(dbPath);
                    DBVersion version = new DBVersion();
                    version.VersionDataBase = versionNow;
                    DBLocalDataStore.GetInstance().addDBVersion(version);
                }
            }


            if (appSettings == null)
            {
                appSettings = new DBAppSettings();
                appSettings.instantContactCheck = true;
                appSettings.getSharedContacts = true;
                DBLocalDataStore.GetInstance().SetAppSettings(appSettings);
                DBLocalDataStore.GetInstance().SetCountHomeScreen(new DBHomeScreenCounts() { countShare = 0, countContacts = 0, countQuestion = 0 });
            }
            var countshomescreen = DBLocalDataStore.GetInstance().GetCountHomeScreen();

            if (countshomescreen == null)
            {
                countshomescreen = new DBHomeScreenCounts() { countShare = 0, countContacts = 0, countQuestion = 0 };
                DBLocalDataStore.GetInstance().SetCountHomeScreen(countshomescreen);
            }
            //

            //countshomescreen
            //countshomescreen = new DBHomeScreenCounts() { countShare = 0, countContacts = 0, countQuestion = 0 };
            //var asd = DBLocalDataStore.GetInstance().GetLocalContacts();
            //var sync1 = DBLocalDataStore.GetInstance().getSyncRequests();
            //for (int i = 0; i <= sync1.Count; i++)
            //{
            //    var receivedData = JsonConvert.DeserializeObject<BoaBeePCL.SyncContext>(sync1[i].serializedSyncContext);
            //    try
            //    {
            //        if (receivedData.orders[0].orderLine.Count != 0)
            //        {
            //            countshomescreen.countShare += receivedData.orders.Count;
            //        }
            //        countshomescreen.countShare += receivedData.forms.Count;
            //        for (int n = 0; n < receivedData.contacts.Count; n++)
            //        {
            //            var contact = asd.Find(e => e.uid == receivedData.contacts[n].uid);
            //            contact.useInRequest = true;
            //            DBLocalDataStore.GetInstance().UpdateLocalContact(contact);
            //        }

            //    }
            //    catch { }
            //}
            countshomescreen.countContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.useInRequest).ToList().Count;
            DBLocalDataStore.GetInstance().SetCountHomeScreen(countshomescreen);
            //

			if (infoUser == null) {
				//if user the first time in here:
				ThreadPool.QueueUserWorkItem (o => {
					Thread.Sleep (500);
					RunOnUiThread (() => {
						StartActivity (typeof(ActivityLoginForm)); //(Introduction)
						Finish ();
					});
				});
			
			} else {
				var sp = DBLocalDataStore.GetInstance ().GetSelectProfile ();
                if (sp == null)
                {
                    StartActivity(typeof(ActivitySelectApp));
                }
                else {
                    if (!usedImportDB)
                    {
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            Thread.Sleep(500);
                            RunOnUiThread(() =>
                            {
                                bool a = true;
                                var _activityHomescreen = new Intent(this, typeof(ActivityHomescreen));
                                _activityHomescreen.PutExtra("InfoPin", a);
                                StartActivity(_activityHomescreen); 
                                    Finish();
                            });
                        });
                    }
                    else {
                        try
                        {
                            var tt = DBLocalDataStore.GetInstance().GetAllLocalFiles();
                            for (int i = 0; i < tt.Count; i++)
                            {
                                if (!new Java.IO.File(tt[i].localpath).Exists())
                                { 
                                    StartActivity(typeof(ActivityReceivingInformation));
                                    Finish();
                                }
                            }

                            bool a = true;
                            var _activityHomescreen = new Intent(this, typeof(ActivityHomescreen));
                            _activityHomescreen.PutExtra("InfoPin", a);
                            StartActivity(_activityHomescreen); //(Introdaction)
                            Finish();
                        }
                        catch { 
                            StartActivity(typeof(ActivityReceivingInformation));
                            Finish();
                        }
                    }
                }
			}
		}
        public string serverVersion;
        public AlertDialog.Builder alert;

        public void CheckVersionFireBase()
        {
            bool timeout = false;
            Context context = this.ApplicationContext;
            System.Timers.Timer _timer = new System.Timers.Timer();
            int countsecond = 31;
            _timer.Interval = 1000;
            _timer.Elapsed += delegate {
                countsecond--;
                if (countsecond == 30) { 
                    int Now = 0;
                    int server = 0;
                    var versionNow = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                    try
                        {
                            serverVersion = FirebaseManager.GetInstance().GetObjectByNameJson("AppVersionAndroid");
                            versionNow = versionNow.Split('.')[2];
                            Now = Convert.ToInt32(versionNow);
                            serverVersion = serverVersion.Split('.')[2];
                            server = Convert.ToInt32(serverVersion);
                        }
                        catch
                        {
                            timeout = true;
                        }

                        if (server >= Now && !timeout)
                        {
                        
                        alertdialog();
                            _timer.Dispose();
                        }
                        else if (!timeout)
                        {
                            if (DBLocalDataStore.GetInstance().GetSelectProfile() != null)
                            {
                                List<DBfileTO> _list_only_files = DBLocalDataStore.GetInstance().GetOnlyLocalFilesWithOutlocalPath();

                                if (_list_only_files.Count != 0)
                                {
                                    StartActivity(typeof(ActivityReceivingInformation));
                                    Finish();
                                    return;
                                }
                            }
                            startApp();
                        }
                        else {
                            _timer.Dispose();
                            startApp();
                        }
                        _timer.Dispose();
                }
                if (serverVersion == null && countsecond == 0 )
                {
                    timeout = true;
                    startApp();
                    _timer.Dispose();
                }
                else if(serverVersion != null){
                    _timer.Stop();
                }
            };
            _timer.Start();


        }
        public void alertdialog()
        { 
            RunOnUiThread(() =>
            {

            alert.SetTitle("A more recent version of the app is available. ");
            alert.SetMessage("Please install this version before continuing.");
            alert.SetCancelable(false);
            alert.SetNegativeButton("Open GooglePlay", delegate
            {
                var uri = Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=com.qtree.BoaBee&hl=en");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
                Finish();
            });
                alert.SetPositiveButton("Later", delegate
                     {
                         Finish();
                     });
                alert.Show();
                    });
        
        }

    }
}


