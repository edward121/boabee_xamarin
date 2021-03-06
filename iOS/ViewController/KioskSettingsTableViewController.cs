// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using BoaBeePCL;

namespace BoaBee.iOS
{
	public partial class KioskSettingsTableViewController : UITableViewController
	{
		public KioskSettingsTableViewController (IntPtr handle) : base (handle)
		{
		}

		public NSDictionary getNewKioskSettings()
		{
			NSMutableDictionary newSettings = new NSMutableDictionary();

			newSettings.Add((NSString)"kioskTitle", (NSString)this.kioskTitleTextField.Text);
			newSettings.Add((NSString)"badgePrinting", NSNumber.FromBoolean(this.badgePrintingSwitch.On));
			newSettings.Add((NSString)"badgePrintingWebhook", (NSString)this.badgePrintingWebhook.Text);

			return newSettings;
		}

		public void initWithKioskSettings(DBKioskSettings kioskSettings)
		{
			this.kioskTitleTextField.Text = kioskSettings.kioskTitle;
			this.badgePrintingSwitch.On = kioskSettings.badgePrinting;
			this.badgePrintingWebhook.Text = kioskSettings.badgePrintingWebhook;
		}

		partial void contactsButtonPressed (UIButton sender)
		{
			NSNotificationCenter.DefaultCenter.PostNotificationName("ShowContactsNotification", null);
		}

		partial void colorSettingsButtonPressed (UIButton sender)
		{
			NSNotificationCenter.DefaultCenter.PostNotificationName("ColorSettingsNotification", null);
		}
	}
}
