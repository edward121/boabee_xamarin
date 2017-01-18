
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BoaBeePCL;
using SQLitePCL;
using System.Runtime.InteropServices;

namespace Leadbox
{
	public class FragmentsInputPassword : DialogFragment
	{
		Activity _context;
		TextView buttonOk;
		TextView buttonCancle;
		EditText editPasswordVerification ;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public  FragmentsInputPassword(Activity context)
		{
			_context = context;
		}
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			var rootView = inflater.Inflate(Resource.Layout.InputPasswordLayout , container, false);
			buttonOk = rootView.FindViewById<TextView>(Resource.Id.textViewOk);
			buttonCancle = rootView.FindViewById<TextView>(Resource.Id.textViewCancle);
			 editPasswordVerification = rootView.FindViewById<EditText>(Resource.Id.editText1);
			buttonOk.Click += (object sender, EventArgs e) => 
				{
					var userInfo = DBLocalDataStore.GetInstance().GetLocalUserInfo();
					if(userInfo.password == editPasswordVerification.Text)
					{

						ActivityTouchKiosk.newFragment.Dismiss();
//						SaveAndLoad.checkCancelExiteKiosk = true;
						_context.StartActivity(typeof(ActivityHomescreen));
						_context.Finish();
					}
					else
					{
						Toast.MakeText(_context, "Password does not match", ToastLength.Long).Show ();
					}
				};
			buttonCancle.Click += (object sender, EventArgs e) => 
				{
					ActivityTouchKiosk.newFragment.Dismiss();
				};
			return rootView;
//			return base.OnCreateView(inflater, container, savedInstanceState);
		}

	}
}

