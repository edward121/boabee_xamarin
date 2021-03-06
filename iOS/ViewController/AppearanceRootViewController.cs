// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using BoaBeePCL;

namespace BoaBee.iOS
{
	public partial class AppearanceRootViewController : UIViewController
	{
		public AppearanceRootViewController (IntPtr handle) : base (handle)
		{
		}

		partial void cancelButtonClick (UIButton sender)
		{
			this.NavigationController.PopViewController(true);
		}

		partial void okButtonClick (UIButton sender)
		{
			AppearanceTableViewController child = (AppearanceTableViewController)this.ChildViewControllers[0];
			if (child.selectedQuestionSize != null)
			{
				DBLocalDataStore.GetInstance().SetQuestionFontSize(new DBQuestionFontSize((int)child.selectedQuestionSize));
			}
			if (child.selectedAnswerSize != null)
			{
				DBLocalDataStore.GetInstance().SetAnswerFontSize(new DBAnswerFontSize((int)child.selectedAnswerSize));
			}

			if (child.selectedQuestionFontColor != null)
			{
				DBLocalDataStore.GetInstance().SetQuestionFontColor(child.selectedQuestionFontColor);
			}

			if (child.selectedQuestionBackgroundColor != null)
			{
				DBLocalDataStore.GetInstance().SetQuestionBackgroundColor(child.selectedQuestionBackgroundColor);
			}

			if (child.selectedAnswerFontColor != null)
			{
				DBLocalDataStore.GetInstance().SetAnswerFontColor(child.selectedAnswerFontColor);
			}

			if (child.selectedAnswerBackgroundColor != null)
			{
				DBLocalDataStore.GetInstance().SetAnswerBackgroundColor(child.selectedAnswerBackgroundColor);
			}

			this.NavigationController.PopViewController(true);
		}
	}
}
