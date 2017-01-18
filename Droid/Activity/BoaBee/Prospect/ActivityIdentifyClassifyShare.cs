using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Views.Animations;
using Android.Graphics;
using Java.Lang;
using SlidingTabLayoutTutorial;
using Android.Support.V7.App;
//using sToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics.Drawables;
using BoaBeePCL;
using Android.Views.InputMethods;

namespace Leadbox
{
	[Activity (Label = "Activity", 
		ScreenOrientation = ScreenOrientation.Portrait, 
		Theme = "@style/ActivityThemeS", 
		WindowSoftInputMode = SoftInput.AdjustResize)]
	public class ActivityIdentifyClassifyShare : FragmentActivity
	{
        public static bool afterCrash = false;
		public static ViewPager  viewPager;
        public static ViewPager   viewPagerShare;
        //TabsAdapter tabsAdapter;
        public static InputMethodManager manager;
        public static GenericFragmentPagerAdaptor adaptor;
        public static GenericFragmentPagerAdaptor adaptorShare;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_identify_classify_share);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");

            manager = (InputMethodManager)GetSystemService(InputMethodService);

            viewPager = FindViewById<ViewPager>(Resource.Id.pager);
            viewPagerShare = FindViewById<ViewPager>(Resource.Id.pager2);
            
            adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptorShare = new GenericFragmentPagerAdaptor(SupportFragmentManager);

            adaptor.AddFragment(new FragmentClassifyScreen { _viewPager = viewPager });
            viewPager.Adapter = adaptor;
            #pragma warning disable 618
            viewPager.SetOnPageChangeListener(new ViewPageListenerForActionBar(adaptor, viewPager, this));
            #pragma warning restore 618
            viewPager.SetCurrentItem(0, true);
            adaptorShare.AddFragment(new FragmentShareScreen { _viewPager = viewPagerShare });
            viewPagerShare.Adapter = adaptorShare;
            #pragma warning disable 618
            viewPagerShare.SetOnPageChangeListener(new ViewPageListenerForActionBar(adaptorShare, viewPagerShare, this));
            #pragma warning restore 618
            viewPagerShare.SetCurrentItem(0, true);

            if (!afterCrash)
            {
                if (DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition()).Count != 0)
                {
                    viewPager.Visibility = ViewStates.Visible;
                    viewPagerShare.Visibility = ViewStates.Gone;
                    var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutInt("ScrennDestroy", 1);
                    prefEditor.Commit();
                }
                else {
                    viewPager.Visibility = ViewStates.Gone;
                    viewPagerShare.Visibility = ViewStates.Visible;
                    var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutInt("ScrennDestroy", 2);
                    prefEditor.Commit();
                }
            }
            else {
                var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                var somePref = prefs.GetInt("ScrennDestroy", 1);
                if (somePref ==  1)
                {
                    viewPager.Visibility = ViewStates.Visible;
                    viewPagerShare.Visibility = ViewStates.Gone;
                    prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutInt("ScrennDestroy", 1);
                    prefEditor.Commit();
                }
                else if(somePref == 2){ 
                    viewPager.Visibility = ViewStates.Gone;
                    viewPagerShare.Visibility = ViewStates.Visible;
                    prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutInt("ScrennDestroy", 2);
                    prefEditor.Commit();
                }
                afterCrash = false;
            }

           Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        }
		public override void Finish ()
		{
			base.Finish ();
		}

		public override void OnBackPressed ()
		{
			//base.OnBackPressed ();
		}
               
        //private void HideKeuboard()
        //{
        //    InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
        //    imm.HideSoftInputFromWindow(windowToken, 0);
        //}
	}

}

