using System;
using UIKit;
namespace BoaBee.iOS
{
    public static class Alert
    {
        public static void PresentAlert(UIAlertController alert, bool animated, Action completion)
        {
            UIViewController top = AppDelegate.getVisibleViewController(UIApplication.SharedApplication.KeyWindow.RootViewController);
            //Console.Error.WriteLine("Top VC Class is: {0}", top.Class.Name);

            top.PresentViewController(alert, animated, completion);
        }
    }
}

