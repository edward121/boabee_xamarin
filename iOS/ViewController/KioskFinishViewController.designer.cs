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
	[Register ("KioskFinishViewController")]
	partial class KioskFinishViewController
	{
		[Outlet]
		UIKit.UIImageView finishImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (finishImageView != null) {
				finishImageView.Dispose ();
				finishImageView = null;
			}
		}
	}
}
