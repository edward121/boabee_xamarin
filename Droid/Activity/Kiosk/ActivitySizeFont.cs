
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BoaBeePCL;
namespace Leadbox
{
	[Activity (Label = "ActivitySizeFont", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]
	public class ActivitySizeFont : Activity
	{
		private string typeFont;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.SizeLayout);

			string [] strMass = Intent.GetStringArrayExtra ("InfoSize");
			typeFont = strMass [0];

			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			var textQuestionSize = FindViewById<TextView> (Resource.Id.textView1);
			var textSmall = FindViewById<TextView> (Resource.Id.textView2);
			var textMedium = FindViewById<TextView> (Resource.Id.textView3);
			var textLarge = FindViewById<TextView> (Resource.Id.textView4);
			var buttonClose = FindViewById<TextView> (Resource.Id.textView5);

			var layoutSmallSize = FindViewById<RelativeLayout> (Resource.Id.layoutSmallSize);
			var layoutMediumSize = FindViewById<RelativeLayout> (Resource.Id.layoutMediumSize);
			var layoutLargeSize = FindViewById<RelativeLayout> (Resource.Id.layoutLargeSize);

			textQuestionSize.SetTypeface (font, TypefaceStyle.Normal);
			textSmall.SetTypeface (font, TypefaceStyle.Normal);
			textMedium.SetTypeface (font, TypefaceStyle.Normal);
			textLarge.SetTypeface (font, TypefaceStyle.Normal);
			buttonClose.SetTypeface (font, TypefaceStyle.Normal);
			layoutSmallSize.Click += (object sender, EventArgs e) => 
			{
				if(typeFont == "question")
				{
					DBQuestionFontSize size = new DBQuestionFontSize ();
					size.size = 15;
					DBLocalDataStore.GetInstance ().SetQuestionFontSize (size);
				}
				else if(typeFont == "answer")
				{
					DBAnswerFontSize size = new DBAnswerFontSize ();
					size.size = 15;
					DBLocalDataStore.GetInstance ().SetAnswerFontSize (size);
				}
				StartActivity (typeof (ActivityColorPicker));
				Finish ();
			};

			layoutMediumSize.Click += (object sender, EventArgs e) => 
			{
				if (typeFont == "question") {
					DBQuestionFontSize size = new DBQuestionFontSize ();
					size.size = 18;
					DBLocalDataStore.GetInstance ().SetQuestionFontSize (size);
				} else if (typeFont == "answer") {
					DBAnswerFontSize size = new DBAnswerFontSize ();
					size.size = 18;
					DBLocalDataStore.GetInstance ().SetAnswerFontSize (size);
				}
				StartActivity (typeof (ActivityColorPicker));
				Finish ();
			};
			layoutLargeSize.Click += (object sender, EventArgs e) =>
			{
				if (typeFont == "question") {
					DBQuestionFontSize size = new DBQuestionFontSize ();
					size.size = 21;
					DBLocalDataStore.GetInstance ().SetQuestionFontSize (size);
				} else if (typeFont == "answer") {
					DBAnswerFontSize size = new DBAnswerFontSize ();
					size.size = 21;
					DBLocalDataStore.GetInstance ().SetAnswerFontSize (size);
				}
				StartActivity (typeof (ActivityColorPicker));
				Finish ();
			};
			buttonClose.Click += (object sender, EventArgs e) =>
			{
				
				StartActivity (typeof (ActivityColorPicker));
				Finish ();
			};

			// Create your application here
		}
	}
}

