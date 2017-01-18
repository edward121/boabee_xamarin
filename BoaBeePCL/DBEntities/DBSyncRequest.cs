using System;
using SQLite;

namespace BoaBeePCL
{
    public class DBSyncRequest
    {
        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int Id { get; set; }
        public string serializedSyncContext { get; set; }
        public bool isSent { get; set; }
    }
}

