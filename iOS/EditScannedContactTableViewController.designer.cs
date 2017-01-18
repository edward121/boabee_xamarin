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
	[Register ("EditScannedContactTableViewController")]
	partial class EditScannedContactTableViewController
	{
		[Outlet]
		UIKit.UITextField cityTextField { get; set; }

		[Outlet]
		UIKit.UITextField companyTextField { get; set; }

		[Outlet]
		UIKit.UITextField countryTextField { get; set; }

		[Outlet]
		UIKit.UITextField emailTextField { get; set; }

		[Outlet]
		UIKit.UITextField firstnameTextField { get; set; }

		[Outlet]
		UIKit.UITextField idTextField { get; set; }

		[Outlet]
		UIKit.UITextField jobTextField { get; set; }

		[Outlet]
		UIKit.UITextField lastNameTextField { get; set; }

		[Outlet]
		UIKit.UILabel messageLabel { get; set; }

		[Outlet]
		UIKit.UITextField phoneTextField { get; set; }

		[Outlet]
		UIKit.UITextField streetTextField { get; set; }

		[Outlet]
		UIKit.UITextField zipTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (messageLabel != null) {
				messageLabel.Dispose ();
				messageLabel = null;
			}

			if (idTextField != null) {
				idTextField.Dispose ();
				idTextField = null;
			}

			if (firstnameTextField != null) {
				firstnameTextField.Dispose ();
				firstnameTextField = null;
			}

			if (lastNameTextField != null) {
				lastNameTextField.Dispose ();
				lastNameTextField = null;
			}

			if (emailTextField != null) {
				emailTextField.Dispose ();
				emailTextField = null;
			}

			if (companyTextField != null) {
				companyTextField.Dispose ();
				companyTextField = null;
			}

			if (phoneTextField != null) {
				phoneTextField.Dispose ();
				phoneTextField = null;
			}

			if (jobTextField != null) {
				jobTextField.Dispose ();
				jobTextField = null;
			}

			if (streetTextField != null) {
				streetTextField.Dispose ();
				streetTextField = null;
			}

			if (cityTextField != null) {
				cityTextField.Dispose ();
				cityTextField = null;
			}

			if (countryTextField != null) {
				countryTextField.Dispose ();
				countryTextField = null;
			}

			if (zipTextField != null) {
				zipTextField.Dispose ();
				zipTextField = null;
			}
		}
	}
}
