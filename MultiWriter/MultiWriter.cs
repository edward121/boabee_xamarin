using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;
using System.Collections;
using BoaBee.iOS;

namespace MultiWriter
{
	public class MultiOutputWriter:TextWriter
	{
		private TextWriter externalWriter;
		private TextWriter errorWriter;
		private TextWriter consoleWriter;

		private string pathForExternalWriter = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiOutConsole.MultiWriter"/> class.
		/// </summary>
		/// <param name="errorWriter">Error writer is used to log inner exceptions</param>
		/// <param name="writers">Writers.</param>
		public MultiOutputWriter(TextWriter errorWriter, TextWriter consoleWriter, TextWriter externalWriter)
		{
			this.externalWriter = externalWriter;
			this.consoleWriter = consoleWriter;
			this.errorWriter = errorWriter;
		}

		public MultiOutputWriter(TextWriter errorWriter, TextWriter consoleWriter, string path)
		{
//			this.externalWriter = externalWriter;
			if (string.IsNullOrWhiteSpace(path))
			{
				errorWriter.WriteLine("'Path' parameter can not be null, empty or contain whitespace characters only!");
				throw new ArgumentException("'Path' parameter can not be null, empty or contain whitespace characters only!");
			}
			else
			{
				try
				{
//					errorWriter.WriteLine("Creating log file");
					StreamWriter fileStream = System.IO.File.CreateText(path);
					this.externalWriter = fileStream;
				}
				catch (Exception e)
				{
					errorWriter.WriteLine(string.Format("Can not create file at path {0}\n{1}", path, e.Message));
                    throw;
					//throw new Exception(string.Format("Can not create file at path {0}\n{1}", path, e.Message));
				}
//				errorWriter.WriteLine("Created log file");
			}

			this.consoleWriter = consoleWriter;
			this.errorWriter = errorWriter;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiOutConsole.MultiWriter"/> class.
		/// </summary>
		/// <param name="errorWriter">Error writer is used to log inner exceptions</param>
		/// <param name="writers">Writers.</param>

		public override void Write(char value)
		{
			try
			{
				this.consoleWriter.Write(value);
				this.consoleWriter.Flush();
			}
			catch (Exception e)
			{
				errorWriter.WriteLine(e.Message);
			}

			DateTime now = DateTime.Now;

			if (this.externalWriter != null)
			{
				try
				{
					this.externalWriter.Write(now.ToString("dd MMM yyyy HH-mm-ss") + " " + value);
					this.externalWriter.Flush();
				}
				catch (Exception e)
				{
					errorWriter.WriteLine(e.Message);
				}
			}
		}

		public override void Write(string value)
		{
			try
			{
				this.consoleWriter.Write(value);
				this.consoleWriter.Flush();
			}
			catch (Exception e)
			{
				errorWriter.WriteLine(e.Message);
			}

			DateTime now = DateTime.Now;

			if (this.externalWriter != null)
			{
				try
				{
					this.externalWriter.Write(now.ToString("dd MMM yyyy HH-mm-ss") + " " + value);
					this.externalWriter.Flush();
				}
				catch (Exception e)
				{
					errorWriter.WriteLine(e.Message);
				}
			}
		}

		public override void WriteLine(string value)
		{
			try
			{
				this.consoleWriter.WriteLine(value);
				this.consoleWriter.Flush();
			}
			catch (Exception e)
			{
				errorWriter.WriteLine(e.Message);
			}

			DateTime now = DateTime.Now;

			if (this.externalWriter != null)
			{
				try
				{
					this.externalWriter.WriteLine(now.ToString("dd MMM yyyy HH-mm-ss") + " " + value);
					this.externalWriter.Flush();
				}
				catch (Exception e)
				{
					errorWriter.WriteLine(e.Message);
				}
			}
		}

		public override void WriteLine(char value)
		{
			try
			{
				this.consoleWriter.WriteLine(value);
				this.consoleWriter.Flush();
			}
			catch (Exception e)
			{
				errorWriter.WriteLine(e.Message);
			}

			DateTime now = DateTime.Now;

			if (this.externalWriter != null)
			{
				try
				{
					this.externalWriter.WriteLine(now.ToString("dd MMM yyyy HH-mm-ss") + " " + value);
					this.externalWriter.Flush();
				}
				catch (Exception e)
				{
					errorWriter.WriteLine(e.Message);
				}
			}
		}

		public override void Flush()
		{
			try
			{
				this.consoleWriter.Flush();
			}
			catch (Exception e)
			{
				errorWriter.WriteLine(e.Message);
			}

			if (this.externalWriter != null)
			{
				try
				{
					this.externalWriter.Flush();
				}
				catch (Exception e)
				{
					errorWriter.WriteLine(e.Message);
				}
			}
		}

		public override void Close()
		{
			try
			{
				this.consoleWriter.Close();
			}
			catch (Exception e)
			{
				errorWriter.WriteLine(e.Message);
			}
	
			if (this.externalWriter != null)
			{
				try
				{
					this.externalWriter.Close();
				}
				catch (Exception e)
				{
					errorWriter.WriteLine(e.Message);
				}
			}
		}

		public override Encoding Encoding
		{
			get { return Encoding.ASCII; }
		}
	}
}

