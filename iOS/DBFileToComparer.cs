using System;
using BoaBeePCL;
using System.Collections.Generic;
using System.Collections;

namespace BoaBee.iOS
{
	public class DBFileToComparer:IComparer<DBfileTO>
	{
		public DBFileToComparer()
		{
		}

		public int Compare(DBfileTO x, DBfileTO y)
		{
			return new CaseInsensitiveComparer().Compare(x.name, y.name);
		}
	}

    public class DBFileToEqualityComparer : IEqualityComparer<DBfileTO>
    {
        public bool Equals(DBfileTO x, DBfileTO y)
        {
            if (x.name.Equals("IMG_1610") && y.name.Equals("IMG_1610"))
            {
                Console.Error.WriteLine();
            }
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            return x.md5.ToLower().Equals(y.md5.ToLower());
        }

        public int GetHashCode(DBfileTO x)
        {
            //int hash = x == null ? 0 : x.GetHashCode();
            int hash = x.md5 == null ? 0 : x.md5.GetHashCode();
            return hash;
        }
    }
}

