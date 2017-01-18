// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace BoaBee.iOS
{
	public partial class InfoSheetDetailsCell : UITableViewCell
	{
		public InfoSheetDetailsCell (IntPtr handle) : base (handle)
		{
		}

		public void initWithQuestion(string question)
		{
			this.questionLabel.Text = question;
		}

		public void initWithAnswerLabel(string answer)
		{
			this.answerLabel.Text = answer;
		}

		public void initWithAnswerTextViewAndTag(string answer, int tag)
		{
			this.answerTextView.Text = answer;
			this.answerTextView.Tag = tag;
		}

		public UITextView getTextView()
		{
			return this.answerTextView;
		}
	}
}