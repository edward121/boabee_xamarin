
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
using Android.Graphics;
using Java.Net;
using BoaBeePCL;
using System.Threading.Tasks;
using BoaBeeLogic;
using Android.Text.Method;

namespace Leadbox
{
	[Activity(Label = "ActivitySelectApp", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivitySelectApp : Activity
	{
		Dialog alertDialog;
		ConnectManager.NetworkState _state;
        string appname ="";
        bool fromSettings = false;


        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView (Resource.Layout.SelectAppLayout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			var tt = "";
			tt = Intent.GetStringExtra ("statusH");
            appname = Intent.GetStringExtra("NameApp");
            fromSettings = Intent.GetBooleanExtra("FromSettings", false);

			var TextTop = FindViewById<TextView>(Resource.Id.textselectapp);

			var buttonCancel = FindViewById<TextView>(Resource.Id.buttonCancel);
			var buttonSwitch = FindViewById<TextView>(Resource.Id.buttonSwitchuser);

			alertDialog = new Dialog(this, Resource.Style.TransparentProgressDialog);
			alertDialog.SetCancelable (false);
			alertDialog.SetCanceledOnTouchOutside (false);
			alertDialog.SetContentView(Resource.Layout.custom_progressdialog); //SetView (progressLoading);
			alertDialog.Show ();
			//PaintData ();

			TextTop.SetTypeface (font, TypefaceStyle.Normal);
			buttonCancel.SetTypeface (font, TypefaceStyle.Normal);
			buttonSwitch.SetTypeface (font, TypefaceStyle.Normal);

			//buttonCancel.Text = buttonCancel.Text.ToUpper ();


			buttonCancel.Click += (object sender, EventArgs e) => {
				if (string.IsNullOrEmpty( tt))
				{
					//StartActivity(typeof(ActivitySettingsScreen));
					Finish();
				}
				else
				{
					StartActivity(typeof(ActivitySettingsScreen));
					Finish();
				}
			};

			buttonSwitch.Click += (object sender, EventArgs e) => {
				StartActivity(typeof(ActivityLoginForm));
				DBLocalDataStore.GetInstance().ClearUserInfo();
				DBLocalDataStore.GetInstance().ClearSelectProfile();
				Finish();

			};
		
		}

        async void PaintData()
        {
            _state = ConnectManager.GetInstance().stateNet();
            try
            {
                if (_state == ConnectManager.NetworkState.ConnectedWifi || _state == ConnectManager.NetworkState.ConnectedData)
                {
                    var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();
                    //await Task.Run(async () => (await NetworkRequests.getProfiles(user)));
                    await NetworkRequests.getProfiles(user);
                }
            }catch
            {
                openPasDialog();
            }

            var prof = DBLocalDataStore.GetInstance().GetProfiles();

            prof = prof.AsEnumerable().OrderBy(s => s.displayName).ToList<DBBasicAuthority>();
            var listitems = FindViewById<ListView>(Resource.Id.listViewApps);

            listitems.Adapter = new ProfilesListAdapter(this, prof);
            listitems.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                if (ConnectManager.GetInstance().StateNet())
                {
                    alertDialog.Show();
                    var _select_profile = DBLocalDataStore.GetInstance().GetSelectProfile();

                    if (_select_profile != null)
                    {
                        if (appname == prof[e.Position].displayName)
                        {
                            StartActivity(typeof(ActivityReceivingInformation));
                            Finish();
                            return;
                        }
                    }
                    DBLocalDataStore.GetInstance().ClearDefaultFileTO();

                    DBLocalDataStore.GetInstance().ClearAllOverwievContacts();
                    DBLocalDataStore.GetInstance().ClearAllOverwievFiles();
                    DBLocalDataStore.GetInstance().ClearAllOverwievAnswers();
                    //SaveAndLoad.GetInstance ().DeleteFile ();
                    DBLocalDataStore.GetInstance().AddSelectProfile(prof[e.Position].displayName);
                    DBLocalDataStore.GetInstance().clearSyncRequests();
                    StartActivity(typeof(ActivityReceivingInformation));
                    Finish();
                }
                else {
                    Toast.MakeText(this, "Access to internet is faild.", ToastLength.Short).Show();
                }
                //DBLocalDataStore.GetInstance().ClearBasicAuthority();
                //StartActivity(typeof(ActivityReceivingInformation));
            };
            alertDialog.Dismiss();
        }

        void returnResult(string m)
		{
			//Toast.MakeText(this, m, ToastLength.Short).Show();
			Console.WriteLine(m);
			PaintData ();
		}

		protected override void OnResume ()
		{
            base.OnResume();
			_state = ConnectManager.GetInstance ().stateNet ();
			Console.WriteLine ("state net = " + _state.ToString());
			if (_state == ConnectManager.NetworkState.ConnectedWifi || _state == ConnectManager.NetworkState.ConnectedData) {
                try
                {
                    ConnectManager.GetInstance().updateProfiles(returnResult);
                }
                catch
                {
                    openPasDialog();

                }
			} else {
				alertDialog.Dismiss();
				Toast.MakeText (this, "Access to internet is faild.", ToastLength.Short).Show ();
			}

			//ConnectManager.GetInstance ().updateProfiles ();
		}

		protected override void OnDestroy ()
		{
			//DBLocalDataStore.GetInstance().ClearBasicAuthority();
			base.OnDestroy ();
		}



		public override void OnBackPressed ()
		{
			//base.OnBackPressed ();
		}
        public void openPasDialog()
        { 
            AlertDialog.Builder builder_ = new AlertDialog.Builder(this, Resource.Style.TransparentProgressDialog);
            AlertDialog ad_ = builder_.Create();
            ad_.SetCancelable(false);
            ad_.SetCanceledOnTouchOutside(false);
            ad_.SetMessage(string.Format("Your password has been changed. Your work will NOT be synced until the most recent password is entered"));
            ad_.SetTitle("WARNING");
            ad_.SetButton("Later".ToUpper(), delegate (object so, DialogClickEventArgs events)
                            {
                                StartActivity(typeof(ActivityHomescreen));
                                Finish();
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
                                userInfo.invalidPassword = false;
                                DBLocalDataStore.GetInstance().AddUserInfo(userInfo);
                                alertDialog.Show();                
                                ConnectManager.GetInstance().updateProfiles(returnResult);
                            });
            ad_.Show();
        
        }
	}

}

