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
	[Register ("KioskEmailViewController")]
	partial class KioskEmailViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonsConstraint { get; set; }

		[Outlet]
		UIKit.UITextField emailTextField { get; set; }

		[Outlet]
		UIKit.UILabel enterEmailLabel { get; set; }

		[Outlet]
		UIKit.UILabel kioskTitleLabel { get; set; }

		[Action ("cancelButtonClicked:")]
		partial void cancelButtonClicked (UIKit.UIButton sender);

		[Action ("nextButtonClicked:")]
		partial void nextButtonClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (enterEmailLabel != null) {
				enterEmailLabel.Dispose ();
				enterEmailLabel = null;
			}

			if (bottomButtonsConstraint != null) {
				bottomButtonsConstraint.Dispose ();
				bottomButtonsConstraint = null;
			}

			if (emailTextField != null) {
				emailTextField.Dispose ();
				emailTextField = null;
			}

			if (kioskTitleLabel != null) {
				kioskTitleLabel.Dispose ();
				kioskTitleLabel = null;
			}
		}
	}
}
