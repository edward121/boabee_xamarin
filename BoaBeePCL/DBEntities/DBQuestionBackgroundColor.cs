using System;

namespace BoaBeePCL
{
	public class DBQuestionBackgroundColor : DBColor
	{
		public DBQuestionBackgroundColor()
		{
		}

		public DBQuestionBackgroundColor(DBColor color)
		{
			this.red = color.red;
			this.green = color.green;
			this.blue = color.blue;

			this.redByte = color.redByte;
			this.greenByte = color.greenByte;
			this.blueByte = color.blueByte;
		}
	}
}
