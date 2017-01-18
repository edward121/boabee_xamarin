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
	[Register ("AnswerTypingCell")]
	partial class AnswerTypingCell
	{
		[Outlet]
		UIKit.UITextField inputTextField { get; set; }

		[Outlet]
		UIKit.UITextView inputTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (inputTextField != null) {
				inputTextField.Dispose ();
				inputTextField = null;
			}

			if (inputTextView != null) {
				inputTextView.Dispose ();
				inputTextView = null;
			}
		}
	}
}
