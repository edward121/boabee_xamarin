// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BoaBee.iOS
{
	[Register ("InfoSheetDetailsCell")]
	partial class InfoSheetDetailsCell
	{
		[Outlet]
		UIKit.UILabel answerLabel { get; set; }

		[Outlet]
		UIKit.UITextView answerTextView { get; set; }

		[Outlet]
		UIKit.UILabel questionLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (answerTextView != null) {
				answerTextView.Dispose ();
				answerTextView = null;
			}

			if (answerLabel != null) {
				answerLabel.Dispose ();
				answerLabel = null;
			}

			if (questionLabel != null) {
				questionLabel.Dispose ();
				questionLabel = null;
			}
		}
	}
}
