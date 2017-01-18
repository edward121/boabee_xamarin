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
	[Register ("ContactInfoSheetCell")]
	partial class ContactInfoSheetCell
	{
		[Outlet]
		UIKit.UILabel contactNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel dateLabel { get; set; }

		[Outlet]
		UIKit.UILabel timeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (contactNameLabel != null) {
				contactNameLabel.Dispose ();
				contactNameLabel = null;
			}

			if (dateLabel != null) {
				dateLabel.Dispose ();
				dateLabel = null;
			}

			if (timeLabel != null) {
				timeLabel.Dispose ();
				timeLabel = null;
			}
		}
	}
}
