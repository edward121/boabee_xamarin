
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
using System.Threading.Tasks;
using BoaBeeLogic;
using Android.Support.V4.App;
using Android;
using System.IO;
using Android.Text;
using Android.Text.Method;

namespace Leadbox
{
	[Activity (Label = "ActivitySettingsScreen", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivitySettingsScreen : Activity
	{
		BSCustomers bs = new BSCustomers ();
		BSAnswersUpdate answers_update = new BSAnswersUpdate();
        TextView textnotsend;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_settings_screen);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            var AppInfo = DBLocalDataStore.GetInstance().GetAppInfo();

            var textStatus = FindViewById<TextView>(Resource.Id.textViewStatus);
            var appName = FindViewById<TextView>(Resource.Id.appName);
            var emailuser = FindViewById<TextView>(Resource.Id.emailuser);
            textnotsend = FindViewById<TextView>(Resource.Id.notsend);
            var txtdefault = FindViewById<TextView>(Resource.Id.defaultSharetxt);

            var btnSwitchApp = FindViewById<RelativeLayout>(Resource.Id.switchApp);
            var btnResetWork = FindViewById<RelativeLayout>(Resource.Id.resetwork);
            var suncmanual = FindViewById<RelativeLayout>(Resource.Id.syncmanually);
            var defaultShare = FindViewById<RelativeLayout>(Resource.Id.buttonDefaultShare);//??
            var btnKiosk = FindViewById<RelativeLayout>(Resource.Id.Kiosk);


            var textreset = FindViewById<TextView>(Resource.Id.textManual);
            var textswith = FindViewById<TextView>(Resource.Id.textBadge);
            var textSync = FindViewById<TextView>(Resource.Id.textSuncmanualy);
            var textkiosk = FindViewById<TextView>(Resource.Id.textkiosk);

            var btnClose = FindViewById<TextView>(Resource.Id.btnClose);
            var btnExport = FindViewById<RelativeLayout>(Resource.Id.export);
            var textExport = FindViewById<TextView>(Resource.Id.textExport);

            textExport.SetTypeface(font, TypefaceStyle.Normal);
            textkiosk.SetTypeface(font, TypefaceStyle.Normal);
            textStatus.SetTypeface(font, TypefaceStyle.Normal);
            appName.SetTypeface(font, TypefaceStyle.Normal);
            emailuser.SetTypeface(font, TypefaceStyle.Normal);
            textreset.SetTypeface(font, TypefaceStyle.Normal);
            textswith.SetTypeface(font, TypefaceStyle.Normal);
            btnClose.SetTypeface(font, TypefaceStyle.Normal);
            textnotsend.SetTypeface(font, TypefaceStyle.Normal);
            textSync.SetTypeface(font, TypefaceStyle.Normal);
            txtdefault.SetTypeface(font, TypefaceStyle.Normal);


            var sp = DBLocalDataStore.GetInstance().GetSelectProfile();
            appName.Text = sp.displayName.ToUpper();
            if (AppInfo.appType == "kiosk")
                    textkiosk.Text = "kiosk setting       ";
          
                
            defaultShare.Click += (sender, e) =>
                {
                    StartActivity(typeof(ActivityDefaultShare));
				};

			emailuser.Text = DBLocalDataStore.GetInstance ().GetUserMail ();

            //if (AppInfo.appType == "kiosk") {
            //    textkiosk.Text = "Kiosok Setting";
            //}

            btnExport.Click += (sender, e) => { 
                string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string dbPath = System.IO.Path.Combine(libraryPath, "database.dbx");
                string MainDB = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ExportDataBaseBoabee/" + "database.dbx";
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
                    var dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/");
                    if (!dir.Exists())
                    {
                        dir.Mkdirs();
                        dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ExportDataBaseBoaBee/");
                        dir.Mkdirs();
                        dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ImportDataBaseBoabee/");
                        dir.Mkdirs();
                    }
                }
                else {
                    if (new Java.IO.File(MainDB).Exists())
                        File.Delete(MainDB);
                    if (MainActivity.usedImportDB)
                    {
                        dbPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BoaBeeDataBase/" + "ImportDataBaseBoabee/" + "database.dbx";
                        File.Copy(dbPath, MainDB);
                    }
                    else {
                        File.Copy(dbPath, MainDB);
                    }
                    AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                    AlertDialog ad_ = builder_.Create();
                    ad_.SetCancelable(false);
                    ad_.SetCanceledOnTouchOutside(false);
                    ad_.SetMessage(string.Format("All data is exported to the file database.dbx in BoaBeeDataBase/ExportDatabase folder."));
                    ad_.SetTitle("Export DataBase");
                    ad_.SetButton("OK".ToUpper(), delegate (object so, DialogClickEventArgs events)
                    {
                    });
                    ad_.Show();
                }
            };

			btnKiosk.Click += (sender, e) =>
			{
                    StartActivity(typeof(ActivitySettingsKiosk));
			};
            if (DBLocalDataStore.GetInstance().GetLocalUserInfo().invalidPassword)
            {
                AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                AlertDialog ad_ = builder_.Create();
                ad_.SetCancelable(false);
                ad_.SetCanceledOnTouchOutside(false);
                ad_.SetMessage(string.Format("Your password has been changed. Your work will NOT be synced until the most recent password is entered"));
                ad_.SetTitle("WARNING");
                ad_.SetButton("Later".ToUpper(), delegate (object so, DialogClickEventArgs events)
                                {
                                });
                var resVIew = Resource.Layout.InputPassword;
                var view = LayoutInflater.Inflate(resVIew, null);
                EditText inputpassword = view.FindViewById<EditText>(Resource.Id.inputPas);
                TextView Eye = view.FindViewById<TextView>(Resource.Id.eye);

                inputpassword.TransformationMethod = PasswordTransformationMethod.Instance;
                Eye.SetBackgroundResource(Resource.Drawable.EyeIcon);
                Eye.Click += delegate
                {
                    if (inputpassword.TransformationMethod == HideReturnsTransformationMethod.Instance)
                    {
                        Eye.SetBackgroundResource(Resource.Drawable.EyeIcon);
                        inputpassword.TransformationMethod = PasswordTransformationMethod.Instance;
                    }
                    else {
                        Eye.SetBackgroundResource(Resource.Drawable.eye_crossed);
                        inputpassword.TransformationMethod = HideReturnsTransformationMethod.Instance;
                    }
                };


                ad_.SetView(view);
                ad_.SetButton2("OK".ToUpper(), delegate (object so, DialogClickEventArgs events)
                                {
                                    var userInfo = DBLocalDataStore.GetInstance().GetLocalUserInfo();
                                    userInfo.password = inputpassword.Text;
                                    userInfo.invalidPassword=false;
                                    DBLocalDataStore.GetInstance().AddUserInfo(userInfo);
                                    syncManualClicked();
                                    
                                });
                ad_.Show();
            }
          
			suncmanual.Click += (sender, e) => {
                var syncRequest = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();
                if (DBLocalDataStore.GetInstance().GetLocalUserInfo().invalidPassword)
                {
                    AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                    AlertDialog ad_ = builder_.Create();
                    ad_.SetCancelable(false);
                    ad_.SetCanceledOnTouchOutside(false);
                    ad_.SetMessage(string.Format("Your password has been changed. Your work will NOT be synced until the most recent password is entered"));
                    ad_.SetTitle("WARNING");
                    ad_.SetButton("Later".ToUpper(), delegate (object so, DialogClickEventArgs events)
                                    {
                                    });

                    var resVIew = Resource.Layout.InputPassword;
                    var view = LayoutInflater.Inflate(resVIew, null);
                    EditText inputpassword = view.FindViewById<EditText>(Resource.Id.inputPas);
                    TextView Eye = view.FindViewById<TextView>(Resource.Id.eye);

                    inputpassword.TransformationMethod = PasswordTransformationMethod.Instance;
                    Eye.SetBackgroundResource(Resource.Drawable.EyeIcon);
                    Eye.Click += delegate {
                        if (inputpassword.TransformationMethod == HideReturnsTransformationMethod.Instance)
                        {
                            Eye.SetBackgroundResource(Resource.Drawable.EyeIcon);
                            inputpassword.TransformationMethod = PasswordTransformationMethod.Instance;
                        }
                        else { 
                            Eye.SetBackgroundResource(Resource.Drawable.eye_crossed);
                            inputpassword.TransformationMethod = HideReturnsTransformationMethod.Instance;
                        }
                    };

                    
                    ad_.SetView(view);
                    ad_.SetButton2("OK".ToUpper(), delegate (object so, DialogClickEventArgs events)
                                    {
                                        var userInfo = DBLocalDataStore.GetInstance().GetLocalUserInfo();
                                        userInfo.password = inputpassword.Text;
                                        userInfo.invalidPassword = false;
                                        DBLocalDataStore.GetInstance().AddUserInfo(userInfo);
                                        syncManualClicked();
                                       
                                    });
                    ad_.Show();

                }
                else {
                    syncManualClicked();
                    }
			};
           

			btnSwitchApp.Click += (sender, e) => {

                var listsync = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();
                if (DBLocalDataStore.GetInstance().GetLocalUserInfo().invalidPassword)
                {
                    AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                    AlertDialog ad_ = builder_.Create();
                    ad_.SetCancelable(false);
                    ad_.SetCanceledOnTouchOutside(false);
                    ad_.SetMessage(string.Format("Your password has been changed, please click 'SYNC MANUALLY' to change password"));
                    ad_.SetTitle("WARNING");
                    ad_.SetButton("Ok".ToUpper(), delegate (object so, DialogClickEventArgs events)
                                    {
                                    });
                    ad_.Show();
                }
                else {
                    if (listsync.Count == 0)
                    {
                        AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                        AlertDialog ad_ = builder_.Create();
                        ad_.SetCancelable(false);
                        ad_.SetCanceledOnTouchOutside(false);
                        ad_.SetMessage(string.Format("All your work is safely stored in the cloud.\nWhen you switch to another app setup you will start with a clean work-status."));
                        ad_.SetTitle("WARNING:");
                        ad_.SetButton("CONTINUE".ToUpper(), delegate (object so, DialogClickEventArgs events)
                        {
                            if (!ConnectManager.GetInstance().StateNet())
                            {
                                AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                                AlertDialog ad = builder.Create();
                                ad.SetCancelable(false);
                                ad.SetCanceledOnTouchOutside(false);
                                ad.SetMessage(string.Format("There is no internet connection available.\nPlease try again later."));
                                ad.SetTitle("WARNING");
                                ad.SetButton("OK".ToUpper(), delegate (object s, DialogClickEventArgs even) { });
                                ad.Show();
                            }
                            else {
                                var _activity = new Intent(this, typeof(ActivitySelectApp));
                                var appname = DBLocalDataStore.GetInstance().GetSelectProfile();
                                _activity.PutExtra("statusH", "true");
                                _activity.PutExtra("FromSettings", true);
                                _activity.PutExtra("NameApp", appname.displayName);
                                StartActivity(_activity);
                                Finish();
                            }
                        });
                        ad_.SetButton2("CANCEL".ToUpper(), (s, ev) =>
                        {

                        });
                        ad_.Show();

                    }
                    else {

                        AlertDialog.Builder builder = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                        AlertDialog ad = builder.Create();
                        ad.SetCancelable(false);
                        ad.SetCanceledOnTouchOutside(false);
                        ad.SetMessage(string.Format("Not all your work is safely stored in the cloud.\nLet’s send it now to the cloud."));
                        ad.SetTitle("WARNING:");
                        Console.WriteLine("Console.WriteLine(\"\");");
                        ad.SetButton("OK".ToUpper(), (s, ev) =>
                        {
                            if (!ConnectManager.GetInstance().StateNet())
                            {
                                AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                                AlertDialog ad_ = builder_.Create();
                                ad_.SetCancelable(false);
                                ad_.SetCanceledOnTouchOutside(false);
                                ad_.SetMessage(string.Format("There is no internet connection available.\nPlease try again later."));
                                ad_.SetTitle("WARNING:");
                                Console.WriteLine("Console.WriteLine(\"\");");
                                ad_.SetButton("OK".ToUpper(), delegate (object so, DialogClickEventArgs events)
                                {

                                });
                                ad_.Show();
                            }
                            else {
                                try
                                {

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }


                        });
                        ad.Show();
                    }
                }
			};

			btnResetWork.Click += (sender, e) => {
				AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetCancelable (false);
				ad.SetCanceledOnTouchOutside (false);
				ad.SetMessage ("Are you sure remove all data from MY WORK?");
				ad.SetTitle ("MY WORK:");
				Console.WriteLine("Console.WriteLine(\"\");");
				ad.SetButton ("YES".ToUpper(),(s, ev) => {
                    OfflineLogic.ClearDataSelected();
                    DBLocalDataStore.GetInstance().clearSyncRequests();
                    var contactsInRequest = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.useInRequest).ToList();
                    for (int i = 0; i < contactsInRequest.Count; i++)
                    {
                        contactsInRequest[i].useInRequest = false;
                        DBLocalDataStore.GetInstance().UpdateLocalContact(contactsInRequest[i]);
                    }
                    var countshomescreen = new DBHomeScreenCounts() { countShare = 0, countContacts = 0, countQuestion = 0 };
                    DBLocalDataStore.GetInstance().SetCountHomeScreen(countshomescreen);


                    base.OnResume();
				});
				ad.SetButton2 ("NO".ToUpper(), (s, ev) => {

				});
				ad.Show ();
//				StartActivity(typeof(ActivityDefaultShare));
			};

			btnClose.Click += (sender, e) => {
				StartActivity(typeof(ActivityHomescreen));
				Finish();
			};

			//var bsc = new BSCustomers ();

		}

