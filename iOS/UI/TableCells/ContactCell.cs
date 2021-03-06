// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

using BoaBeePCL;

namespace BoaBee.iOS
{
	public partial class ContactCell : UITableViewCell
	{
		public ContactCell (IntPtr handle) : base (handle)
		{
		}

		public void initWithContact(DBlocalContact contact, bool isKiosk)
		{
			if (!string.IsNullOrEmpty(contact.firstname) || !string.IsNullOrEmpty(contact.lastname))
			{
				if (!string.IsNullOrEmpty(contact.company))
				{
					this.contactNameLabel.Text = string.Format("{0} {1} ({2})", contact.firstname, contact.lastname, contact.company);
				}
				else
				{
					this.contactNameLabel.Text = string.Format("{0} {1}", contact.firstname, contact.lastname);
				}
			}
            else
            {
                this.contactNameLabel.Text = contact.uid;
            }

			if (isKiosk && this.editButton != null)
			{
				this.editButton.Hidden = true;
			}
		}
	}
}
