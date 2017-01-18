// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace BoaBee.iOS
{
	public partial class KioskLoopNavigationController : UINavigationController, IUIAlertViewDelegate
	{
		private NSObject idleTimerNotificationToken;

		private NSTimer countdownTimer;

		private CustomUIApplication uiApplication;

		private int countDownTicks = 0;

		public KioskLoopNavigationController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.uiApplication = (CustomUIApplication)CustomUIApplication.SharedApplication;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

//			this.uiApplication.idleTimerEnabled = true;
			if (this.idleTimerNotificationToken == null)
			{
				this.idleTimerNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"AppDidTimeOut", idleTimerCallback);
			}
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			this.uiApplication.idleTimerEnabled = false;

			if (this.idleTimerNotificationToken != null)
			{
				this.idleTimerNotificationToken.Dispose();
				this.idleTimerNotificationToken = null;
			}
		}

		private void idleTimerCallback(NSNotification notification)
		{
			this.uiApplication.idleTimerEnabled = false;

            if (this.TopViewController.GetType().Equals(typeof(KioskFinishViewController)))
            {
                Console.Error.WriteLine("Idle timer fired on finish screen");
                return;
            }

            if (this.TopViewController.GetType().Equals(typeof(KioskWelcomeViewController)))
            {
                Console.Error.WriteLine("Idle timer fired on welcome screen");
                return;
            }

			UIAlertView idleAlert = new UIAlertView(null, "This session will be closed in 10 seconds", null, "No, let me continue", null);

			idleAlert.Delegate = this;
			idleAlert.Show();

			this.countdownTimer = NSTimer.CreateScheduledTimer(1, this, new ObjCRuntime.Selector("countdownTimerTick:"), idleAlert, true);
		}

		[Export("countdownTimerTick:")]
		private void countdownTimerTick(NSTimer timer)
		{
			this.countDownTicks++;
			UIAlertView alert = (UIAlertView)timer.UserInfo;
			if (10 - this.countDownTicks <= 0)
			{
				timer.Invalidate();
				timer.Dispose();
				this.countDownTicks = 0;

				alert.DismissWithClickedButtonIndex(-1, true);
				this.PopToRootViewController(true);
			}
			else
			{
				string messageString = string.Format("This session will be closed in {0} seconds", 10 - this.countDownTicks);
				alert.Message = messageString;
			}
			//Console.Error.WriteLine("countdownTimerTick");
		}

		[Export("countdownTimerExceeded:")]
		private void countdownTimerExceeded(NSTimer timer)
		{
			UIAlertView alert = (UIAlertView)timer.UserInfo;
			alert.DismissWithClickedButtonIndex(-1, true);
			this.PopToRootViewController(true);
		}

		[Export("alertView:clickedButtonAtIndex:")]
		public void alertClicked(UIAlertView alertview, nint buttonIndex)
		{
			if (buttonIndex == 0)
			{
				this.uiApplication.idleTimerEnabled = true;
				this.uiApplication.resetIdleTimer();
				this.countdownTimer.Invalidate();
				this.countdownTimer.Dispose();
				this.countDownTicks = 0;
			}
		}

		public override UIViewController[] PopToRootViewController(bool animated)
		{
			this.uiApplication.idleTimerEnabled = false;
			this.uiApplication.resetIdleTimer();

			return base.PopToRootViewController(animated);
		}

		public override UIViewController PopViewController(bool animated)
		{
			UIViewController returnValue = base.PopViewController(animated);

			if (returnValue.GetType().Equals(typeof(KioskEmailViewController)))
			{
				this.uiApplication.idleTimerEnabled = false;
				this.uiApplication.resetIdleTimer();
			}
			return returnValue;
		}

		public override void PushViewController(UIViewController viewController, bool animated)
		{
			base.PushViewController(viewController, animated);
			this.uiApplication.idleTimerEnabled = true;
		}
	}
}
