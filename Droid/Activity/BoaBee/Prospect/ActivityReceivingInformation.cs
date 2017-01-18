
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
using Android.Graphics;
using System.Threading;
using Android.Views.Animations;
using Android.Content.PM;
using System.IO;
using BoaBeePCL;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using System.Threading.Tasks;

namespace Leadbox
{
	[Activity (Label = "ActivityReceivingInformation", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityReceivingInformation : Activity, ActivityCompat.IOnRequestPermissionsResultCallback
	{
		
		private int MY_PERMISSIONS_REQUEST_FILES_WRITE;

		Animation fadeOut;
		Animation fadeIn;
		AlertDialog.Builder builder;
		TextView percent;
		ImageView logobee;
		List<DBfileTO> _list_only_files;
		int i = 0;
		bool flag_next_start = true;
		int increment = 0;
		int result = 0;
		bool need_kiosk = false;
		bool realy_finished = false;

		public void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			//base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			//Console.WriteLine("MY_PERMISSIONS_REQUEST_READ_CONTACTS = " + MY_PERMISSIONS_REQUEST_CAMERA);


			if(requestCode == MY_PERMISSIONS_REQUEST_FILES_WRITE)
			{
				if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
				{
					Console.WriteLine("MY_PERMISSIONS_REQUEST_FILES_WRITE");
					SaveFilesManager.GetInstance().CreateAllFolder(downloadFiles, downloadAppTypeImages, this);
					// permission was granted, yay! Do the
					// contacts-related task you need to do.

				}
				else
				{

					// permission denied, boo! Disable the
					// functionality that depends on this permission.
				}
			}


		}

		private void CheckPermissions(int my_permissions, string[] _permissions)
		{
			bool flag_need_all_permissions = false;
			if ((int)Build.VERSION.SdkInt < 23)
			{
				//OPenScanCode();
				return;
			}

			for (int i = 0; i < _permissions.Length; i++)
			{
				if (ContextCompat.CheckSelfPermission(this, _permissions[i]) != Permission.Granted)
				{
					
					// Should we show an explanation?
					if (ActivityCompat.ShouldShowRequestPermissionRationale(this, _permissions[i])) {
						

					}
					else
					{
						flag_need_all_permissions = true;
						
						// No explanation needed, we can request the permission.
						//flag_need_all_permissions = false;
					}
					
				}
			}

			if (flag_need_all_permissions)
			{
				ActivityCompat.RequestPermissions(this,
					_permissions,
					my_permissions);
			}
			else
			{
				SaveFilesManager.GetInstance().CreateAllFolder(downloadFiles, downloadAppTypeImages, this);
			}

		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.Receiving_information_layout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			logobee = FindViewById<ImageView> (Resource.Id.logobee);
			percent = FindViewById<TextView>(Resource.Id.percentDownload);
			var textinfo = FindViewById<TextView>(Resource.Id.infodownload);

			percent.SetTypeface (font, TypefaceStyle.Normal);
			textinfo.SetTypeface (font, TypefaceStyle.Normal);
			percent.Text = string.Format ("{0:##0'%'}", 0);


			animFadeIn();

			//Keycode.Home

			//
			_list_only_files = DBLocalDataStore.GetInstance ().GetOnlyLocalFilesWithOutlocalPath ();
			Console.WriteLine("_list_only_files.Count = ", _list_only_files.Count);
            if (DBLocalDataStore.GetInstance().GetLocalContacts().Count != 0)
            {
                var contactsInRequest = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.useInRequest).ToList();
                for (int i = 0; i < contactsInRequest.Count; i++)
                {
                    contactsInRequest[i].useInRequest = false;
                    DBLocalDataStore.GetInstance().UpdateLocalContact(contactsInRequest[i]);
                }
                var countshomescreen = new DBHomeScreenCounts() { countShare = 0, countContacts = 0, countQuestion = 0 };
                DBLocalDataStore.GetInstance().SetCountHomeScreen(countshomescreen);
            }

			if (_list_only_files.Count != 0){
				AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetCancelable (false);
				ad.SetCanceledOnTouchOutside (false);
				ad.SetMessage ("The last download of files was interrupted. Please try again");
				ad.SetTitle ("Retry process:");
				ad.SetButton ("Retry",(s, ev) => {
					if (ConnectManager.GetInstance().StateNet()){
						Console.WriteLine ("Restart files downloading");
						i = 25;
						percent.Text = string.Format ("{0:##0}%", i);
						ConnectManager.GetInstance ().GetUpdateApp (dataUpdate, this);
					} else {
						alertDialog ("No internet connection available. Please connect to the internet and retry.");
					}
				});
				ad.Show ();
				return;
			}

            if (!MainActivity.usedImportDB)
                ConnectManager.GetInstance().GetUpdateApp(dataUpdate, this);
            else
                updateFiles();


        }


        async void updateFiles()
        {
            await NetworkRequests.GetFilesAndForms((success, message, isKiosk) => { 
            if (success)
                {
                    RunOnUiThread(() =>
                    {
                        i = 25;
                        percent.Text = string.Format("{0:##0}%", i);
                        try
                        {
                            if (!MainActivity.usedImportDB)
                                updateContacts();
                            else { 
                                SaveFilesManager.GetInstance().CreateAllFolder(downloadFiles, downloadAppTypeImages, this);
                                Finish();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });

                }
            
            
            
            
            });

        }

        async void updateContacts()
        { 
            await NetworkRequests.GetContacts((success, message) => {
                if (success) 
                {
                    RunOnUiThread(() =>
                    {
                        i = 75;
                        percent.Text = string.Format("{0:##0}%", i);
                        try
                        {
                            Finish();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });

                }
            });
        
        }




		void downloadAppTypeImages(bool flag, string m)
		{
			RunOnUiThread(() =>
				{
					if (flag)
					{
						if (m == "finished")
						{
							need_kiosk = true;
							Finish();
						}
					}
					else
					{
						Console.WriteLine("STATUS DOWLOAD FILE: " + m);
						popupDialog(m);
					}
				});
		}

		void downloadFiles(bool flag, string m){
            RunOnUiThread (() =>{
				if (flag) {
					switch (m){
					case "UP":{
							i = i + increment;
							result--;
							if (result == 0){
                                    updateContacts();
							}
							percent.Text = string.Format ("{0:##0}%", i);
							break;
						}
					case "FINISH":{
                                updateContacts();
							break;
						}
					default:{
							Console.WriteLine("STATUS DOWLOAD FILE: " + m);
							break;
						}
					}
				} else {
					Console.WriteLine("STATUS DOWLOAD FILE: " + m);
					popupDialog(m);
					//Toast.MakeText (this, m, ToastLength.Long).Show ();
				}
			});
		}

		void dataUpdate(bool flag, string m)
		{
			RunOnUiThread (()=>{
				if (flag) {
					i = 50;
					percent.Text = string.Format ("{0:##0}%", i);
					var list_only_files = DBLocalDataStore.GetInstance ().GetOnlyLocalFiles ();
					result = list_only_files.Count;
					if (result != 0)
						increment = (int)(50 / result);

					if (!DBLocalDataStore.GetInstance().GetShowLocalFiles()) {
                        //						string _permission_r = Manifest.Permission.ReadExternalStorage;
                        //						string _permission_w = Manifest.Permission.WriteExternalStorage;
                        //						CheckPermissions (MY_PERMISSIONS_REQUEST_FILES_WRITE, new []{_permission_w, _permission_r});

                        SaveFilesManager.GetInstance().CreateAllFolder(downloadFiles, downloadAppTypeImages, this);
                  
					}
					else{
                        SaveFilesManager.GetInstance ().CreateAllFolder (downloadFiles, downloadAppTypeImages, this);
                        SaveFilesManager.GetInstance ().downloadFileAppType (downloadAppTypeImages);
                        updateContacts();
					}
				} else {
					//Toast.MakeText (this, m, ToastLength.Long).Show ();
					Console.WriteLine (m);
					popupDialog(m);
				}
			});
		}

		private void animFadeIn()
		{

			fadeIn = new AlphaAnimation (1, 0.2f);
			fadeIn.Duration = 500;
			logobee.StartAnimation (fadeIn);
			fadeIn.AnimationEnd += (sender, e) => animFadeOut ();
		}

		private void animFadeOut()
		{
			fadeOut = new AlphaAnimation (0.2f, 1);
			fadeOut.Duration = 500;
			logobee.StartAnimation (fadeOut);
			fadeOut.AnimationEnd += (sender, e) => animFadeIn ();
		}

		public override void Finish ()
		{
			percent.Text = string.Format ("{0:##0'%'}", 100);

			if (realy_finished)
			{
				base.Finish();
				return;
			}

//			if (!need_kiosk)
//				return;

			if (flag_next_start)
				StartActivity (typeof(ActivitySelectQuestion));
			else 
			{
				PendingIntent pendingIntent = PendingIntent.GetActivity (this, 0, new Intent(this, typeof(ActivitySelectQuestion)), PendingIntentFlags.UpdateCurrent);
				Notification.Builder builder = new Notification.Builder (this)
					.SetContentIntent (pendingIntent)
					.SetContentTitle ("BoaBee")
					.SetContentText ("All files was dowloaded. ")
					.SetDefaults (NotificationDefaults.Sound | NotificationDefaults.Vibrate)
					.SetAutoCancel(false)
					.SetSmallIcon (Resource.Drawable.bee);
				// Build the notification:
				Notification notification = builder.Build();

				// Get the notification manager:
				NotificationManager notificationManager =
					GetSystemService ("notification") as NotificationManager;

				// Publish the notification:
				const int notificationId = 0;
				notificationManager.Notify (notificationId, notification);
				//Console.WriteLine ("NOTIFICATION ON");
				//Toast.MakeText (this, "NOTIFICATION", ToastLength.Long).Show ();
			}


			base.Finish ();
			//i = 100;
			//percent.Text = string.Format ("{0:##0'%'}", i);
		}

		public override void OnBackPressed ()
		{
			//base.OnBackPressed ();
		}

		public void popupDialog(string alertMassege)
		{
			if (builder != null) {
				//builder.Dispose ();
				//builder = null;
				return;
			}

			builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
			AlertDialog ad = builder.Create ();
			ad.SetMessage (alertMassege);
			ad.SetTitle ("Alert:");
			ad.SetButton ("Ok",(s, ev) => {
				realy_finished = true;
				StartActivity(typeof(ActivitySelectApp));
				Finish();
//				if (!ConnectManager.GetInstance().StateNet()){
//					alertDialog ("No internet connection available. Please connect to the internet and retry.");
//				} else {
//					ConnectManager.GetInstance ().GetContacts (ContactsUpdate);
//				}
			});
			ad.Show ();
		}

		public void alertDialog(string alertMassege)
		{
			var builder_ = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
			AlertDialog ad = builder_.Create ();
			ad.SetMessage (alertMassege);
			ad.SetTitle ("Error:");
			ad.SetButton ("RETRY", (s, ev) => {
				if (!ConnectManager.GetInstance().StateNet()){
					alertDialog ("No internet connection available. Please connect to the internet and retry.");
				} else {
                    updateContacts();
				}
			});
			ad.Show ();
		}

		protected override void OnStop ()
		{
			flag_next_start = false;
			//OnStart ();
			//Toast.MakeText (this, "OnStop", ToastLength.Long).Show ();
			base.OnStop ();
		}

		//protected override void OnStart ()
		//{
		//	//Toast.MakeText (this, "OnStart", ToastLength.Long).Show ();
		//	flag_next_start = true;
		//	base.OnStart ();
		//}
	}
}

