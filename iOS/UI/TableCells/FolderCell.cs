// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace BoaBee.iOS
{
	public partial class FolderCell : UITableViewCell
	{
		public FolderCell (IntPtr handle) : base (handle)
		{
		}

        public void initWithFolderName(string name)
        {
            this.folderNameLabel.Text = name;
        }
	}
}
