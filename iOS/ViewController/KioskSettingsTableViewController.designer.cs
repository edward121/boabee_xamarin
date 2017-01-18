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
	[Register ("KioskSettingsTableViewController")]
	partial class KioskSettingsTableViewController
	{
		[Outlet]
		UIKit.UISwitch badgePrintingSwitch { get; set; }

		[Outlet]
		UIKit.UITextField badgePrintingWebhook { get; set; }

		[Outlet]
		UIKit.UITextField kioskTitleTextField { get; set; }

		[Action ("colorSettingsButtonPressed:")]
		partial void colorSettingsButtonPressed (UIKit.UIButton sender);

		[Action ("contactsButtonPressed:")]
		partial void contactsButtonPressed (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (badgePrintingSwitch != null) {
				badgePrintingSwitch.Dispose ();
				badgePrintingSwitch = null;
			}

			if (badgePrintingWebhook != null) {
				badgePrintingWebhook.Dispose ();
				badgePrintingWebhook = null;
			}

			if (kioskTitleTextField != null) {
				kioskTitleTextField.Dispose ();
				kioskTitleTextField = null;
			}
		}
	}
}
