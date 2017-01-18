
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
using BoaBeePCL;
using Android.Text;
using Android.Graphics;
using Android.Content.PM;
using Android.Support.V7.Widget;


namespace Leadbox
{
	[Activity(Label = "ActivityTouchKiosk",ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityTouchKiosk : Activity
	{
		public static FragmentsInputPassword newFragment;
		BSCustomers bs = new BSCustomers ();
		BSAnswersUpdate answers_update = new BSAnswersUpdate();
		protected override void OnResume ()
		{
            base.OnResume();
		}
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);	
			SaveAndLoad.checkCancelExiteKiosk = false;
			SetContentView(Resource.Layout.Touch_start_layout);

			var textKioskTitle = FindViewById<TextView>(Resource.Id.textView1);
			var breakHomescreen = FindViewById<TextView>(Resource.Id.textView2);
			var KioskLogotip = FindViewById<ImageView>(Resource.Id.imageView1);
			var touchlayout = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
//			var fragmentPassword = FindViewById<Fragment>(Resource.Id.fragment1);
            var AppInfo = DBLocalDataStore.GetInstance().GetAppInfo();
			KioskLogotip.SetBackgroundColor(Color.Black);
			//SaveFilesManager.GetInstance().downloadFile(DBAppType,1);
			if (AppInfo.welcomeImageMD5 != null)
			{
				textKioskTitle.Visibility = ViewStates.Gone;
				KioskLogotip.Visibility = ViewStates.Visible;
				Android.Net.Uri url = Android.Net.Uri.Parse("file:///" + AppInfo.welcomeImageLocalPath + "/" +AppInfo.welcomeImageMD5+"."+ AppInfo.welcomeImageFileType );
				KioskLogotip.SetImageURI(url);
			//	touchlayout.SetBackgroundResource(loca);
			}
			else
			{
				textKioskTitle.Visibility = ViewStates.Visible;
				KioskLogotip.Visibility = ViewStates.Gone;
			}
			touchlayout.Touch += delegate
			{
					StartActivity(typeof(ActivityEmailKiosk));
					Finish();
				};
			breakHomescreen.Click += (object sender, EventArgs e) => 
				{
					newFragment = new FragmentsInputPassword(this);
					var ft1 = FragmentManager.BeginTransaction();

					newFragment.Show(ft1,"FragmentsInputPassword");
					//newFragment.Show();

				};
			
			
		}
		public override void OnBackPressed ()
		{
			newFragment = new FragmentsInputPassword (this);
			var ft1 = FragmentManager.BeginTransaction ();

			newFragment.Show (ft1, "FragmentsInputPassword");

		}


	}
}

