
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
//using MonoDroid.ColorPickers;
using System.Diagnostics;
using Android.Support.V7.Widget;

namespace Leadbox
{
	[Activity(Label = "ActivitySettingsKiosk" , ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivitySettingsKiosk : Activity
	{

		Dialog alertDialog;
		public DBKioskSettings settingKiosk ;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			settingKiosk =  DBLocalDataStore.GetInstance().GetKioskSettings();
			SetContentView (Resource.Layout.KioskSettingLayout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			var textKioskTitle = FindViewById<TextView> (Resource.Id.textView2);
			var textKioskSetting = FindViewById<TextView> (Resource.Id.textView1);
			var textApperance = FindViewById<TextView> (Resource.Id.textApperance);
			var textContacts = FindViewById<TextView> (Resource.Id.textContacts);
			var textWelcome = FindViewById<EditText> (Resource.Id.editTextTitle);
			var textBadgePrinting = FindViewById<TextView> (Resource.Id.textView4);
			var textWebhook = FindViewById<TextView> (Resource.Id.textViewwebhook);
			var textlink = FindViewById<EditText> (Resource.Id.editText1);
			var textback = FindViewById<TextView> (Resource.Id.textClose);
			var textswitch = FindViewById<Android.Widget.Switch> (Resource.Id.switchCompat);
			var textColorPicker = FindViewById<RelativeLayout> (Resource.Id.LayoutKioskColorPicker);
			var layoutKioskContacts = FindViewById<RelativeLayout> (Resource.Id.LayoutKioskContacts);
            var LayoutForKiosok = FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
            var LayoutForApp = FindViewById<RelativeLayout>(Resource.Id.relativeLayout6);
            var textContactCheck = FindViewById<TextView>(Resource.Id.TextContactCheck);
            var textback2 = FindViewById<TextView>(Resource.Id.textClose1);
            var TextTitle = FindViewById<TextView>(Resource.Id.textTitleApp);
            var ContactCheck = FindViewById<Android.Widget.Switch>(Resource.Id.ContactCheckSwitch);
            var getContacts = FindViewById<Android.Widget.Switch>(Resource.Id.getcontactsSwitch);

            TextTitle.SetTypeface(font, TypefaceStyle.Normal);
            textback2.SetTypeface(font, TypefaceStyle.Normal);
            textContactCheck.SetTypeface(font, TypefaceStyle.Normal);
            textKioskTitle.SetTypeface (font, TypefaceStyle.Normal);
			textKioskSetting.SetTypeface (font, TypefaceStyle.Normal);
			textWelcome.SetTypeface (font, TypefaceStyle.Normal);
			textBadgePrinting.SetTypeface (font, TypefaceStyle.Normal);
			textWebhook.SetTypeface (font, TypefaceStyle.Normal);
			textlink.SetTypeface (font, TypefaceStyle.Normal);
			textback.SetTypeface (font, TypefaceStyle.Normal);
			textContacts.SetTypeface (font, TypefaceStyle.Normal);
			textApperance.SetTypeface (font, TypefaceStyle.Normal);
			textswitch.SetTypeface (font, TypefaceStyle.Normal);
			textswitch.SetTypeface (font, TypefaceStyle.Normal);

            var AppInfo = DBLocalDataStore.GetInstance().GetAppInfo();
            if (AppInfo.appType == "kiosk")
            {
                LayoutForKiosok.Visibility = ViewStates.Visible;
                textKioskSetting.Text = "APP SETTING";
            }
            else
            {
                LayoutForKiosok.Visibility = ViewStates.Gone;
                textKioskSetting.Text = "KIOSOK SETTING";
            }
			if (settingKiosk == null) {
				settingKiosk = new DBKioskSettings ();
				textWelcome.Text = "Welcome!";
			} else {
				if (settingKiosk.kioskTitle == null){
					textWelcome.Text = "Welcome!";
				}
				else
				{
					textWelcome.Text = settingKiosk.kioskTitle;
					//textswitch.Checked = settingKiosk.badgePrinting;
				}
			}
            DBAppSettings appSettings = new DBAppSettings();
            appSettings = DBLocalDataStore.GetInstance().GetAppSettings();
            ContactCheck.Checked = appSettings.instantContactCheck;
            ContactCheck.Click += delegate
            {
                appSettings.instantContactCheck = !appSettings.instantContactCheck;
                DBLocalDataStore.GetInstance().SetAppSettings(appSettings);
            };
            getContacts.Checked = appSettings.getSharedContacts;
            getContacts.Click += delegate
            {
                appSettings.getSharedContacts = !appSettings.getSharedContacts;
                DBLocalDataStore.GetInstance().SetAppSettings(appSettings);
            };

            layoutKioskContacts.Click += (object sender, EventArgs e) => { 
				StartActivity (typeof (ActivitySelectContact));
				Finish ();
			};
			textWelcome.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				settingKiosk.kioskTitle = e.Text.ToString();
			};
			textColorPicker.Click += (object sender, EventArgs e) => 
			{
				StartActivity (typeof (ActivityColorPicker));
				Finish ();
			};
			//if(settingKiosk == null)
			//{
			//	settingKiosk = new DBKioskSettings();
			//}
			textlink.Text = settingKiosk.badgePrintingWebhook;
            if (settingKiosk.kioskTitle != null)
            {
                settingKiosk.kioskTitle = textWelcome.Text;
            }
            if (settingKiosk.badgePrintingWebhook == null)
            {
                settingKiosk.badgePrintingWebhook = textlink.Hint;
            }
			textlink.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => 
				{
					settingKiosk.badgePrintingWebhook = textlink.Text;
				};
			if(settingKiosk !=null)
			{
			textswitch.Checked = settingKiosk.badgePrinting;
			}
			else
			{
				textswitch.Checked = true;
			}
			// Create your application here
			textswitch.Click += delegate
			{
				textswitch.SetHintTextColor (Color.Rgb (0xed, 0xcd, 0x00));
				textswitch.SetHighlightColor (Color.Rgb (0xed, 0xcd, 0x00));
				textswitch.SetHighlightColor (Color.Rgb (0xed, 0xcd, 0x00));
				textswitch.SetTextColor (Color.Rgb (0xed, 0xcd, 0x00));

				if (textswitch.Checked == true)
			{
					textswitch.SetHintTextColor (Color.Rgb (0xed, 0xcd, 0x00));
					textswitch.SetHighlightColor (Color.Rgb (0xed, 0xcd, 0x00));
					textswitch.SetHighlightColor(Color.Rgb (0xed, 0xcd, 0x00));
					textswitch.SetTextColor(Color.Rgb (0xed, 0xcd, 0x00));
					settingKiosk.kioskTitle = textWelcome.Text;
					settingKiosk.badgePrinting = true;

					}
			else
			{
				settingKiosk.kioskTitle = textWelcome.Text;
				settingKiosk.badgePrinting = false;
			}

				};

			textColorPicker.Click += (object sender, EventArgs e) => 
			{
				StartActivity (typeof(ActivityColorPicker));
			};
			textback.Click += delegate
			{
				DBLocalDataStore.GetInstance().SetKioskSettings(settingKiosk);
				StartActivity (typeof (ActivitySettingsScreen));
				Finish();
			};
            textback2.Click += delegate
            {
               
                StartActivity(typeof(ActivitySettingsScreen));
                Finish();
            };
		}

		void WebHookResponseHandler(string message)
		{
			RunOnUiThread(()=>{
				
			});
		}

	}
}

