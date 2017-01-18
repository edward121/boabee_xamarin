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
	[Register ("DefaultShareOverviewCell")]
	partial class DefaultShareOverviewCell
	{
		[Outlet]
		UIKit.UILabel fileNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (fileNameLabel != null) {
				fileNameLabel.Dispose ();
				fileNameLabel = null;
			}
		}
	}
}
