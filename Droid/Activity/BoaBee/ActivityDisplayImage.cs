
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
using Android.Webkit;
using Android.Graphics;
using Android.Telephony.Gsm;
using Android.Content.PM;

namespace Leadbox
{
	[Activity(Label = "ActivityDisplayImage",ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityDisplayImage : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.DisplayImage);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			var ID = Intent.GetStringExtra("id_form_overview");
			var imageView = FindViewById<ImageView>(Resource.Id.imagedisplayview);
			var closeimage = FindViewById<TextView>(Resource.Id.CloseImage);
			closeimage.SetTypeface (font, TypefaceStyle.Normal);

			imageView.SetBackgroundColor(Color.Black);
			Android.Net.Uri url = Android.Net.Uri.Parse("file:///" + ID);
			imageView.SetImageURI(url);
//			imageView.Settings.SetSupportZoom(true);
//			imageView.LoadUrl("file:///" + ID);
//			imageView.Settings.SetSupportZoom(true);
//
//			webView.Settings.BuiltInZoomControls = true;
//			webView.SetPadding(0, 0, 0, 0);
//
			closeimage.Click  += delegate
			{
					Finish();
				};
//			Bitmap img = imageUtil.getImageBitmap();
//
//			int picWidth = img.getWidth();
//			int picHeight = img.getHeight();
//			// Create your application here
		}

		public override void Finish()
		{
			GC.Collect();
			base.Finish();
		}
	}
}
