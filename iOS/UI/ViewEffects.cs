using System;
using UIKit;
using CoreGraphics;

namespace BoaBee.iOS
{
	public static class ViewBouncer
	{
		public static void bounceViewWithScaleFactor(UIView view, float scaleFactor)
		{
            UIView.Animate(0.2f, 0, UIViewAnimationOptions.AllowUserInteraction,
			() =>
			{
				view.Transform = CGAffineTransform.Scale(CGAffineTransform.MakeIdentity(), scaleFactor, scaleFactor);
			},
			() =>
			{
				view.Transform = CGAffineTransform.Scale(CGAffineTransform.MakeIdentity(), scaleFactor, scaleFactor);
				UIView.Animate(0.2f, 0, UIViewAnimationOptions.AllowUserInteraction, 
				() =>
				{
					view.Transform = CGAffineTransform.MakeIdentity();
                },
                null);
			});
		}

        public static void bounceViewWithScaleFactor(UIView view, float scaleFactor, float duration)
        {
            UIView.Animate(duration / 2, 0, UIViewAnimationOptions.AllowUserInteraction,
            () =>
            {
                view.Transform = CGAffineTransform.Scale(CGAffineTransform.MakeIdentity(), scaleFactor, scaleFactor);
            },
            () =>
            {
                view.Transform = CGAffineTransform.Scale(CGAffineTransform.MakeIdentity(), scaleFactor, scaleFactor);
                UIView.Animate(duration / 2, 0, UIViewAnimationOptions.AllowUserInteraction,
                () =>
                {
                    view.Transform = CGAffineTransform.MakeIdentity();
                }, 
                null);
            });
        }
	}

    public static class ViewBlinker
    {
        public static void fadeOut(UIView view, nfloat alpha, bool continuous, bool isFirstStep = true)
        {
            if (view == null)
            {
                return;
            }
            var initialAlpha = view.Alpha;

            UIView.Animate(0.2,
            () => { view.Alpha = alpha; },
            () => 
            {
                if (continuous || isFirstStep)
                {
                    fadeIn(view, initialAlpha, continuous, !isFirstStep);
                }
            });
        }

        public static void fadeIn(UIView view, nfloat alpha, bool continuous, bool isFirstStep = true)
        {
            if (view == null)
            {
                return;
            }

            var initialAlpha = view.Alpha;
            UIView.Animate(0.2,
            () => { view.Alpha = alpha; },
            () => 
            {
                if (continuous || isFirstStep)
                {
                    fadeOut(view, initialAlpha, continuous, !isFirstStep);
                }
            });
        }
    }
}

