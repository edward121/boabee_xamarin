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
	[Register ("QuestionCell")]
	partial class QuestionCell
	{
		[Outlet]
		UIKit.UILabel questionLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (questionLabel != null) {
				questionLabel.Dispose ();
				questionLabel = null;
			}
		}
	}
}
