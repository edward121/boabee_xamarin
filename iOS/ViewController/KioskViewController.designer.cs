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
	[Register ("KioskViewController")]
	partial class KioskViewController
	{
		[Outlet]
		UIKit.UILabel appNameLabel { get; set; }

		[Outlet]
		UIKit.UIButton contactsMetButton { get; set; }

		[Outlet]
		UIKit.UIButton emailsSentButton { get; set; }

		[Outlet]
		UIKit.UIButton infoSheetsButton { get; set; }

		[Outlet]
		UIKit.UIButton settingsButton { get; set; }

		[Action ("contactsMetButtonPressed:")]
		partial void contactsMetButtonPressed (UIKit.UIButton sender);

		[Action ("emailsSentButtonPressed:")]
		partial void emailsSentButtonPressed (UIKit.UIButton sender);

		[Action ("hiddenShowContactsViewTap:")]
		partial void hiddenShowContactsViewTap (UIKit.UITapGestureRecognizer sender);

		[Action ("infoSheetsButtonClicked:")]
		partial void infoSheetsButtonClicked (UIKit.UIButton sender);

		[Action ("settingsButtonClicked:")]
		partial void settingsButtonClicked (UIKit.UIButton sender);

		[Action ("startKioskButtonClicked:")]
		partial void startKioskButtonClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (appNameLabel != null) {
				appNameLabel.Dispose ();
				appNameLabel = null;
			}

			if (contactsMetButton != null) {
				contactsMetButton.Dispose ();
				contactsMetButton = null;
			}

			if (emailsSentButton != null) {
				emailsSentButton.Dispose ();
				emailsSentButton = null;
			}

			if (infoSheetsButton != null) {
				infoSheetsButton.Dispose ();
				infoSheetsButton = null;
			}

			if (settingsButton != null) {
				settingsButton.Dispose ();
				settingsButton = null;
			}
		}
	}
}
