// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using System.Runtime.InteropServices;
using BoaBeePCL;
using CoreGraphics;
using System.Drawing;

namespace BoaBee.iOS
{
	public partial class AppearanceTableViewController : UITableViewController, IUIActionSheetDelegate
	{
		private TestColorPicker colorPicker;

		public FontSize? selectedQuestionSize = null;
		public FontSize? selectedAnswerSize = null;

		public DBColor selectedQuestionBackgroundColor;
		public DBColor selectedQuestionFontColor;

		public DBColor selectedAnswerBackgroundColor;
		public DBColor selectedAnswerFontColor;

		public AppearanceTableViewController (IntPtr handle) : base (handle)
		{
		}

		private UIImage getImageWithColorAndSize(UIColor color, CGSize size)
		{
			CGRect rect = new CGRect(CGPoint.Empty, size);

			UIGraphics.BeginImageContext(rect.Size);
			CGContext context = UIGraphics.GetCurrentContext();

			context.SetFillColor(color.CGColor);
			context.FillRect(rect);

			UIImage image = UIGraphics.GetImageFromCurrentImageContext().Scale(size, UIScreen.MainScreen.Scale);

			UIGraphics.EndImageContext();

			return image;
		}

		private void init()
		{
			this.initAnswerSize();
			this.initQuestionSize();
			this.initQuestionBackgroundColor();
			this.initQuestionFontColor();
			this.initAnswerFontColor();
			this.initAnswerBackgroundColor();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			this.init();
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			return new nfloat (0.0001);
		}

		public override nfloat GetHeightForFooter (UITableView tableView, nint section)
		{
			return new nfloat(0.0001);
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);

			UITableViewCell selectedCell = tableView.CellAt(indexPath);

			UIAlertController alertController = null;
			switch (selectedCell.Tag)
			{
				case 1:
				{
					alertController = UIAlertController.Create("Question font size", null, UIAlertControllerStyle.ActionSheet);

					alertController.AddAction(UIAlertAction.Create("Small", UIAlertActionStyle.Default, ((UIAlertAction action) => 
					{
						this.selectedQuestionSize = FontSize.Small;
						this.questionFontSizeLabel.Text = "SMALL";
						this.questionFontSizeLabel.Font = this.questionFontSizeLabel.Font.WithSize((int)FontSize.Small);
					})));
					alertController.AddAction(UIAlertAction.Create("Medium", UIAlertActionStyle.Default, ((UIAlertAction action) => 
					{
						this.selectedQuestionSize = FontSize.Medium;
						this.questionFontSizeLabel.Text = "MEDIUM";
						this.questionFontSizeLabel.Font = this.questionFontSizeLabel.Font.WithSize((int)FontSize.Medium);
					})));
					alertController.AddAction(UIAlertAction.Create("Large", UIAlertActionStyle.Default, ((UIAlertAction action) => 
					{
						this.selectedQuestionSize = FontSize.Large;
						this.questionFontSizeLabel.Text = "LARGE";
						this.questionFontSizeLabel.Font = this.questionFontSizeLabel.Font.WithSize((int)FontSize.Large);
					})));

					alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
					break;
				}
				case 2:
				{
					this.colorPicker = (TestColorPicker)this.Storyboard.InstantiateViewController("TestColorPicker");
					this.colorPicker.mode = colorPickerMode.QuestionFontColor;
					this.colorPicker.initialColor = this.selectedQuestionFontColor;
					this.colorPicker.onPickColor += ColorPicker_onPickColor;
					this.PresentViewController(this.colorPicker, true, null);

					break;
				}
				case 3:
				{
					this.colorPicker = (TestColorPicker)this.Storyboard.InstantiateViewController("TestColorPicker");
					this.colorPicker.mode = colorPickerMode.QuestionBackground;
					this.colorPicker.initialColor = this.selectedQuestionBackgroundColor;
					this.colorPicker.onPickColor += ColorPicker_onPickColor;
					this.PresentViewController(this.colorPicker, true, null);

					break;
				}
				case 4:
				{
					alertController = UIAlertController.Create("Answer font size", null, UIAlertControllerStyle.ActionSheet);
					
					alertController.AddAction(UIAlertAction.Create("Small", UIAlertActionStyle.Default, ((UIAlertAction action) => 
					{
						this.selectedAnswerSize = FontSize.Small;
						this.answerFontSizeLabel.Text = "SMALL";
						this.answerFontSizeLabel.Font = this.answerFontSizeLabel.Font.WithSize((int)FontSize.Small);
					})));
					alertController.AddAction(UIAlertAction.Create("Medium", UIAlertActionStyle.Default, ((UIAlertAction action) => 
					{
						this.selectedAnswerSize = FontSize.Medium;
						this.answerFontSizeLabel.Text = "MEDIUM";
						this.answerFontSizeLabel.Font = this.answerFontSizeLabel.Font.WithSize((int)FontSize.Medium);
					})));
					alertController.AddAction(UIAlertAction.Create("Large", UIAlertActionStyle.Default, ((UIAlertAction action) => 
					{
						this.selectedAnswerSize = FontSize.Large;
						this.answerFontSizeLabel.Text = "LARGE";
						this.answerFontSizeLabel.Font = this.answerFontSizeLabel.Font.WithSize((int)FontSize.Large);
					})));

					alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
					break;
				}
				case 5:
				{
					this.colorPicker = (TestColorPicker)this.Storyboard.InstantiateViewController("TestColorPicker");
					this.colorPicker.mode = colorPickerMode.AnswerFontColor;
					this.colorPicker.initialColor = this.selectedAnswerFontColor;
					this.colorPicker.onPickColor += ColorPicker_onPickColor;
					this.PresentViewController(this.colorPicker, true, null);

					break;
				}
				case 6:
				{
					this.colorPicker = (TestColorPicker)this.Storyboard.InstantiateViewController("TestColorPicker");
					this.colorPicker.mode = colorPickerMode.AnswerBackground;
					this.colorPicker.initialColor = this.selectedAnswerBackgroundColor;
					this.colorPicker.onPickColor += ColorPicker_onPickColor;
					this.PresentViewController(this.colorPicker, true, null);

					break;
				}
			}
			if (alertController != null)
			{
				if (alertController.PopoverPresentationController != null)
				{
					alertController.PopoverPresentationController.SourceView = this.TableView.CellAt(indexPath);
					alertController.PopoverPresentationController.SourceRect = this.TableView.CellAt(indexPath).Bounds;
				}
				this.PresentViewController(alertController, true, null);
			}
		}

