
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

namespace Leadbox
{
	[Activity(Label = "ActivitySelectQuestion", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivitySelectQuestion : Activity
	{
		Dialog alertDialog;
		ConnectManager.NetworkState _state;
		//select the questionnaire that you want to use on the clussify tab
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			NotificationManager _notify = GetSystemService ("notification") as NotificationManager;
			_notify.CancelAll ();

			SetContentView (Resource.Layout.SelectQuestionsLayout);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			var TextTop = FindViewById<TextView>(Resource.Id.textselectapp);
			var TextTopInfo = FindViewById<TextView>(Resource.Id.textInfo);

			var buttonCancel = FindViewById<TextView>(Resource.Id.buttonCancel);




			alertDialog = new Dialog(this, Resource.Style.TransparentProgressDialog);
			alertDialog.SetCancelable (false);
			alertDialog.SetCanceledOnTouchOutside (false);
			alertDialog.SetContentView(Resource.Layout.custom_progressdialog); //SetView (progressLoading);
			alertDialog.Show ();

			//PaintData ();
			TextTopInfo.Text = string.Format ("select the questionnaire that you \nwant to use on the classify tab");
			TextTop.SetTypeface (font, TypefaceStyle.Normal);
			TextTopInfo.SetTypeface (font, TypefaceStyle.Normal);
			buttonCancel.SetTypeface (font, TypefaceStyle.Normal);


			buttonCancel.Click += (object sender, EventArgs e) => {
				StartActivity(typeof(ActivitySelectApp));
				Finish();
			};

			PaintData ();
		}

		void PaintData()
		{
			var prof = DBLocalDataStore.GetInstance ().GetLocalFormDefinitions ();

			if (prof.Count == 0) {
				StartActivity(typeof(ActivityHomescreen));
				Finish();
				return;
			}

			if (prof.Count == 1) {
				DBLocalDataStore.GetInstance().SetSelectedFormDefinitions(prof[0].uuid);
				SaveAndLoad.GetInstance ().DeleteFile ();
				StartActivity(typeof(ActivityHomescreen));
				Finish();
				return;
			}
				

			prof = prof.AsEnumerable ().OrderBy (s => s.objectName).ToList();
			var listitems = FindViewById<ListView> (Resource.Id.listViewApps);

			listitems.Adapter = new QuestionSelectListAdapter (this, prof);
			listitems.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				DBLocalDataStore.GetInstance().SetSelectedFormDefinitions(prof[e.Position].uuid);
				SaveAndLoad.GetInstance ().DeleteFile ();
				StartActivity(typeof(ActivityHomescreen));
				Finish();
				//DBLocalDataStore.GetInstance().ClearBasicAuthority();
				//StartActivity(typeof(ActivityReceivingInformation));
			};
			alertDialog.Dismiss();
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

	}
}

