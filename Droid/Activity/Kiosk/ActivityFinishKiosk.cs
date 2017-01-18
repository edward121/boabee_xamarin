
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
using System.Threading;
using Java.Util;
using Android.Graphics;
using Android.Content.PM;
using BoaBeeLogic;
using System.Threading.Tasks;

namespace Leadbox
{

    [Activity(Label = "ActivityFinishKiosk", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]
    public class ActivityFinishKiosk : Activity
    {
        ConnectManager.NetworkState _state;
        BSCustomers bs = new BSCustomers();
        BSAnswersUpdate answers_update = new BSAnswersUpdate();
        Dialog alertDialog;
        System.Timers.Timer t;
        public static string uid;

        protected override void OnResume()
        {
            base.OnResume();
            _state = ConnectManager.GetInstance().stateNet();
            SendData();

        }

        async void SendData()
        {
            DBKioskSettings kioskSettings = DBLocalDataStore.GetInstance().GetKioskSettings();
            try
            {
                OfflineLogic.prepareSync(true);
                await NetworkRequests.SyncDataServer();
                if (kioskSettings != null && kioskSettings.badgePrinting && !string.IsNullOrWhiteSpace(kioskSettings.badgePrintingWebhook))
                {
                    if (await Reachability.isWebhookAvailable() == true)
                    {
                        await NetworkRequests.sendBadge(DBLocalDataStore.GetInstance().GetLocalContacts().Find(s => s.activeContact), kioskSettings.badgePrintingWebhook);
                    }
                    else {
                        Toast.MakeText(this, "Sending failed.", ToastLength.Short).Show();
                    }
                }
            }
            catch { 
            Toast.MakeText(this, "Sending failed.", ToastLength.Short).Show();
            
            }
            OfflineLogic.ClearDataSelected();

        }




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TouchFinishLayout);
            var textKioskfinish = FindViewById<TextView>(Resource.Id.textView1);
            var KioskLogotipFinish = FindViewById<ImageView>(Resource.Id.imageView);
            var apptype = DBLocalDataStore.GetInstance().GetAppInfo();
            var kioskSettings = DBLocalDataStore.GetInstance().GetKioskSettings();



            KioskLogotipFinish.SetBackgroundColor(Color.Black);

            if (kioskSettings == null)
            {
                kioskSettings = new DBKioskSettings();
            }
            if (apptype.finishedImageMD5 != null)
            {
                textKioskfinish.Visibility = ViewStates.Gone;
                KioskLogotipFinish.Visibility = ViewStates.Visible;
                Android.Net.Uri url = Android.Net.Uri.Parse("file:///" + apptype.finishedImageLocalPath + "/" + apptype.finishedImageMD5 + "." + apptype.finishedImageFileType);
                KioskLogotipFinish.SetImageURI(url);
                //  touchlayout.SetBackgroundResource(loca);
            }
            else
            {
                textKioskfinish.Visibility = ViewStates.Visible;
                KioskLogotipFinish.Visibility = ViewStates.Gone;
            }

            //if(kioskSettings.badgePrintingWebhook != null){
            //ConnectManager.GetInstance().SetWebHook(SaveAndLoad.tempContact, WebHookResponseHandler,kioskSettings.badgePrintingWebhook);
            //}
            t = new System.Timers.Timer();
            t.Interval = 10000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Start();




        }
        void SendCompleted(bool flag, string message)
        {
            RunOnUiThread(() =>
            {
                if (flag)
                {
                    base.OnResume();
                }
                else {
                    Console.WriteLine(message);
                }
            });

        }
        public void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            StartActivity(typeof(ActivityTouchKiosk));
            Finish();
        }
        void WebHookResponseHandler(string message)
        {
            RunOnUiThread(() =>
            {

            });
        }
    }
}

