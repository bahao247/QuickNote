using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace QuickNote1
{
    public class QuickNote
    {
        [PrimaryKey, AutoIncrement]
        //Info of Note
        public int IDNote { get; set; }
        public string NameNote { get; set; }
        public string TypeNote { get; set; }
        public int PriorityNote { get; set; }
        public string TextNote { get; set; }
        public DateTime DueDateNote { get; set; }
        public bool StatusNote { get; set; }

        public bool DoneNote { get; set; }
        public TimeSpan DueTimeNote { get; internal set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", IDNote, NameNote,
                TextNote, DueDateNote, DoneNote);
        }

    }
}

