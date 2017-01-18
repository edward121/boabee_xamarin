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
	[Register ("ScannerViewController")]
	partial class ScannerViewController
	{
		[Outlet]
		UIKit.UIButton badgesCountButton { get; set; }

		[Outlet]
		UIKit.UIButton button { get; set; }

		[Outlet]
		UIKit.UIView cameraUnderlay { get; set; }

		[Outlet]
		UIKit.UIButton nextButton { get; set; }

		[Outlet]
		UIKit.UIButton singleMultiButton { get; set; }

		[Action ("badgesCountButtonClick:")]
		partial void badgesCountButtonClick (UIKit.UIButton sender);

		[Action ("buttonClick:")]
		partial void buttonClick (UIKit.UIButton sender);

		[Action ("closeButtonClick:")]
		partial void closeButtonClick (UIKit.UIButton sender);

		[Action ("lookupButtonClick:")]
		partial void lookupButtonClick (UIKit.UIButton sender);

		[Action ("manualButtonClick:")]
		partial void manualButtonClick (UIKit.UIButton sender);

		[Action ("nextButtonClick:")]
		partial void nextButtonClick (UIKit.UIButton sender);

		[Action ("singleMultiButtonClick:")]
		partial void singleMultiButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (badgesCountButton != null) {
				badgesCountButton.Dispose ();
				badgesCountButton = null;
			}

			if (button != null) {
				button.Dispose ();
				button = null;
			}

			if (cameraUnderlay != null) {
				cameraUnderlay.Dispose ();
				cameraUnderlay = null;
			}

			if (nextButton != null) {
				nextButton.Dispose ();
				nextButton = null;
			}

			if (singleMultiButton != null) {
				singleMultiButton.Dispose ();
				singleMultiButton = null;
			}
		}
	}
}
