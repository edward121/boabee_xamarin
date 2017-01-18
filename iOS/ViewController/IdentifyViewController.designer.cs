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
	[Register ("IdentifyViewController")]
	partial class IdentifyViewController
	{
		[Outlet]
		UIKit.UIButton contactsOverviewButton { get; set; }

		[Action ("badgeButtonClick:")]
		partial void badgeButtonClick (UIKit.UIButton sender);

		[Action ("crossButtonClicked:")]
		partial void crossButtonClicked (UIKit.UIButton sender);

		[Action ("lookUpButtonClick:")]
		partial void lookUpButtonClick (UIKit.UIButton sender);

		[Action ("manualButtonClick:")]
		partial void manualButtonClick (UIKit.UIButton sender);

		[Action ("overviewButtonClick:")]
		partial void overviewButtonClick (UIKit.UIButton sender);

		[Action ("tickButtonClicked:")]
		partial void tickButtonClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (contactsOverviewButton != null) {
				contactsOverviewButton.Dispose ();
				contactsOverviewButton = null;
			}
		}
	}
}
