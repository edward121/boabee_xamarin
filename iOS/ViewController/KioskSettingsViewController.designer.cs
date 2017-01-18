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
	[Register ("KioskSettingsViewController")]
	partial class KioskSettingsViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonConstraint { get; set; }

		[Action ("closeButtonClicked:")]
		partial void closeButtonClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomButtonConstraint != null) {
				bottomButtonConstraint.Dispose ();
				bottomButtonConstraint = null;
			}
		}
	}
}
