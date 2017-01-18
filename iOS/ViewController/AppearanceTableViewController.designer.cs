// WARNING
//
// This file has been generated automatically by Xamarin Studio Indie to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BoaBee.iOS
{
	[Register ("AppearanceTableViewController")]
	partial class AppearanceTableViewController
	{
		[Outlet]
		UIKit.UIImageView answerBackgroundPreview { get; set; }

		[Outlet]
		UIKit.UIImageView answerFontColorPreview { get; set; }

		[Outlet]
		UIKit.UILabel answerFontSizeLabel { get; set; }

		[Outlet]
		UIKit.UIImageView questionBackgroundPreview { get; set; }

		[Outlet]
		UIKit.UIImageView questionFontColorPreview { get; set; }

		[Outlet]
		UIKit.UILabel questionFontSizeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (answerFontColorPreview != null) {
				answerFontColorPreview.Dispose ();
				answerFontColorPreview = null;
			}

			if (answerBackgroundPreview != null) {
				answerBackgroundPreview.Dispose ();
				answerBackgroundPreview = null;
			}

			if (answerFontSizeLabel != null) {
				answerFontSizeLabel.Dispose ();
				answerFontSizeLabel = null;
			}

			if (questionBackgroundPreview != null) {
				questionBackgroundPreview.Dispose ();
				questionBackgroundPreview = null;
			}

			if (questionFontColorPreview != null) {
				questionFontColorPreview.Dispose ();
				questionFontColorPreview = null;
			}

			if (questionFontSizeLabel != null) {
				questionFontSizeLabel.Dispose ();
				questionFontSizeLabel = null;
			}
		}
	}
}
