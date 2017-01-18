
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
using Com.Joanzapata.Pdfview;
using Com.Joanzapata.Pdfview.Listener;

namespace Leadbox
{
	[Activity(Label = "ActivityDisplayPdf",ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]			
	public class ActivityDisplayPdf : Activity, IOnPageChangeListener
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.DisplayPdf);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			var filePath = Intent.GetStringExtra("id_form_overview");
			var PDFView = FindViewById<PDFView>(Resource.Id.pdfView1);
			var closebtn = FindViewById<TextView>(Resource.Id.dCancleDisplayView);
			closebtn.SetTypeface (font, TypefaceStyle.Normal);
			closebtn.Click += (sender, e) => Finish();
			var fileNeedShow = new Java.IO.File(filePath);
			PDFView.FromFile(fileNeedShow)
				.DefaultPage (1)
				.OnPageChange(this)
				.EnableDoubletap (true)
				//.SwipeVertical(true)
				.Load ();
			// Create your application here
		}
		void IOnPageChangeListener.OnPageChanged (int p0, int p1)
		{
			System.Diagnostics.Debug.WriteLine ("Change to Page " + p0);
		}

		public override void Finish()
		{
			GC.Collect();
			base.Finish();
		}

	}

}

