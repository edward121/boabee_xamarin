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
	[Register ("SelectAnswerViewController")]
	partial class SelectAnswerViewController
	{
		[Outlet]
		UIKit.UITableView answersTableView { get; set; }

		[Outlet]
		UIKit.UIView bottomView { get; set; }

		[Outlet]
		UIKit.UIView mainContentView { get; set; }

		[Outlet]
		UIKit.UILabel questionLabel { get; set; }

		[Action ("backButtonClick:")]
		partial void backButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomView != null) {
				bottomView.Dispose ();
				bottomView = null;
			}

			if (mainContentView != null) {
				mainContentView.Dispose ();
				mainContentView = null;
			}

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
