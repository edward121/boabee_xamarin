using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Content;
using BoaBeePCL;

namespace Leadbox
{
    public class GenericFragmentPagerAdaptor : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> _fragmentList = new List<Android.Support.V4.App.Fragment>();
        public GenericFragmentPagerAdaptor(Android.Support.V4.App.FragmentManager fm)
            : base(fm) {}

        public override int Count
        {
            get { return _fragmentList.Count; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return _fragmentList[position];
        }

		public void AddFragment(Android.Support.V4.App.Fragment fragment)
        {
            _fragmentList.Add(fragment);
        }

        public void AddFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> view)
        {
            _fragmentList.Add(new GenericViewPagerFragment(view));
        }
    }
	public class ViewPageListenerForActionBar : ViewPager.SimpleOnPageChangeListener
    {


        public ViewPageListenerForActionBar(FragmentPagerAdapter _adapter,
            ViewPager _viewPager,
            Activity _context)
			
        {}
			
		public override void OnPageScrolled (int position, float positionOffset, int positionOffsetPixels)
		{
			base.OnPageScrolled (position, positionOffset, positionOffsetPixels);
		}
    }
    public static class ViewPagerExtensions
    {
    }

}