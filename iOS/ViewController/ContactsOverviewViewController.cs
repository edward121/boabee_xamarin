// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;

using CoreGraphics;

using BoaBeePCL;

using Foundation;
using UIKit;
using System.Linq;

namespace BoaBee.iOS
{
	public partial class ContactsOverviewViewController : UIViewController, IUITableViewDataSource, IUITableViewDelegate
	{
        public List<DBlocalContact> selectedContacts;// = new List<DBlocalContact>();

		public ContactsOverviewViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.contactsTableView.EstimatedRowHeight = 44;
			this.contactsTableView.RowHeight = UITableView.AutomaticDimension;

            this.selectedContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.activeContact).ToList();
		}

		public nint RowsInSection(UITableView tableView, nint section)
		{
			return this.selectedContacts.Count;
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			ContactOverviewCell cell = (ContactOverviewCell)tableView.DequeueReusableCell("ContactCell", indexPath);

			cell.initWithContact(selectedContacts[indexPath.Row]);
			return cell;
		}

		partial void cancelButtonClick (UIButton sender)
		{
			if (this.ModalPresentationStyle == UIModalPresentationStyle.Popover)
			{
				this.DismissViewController(true, null);
			}
			else
			{
				this.NavigationController.PopViewController(true);
			}
		}

		partial void deleteButtonClick (UIButton sender, UIEvent @event)
		{
			UITouch touch = (UITouch)@event.TouchesForView(sender).AnyObject;
			CGPoint touchLocation = touch.LocationInView(this.contactsTableView);

			NSIndexPath indexPath = this.contactsTableView.IndexPathForRowAtPoint(touchLocation);

            DBlocalContact contact = this.selectedContacts[indexPath.Row];
            contact.activeContact = false;
            DBLocalDataStore.GetInstance().UpdateLocalContact(contact);

			this.selectedContacts.RemoveAt(indexPath.Row);
			this.contactsTableView.BeginUpdates();

			this.contactsTableView.DeleteRows(new NSIndexPath[]{indexPath}, UITableViewRowAnimation.Automatic);

			this.contactsTableView.EndUpdates();

			if (this.selectedContacts.Count == 0)
			{
				if (this.ModalPresentationStyle == UIModalPresentationStyle.Popover)
				{
					this.DismissViewController(true, null);
				}
				else
				{
					this.NavigationController.PopViewController(true);
				}
			}
		}
	}
}
