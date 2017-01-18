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
	[Register ("EmailSentViewController")]
	partial class EmailSentViewController
	{
		[Outlet]
		UIKit.UILabel contactNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel dateTimeLabel { get; set; }

		[Outlet]
		UIKit.UITableView filesTableView { get; set; }

		[Outlet]
		UIKit.UILabel statusLabel { get; set; }

		[Action ("closeButtonClick:")]
		partial void closeButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (contactNameLabel != null) {
				contactNameLabel.Dispose ();
				contactNameLabel = null;
			}

			if (dateTimeLabel != null) {
				dateTimeLabel.Dispose ();
				dateTimeLabel = null;
			}

			if (statusLabel != null) {
				statusLabel.Dispose ();
				statusLabel = null;
			}

			if (filesTableView != null) {
				filesTableView.Dispose ();
				filesTableView = null;
			}
		}
	}
}
