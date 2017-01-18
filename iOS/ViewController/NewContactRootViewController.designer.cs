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
	[Register ("NewContactRootViewController")]
	partial class NewContactRootViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonsConstraint { get; set; }

		[Action ("cancelButtonClick:")]
		partial void cancelButtonClick (UIKit.UIButton sender);

		[Action ("createButtonClick:")]
		partial void createButtonClick (UIKit.UIButton sender);

		[Action ("crossButtonClick:")]
		partial void crossButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomButtonsConstraint != null) {
				bottomButtonsConstraint.Dispose ();
				bottomButtonsConstraint = null;
			}
		}
	}
}
