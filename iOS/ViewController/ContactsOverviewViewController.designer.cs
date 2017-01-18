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
	[Register ("ContactsOverviewViewController")]
	partial class ContactsOverviewViewController
	{
		[Outlet]
		UIKit.UITableView contactsTableView { get; set; }

		[Action ("cancelButtonClick:")]
		partial void cancelButtonClick (UIKit.UIButton sender);

		[Action ("deleteButtonClick:forEvent:")]
		partial void deleteButtonClick (UIKit.UIButton sender, UIKit.UIEvent @event);
		
		void ReleaseDesignerOutlets ()
		{
			if (contactsTableView != null) {
				contactsTableView.Dispose ();
				contactsTableView = null;
			}
		}
	}
}
