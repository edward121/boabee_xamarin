using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace BoaBee.iOS
{
	[Register ("CustomUIApplication")]
	public class CustomUIApplication:UIApplication
	{
		private NSTimer idleTimer;
		public bool idleTimerEnabled;

		public CustomUIApplication(IntPtr handle) : base (handle)
		{
		}

		public CustomUIApplication():base()
		{
		}

		public override void SendEvent(UIEvent uievent)
		{
			base.SendEvent(uievent);
			if (!idleTimerEnabled || uievent.Type != UIEventType.Touches)
			{
				return;
			}

			NSSet allTouches = uievent.AllTouches;
			if (allTouches != null && allTouches.Count > 0)
			{
				this.resetIdleTimer();
			}
		}

		public void resetIdleTimer()
		{
			if (this.idleTimer != null)
			{
				this.idleTimer.Invalidate();
				this.idleTimer = null;
			}
			if (idleTimerEnabled)
			{
				int timeout = 30;
				this.idleTimer = NSTimer.CreateScheduledTimer(timeout, this, new ObjCRuntime.Selector("idleTimerExceeded"), null, false);
			}
		}

		[Export("idleTimerExceeded")]
		private void idleTimerExceeded()
		{
			NSNotificationCenter.DefaultCenter.PostNotificationName("AppDidTimeOut", null);
		}
	}
}

