using System;
using System.Collections.Generic;

namespace BoaBeePCL
{
	public class DBLocalContactsComparer : IEqualityComparer<DBlocalContact>
	{
        public bool Equals(DBlocalContact x, DBlocalContact y)
		{
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            return string.Equals(x.uid, y.uid);
            //return x.uid.Equals(y.uid);
		}

		public int GetHashCode(DBlocalContact x)
		{
            int hash = x.uid == null ? 0 : x.uid.GetHashCode();
            return hash;
		}
	}

    public class CustomerTypeComparer : IEqualityComparer<CustomerType>
    {
        public bool Equals(CustomerType x, CustomerType y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            return string.Equals(x.uid, y.uid);
            //return x.uid.Equals(y.uid);
        }

        public int GetHashCode(CustomerType x)
        {
            int hash = x.uid == null ? 0 : x.uid.GetHashCode();
            return hash;
        }
    }
}

