using System;
using System.Collections.Generic;
using SQLite;

namespace BoaBeePCL
{
    public class DBOrderLine
    {
        public string item { get; set; }
        public string itemDescription { get; set; }
    }

    public class DBOrder
    {
        public string contactUid { get; set; }
        public string created { get; set; }
        public string creator { get; set; }
        public List<DBOrderLine> orderLine { get; set; }
    }
}

