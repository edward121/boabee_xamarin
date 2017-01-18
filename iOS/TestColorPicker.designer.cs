// WARNING
//
// This file has been generated automatically by Xamarin Studio Indie to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BoaBee.iOS
{
	[Register ("TestColorPicker")]
	partial class TestColorPicker
	{
		[Outlet]
		UIKit.UISlider blueSlider { get; set; }

		[Outlet]
		UIKit.UITextField blueTextField { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint bottomViewConstraint { get; set; }

		[Outlet]
		UIKit.UISlider greenSlider { get; set; }

		[Outlet]
		UIKit.UITextField greenTextField { get; set; }

		[Outlet]
		UIKit.UISwitch isHexSwitch { get; set; }

		[Outlet]
		UIKit.UIView pickerPlaceholder { get; set; }

		[Outlet]
		UIKit.UISlider redSlider { get; set; }

		[Outlet]
		UIKit.UITextField redTextField { get; set; }

		[Action ("bSliderValueChanged:")]
		partial void bSliderValueChanged (UIKit.UISlider sender);

		[Action ("bTextValueChanged:")]
		partial void bTextValueChanged (UIKit.UITextField sender);

		[Action ("closeButtonClick:")]
		partial void closeButtonClick (UIKit.UIButton sender);

		[Action ("gSliderValueChanged:")]
		partial void gSliderValueChanged (UIKit.UISlider sender);

		[Action ("gTextValueChanged:")]
		partial void gTextValueChanged (UIKit.UITextField sender);

		[Action ("isHexValueChanged:")]
		partial void isHexValueChanged (UIKit.UISwitch sender);

		[Action ("rSliderValueChanged:")]
		partial void rSliderValueChanged (UIKit.UISlider sender);

		[Action ("rTextValueChanged:")]
		partial void rTextValueChanged (UIKit.UITextField sender);

		[Action ("saveButtonClick:")]
		partial void saveButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (blueSlider != null) {
				blueSlider.Dispose ();
				blueSlider = null;
			}

			if (blueTextField != null) {
				blueTextField.Dispose ();
				blueTextField = null;
			}

			if (bottomViewConstraint != null) {
				bottomViewConstraint.Dispose ();
				bottomViewConstraint = null;
			}

			if (greenSlider != null) {
				greenSlider.Dispose ();
				greenSlider = null;
			}

			if (greenTextField != null) {
				greenTextField.Dispose ();
				greenTextField = null;
			}

			if (isHexSwitch != null) {
				isHexSwitch.Dispose ();
				isHexSwitch = null;
			}

			if (pickerPlaceholder != null) {
				pickerPlaceholder.Dispose ();
				pickerPlaceholder = null;
			}

			if (redSlider != null) {
				redSlider.Dispose ();
				redSlider = null;
			}

			if (redTextField != null) {
				redTextField.Dispose ();
				redTextField = null;
			}
		}
	}
}
