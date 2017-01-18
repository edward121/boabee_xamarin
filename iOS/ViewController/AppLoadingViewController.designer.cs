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
	[Register ("AppLoadingViewController")]
	partial class AppLoadingViewController
	{
		[Outlet]
		UIKit.UIImageView blinkingLogo { get; set; }

		[Outlet]
		UIKit.UILabel percentLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (blinkingLogo != null) {
				blinkingLogo.Dispose ();
				blinkingLogo = null;
			}

			if (percentLabel != null) {
				percentLabel.Dispose ();
				percentLabel = null;
			}
		}
	}
}
