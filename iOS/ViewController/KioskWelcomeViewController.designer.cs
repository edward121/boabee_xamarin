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
	[Register ("KioskWelcomeViewController")]
	partial class KioskWelcomeViewController
	{
		[Outlet]
		UIKit.UIImageView welcomeImageView { get; set; }

		[Action ("imageTapped:")]
		partial void imageTapped (UIKit.UITapGestureRecognizer sender);

		[Action ("invisibleExitButtonClicked:")]
		partial void invisibleExitButtonClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (welcomeImageView != null) {
				welcomeImageView.Dispose ();
				welcomeImageView = null;
			}
		}
	}
}