		private void ColorPicker_onPickColor (colorPickerMode mode, DBColor color)
		{
			switch (mode)
			{
				case colorPickerMode.QuestionFontColor:
				{
					this.selectedQuestionFontColor = color;

					break;
				}
				case colorPickerMode.QuestionBackground:
				{
					this.selectedQuestionBackgroundColor = color;

					break;
				}
				case colorPickerMode.AnswerFontColor:
				{
					this.selectedAnswerFontColor = color;

					break;
				}
				case colorPickerMode.AnswerBackground:
				{
					this.selectedAnswerBackgroundColor = color;

					break;
				}
			}
		}

		private void initQuestionSize()
		{
			int questionSize;
			if (this.selectedQuestionSize != null)
			{
				questionSize = (int)this.selectedQuestionSize;
			}
			else
			{
				questionSize = DBLocalDataStore.GetInstance().GetQuestionFontSize().size;
			}

			if (questionSize < (int)FontSize.Medium)
			{
				this.questionFontSizeLabel.Text = "SMALL";
			}
			else if (questionSize > (int)FontSize.Medium)
			{
				this.questionFontSizeLabel.Text = "LARGE";
			}
			else
			{
				this.questionFontSizeLabel.Text = "MEDIUM";
			}
			this.questionFontSizeLabel.Font = this.questionFontSizeLabel.Font.WithSize(questionSize);
		}

		private void initAnswerSize()
		{
			int answerSize;
			if (this.selectedAnswerSize != null)
			{
				answerSize = (int)this.selectedAnswerSize;
			}
			else
			{
				answerSize = DBLocalDataStore.GetInstance().GetAnswerFontSize().size;
			}

			if (answerSize < (int)FontSize.Medium)
			{
				this.answerFontSizeLabel.Text = "SMALL";
			}
			else if (answerSize > (int)FontSize.Medium)
			{
				this.answerFontSizeLabel.Text = "LARGE";
			}
			else
			{
				this.answerFontSizeLabel.Text = "MEDIUM";
			}
			this.answerFontSizeLabel.Font = this.answerFontSizeLabel.Font.WithSize(answerSize);
		}

		private void initQuestionFontColor()
		{
			DBColor questionFontColor;
			if (this.selectedQuestionFontColor != null)
			{
				questionFontColor = this.selectedQuestionFontColor;
			}
			else
			{
				questionFontColor = DBLocalDataStore.GetInstance().GetQuestionFontColor();
			}
			this.questionFontColorPreview.Layer.BorderColor = UIColor.White.CGColor;
			this.questionFontColorPreview.Image = this.getImageWithColorAndSize(UIColor.FromRGB(questionFontColor.red, questionFontColor.green, questionFontColor.blue), this.questionFontColorPreview.Frame.Size);
		}

		private void initQuestionBackgroundColor()
		{
			DBColor questionBackgroundColor;
			if (this.selectedQuestionBackgroundColor != null)
			{
				questionBackgroundColor = this.selectedQuestionBackgroundColor;
			}
			else
			{
				questionBackgroundColor = DBLocalDataStore.GetInstance().GetQuestionBackgroundColor();
			}
			this.questionBackgroundPreview.Layer.BorderColor = UIColor.White.CGColor;
			this.questionBackgroundPreview.Image = this.getImageWithColorAndSize(UIColor.FromRGB(questionBackgroundColor.red, questionBackgroundColor.green, questionBackgroundColor.blue), this.questionBackgroundPreview.Frame.Size);
		}

		private void initAnswerFontColor()
		{
			DBColor answerFontColor;
			if (this.selectedAnswerFontColor != null)
			{
				answerFontColor = this.selectedAnswerFontColor;
			}
			else
			{
				answerFontColor = DBLocalDataStore.GetInstance().GetAnswerFontColor();
			}

			this.answerFontColorPreview.Layer.BorderColor = UIColor.White.CGColor;
			this.answerFontColorPreview.Image = this.getImageWithColorAndSize(UIColor.FromRGB(answerFontColor.red, answerFontColor.green, answerFontColor.blue), this.answerFontColorPreview.Frame.Size);
		}

		private void initAnswerBackgroundColor()
		{
			DBColor answerBackgroundColor;
			if (this.selectedAnswerBackgroundColor != null)
			{
				answerBackgroundColor = this.selectedAnswerBackgroundColor;
			}
			else
			{
				answerBackgroundColor = DBLocalDataStore.GetInstance().GetAnswerBackgroundColor();
			}

			this.answerBackgroundPreview.Layer.BorderColor = UIColor.White.CGColor;
			this.answerBackgroundPreview.Image = this.getImageWithColorAndSize(UIColor.FromRGB(answerBackgroundColor.red, answerBackgroundColor.green, answerBackgroundColor.blue), this.answerBackgroundPreview.Frame.Size);
		}
	}
}