		void SendCompleted(bool flag, string message){
			RunOnUiThread (()=>{
				if (flag) {
				 base.OnResume();
				} else {
					Console.WriteLine(message);
				}
			});
		}

		public override void OnBackPressed ()
		{
			base.OnBackPressed ();
			StartActivity(typeof(ActivityHomescreen));
			Finish();
		}

		public override void Finish ()
		{
			//bs.SetStopped (true);

			base.Finish ();
		}

		protected override void OnResume ()
		{
            base.OnResume();
            //_state = ConnectManager.GetInstance ().stateNet ();
			var textnotsend = FindViewById<TextView> (Resource.Id.notsend);
            //answers_update
            var listsync = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();

            if (listsync.Count != 0) {
				//textnotsend.Visibility = ViewStates.Visible;
				textnotsend.Text = string.Format ("Your work is saved on your device. With a stable internet connection, press ‘SYNC MANUALLY’ to send it to our servers.");
				textnotsend.SetTextColor (Color.ParseColor("#FF0000"));
//				
				
			} else {
				textnotsend.Text = "All your work is safely stored in the cloud now.\nIt is available in the report via your dashboard.";
				textnotsend.SetTextColor (Color.ParseColor("#EDCD00"));
				//textnotsend.Visibility = ViewStates.Invisible;
			}

		}
        public void syncManualClicked()
        { 
            var syncRequest = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();
            if (syncRequest.Count == 0)
            {
                Console.WriteLine("Have not send data to cloud.");
                AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                AlertDialog ad_ = builder_.Create();
                ad_.SetCancelable(false);
                ad_.SetCanceledOnTouchOutside(false);
                ad_.SetMessage(string.Format("All your work is safely stored in the cloud."));
                ad_.SetTitle("MY WORK:");
                ad_.SetButton("OK".ToUpper(), delegate (object so, DialogClickEventArgs events)
                {

                });
                ad_.Show();
            }
            else {
                if (!ConnectManager.GetInstance().StateNet())
                {
                    AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
                    AlertDialog ad_ = builder_.Create();
                    ad_.SetCancelable(false);
                    ad_.SetCanceledOnTouchOutside(false);
                    ad_.SetMessage(string.Format("There is no internet connection available.\nPlease try again later."));
                    ad_.SetTitle("WARNING");
                    Console.WriteLine("Console.WriteLine");
                    ad_.SetButton("OK".ToUpper(), delegate (object so, DialogClickEventArgs events)
                    {
                    });
                    ad_.Show();
                }
                else {
                    Task.Run(async () =>
                    {
                        await NetworkRequests.SyncDataServer();
                        if (!DBLocalDataStore.GetInstance().GetLocalUserInfo().invalidPassword)
                        {
                            RunOnUiThread(() =>
                            {
                                textnotsend.Text = "All your work is safely stored in the cloud now.\nIt is available in the report via your dashboard.";
                                textnotsend.SetTextColor(Color.ParseColor("#EDCD00"));
                            });
                        }
                        else { 
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "The password is not correct", ToastLength.Short).Show();
                            });
                        }
                    });

                }
            }
        
        
        
        
        }

	}
}
