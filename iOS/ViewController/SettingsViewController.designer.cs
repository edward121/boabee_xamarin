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
	[Register ("SettingsViewController")]
	partial class SettingsViewController
	{
		[Outlet]
		UIKit.UILabel appNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel statusLabel { get; set; }

		[Outlet]
		UIKit.UILabel userNameLabel { get; set; }

		[Action ("appSettingsButtonClick:")]
		partial void appSettingsButtonClick (UIKit.UIButton sender);

		[Action ("closeButtonClick:")]
		partial void closeButtonClick (UIKit.UIButton sender);

		[Action ("defaultSharingButtonClick:")]
		partial void defaultSharingButtonClick (UIKit.UIButton sender);

		[Action ("resetWorkbuttonClick:")]
		partial void resetWorkbuttonClick (UIKit.UIButton sender);

		[Action ("switchAppInvisibleTouchUpInside:")]
		partial void switchAppInvisibleTouchUpInside (UIKit.UIButton sender);

		[Action ("syncManuallyButtonClick:")]
		partial void syncManuallyButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (appNameLabel != null) {
				appNameLabel.Dispose ();
				appNameLabel = null;
			}

			if (statusLabel != null) {
				statusLabel.Dispose ();
				statusLabel = null;
			}

			if (userNameLabel != null) {
				userNameLabel.Dispose ();
				userNameLabel = null;
			}
		}
	}
}
