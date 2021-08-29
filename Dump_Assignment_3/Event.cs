using System;
using System.Collections.Generic;
using System.Text;
using Dump_Assignment_3.Enums;

namespace Dump_Assignment_3
{
    class Event
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public _EventType EventType { get; set; }

        public Event() { }
        public Event(string name, DateTime startTime, DateTime endTime, _EventType eventType)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            EventType = eventType;
        }
        public override string ToString()
        {
            return $"{Name} - {StartTime} - {EndTime} - {EventType}";
        }
    }
}
