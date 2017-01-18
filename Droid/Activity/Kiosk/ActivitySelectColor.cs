
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using BoaBeePCL;
using Android.Content.PM;

using Android.Graphics;
using System.Linq.Expressions;
namespace Leadbox
{
	[Activity (Label = "ActivitySelectColor", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme")]
	public class ActivitySelectColor : Activity
	{
		public ColorPickerView colorPicker;
		public EditText inputColorR;
		public EditText inputColorG;
		public EditText inputColorB;
		public string typeFont;
		string inputColorsR;
		string inputColorsG;
		string inputColorsB;
		public int tempColor;
		Color blend ;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.SelectColorLayout);
			string [] strMass = Intent.GetStringArrayExtra ("InfoFontColor");
			typeFont = strMass [0];
			colorPicker = FindViewById<ColorPickerView> (Resource.Id.color_picker_view);
			var newColor = FindViewById<ColorPickerPanelView> (Resource.Id.new_color_panel);
			blend = Color.White;
			inputColorR = FindViewById<EditText> (Resource.Id.editTextR);
			inputColorG = FindViewById<EditText> (Resource.Id.editTextG);
			inputColorB = FindViewById<EditText> (Resource.Id.editTextB);
			TextView textInputColorR = FindViewById<TextView> (Resource.Id.textR);
			TextView textInputColorG = FindViewById<TextView> (Resource.Id.textG);
			TextView textInputColorB = FindViewById<TextView> (Resource.Id.textB);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

			var questinoFontColor = DBLocalDataStore.GetInstance ().GetQuestionFontColor ();
			var answerFontColor = DBLocalDataStore.GetInstance ().GetAnswerFontColor ();
			var answerBackgroundColor = DBLocalDataStore.GetInstance ().GetAnswerBackgroundColor ();
			var questinoBackgroundColor = DBLocalDataStore.GetInstance ().GetQuestionBackgroundColor ();

			DBColor colors = new DBColor ();
			var finish = FindViewById<TextView> (Resource.Id.textView2);
			var close = FindViewById<TextView> (Resource.Id.textView3);
			inputColorR.SetTypeface (font, TypefaceStyle.Normal);
			inputColorG.SetTypeface (font, TypefaceStyle.Normal);
			inputColorB.SetTypeface (font, TypefaceStyle.Normal);
			textInputColorR.SetTypeface (font, TypefaceStyle.Normal);
			textInputColorG.SetTypeface (font, TypefaceStyle.Normal);
			textInputColorB.SetTypeface (font, TypefaceStyle.Normal);
			finish.SetTypeface (font, TypefaceStyle.Normal);
			close.SetTypeface (font, TypefaceStyle.Normal);
			// Create your application here
			switch (typeFont)
			{
				case "AnswerBackgroundColor":
					if (answerBackgroundColor == null) {
						colorPicker.Color = Color.Rgb (255, 255, 255);
						newColor.Color = Color.Rgb (255, 255, 255);
						inputColorR.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsR = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
					} else {
						var tempForColors = Color.Rgb ((Int32)answerBackgroundColor.redByte, (Int32)answerBackgroundColor.greenByte, (Int32)answerBackgroundColor.blueByte);
						//	colorPickerAnswerBackgroundColor.Color = tempForColors;
						colorPicker.Color = tempForColors;
						newColor.Color = tempForColors;
						inputColorR.Text = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";
						inputColorsR = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";
				}
					break;
				case "QuestionBackgroundColor":
					if (questinoBackgroundColor == null) {
						colorPicker.Color = Color.Rgb (255, 255, 255);
						newColor.Color = Color.Rgb (255, 255, 255);
						inputColorR.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsR = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
					} else {
						var tempForColors = Color.Rgb ((Int32)questinoBackgroundColor.redByte, (Int32)questinoBackgroundColor.greenByte, (Int32)questinoBackgroundColor.blueByte);
						//	colorPickerAnswerBackgroundColor.Color = tempForColors;
						colorPicker.Color = tempForColors;
						newColor.Color = tempForColors;
						inputColorR.Text = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";
						inputColorsR = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";
				}
				break;
				case "AnswerFontColor":
					if (answerFontColor == null) {
						colorPicker.Color = Color.Rgb (255, 255, 255);
						newColor.Color = Color.Rgb (255, 255, 255);
						inputColorR.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsR = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
					} else {
						var tempForColors = Color.Rgb ((Int32)answerFontColor.redByte, (Int32)answerFontColor.greenByte, (Int32)answerFontColor.blueByte);
						//	colorPickerAnswerBackgroundColor.Color = tempForColors;
						colorPicker.Color = tempForColors;
						newColor.Color = tempForColors;
						inputColorR.Text = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";
						inputColorsR = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";
				}
				break;
				case "QuestionFontColor":
					if (questinoFontColor == null) {
						colorPicker.Color = Color.Rgb (255, 255, 255);
						newColor.Color = Color.Rgb (255, 255, 255);
						inputColorR.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsR = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (255) + (1 - 255 / 255) * blend.R + "";
					} else {
						var tempForColors = Color.Rgb ((Int32)questinoFontColor.redByte, (Int32)questinoFontColor.greenByte, (Int32)questinoFontColor.blueByte);
						//	colorPickerAnswerBackgroundColor.Color = tempForColors;
						colorPicker.Color = tempForColors;
						newColor.Color = tempForColors;
						inputColorR.Text = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorG.Text = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorB.Text = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";

						inputColorsR = (255 / 255) * Convert.ToSingle (tempForColors.R) + (1 - 255 / 255) * blend.R + "";
						inputColorsG = (255 / 255) * Convert.ToSingle (tempForColors.G) + (1 - 255 / 255) * blend.G + "";
						inputColorsB = (255 / 255) * Convert.ToSingle (tempForColors.B) + (1 - 255 / 255) * blend.R + "";
				}
				break;
				default:
					//Console.WriteLine ("Default case");
				break;
			}
			colorPicker.ColorChanged += (sender, args) => {

				newColor.Color = args.Color;
				inputColorR.Text = (255 / 255) * Convert.ToSingle (args.Color.R) + (1 - 255 / 255) * blend.R + "";
				inputColorG.Text = (255 / 255) * Convert.ToSingle (args.Color.G) + (1 - 255 / 255) * blend.G + "";
				inputColorB.Text = (255 / 255) * Convert.ToSingle (args.Color.B) + (1 - 255 / 255) * blend.R + "";
				inputColorsR = (255 / 255) * Convert.ToSingle (args.Color.R) + (1 - 255 / 255) * blend.R + "";
				inputColorsG = (255 / 255) * Convert.ToSingle (args.Color.G) + (1 - 255 / 255) * blend.G + "";
				inputColorsB = (255 / 255) * Convert.ToSingle (args.Color.B) + (1 - 255 / 255) * blend.R + "";

				finish.Clickable = true;
			};
			inputColorR.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				try {
					var a = Convert.ToSingle (inputColorR.Text);
					if (a > 255 || a < 0) {
						Toast.MakeText (this, "Input values from 0 to 255", ToastLength.Short).Show ();
						finish.Clickable = false;
						//inputColorR.Focusable = false;
					} else {
						//inputColorR.Focusable = true;
						finish.Clickable = true;
						inputColorsR = e.Text.ToString ();
					}
				}
				catch{}
			};
			inputColorG.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				try 
				{
					var a = Convert.ToSingle (inputColorG.Text);
					if (a > 255|| a < 0) {
						Toast.MakeText (this, "Input values from 0 to 255", ToastLength.Short).Show ();
						finish.Clickable = false;
						//inputColorR.Focusable = false;
					} else {
						//inputColorR.Focusable = true;
						finish.Clickable = true;
						inputColorsG = e.Text.ToString ();
					}
				}
				catch { }

			};
		inputColorB.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				try
				{
				var a = Convert.ToInt32 (inputColorB.Text);
				if (a> 255|| a< 0) {
					Toast.MakeText (this, "Input values from 0 to 255", ToastLength.Short).Show ();
					finish.Clickable = false;
					//inputColorR.Focusable = false;
				} else {
					//inputColorR.Focusable = true;
					finish.Clickable = true;
					inputColorsB = e.Text.ToString ();
				}
			}
				catch { }

		};
			close.Click += (object sender, EventArgs e) => {
				Finish ();
			};
			finish.Click += (object sender, EventArgs e) => {
				colors.blueByte = Convert.ToByte (inputColorsB) ;//Convert.ToSingle( inputColorsR);
				colors.greenByte = Convert.ToByte (inputColorsG) ;
				colors.redByte = Convert.ToByte (inputColorsR) ;

				switch (typeFont) 
				{
				case "QuestionFontColor":
					DBLocalDataStore.GetInstance ().SetQuestionFontColor(colors); 
					break;
				case "AnswerFontColor":
					DBLocalDataStore.GetInstance ().SetAnswerFontColor (colors);
					break;
				case "QuestionBackgroundColor":
					DBLocalDataStore.GetInstance ().SetQuestionBackgroundColor (colors);
					break;
				case "AnswerBackgroundColor":
					DBLocalDataStore.GetInstance ().SetAnswerBackgroundColor (colors);
					break;
				}
				StartActivity (typeof(ActivityColorPicker));
				Finish ();
			};
			//oldColor.Color = color;
			//colorPicker.Color = color;
		}
	}
}

