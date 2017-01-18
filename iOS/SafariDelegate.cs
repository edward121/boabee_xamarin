using System;
using SafariServices;
using UIKit;

namespace BoaBee.iOS
{
	public class SafariDelegate:SFSafariViewControllerDelegate
	{
		public SafariDelegate()
		{
		}

		public override void DidFinish(SFSafariViewController controller)
		{
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, true);
		}
	}
}

