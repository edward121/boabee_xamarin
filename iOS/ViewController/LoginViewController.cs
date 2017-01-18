// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BoaBeeLogic;
using BoaBeePCL;
using CoreGraphics;
using FireBase;
using Foundation;
using MultiWriter;
using SafariServices;
using UIKit;

namespace BoaBee.iOS
{
    public partial class LoginViewController : UIViewController, INSUrlConnectionDataDelegate, ISFSafariViewControllerDelegate
    {
        private NSObject kbFrameChangeNoificationToken;

        private LoadingOverlay overlay = null;

        private DBUserLoginRequest userLoginInfo { get; set; }

        private List<DBBasicAuthority> profilesList = new List<DBBasicAuthority>();

        private DBUserLoginRequest lastUser;

        public LoginViewController (IntPtr handle) : base (handle)
        {
        }

        private void checkDBVersion()
        {
            string messageString = "Due to changes in the internal data structure, if you were using app version 1.08 or lower, we need to clean your data.\n" +
                "Otherwise you may experience unexpected app behaviour.\n" +
                "Proceed?";

            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = System.IO.Path.Combine(libraryPath, "database.dbx");

            NSDictionary infoDictionary = NSBundle.MainBundle.InfoDictionary;
            var localVersion = infoDictionary["CFBundleShortVersionString"].ToString();
            var localBuild = infoDictionary["CFBundleVersion"].ToString();

            var alert = UIAlertController.Create(null, messageString, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Destructive, (obj) =>
            {
                File.Delete(dbPath);

                NSUserDefaults.StandardUserDefaults.SetString(localVersion, "LastVersion");
                NSUserDefaults.StandardUserDefaults.SetString(localBuild, "LastBuild");
                NSUserDefaults.StandardUserDefaults.Synchronize();

                this.NavigationController.PopToRootViewController(false);
                this.DismissViewController(true, null);
            }));
            alert.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Default, (obj) =>
            {
                NSUserDefaults.StandardUserDefaults.SetString(localVersion, "LastVersion");
                NSUserDefaults.StandardUserDefaults.SetString(localBuild, "LastBuild");
                NSUserDefaults.StandardUserDefaults.Synchronize();
            }));

            var lastRunVersion = NSUserDefaults.StandardUserDefaults.StringForKey("LastVersion");

            if (lastRunVersion == null || float.Parse(lastRunVersion, CultureInfo.InvariantCulture.NumberFormat) <= 1.09)
            {
                Alert.PresentAlert(alert, true, null);
            }
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSDictionary infoDictionary = NSBundle.MainBundle.InfoDictionary;
            var localVersion = infoDictionary["CFBundleShortVersionString"];
            var localBuild = infoDictionary["CFBundleVersion"];

            this.versionLabel.Text = string.Format("Version {0} Build {1}", infoDictionary["CFBundleShortVersionString"], infoDictionary["CFBundleVersion"]);

            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = System.IO.Path.Combine(libraryPath, "database.dbx");

            if (File.Exists(dbPath))
            {
                this.checkDBVersion();
            }
            else
            {
                NSUserDefaults.StandardUserDefaults.SetString(localVersion.ToString(), "LastVersion");
                NSUserDefaults.StandardUserDefaults.SetString(localBuild.ToString(), "LastBuild");
                NSUserDefaults.StandardUserDefaults.Synchronize();
            }

            try
            {
                var appSettings = DBLocalDataStore.GetInstance().GetAppSettings();
                if (appSettings == null)
                {
                    appSettings = new DBAppSettings();
                    appSettings.instantContactCheck = true;
                    DBLocalDataStore.GetInstance().SetAppSettings(appSettings);
                    Console.Error.WriteLine("AppSettings initialized");
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("AppSettings initialization failed with exception: {0}", e.Message);
            }

            //Logfile init
            string nowDate = DateTime.Now.ToString("dd MMM yyyy HH-mm-ss");
            string logPath = System.IO.Path.Combine(libraryPath, string.Format("Log-{0}.txt", nowDate));

            try
            {
                Console.SetOut(new MultiOutputWriter(Console.Error, Console.Out, logPath));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            
            //Set attributes
            UIStringAttributes placeholderAttributes = new UIStringAttributes();
            placeholderAttributes.ForegroundColor = UIColor.White;

            NSAttributedString loginPlaceholder = new NSAttributedString (loginUserTextField.Placeholder, placeholderAttributes);
            NSAttributedString passPlaceholder = new NSAttributedString (loginPassTextField.Placeholder, placeholderAttributes);
            NSAttributedString namePlaceholder = new NSAttributedString(nameTextField.Placeholder, placeholderAttributes);

            loginUserTextField.AttributedPlaceholder = loginPlaceholder;
            loginPassTextField.AttributedPlaceholder = passPlaceholder;
            nameTextField.AttributedPlaceholder = namePlaceholder;

            loginUserTextField.Layer.BorderColor = UIColor.FromRGB(0x4A, 0x4A, 0x4A).CGColor;
            loginPassTextField.Layer.BorderColor = UIColor.FromRGB(0x4A, 0x4A, 0x4A).CGColor;
            nameTextField.Layer.BorderColor = UIColor.FromRGB(0x4A, 0x4A, 0x4A).CGColor;

            try
            {
                this.lastUser = DBLocalDataStore.GetInstance().GetLocalUserInfo();

                if (this.lastUser != null)
                {
                    this.loginUserTextField.Text = this.lastUser.username;
                    this.loginPassTextField.Text = this.lastUser.password;
                    if (this.lastUser.username.Equals(this.lastUser.tags))
                    {
                        this.nameTextField.Text = "";
                    }
                    else
                    {
                        this.nameTextField.Text = this.lastUser.tags;
                    }
                }
            }
            catch
            {
                Console.Error.WriteLine("Failed to get last user info");
            }

            #if DEBUG
            {
                this.loginUserTextField.Text = "kolibrisoftware@gmail.com";
                //this.loginUserTextField.Text = "fedor";
                this.loginPassTextField.Text = "St@rtN0w";
            }
            #endif

            await Task.Run(async () =>
            {
                if (await Reachability.isConnected())
                {
                    string serverVersion = FirebaseManager.GetInstance().GetObjectByNameJson("AppVersionIOS");

                    float serverVersionFloat = float.Parse(serverVersion, CultureInfo.InvariantCulture.NumberFormat);
                    float localVersionFloat = float.Parse(localVersion.ToString(), CultureInfo.InvariantCulture.NumberFormat);

                    if (serverVersionFloat > localVersionFloat)
                    {
                        this.InvokeOnMainThread(() =>
                        {
                            UIAlertController alert = UIAlertController.Create(null, "A more recent version of the app is available. Please install this version before continuing.", UIAlertControllerStyle.Alert);
                            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (UIAlertAction obj) =>
                            {
                                Console.Error.WriteLine("update");
                                UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/ru/app/boabee-2/id1070567969?mt=8"));
                            }));

                            Alert.PresentAlert(alert, true, null);
                        });
                        Console.Error.WriteLine("!!Need to update!!");
                    }
                    else
                    {
                        Console.Error.WriteLine("No need to update");
                    }

                    Console.Error.WriteLine("serverVersion:{0}\nLocalVersion:{1}", serverVersionFloat, localVersionFloat);
                }
            });
        }

        static string MD5(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] md5 = md5Provider.ComputeHash(bytes);
            string result = "";
            foreach (var b in md5)
            {
                result += b.ToString("x2");
            }
            return result;
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);

            var name = UIDevice.CurrentDevice.Name;
            var vendorID = UIDevice.CurrentDevice.IdentifierForVendor.AsString();

            //var appID = string.Concat(vendorID, NSBundle.MainBundle.BundleIdentifier);
            //appID = LoginViewController.MD5(appID);

            //new UIAlertView(string.Format("Device unique vendorID {0}", vendorID), "Vendor ID stays the same until all apps of specific vendor are removed from device", null, "OK", null).Show();
            //new UIAlertView(string.Format("Unique appID {0}", appID), null, null, "OK", null).Show();

            NSDictionary infoDictionary = NSBundle.MainBundle.InfoDictionary;
            string version = string.Format("Version {0} Build {1}", infoDictionary["CFBundleShortVersionString"], infoDictionary["CFBundleVersion"]);
            this.versionLabel.Text = version;

            //Autologin
            var shouldAutologin = DBLocalDataStore.GetInstance().GetShouldAutologin();

            if (this.lastUser != null && shouldAutologin.shouldAutologin)
            {
                this.profilesList = DBLocalDataStore.GetInstance ().GetProfiles ();

                SelectAppViewController selectAppVC = (SelectAppViewController)this.Storyboard.InstantiateViewController ("SelectAppViewController");
                selectAppVC.dbAppEntities = this.profilesList;

                var selectedApp = DBLocalDataStore.GetInstance().GetSelectProfile();
                if (selectedApp != null)
                {
                    selectAppVC.isFromLogin = true;
                }
                else
                {
                    selectAppVC.isFromLogin = false;
                }

                this.NavigationController.PushViewController (selectAppVC, false);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (this.kbFrameChangeNoificationToken == null)
            {
                this.kbFrameChangeNoificationToken = UIKeyboard.Notifications.ObserveWillChangeFrame(kbFrameChangeCallback);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (this.kbFrameChangeNoificationToken != null)
            {
                this.kbFrameChangeNoificationToken.Dispose();
                this.kbFrameChangeNoificationToken = null;
            }
        }

        private void kbFrameChangeCallback (object sender, UIKeyboardEventArgs args)
        {
            CGRect beginFrame = args.FrameBegin;
            CGRect endFrame = args.FrameEnd;

            Console.Error.WriteLine("kbFrameChange \nbegin frame \n{0} \nend frame \n{1}",beginFrame.ToString(), endFrame.ToString());

            if (beginFrame.Equals(endFrame))
            {
                return;
            }
                
            UIView.Animate(args.AnimationDuration, 
                () =>
                {
                    this.viewBottomConstraint.Constant += (beginFrame.Y - endFrame.Y);
                    this.View.LayoutIfNeeded();
                });
        }

        partial void eyeTouchUp(UIButton sender)
        {
            if (this.loginPassTextField.IsFirstResponder)
            {
                var selectedRange = this.loginPassTextField.SelectedTextRange;                 var cursorPosition = this.loginPassTextField.GetOffsetFromPosition(this.loginPassTextField.BeginningOfDocument, selectedRange.Start);

                this.kbFrameChangeNoificationToken.Dispose();
                this.kbFrameChangeNoificationToken = null;
                this.loginPassTextField.ResignFirstResponder();                 if (this.loginPassTextField.SecureTextEntry)
                {
                    this.loginPassTextField.SecureTextEntry = false;
                    this.eyeButton.Selected = true;
                }
                else
                {
                    this.loginPassTextField.SecureTextEntry = true;
                    this.eyeButton.Selected = false;
                }                 this.loginPassTextField.BecomeFirstResponder();
                this.kbFrameChangeNoificationToken = UIKeyboard.Notifications.ObserveWillChangeFrame(kbFrameChangeCallback);
                 var newPosition = this.loginPassTextField.GetPosition(this.loginPassTextField.BeginningOfDocument, cursorPosition);                 this.loginPassTextField.SelectedTextRange = this.loginPassTextField.GetTextRange(newPosition, newPosition);
            }
            else
            {
                if (this.loginPassTextField.SecureTextEntry)
                {
                    this.loginPassTextField.SecureTextEntry = false;
                    this.eyeButton.Selected = true;
                }
                else
                {
                    this.loginPassTextField.SecureTextEntry = true;
                    this.eyeButton.Selected = false;
                }
            }
        }

        async partial void firstTimeUserButtonClick (UIButton sender)
        {
            //if (await Reachability.isConnected())
            //{
            //    SFSafariViewController safari = new SFSafariViewController(NSUrl.FromString("https://www.boabee.com/signup/"));
            //    safari.Delegate = new SafariDelegate();
            //    UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.Default, true);
            //    this.PresentViewController(safari, true, null);

            //    //UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://www.boabee.com/signup/"));
            //}
            //else
            //{
            //    UIAlertController alertController = UIAlertController.Create(null, "No internet access, try again later.", UIAlertControllerStyle.Alert);
            //    alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            //    this.PresentViewController(alertController, true, null);
            //}
        }

        async partial void forgotPasswordButtonClick (UIButton sender)
        {
//            if (await Reachability.isConnected())
//            {
//                SFSafariViewController safari = new SFSafariViewController(NSUrl.FromString("https://cloud.boabee.com"));
//                safari.Delegate = new SafariDelegate();
//                UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.Default, true);
//                this.PresentViewController(safari, true, null);

////              UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("https://cloud.boabee.com"));
//            }
//            else
//            {
                //UIAlertController alertController = UIAlertController.Create(null, "No internet access, try again later.", UIAlertControllerStyle.Alert);
                //alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                //this.PresentViewController(alertController, true, null);
//            }
        }

        async partial void loginLoginBtnTouchUpInside (Foundation.NSObject sender)
        {
            string user = this.loginUserTextField.Text;
            string pass = this.loginPassTextField.Text;
            string name = (this.nameTextField.Text != null && this.nameTextField.Text.Length > 0) ? this.nameTextField.Text : this.loginUserTextField.Text;

            if(!isUserAndPassValid(user, pass))
            {
                UIAlertController alertController = UIAlertController.Create("Error:","E-mail or password incorrect.", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                this.PresentViewController(alertController, true, null);
                return;
            }

            string passEncrypted = passEncrypt(pass);

            this.loginUserTextField.ResignFirstResponder();
            this.loginPassTextField.ResignFirstResponder();

            //Show loading spinner
            this.overlay = new LoadingOverlay((RectangleF)this.View.Frame, "Loading Data...");

            this.View.AddSubview(overlay);

            //Check connection with server and then authenticate user
            if (!await Reachability.isConnected())
            {
                UIAlertController alertController = UIAlertController.Create(null, "No internet access or the server is not available now, try again later.", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                this.PresentViewController(alertController, true, null);

                overlay.RemoveFromSuperview();

                return;
            }

            try
            {
                //Perform data for post
                userLoginInfo = new DBUserLoginRequest();
                userLoginInfo.username = user;
                userLoginInfo.password = passEncrypted;
                userLoginInfo.tags = name;

                //Post request
                await NetworkRequests.performAuth(userLoginInfo, (success, messageTitle, message, list) =>
                {
                    this.InvokeOnMainThread(() =>
                    {
                        this.overlay.RemoveFromSuperview();

                        if (!success)
                        {
                            UIAlertController alertController = UIAlertController.Create(messageTitle, message, UIAlertControllerStyle.Alert);
                            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                            this.PresentViewController(alertController, true, null);
                        }
                        else
                        {
                            var shouldAutologin = new DBUserAutologin();
                            shouldAutologin.shouldAutologin = true;
                            DBLocalDataStore.GetInstance().SetShouldAutologin(shouldAutologin);

                            this.profilesList = list;

                            this.PerformSegue(@"toSelectApp", null);
                        }
                    });
                });
            }
            catch(Exception exeption)
            {
                this.overlay.RemoveFromSuperview();

                UIAlertController alertController = UIAlertController.Create("Error:", exeption.Message, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                this.PresentViewController(alertController, true, null);

                Console.Error.WriteLine (exeption.Message);
            }
        }

        [Export(@"textFieldShouldReturn:")]
        bool textFieldShouldReturn(UITextField sender)
        {
            if(sender.Equals (this.loginUserTextField))
            {
                this.loginPassTextField.BecomeFirstResponder ();
                return true;
            }
            if (sender.Equals (this.loginPassTextField))
            {
                this.nameTextField.BecomeFirstResponder();
                return true;
            }
            if (sender.Equals (this.nameTextField))
            {
                this.nameTextField.ResignFirstResponder ();
                return true;
            }
            return true;
        }

        public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier.Equals("toSelectApp"))
            {
                SelectAppViewController target = (SelectAppViewController)segue.DestinationViewController;

                target.dbAppEntities = this.profilesList;
                target.isFromLogin = true;
            }
        }

        private bool isEmail(string email)
        {
            string regexPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$";

            if (Regex.Match (email, regexPattern).Success)
            {
                return true;
            }

            return false;
        }

        private bool isUserAndPassValid(string user, string pass)
        {
            if (string.IsNullOrWhiteSpace (user))
            {
                return false;
            }
//          if (!isEmail(user))
//          {
//              return false;
//          }
            if (string.IsNullOrWhiteSpace(pass))
            {
                return false;
            }

            return true;
        }

        private string passEncrypt(string pass)
        {
            return pass;
        }
    }
}
