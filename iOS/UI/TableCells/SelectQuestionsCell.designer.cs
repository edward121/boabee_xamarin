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
	[Register ("SelectQuestionsCell")]
	partial class SelectQuestionsCell
	{
		[Outlet]
		UIKit.UILabel questionnaireNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (questionnaireNameLabel != null) {
				questionnaireNameLabel.Dispose ();
				questionnaireNameLabel = null;
			}
		}
	}
}
