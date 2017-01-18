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
	[Register ("InfoSheetsViewController")]
	partial class InfoSheetsViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonsConstraint { get; set; }

		[Outlet]
		UIKit.UITableView contactsTableView { get; set; }

		[Outlet]
		UIKit.UILabel infoSheetsLabel { get; set; }

		[Outlet]
		UIKit.UITextField searchTextField { get; set; }

		[Action ("cancelButtonClick:")]
		partial void cancelButtonClick (UIKit.UIButton sender);

		[Action ("searchTextFieldEditingChanged:")]
		partial void searchTextFieldEditingChanged (UIKit.UITextField sender);

		[Action ("searchTextFieldEditingDidBegin:")]
		partial void searchTextFieldEditingDidBegin (UIKit.UITextField sender);

		[Action ("searchTextFieldEditingDidEnd:")]
		partial void searchTextFieldEditingDidEnd (UIKit.UITextField sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomButtonsConstraint != null) {
				bottomButtonsConstraint.Dispose ();
				bottomButtonsConstraint = null;
			}

			if (contactsTableView != null) {
				contactsTableView.Dispose ();
				contactsTableView = null;
			}

			if (infoSheetsLabel != null) {
				infoSheetsLabel.Dispose ();
				infoSheetsLabel = null;
			}

			if (searchTextField != null) {
				searchTextField.Dispose ();
				searchTextField = null;
			}
		}
	}
}
