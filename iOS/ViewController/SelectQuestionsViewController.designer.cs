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
	[Register ("SelectQuestionsViewController")]
	partial class SelectQuestionsViewController
	{
		[Outlet]
		UIKit.UITableView questionnaireTableView { get; set; }

		[Action ("closeButtonClick:")]
		partial void closeButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (questionnaireTableView != null) {
				questionnaireTableView.Dispose ();
				questionnaireTableView = null;
			}
		}
	}
}
