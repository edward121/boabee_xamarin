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
	[Register ("MyWorkViewController")]
	partial class MyWorkViewController
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

		[Action ("addProspectButtonClicked:")]
		partial void addProspectButtonClicked (UIKit.UIButton sender);

		[Action ("contactsMetButtonPressed:")]
		partial void contactsMetButtonPressed (UIKit.UIButton sender);

		[Action ("emailsSentButtonClicked:")]
		partial void emailsSentButtonClicked (UIKit.UIButton sender);

		[Action ("infoSheetsButtonClicked:")]
		partial void infoSheetsButtonClicked (UIKit.UIButton sender);

		[Action ("settingsButtonClicked:")]
		partial void settingsButtonClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (settingsButton != null) {
				settingsButton.Dispose ();
				settingsButton = null;
			}

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
		}
	}
}
