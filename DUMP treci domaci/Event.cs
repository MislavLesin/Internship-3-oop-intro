using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DUMP_treci_domaci
{
    class Event
    {

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public _EventType EventType { get; set; }
        public enum _EventType
        { 
            Coffe,          //0
            Lecture,        //1
            Concert,        //2
            StudySession    //3
        }
    }
}
