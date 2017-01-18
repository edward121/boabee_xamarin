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
	[Register ("ContactsMetViewController")]
	partial class ContactsMetViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonsConstraint { get; set; }

		[Outlet]
		UIKit.UILabel contactsMetCount { get; set; }

		[Outlet]
		UIKit.UITableView contactsmetTableView { get; set; }

		[Outlet]
		UIKit.UITextField searchTextField { get; set; }

		[Action ("cancelButtonClick:")]
		partial void cancelButtonClick (Foundation.NSObject sender);

		[Action ("searchTextFieldEditingChanged:")]
		partial void searchTextFieldEditingChanged (UIKit.UITextField sender);

		[Action ("textFieldEditingDidBegin:")]
		partial void textFieldEditingDidBegin (UIKit.UITextField sender);

		[Action ("textFieldEditingDidEnd:")]
		partial void textFieldEditingDidEnd (UIKit.UITextField sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomButtonsConstraint != null) {
				bottomButtonsConstraint.Dispose ();
				bottomButtonsConstraint = null;
			}

			if (contactsMetCount != null) {
				contactsMetCount.Dispose ();
				contactsMetCount = null;
			}

			if (contactsmetTableView != null) {
				contactsmetTableView.Dispose ();
				contactsmetTableView = null;
			}

			if (searchTextField != null) {
				searchTextField.Dispose ();
				searchTextField = null;
			}
		}
	}
}
