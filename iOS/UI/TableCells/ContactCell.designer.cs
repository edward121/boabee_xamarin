// WARNING
//
// This file has been generated automatically by Xamarin Studio Indie to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BoaBee.iOS
{
	[Register ("ContactCell")]
	partial class ContactCell
	{
		[Outlet]
		UIKit.UILabel contactNameLabel { get; set; }

		[Outlet]
		UIKit.UIButton editButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (editButton != null) {
				editButton.Dispose ();
				editButton = null;
			}

			if (contactNameLabel != null) {
				contactNameLabel.Dispose ();
				contactNameLabel = null;
			}
		}
	}
}
