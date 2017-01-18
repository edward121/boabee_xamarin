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
	[Register ("SelectContactViewController")]
	partial class SelectContactViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonConstraint { get; set; }

		[Outlet]
		UIKit.UITableView contactsTableView { get; set; }

		[Outlet]
		UIKit.UIButton crossButton { get; set; }

		[Outlet]
		UIKit.UITextField searchTextField { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }

		[Action ("cancelButtonClick:")]
		partial void cancelButtonClick (UIKit.UIButton sender);

		[Action ("crossButtonClick:")]
		partial void crossButtonClick (UIKit.UIButton sender);

		[Action ("editButtonClick:forEvent:")]
		partial void editButtonClick (UIKit.UIButton sender, UIKit.UIEvent @event);

		[Action ("textFieldEditingChanged:")]
		partial void textFieldEditingChanged (UIKit.UITextField sender);

		[Action ("textFieldEditingDidBegin:")]
		partial void textFieldEditingDidBegin (UIKit.UITextField sender);

		[Action ("textFieldEditingDidEnd:")]
		partial void textFieldEditingDidEnd (UIKit.UITextField sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (crossButton != null) {
				crossButton.Dispose ();
				crossButton = null;
			}

			if (bottomButtonConstraint != null) {
				bottomButtonConstraint.Dispose ();
				bottomButtonConstraint = null;
			}

			if (contactsTableView != null) {
				contactsTableView.Dispose ();
				contactsTableView = null;
			}

			if (searchTextField != null) {
				searchTextField.Dispose ();
				searchTextField = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}
		}
	}
}
