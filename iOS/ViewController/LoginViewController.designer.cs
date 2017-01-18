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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIButton eyeButton { get; set; }

		[Outlet]
		UIKit.UIButton loginLoginButton { get; set; }

		[Outlet]
		UIKit.UIImageView loginLogoImageView { get; set; }

		[Outlet]
		UIKit.UITextField loginPassTextField { get; set; }

		[Outlet]
		UIKit.UITextField loginUserTextField { get; set; }

		[Outlet]
		UIKit.UITextField nameTextField { get; set; }

		[Outlet]
		UIKit.UILabel versionLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint viewBottomConstraint { get; set; }

		[Action ("eyeTouchUp:")]
		partial void eyeTouchUp (UIKit.UIButton sender);

		[Action ("firstTimeUserButtonClick:")]
		partial void firstTimeUserButtonClick (UIKit.UIButton sender);

		[Action ("forgotPasswordButtonClick:")]
		partial void forgotPasswordButtonClick (UIKit.UIButton sender);

		[Action ("loginLoginBtnTouchUpInside:")]
		partial void loginLoginBtnTouchUpInside (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (eyeButton != null) {
				eyeButton.Dispose ();
				eyeButton = null;
			}

			if (loginLoginButton != null) {
				loginLoginButton.Dispose ();
				loginLoginButton = null;
			}

			if (loginLogoImageView != null) {
				loginLogoImageView.Dispose ();
				loginLogoImageView = null;
			}

			if (loginPassTextField != null) {
				loginPassTextField.Dispose ();
				loginPassTextField = null;
			}

			if (loginUserTextField != null) {
				loginUserTextField.Dispose ();
				loginUserTextField = null;
			}

			if (nameTextField != null) {
				nameTextField.Dispose ();
				nameTextField = null;
			}

			if (versionLabel != null) {
				versionLabel.Dispose ();
				versionLabel = null;
			}

			if (viewBottomConstraint != null) {
				viewBottomConstraint.Dispose ();
				viewBottomConstraint = null;
			}
		}
	}
}
