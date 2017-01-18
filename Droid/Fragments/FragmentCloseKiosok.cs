
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Leadbox
{
[Activity(Label = "FragmentCloseKiosok")]
public class FragmentCloseKiosok : DialogFragment
	{
	Context context;
	System.Timers.Timer _timer;
	int _countSeconds;
	int ActivityNow;
	TextView txtview;
	public FragmentCloseKiosok(Context context, System.Timers.Timer _timer,int ActivityNow)
	{
		this.context = context;
		this._timer = _timer;
		this.ActivityNow = ActivityNow;
	}
		public override void OnCancel(IDialogInterface dialog)
			{
		//	((ActivityEmailKiosk)context).timer();
			}
	public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Typeface font = MainActivity.font;
			//base.OnCreateView(inflater, container, savedInstanceState);
			var view = inflater.Inflate(Resource.Layout.DialogCloseKiosok, container, false);
			var btnok = view.FindViewById<Button>(Resource.Id.buttonclosedialog);
			txtview = view.FindViewById<TextView>(Resource.Id.textViewCloseDialog);
			txtview.Text = "This session will be closed in 10 seconds";
			btnok.Text = "No, let me continue";
			btnok.Click += btnok_click;
			timer();

			txtview.SetTypeface(font, TypefaceStyle.Normal);
			btnok.SetTypeface(font, TypefaceStyle.Normal);

			return view;
		}
	void btnok_click(object sender, EventArgs e)
		{
			
			if(ActivityNow == 1)
				{
					((ActivityEmailKiosk)context).timer();
				}

			else if (ActivityNow == 2)
				{
					((ActivityKioskMain)context).timer();
				}
			else { }
			Dialog.Cancel();
		}

	public void timer()
		{
			if (_timer != null)
			{
				_timer.Enabled = false;
				_timer.Close();
			}
			_timer = new System.Timers.Timer();
			_timer.Interval = 1000;
			_timer.Elapsed += OnTimedEvent;
			_countSeconds = 10;
			_timer.Enabled = true;
		}
	public void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
		{
			_countSeconds--;
			Activity.RunOnUiThread(() => { 
				txtview.Text = string.Format("This session will be closed in {0} seconds", _countSeconds);
			});


			if (_countSeconds == 0)
				{
					StartActivity(new Intent(context, typeof(ActivityTouchKiosk)));
					((Activity)context).Finish();
				}
		}
	}

}

