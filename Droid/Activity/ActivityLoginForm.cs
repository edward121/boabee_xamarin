using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text.Method;
using Android.Widget;
using BoaBeeLogic;
using BoaBeePCL;
using Newtonsoft.Json;

namespace Leadbox
{
	[Activity(Label = "ActivityLoginForm", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityLoginForm : Activity
	{
		DBUserLoginRequest user = new DBUserLoginRequest ();
		ExchengeInfo receivedData;
		Dialog alertDialog;
		TextView buttonLogin;
		int countConnection = 0;
		//AlertDialog.Builder ad = new AlertDialog.Builder (this);
		ConnectManager.NetworkState _state;

		protected override void OnResume ()
		{
            base.OnResume();
            _state = ConnectManager.GetInstance ().stateNet ();
			Console.WriteLine ("state net = " + _state.ToString());
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView (Resource.Layout.LoginLayout);



			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			//var loginTextTop = FindViewById<TextView>(Resource.Id.textlogin);
			var userLogin = FindViewById<EditText>(Resource.Id.editlogin);
			var userPassword = FindViewById<EditText>(Resource.Id.editPassword);
			//var gotoRegistr = FindViewById<TextView>(Resource.Id.gotoRegister);
			var testButton = FindViewById<TextView> (Resource.Id.textView1);
			//var gotoForgotPass = FindViewById<TextView>(Resource.Id.gotoForgotpassword);
            var userName = FindViewById<EditText>(Resource.Id.editName);
            var btnEye = FindViewById<ImageView>(Resource.Id.ImageEye);
			buttonLogin = FindViewById<TextView>(Resource.Id.btnLogin);
			alertDialog = new Dialog(this, Resource.Style.TransparentProgressDialog);
			alertDialog.SetCancelable (false);
			alertDialog.SetCanceledOnTouchOutside (false);
			alertDialog.SetContentView(Resource.Layout.custom_progressdialog); //SetView (progressLoading);
                                                                               //var service = new Service (this, alertDialog);

            //#if DEBUG
            //{
            //    userLogin.Text = "kolibrisoftware@gmail.com";//
            //    userPassword.Text = "St@rtN0w";//
            //    userName.Text = "TestName";
            //}
            //#else
            //{
            //    //.Text = "";
            //    //userPassword.Text = "";
            //    userLogin.Text = "kolibrisoftware@gmail.com";//
            //    userPassword.Text = "St@rtN0w";//
            //    userName.Text = "TestName";
            //}
            //#endif

            btnEye.Click += delegate {
                if (userPassword.TransformationMethod == PasswordTransformationMethod.Instance)
                {
                    btnEye.Background = ContextCompat.GetDrawable(Application, Resource.Drawable.eye_crossed);
                    userPassword.TransformationMethod = HideReturnsTransformationMethod.Instance;
                }
                else {
                    btnEye.Background = ContextCompat.GetDrawable(Application, Resource.Drawable.EyeIcon);
                    userPassword.TransformationMethod = PasswordTransformationMethod.Instance;
                }
            };

            userName.SetTypeface(font, TypefaceStyle.Normal);
            userLogin.SetTypeface (font, TypefaceStyle.Normal);
			userPassword.SetTypeface (font, TypefaceStyle.Normal);
			buttonLogin.SetTypeface (font, TypefaceStyle.Normal);
            userPassword.TransformationMethod = PasswordTransformationMethod.Instance;

			//gotoForgotPass.PaintFlags = PaintFlags.UnderlineText;  // (Paint.UNDERLINE_TEXT_FLAG);
			//gotoRegistr.PaintFlags = PaintFlags.UnderlineText;

			buttonLogin.Click += (sender, e) => {
				buttonLogin.Clickable = true;
				//StartActivity(typeof(ActivitySelectApp));
				//Finish();
				 alertDialog.Show ();
				user.username = userLogin.Text;
                user.password = userPassword.Text;
                if (string.IsNullOrEmpty(userName.Text))
                {
                    user.tags = userLogin.Text ;
                }
                else {
                    user.tags = userName.Text ;
                }
                //
                //
               
       

				_state = ConnectManager.GetInstance ().stateNet ();

				if(ValidateForm(userLogin, userPassword))
				{


                    if (_state == ConnectManager.NetworkState.ConnectedWifi || _state == ConnectManager.NetworkState.ConnectedData)
                        Task.Run(async () =>
                        {
                            await NetworkRequests.performAuth(user, (success, messageTitle, message, list) =>
                            {
                                this.RunOnUiThread(() =>
                                {
                                    if (success)
                                    {
                                        if (message != "OK")
                                        {
                                            Toast.MakeText(this, message, ToastLength.Long).Show();
                                            alertDialog.Dismiss();
                                        }
                                        else {
                                            StartActivity(typeof(ActivitySelectApp));
                                            Finish();
                                        }
                                    }
                                    else
                                    {
                                        Toast.MakeText(this, message, ToastLength.Long).Show();
                                        alertDialog.Dismiss();
                                    }
                                });
                            });
                    });
					else {
						buttonLogin.Clickable = true;
						alertDialog.Dismiss();
						popupDialog("No internet access, try again later.");
						//Toast.MakeText(this, "No internet access, try again later.", ToastLength.Long).Show();
					}

				}
				else
				{
					buttonLogin.Clickable = true;
					alertDialog.Dismiss();
					Toast.MakeText(this, "E-mail or password incorrect.", ToastLength.Short).Show();
				}
			};
			//testButton.Click += (object sender, EventArgs e) => { 
				
			//};
			//gotoRegistr.Click += (object sender, EventArgs e) => {
			//	var uri = Android.Net.Uri.Parse ("https://boabee.com/signup/");
			//	var intent = new Intent (Intent.ActionView, uri); 
			//	StartActivity (intent); 
			//};

			//gotoForgotPass.Click += (object sender, EventArgs e) => {
			//	var uri = Android.Net.Uri.Parse ("https://cloud.boabee.com/");
			//	var intent = new Intent (Intent.ActionView, uri); 
			//	StartActivity (intent); 
			//};

		}

        private bool ValidateForm(EditText user, EditText passwd)
		{
			//var userNameText = user.Text;
			//if (string.IsNullOrWhiteSpace (userNameText))
			//	return false;

			////if(!isEmail(userNameText))
			////	return false;


			//var passwordText = passwd.Text;
			//if (string.IsNullOrEmpty (passwordText))
			//	return false;

			return true;

		}

		private bool isEmail(string email)
		{
			var regexPatter = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$";

			if(Regex.Match(email,regexPatter).Success)
				return true;

			return false;
		}




		public async void Auth()
		{
			try{
                
			var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/auth/login");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			httpWebRequest.ServicePoint.ConnectionLimit = 3;
			//httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Timeout = 10000;

			using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

				//streamWriter.Write (jsonUser);
			}

				Task<WebResponse> task = Task.Factory.FromAsync(
					httpWebRequest.BeginGetResponse,
					asyncResult => httpWebRequest.EndGetResponse(asyncResult),
					(object)null);
				await task.ContinueWith(t => ReadStreamFromResponse(t.Result));
			}
			catch(Exception ex){
				popupDialog("Incorrect credentials. Please try again.1");
				Console.WriteLine (ex.Message);
			}

		}

		private string ReadStreamFromResponse (WebResponse result)
		{
			try{
				var httpResponse = result.GetResponseStream ();
				using (var streamReader = new StreamReader (httpResponse)) {
					var result1 = streamReader.ReadToEnd ();
					Console.WriteLine (result1);
					receivedData = JsonConvert.DeserializeObject<ExchengeInfo> (result1);
					resultLogin (receivedData);
					return result1;
				}
			}
			catch(Exception ex)
			{
				popupDialog("Incorrect credentials. Please try again.");
				Console.WriteLine (ex.Message);
			}

			return "";
		}







		void TrueLogin (ExchengeInfo data)
		{
			var lds = DBLocalDataStore.GetInstance ();
            lds.AddUserInfo(user);
			if (data.profiles.Length == 0) {
				popupDialog ("You currently have no app setups available. Go to your dashboard and create one.");
			}
			else{
				lds.ClearBasicAuthority ();
				foreach (var tt in data.profiles) {
					lds.AddBasicAuthority (tt);
				}
				StartActivity (typeof(ActivitySelectApp));
				Finish ();
			}
		}

		void resultLogin(ExchengeInfo data)
		{
			RunOnUiThread (() =>
            {
                alertDialog.Dismiss();
                if (data.success){
				//if (data.success == "True") {
					TrueLogin (data);
				}
				else {
					popupDialog("Incorrect credentials. Please try again");
					buttonLogin.Clickable = true;
				}
			});

		}

		public void popupDialog(string alertMassege)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
			AlertDialog ad = builder.Create ();
			ad.SetMessage (alertMassege);
			ad.SetTitle ("Alert:");
			ad.SetButton ("Ok",(s, ev) => {
				buttonLogin.Clickable = true;
				alertDialog.Dismiss ();
			});
			ad.Show ();
		}

		public void popupDialog(string alertMassege, bool flagAuth)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this, Resource.Style.TransparentProgressDialog);
			AlertDialog ad = builder.Create ();
			ad.SetMessage (alertMassege);
			ad.SetTitle ("Error:");
			ad.SetButton ("Ok",(s, ev) => {
				if (flagAuth){
                    Task.Run(async () =>
                        {
                            await NetworkRequests.performAuth(user, (success, messageTitle, message, list) =>
                            {
                                this.RunOnUiThread(() =>
                                {
                                    if (success)
                                    {
                                        if (message != "OK")
                                        {
                                            Toast.MakeText(this, message, ToastLength.Long).Show();
                                            alertDialog.Dismiss();
                                        }
                                        else {
                                            StartActivity(typeof(ActivitySelectApp));
                                            Finish();
                                        }
                                    }
                                    else
                                    {
                                        Toast.MakeText(this, message, ToastLength.Long).Show();
                                        alertDialog.Dismiss();
                                    }
                                });
                            });
                        });
				}
			});
			ad.Show ();
		}

		public override void OnBackPressed ()
		{
			//base.OnBackPressed ();
			Finish();
		}

	}
}

