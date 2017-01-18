using System;

namespace BoaBeePCL
{
	public class SalesOrderContext
	{
		public RequestData context { get; set; }
		public OrderType[] orders { get; set; }
	}
}

