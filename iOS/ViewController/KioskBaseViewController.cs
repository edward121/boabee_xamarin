using System;
using UIKit;
using BoaBeePCL;

namespace BoaBee.iOS
{
	public class KioskBaseViewController: UIViewController
	{
		protected int questionFontSize;
		protected int answerFontSize;

		protected DBColor questionFontColor;
		protected DBColor questionBackgroundColor;

		protected DBColor answerFontColor;
		protected DBColor answerBackgroundColor;

		public KioskBaseViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.questionFontSize = DBLocalDataStore.GetInstance().GetQuestionFontSize().size;
			this.answerFontSize = DBLocalDataStore.GetInstance().GetAnswerFontSize().size;

			this.questionFontColor = DBLocalDataStore.GetInstance().GetQuestionFontColor();
			this.questionBackgroundColor = DBLocalDataStore.GetInstance().GetQuestionBackgroundColor();

			this.answerFontColor = DBLocalDataStore.GetInstance().GetAnswerFontColor();
			this.answerBackgroundColor = DBLocalDataStore.GetInstance().GetAnswerBackgroundColor();
		}

//		public new KioskLoopNavigationController NavigationController
//		{
//			get
//			{
//				return (KioskLoopNavigationController)base.NavigationController;
//			}
//		}
	}
}

