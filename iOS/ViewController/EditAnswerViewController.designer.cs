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
	[Register ("EditAnswerViewController")]
	partial class EditAnswerViewController
	{
		[Outlet]
		UIKit.UITableView answersTableView { get; set; }

		[Outlet]
		UIKit.UILabel questionLabel { get; set; }

		[Action ("cancelButtonClick:")]
		partial void cancelButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (answersTableView != null) {
				answersTableView.Dispose ();
				answersTableView = null;
			}

			if (questionLabel != null) {
				questionLabel.Dispose ();
				questionLabel = null;
			}
		}
	}
}
