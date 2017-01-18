// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using BoaBeePCL;
using System.Threading.Tasks;
using BoaBeeLogic;

namespace BoaBee.iOS
{
	public partial class KioskFinishViewController : UIViewController
	{
		private NSTimer restartTimer;

        public string contactUID;

		public KioskFinishViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            DBAppInfo appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
			var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

			if (!string.IsNullOrWhiteSpace(appInfo.finishedImageLocalPath))
			{
				var imagePath = System.IO.Path.Combine(documentsDirectory, appInfo.finishedImageLocalPath);
				UIImage finishImage = UIImage.FromFile(imagePath);
				if (finishImage != null)
				{
					this.finishImageView.Image = finishImage;
				}
				else
				{
					this.finishImageView.Image = UIImage.FromBundle("KioskDefaultFinish");
				}
			}
			else
			{
				this.finishImageView.Image = UIImage.FromBundle("KioskDefaultFinish");
			}

		}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIButton popButton = new UIButton(this.View.Frame);
            popButton.TouchUpInside += (sender, e) =>
            {
                this.NavigationController.PopToRootViewController(true);
            };

            popButton.BackgroundColor = UIColor.Clear;
            popButton.Alpha = (nfloat)0.5;

            this.View.AddSubview(popButton);
        }

        private async void printBadge()
        {
            DBKioskSettings kioskSettings = DBLocalDataStore.GetInstance().GetKioskSettings();
            if (kioskSettings != null && kioskSettings.badgePrinting && !string.IsNullOrWhiteSpace(kioskSettings.badgePrintingWebhook))
            {
                //if (await Reachability.isWebhookAvailable())
                {
                    DBlocalContact newContact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(this.contactUID);

                    //Console.WriteLine("Webhook available");

                    NetworkRequests.onFinishPrint += this.printCallbact;

                    await NetworkRequests.sendBadge(newContact, kioskSettings.badgePrintingWebhook);
                }
                //else
                //{
                //    this.restartTimer = NSTimer.CreateScheduledTimer(5.0, this.timerAction);
                //    Console.WriteLine("Webhook not available");
                //}
            }
            else
            {
                this.restartTimer = NSTimer.CreateScheduledTimer(5.0, this.timerAction);
                Console.WriteLine("Webhook disabled");
            }
        }

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

            this.printBadge();
		}

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            NetworkRequests.onFinishPrint -= this.printCallbact;
        }

        private void printCallbact(bool success)
        {
            if (success)
            {
                this.restartTimer = NSTimer.CreateScheduledTimer(5.0, this.timerAction);
            }
            else
            {
                UIAlertController alert = UIAlertController.Create(null, "Sending to printer failed", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Ignore", UIAlertActionStyle.Default, async (obj) =>
                {
                    await Task.Delay(500);
                    this.NavigationController.PopToRootViewController(true);
                }));
                alert.AddAction(UIAlertAction.Create("Retry", UIAlertActionStyle.Default, (obj) =>
                {
                    this.printBadge();
                }));
                this.PresentViewController(alert, true, null);
                //Alert.PresentAlert(alert, true, null);
            }
        }

		private void timerAction(NSTimer timer)
		{
			if (this.restartTimer.IsValid)
			{
				this.restartTimer.Invalidate();
			}
            try
            {
                if (this.NavigationController.TopViewController.Equals(this))
                {
                    this.NavigationController.PopToRootViewController(true);
                }
            }
            catch { }
		}
	}
}
