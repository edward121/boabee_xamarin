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
	[Register ("AddProspectRootViewController")]
	partial class AddProspectRootViewController
	{
		[Outlet]
		UIKit.UIButton[] arrowButtonsCollection { get; set; }

		[Outlet]
		UIKit.UIButton identifyButton { get; set; }

		[Outlet]
		UIKit.UIButton infoButton { get; set; }

		[Outlet]
		UIKit.UIButton[] selectionButtonsCollection { get; set; }

		[Outlet]
		UIKit.UIButton shareButton { get; set; }

		[Action ("identifyButtonClick:")]
		partial void identifyButtonClick (UIKit.UIButton sender);

		[Action ("infoButtonClick:")]
		partial void infoButtonClick (UIKit.UIButton sender);

		[Action ("shareButtonClick:")]
		partial void shareButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (identifyButton != null) {
				identifyButton.Dispose ();
				identifyButton = null;
			}

			if (infoButton != null) {
				infoButton.Dispose ();
				infoButton = null;
			}

			if (shareButton != null) {
				shareButton.Dispose ();
				shareButton = null;
			}
		}
	}
}
