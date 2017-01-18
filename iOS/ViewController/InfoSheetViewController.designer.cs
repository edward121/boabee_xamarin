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
	[Register ("InfoSheetViewController")]
	partial class InfoSheetViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonConstraint { get; set; }

		[Outlet]
		UIKit.UILabel contactNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel dateTimeLabel { get; set; }

		[Outlet]
		UIKit.UITableView quesionsTableView { get; set; }

		[Outlet]
		UIKit.UILabel statusLabel { get; set; }

		[Action ("closeButtonClick:")]
		partial void closeButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomButtonConstraint != null) {
				bottomButtonConstraint.Dispose ();
				bottomButtonConstraint = null;
			}

			if (contactNameLabel != null) {
				contactNameLabel.Dispose ();
				contactNameLabel = null;
			}

			if (dateTimeLabel != null) {
				dateTimeLabel.Dispose ();
				dateTimeLabel = null;
			}

			if (quesionsTableView != null) {
				quesionsTableView.Dispose ();
				quesionsTableView = null;
			}

			if (statusLabel != null) {
				statusLabel.Dispose ();
				statusLabel = null;
			}
		}
	}
}
