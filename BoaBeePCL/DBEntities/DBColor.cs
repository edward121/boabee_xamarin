using System;

namespace BoaBeePCL
{
	public class DBColor
	{
		public float red { get; set; }
		public float green { get; set; }
		public float blue { get; set; }

		public byte redByte { get; set; }
		public byte greenByte { get; set; }
		public byte blueByte { get; set; }

		public string tag { get; set; }

		public DBColor()
		{
		}

		public DBColor(float red, float green, float blue)
		{
			this.red = Math.Min(Math.Abs(red), 1);
			this.green = Math.Min(Math.Abs(green), 1);
			this.blue = Math.Min(Math.Abs(blue), 1);

			this.redByte = (byte)Math.Round(this.red * 255);
			this.greenByte = (byte)Math.Round(this.green * 255);
			this.blueByte = (byte)Math.Round(this.blue * 255);
		}

		public DBColor(byte red, byte green, byte blue)
		{
			this.red = red / (float)255;
			this.green = green / (float)255;
			this.blue = blue / (float)255;

			this.redByte = red;
			this.greenByte = green;
			this.blueByte = blue;
		}
	}
}

