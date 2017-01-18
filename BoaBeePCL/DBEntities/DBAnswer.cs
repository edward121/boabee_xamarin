using System;
using SQLite;

namespace BoaBeePCL
{
    public class DBAnswer
    {
        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int Id { get; set; }

        public string question_name { get; set;}
        public string question { get; set;}
        public string answer { get; set;}

        public DBAnswer()
        {
        }
    }
}

