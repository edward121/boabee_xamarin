
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using BoaBeePCL;

namespace Leadbox
{
	[Activity (Label = "ActivityColorPicker", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]
	public class ActivityColorPicker : Activity
	{
		

		private ColorPickerPanelView _panelNoAlpha;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.ColorPickerLayout);

			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			var textQuestionSize = FindViewById<TextView> (Resource.Id.textQuestionSize);
			var textQuestionFontSize = FindViewById<TextView> (Resource.Id.textQuestionFontSize);
			var textQuestionFontColor = FindViewById<TextView> (Resource.Id.textQuestionFontColor);
			var textQuestionBackgroundColor = FindViewById<TextView> (Resource.Id.textQuestionBackgroundColor);
			var textAnswerSize = FindViewById<TextView> (Resource.Id.textAnswerSize);
			var textAnswerFontSize = FindViewById<TextView> (Resource.Id.textAnswerFontSize);
			var textAnswerFontColor = FindViewById<TextView> (Resource.Id.textAnswerFontColor);
			var textTitle = FindViewById<TextView> (Resource.Id.textView1);
			var textAnswerBackgroundColor = FindViewById<TextView> (Resource.Id.textAnswerBackgroundColor);
			var buttonOk = FindViewById<TextView> (Resource.Id.buttonOk);
			var buttonBreak = FindViewById<TextView> (Resource.Id.buttonBreak);

			var layoutQuestionFontSize = FindViewById<RelativeLayout> (Resource.Id.layoutQuestionFontSize);
			var layoutAnswerFontSize = FindViewById<RelativeLayout> (Resource.Id.layoutAnswerFontSize);
			var layoutQuestionFontColor = FindViewById<RelativeLayout> (Resource.Id.layoutQuestionFontColor);
			var layoutAnswerFontColor = FindViewById<RelativeLayout> (Resource.Id.layoutAnswerFontColor);
			var layoutQuestionBackgroundColor = FindViewById<RelativeLayout> (Resource.Id.layoutQuestionBackgroundColor);
			var layoutAnswerBackgroundColor = FindViewById<RelativeLayout> (Resource.Id.layoutAnswerBackgroundColor);


			var colorPickerQuestionFontColor = FindViewById<ColorPickerPanelView> (Resource.Id.colorPickerViewQuestionFontColor);
			var colorPickerQuestionBackgroundColor = FindViewById<ColorPickerPanelView> (Resource.Id.colorPickerQuestionBackgroundColor);
			var colorPickerAnswerFontColor = FindViewById<ColorPickerPanelView> (Resource.Id.colorPickerAnswerFontColor);
			var colorPickerAnswerBackgroundColor = FindViewById<ColorPickerPanelView> (Resource.Id.colorPickerAnswerBackgroundColor);

			textQuestionSize.SetTypeface (font, TypefaceStyle.Normal);
			textTitle.SetTypeface (font, TypefaceStyle.Normal);
			textQuestionFontSize.SetTypeface (font, TypefaceStyle.Normal);
			textQuestionFontColor.SetTypeface (font, TypefaceStyle.Normal);
			textQuestionBackgroundColor.SetTypeface (font, TypefaceStyle.Normal);
			textAnswerSize.SetTypeface (font, TypefaceStyle.Normal);
			textAnswerFontSize.SetTypeface (font, TypefaceStyle.Normal);
			textAnswerFontColor.SetTypeface (font, TypefaceStyle.Normal);
			textAnswerBackgroundColor.SetTypeface (font, TypefaceStyle.Normal);
			textAnswerFontColor.SetTypeface (font, TypefaceStyle.Normal);
			textAnswerBackgroundColor.SetTypeface (font, TypefaceStyle.Normal);

			var questionFontSize = DBLocalDataStore.GetInstance ().GetQuestionFontSize ();
			var answerSize = DBLocalDataStore.GetInstance ().GetAnswerFontSize ();
			var questinoFontColor = DBLocalDataStore.GetInstance ().GetQuestionFontColor ();
			var answerFontColor = DBLocalDataStore.GetInstance ().GetAnswerFontColor ();
			var answerBackgroundColor = DBLocalDataStore.GetInstance ().GetAnswerBackgroundColor ();
			var questinoBackgroundColor = DBLocalDataStore.GetInstance ().GetQuestionBackgroundColor ();

			if (questionFontSize == null) {
				textQuestionSize.TextSize = 18;
			} else {
				textQuestionSize.TextSize = questionFontSize.size;
			}
			if (answerSize == null) {
				textAnswerSize.TextSize = 18;
			} else {
				textAnswerSize.TextSize = answerSize.size;
			}
			if (questinoFontColor == null) {
				colorPickerQuestionFontColor.Color = Color.Rgb(0,0,0);
			} else {
				var tempForColors = Color.Rgb ((Int32)questinoFontColor.redByte,(Int32)questinoFontColor.greenByte,(Int32)questinoFontColor.blueByte);
				colorPickerQuestionFontColor.Color=tempForColors;
  			}

			if (answerFontColor == null) {
				colorPickerAnswerFontColor.Color = Color.Rgb (237, 205, 0);
			} else {
				var tempForColors = Color.Rgb ((Int32)answerFontColor.redByte, (Int32)answerFontColor.greenByte, (Int32)answerFontColor.blueByte);
				colorPickerAnswerFontColor.Color = tempForColors;
			}

			if (answerBackgroundColor == null) {
				colorPickerAnswerBackgroundColor.Color = Color.Rgb (255, 255, 255);
			} else {
				var tempForColors = Color.Rgb ((Int32)answerBackgroundColor.redByte, (Int32)answerBackgroundColor.greenByte, (Int32)answerBackgroundColor.blueByte);
			//	colorPickerAnswerBackgroundColor.Color = tempForColors;
				colorPickerAnswerBackgroundColor.Color = tempForColors;
			}

			if (questinoBackgroundColor == null) {
				colorPickerQuestionBackgroundColor.Color = Color.Rgb (0x00, 0x00, 0x00);
			} else {
				var tempForColors = Color.Rgb ((Int32)questinoBackgroundColor.redByte, (Int32)questinoBackgroundColor.greenByte, (Int32)questinoBackgroundColor.blueByte);
			//	colorPickerQuestionBackgroundColor.Color = tempForColors;

				colorPickerQuestionBackgroundColor.Color = tempForColors;
			}

			if (questionFontSize == null) 
			{

			} 
			else 
			{
				switch (questionFontSize.size)
				{
				case 15:
					textQuestionSize.Text = "SMALL";
					break;
				case 18:
					textQuestionSize.Text = "MEDIUM";
					break;
				case 21:
					textQuestionSize.Text = "LARGE";
					break;
				}
			}
			var answerFontSize = DBLocalDataStore.GetInstance ().GetAnswerFontSize ();
			if (answerFontSize == null)
			{

			} 
			else
			{
				switch (answerFontSize.size) {
				case 15:
					textAnswerSize.Text = "Small";
					break;
				case 18:
					textAnswerSize.Text = "Medium";
					break;
				case 21:
					textAnswerSize.Text = "Large";
					break;
				}
			}
			layoutQuestionFontColor.Click += (object sender, EventArgs e) => {
				var activityColorFont = new Intent (this, typeof (ActivitySelectColor));
				activityColorFont.PutExtra ("InfoFontColor", new [] { "QuestionFontColor" });
				StartActivity (activityColorFont);
				Finish ();
			};
			layoutAnswerFontColor.Click += (object sender, EventArgs e) => {
				var activityColorFont = new Intent (this, typeof (ActivitySelectColor));
				activityColorFont.PutExtra ("InfoFontColor", new [] { "AnswerFontColor" });
				StartActivity (activityColorFont);
				Finish ();
			};
			layoutQuestionBackgroundColor.Click += (object sender, EventArgs e) => {
				var activityColorFont = new Intent (this, typeof (ActivitySelectColor));
				activityColorFont.PutExtra ("InfoFontColor", new [] { "QuestionBackgroundColor" });
				StartActivity (activityColorFont);
				Finish ();
			};
			layoutAnswerBackgroundColor.Click += (object sender, EventArgs e) => {
				var activityColorFont = new Intent (this, typeof (ActivitySelectColor));
				activityColorFont.PutExtra ("InfoFontColor", new [] { "AnswerBackgroundColor" });
				StartActivity (activityColorFont);
				Finish ();
			};
			buttonOk.Click += (object sender, EventArgs e) => 
			{
				StartActivity (typeof(ActivitySettingsKiosk));
				Finish ();
			};

			buttonBreak.Click += (object sender, EventArgs e) => 
			{
				StartActivity (typeof (ActivitySettingsKiosk));
				Finish ();
			};

			layoutQuestionFontSize.Click += (object sender, EventArgs e) => 
			{
				var activitySizeFont = new Intent (this, typeof (ActivitySizeFont));
				activitySizeFont.PutExtra ("InfoSize", new [] { "question" });
				StartActivity (activitySizeFont);
				Finish ();
			};

			layoutAnswerFontSize.Click += (object sender, EventArgs e) => 
			{
				var activitySizeFont = new Intent (this, typeof (ActivitySizeFont));
				activitySizeFont.PutExtra ("InfoSize", new [] { "answer" });
				StartActivity (activitySizeFont);
				Finish ();
			};
			//_panelNoAlpha = FindViewById<ColorPickerPanelView> (Resource.Id.PanelColorQuestionFontColor);
			//_panelNoAlpha.Color = Color.Black;

			// Create your application here
		}
	}
}

