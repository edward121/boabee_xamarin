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
	[Register ("FolderCell")]
	partial class FolderCell
	{
		[Outlet]
		UIKit.UILabel folderNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (folderNameLabel != null) {
				folderNameLabel.Dispose ();
				folderNameLabel = null;
			}
		}
	}
}
