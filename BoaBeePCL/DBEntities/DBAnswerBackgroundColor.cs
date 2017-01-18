using System;

namespace BoaBeePCL
{
	public class DBAnswerBackgroundColor : DBColor
	{
		public DBAnswerBackgroundColor()
		{
		}

		public DBAnswerBackgroundColor(DBColor color)
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

