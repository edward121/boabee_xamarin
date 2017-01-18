using System;
using System.IO;
using System.Collections.Generic;
using BoaBeePCL;
using SQLite.Extensions;
using Java.Util.Zip;

namespace Leadbox
{
	public class SaveAndLoad 
	{
		public static List<DBfileTO> files_selected = new List<DBfileTO>();
		public static bool checkCancelExiteKiosk = false;
		public static List<DBOverviewQuestionAnswer> tempAnswer = new List<DBOverviewQuestionAnswer>();
		public static DBlocalContact tempContact = new DBlocalContact();
		private static SaveAndLoad _instance;
		private static object _lockCreate = new object();
		int _count;

		private SaveAndLoad() {
		}

		public static SaveAndLoad GetInstance()
		{
			lock (_lockCreate)
			{
				if (_instance == null)
				{
					_instance = new SaveAndLoad();
				}
				return _instance;
			}
		}

		private string GetPath()
		{
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			return Path.Combine (documentsPath, "answers.txt");
		}

		private string GetPath(int _id)
		{
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			return Path.Combine (documentsPath, _id + ".txt");
		}

		public void SetPath(int _id)
		{
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filepath = Path.Combine (documentsPath, _id + ".txt");
			File.WriteAllText (filepath, "");
		}

		public void SaveTextEdit (string _answer, int _id) {
			File.WriteAllText (GetPath(_id), _answer);
		}

		public string LoadTextEdit (int _id) {
			File.AppendAllText (GetPath(_id), "");
			//DeleteFile (_id);
			var tt = File.ReadAllText (GetPath(_id));
			Console.WriteLine (tt);
			return tt;
		}

		public void SetCount()
		{
			var lq = DBLocalDataStore.GetInstance ().GetLocalQuestions (DBLocalDataStore.GetInstance ().GetSelectedQuestionPosition());

			if (lq.Count != 0) {
				string[] stra = new string[lq.Count];
				for (int i = 0; i < lq.Count; i++) {
					if (lq [i].maxLength == 0)
						stra [i] = "select a value";
					else {
						stra [i] = "_,___" + i + i;
						//File.AppendAllText (GetPath(i), "          ");
					}
				}
				//if (!File.Exists (GetPath()))
				File.WriteAllLines (GetPath (), stra);
			}
		}

		public void SaveText (string answer, int _position) {
			string[] str = File.ReadAllLines (GetPath());
			str [_position] = answer;
			File.WriteAllLines (GetPath(), str);
		}
		public string LoadText () {
			var tt = File.ReadAllText (GetPath());
			Console.WriteLine (tt);
			return tt;
		}

		public string[] GetLinesText () {
			try
			{
				var tt = File.ReadAllLines (GetPath());
				Console.WriteLine (tt);
				return tt;
			}
			catch
			{
				Console.WriteLine("No to find file with answers");
			}
			return null;
		}

		public string GetTextPosition (int _position) {
			var tt = File.ReadAllLines (GetPath());
			Console.WriteLine (tt[_position]);
			return tt[_position];
		}

		public int GetPositionForValue (string value) {
			var tt = File.ReadAllLines (GetPath());
			for(int t = 0; t < tt.Length; t++)
			{
				if (tt[t] == value) {
					return t;
				}
			}
			return -1;
		}

		public void DeleteFile () {
			var flag = File.Exists (GetPath ());
			if (flag) {
				
				var lt = GetLinesText ();
				for (int i = 0; i < lt.Length; i++) {
					if (lt [i] != "select a value") {
						System.IO.File.Delete (GetPath (i));
					} //else
					//System.IO.File.Delete (GetPath());

				}

			}
			SetCount ();
			//System.IO.File.Delete (GetPath());

		}
		public void DeleteFile (int _id) {
			System.IO.File.Delete (GetPath(_id));
		}

		public List<string> GetAllAnswers()
		{
			List<string> allanswers = new List<string> ();
			var lt = GetLinesText ();
			if (lt != null)
			{
				for (int i = 0; i < lt.Length; i++)
				{
					if (lt[i] == ("_,___" + i + i))
					{
						if (LoadTextEdit(i) == "")
							allanswers.Add("_,___");
						else
							allanswers.Add(LoadTextEdit(i));
					}
					else
						allanswers.Add(lt[i]);

				}
			}

			//LoadTextEdit
			return allanswers;
		}

	}
}

