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
	[Register ("EditContactRootViewController")]
	partial class EditContactRootViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonsConstraint { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }

		[Outlet]
		UIKit.UIButton updateButton { get; set; }

		[Action ("cancelButtonClick:")]
		partial void cancelButtonClick (UIKit.UIButton sender);

		[Action ("updateButtonClick:")]
		partial void updateButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (updateButton != null) {
				updateButton.Dispose ();
				updateButton = null;
			}

			if (bottomButtonsConstraint != null) {
				bottomButtonsConstraint.Dispose ();
				bottomButtonsConstraint = null;
			}
		}
	}
}
