using Dump_Assignment_3.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;

namespace Dump_Assignment_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<Event, List<Person>> data = new Dictionary<Event, List<Person>>();
            var userInput = -1;
            while(userInput != 0)
            {
                Console.Clear();
                Showmenu();
                while (!int.TryParse(Console.ReadLine(), out userInput))
                    Console.WriteLine("Wrong input, Try again");
                switch (userInput)
                {
                    case 0:
                        Console.WriteLine("Exitting");
                        break;
                    case 1:
                        AddNewEvent(data);
                        break;
                    case 2:
                        DeleteEvent(data);
                        break;
                    case 3:
                        EditEvent(data);
                        break;
                    case 4:
                        if(!AddPersonsToEvent(data))
                            Console.WriteLine("Action Canceled");
                        break;
                    case 5:
                        RemovePersonsFromEvent(data);
                        break;
                    case 6:
                        if(data.Count == 0)
                            Console.WriteLine("Database is empty");
                        else
                            ShowEventDetails(data);
                        break;
                    case 99:
                        Initialize(data);
                        continue;
                }
                Console.ReadKey();
            }
           
        }
        public static bool CheckIfEmpty(Dictionary<Event, List<Person>> data)
        {
            if (data.Keys.Count > 0)
                return false;
            Console.WriteLine("There are no saved events");
            return true;
        }
        public static bool RemovePersonsFromEvent(Dictionary<Event, List<Person>> data)
        {
            if (CheckIfEmpty(data))
                return false;

            Console.Clear();
            Console.WriteLine("Select event to remove person from:");
            if(!SelectEventKVPOrExit(out var eventKVP, data))
            {
                Console.WriteLine("Exitting");
                return false;
            }
            if(eventKVP.Value.Count < 1)
            {
                Console.WriteLine("This event has no persons to remove, exitting");
                return false;
            }
            var personIndex = 1;
            foreach(var person in eventKVP.Value)
            {
                Console.WriteLine($"[{personIndex}] - {person}");
                personIndex++;
            }
            Console.WriteLine("\nEnter index of person to remove:");
            Console.WriteLine("Enter 0 to cancel");
            var selectedIndex = GetIntFromRange(0, personIndex - 1) - 1;
            if(selectedIndex < 0)
            {
                Console.WriteLine("Aciton canceled");
                return false;
            }
            Console.WriteLine($"Are you sure you want to remove person {eventKVP.Value[selectedIndex].FirstName} {eventKVP.Value[selectedIndex].LastName} " +
                $"from event {eventKVP.Key.Name} ?");
            if(StringDecisonYes())
            {
                eventKVP.Value.RemoveAt(selectedIndex);
                Console.WriteLine("Person removed!");
            }

            return true;
        }
        public static bool AddPersonsToEvent(Dictionary<Event, List<Person>> data)
        {
            if (CheckIfEmpty(data))
                return false;

            Console.Clear();
            Console.WriteLine("\nSelect Event to add persons to: \n");
            if(!SelectEventKVPOrExit(out var eventKVP,data))
            {
                Console.WriteLine("Exitting");
                return false;
            }
            do
            {
                Console.Clear();
                Console.WriteLine($"Event {eventKVP.Key.Name} has {eventKVP.Value.Count} participants:\n");
                if (eventKVP.Value.Count > 0)
                {
                    Console.WriteLine("--- Participants ---\n");
                    foreach (var person in eventKVP.Value)
                    {
                        Console.WriteLine(person + "\n");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Do you want to add new person to this event?");
                if (StringDecisonYes())
                {
                    var newPerson = new Person();
                    Console.WriteLine("\nEnter requested information, or leave empty to exit\n");
                    Console.WriteLine("Enter person's First Name: ");
                    if(!StringInputValidation(out var userInput))
                        {
                            return false;
                        }
                    newPerson.FirstName = userInput;

                    Console.WriteLine("Enter person's Last Name: ");
                    if (!StringInputValidation(out userInput))
                        return false;
                    newPerson.LastName = userInput;

                    Console.WriteLine("Enter person's Phone Number: ");
                    if (!StringInputValidation(out userInput))
                        return false;
                    newPerson.PhoneNumber = userInput;

                    Console.WriteLine("Enter person's OIB: ");
                    if (!StringInputValidation(out userInput))
                        return false;
                    newPerson.OIB = userInput;

                    Console.WriteLine($"Do you want to add person: \n {newPerson} to event {eventKVP.Key} ?\n");
                    if (StringDecisonYes())
                    {
                        eventKVP.Value.Add(newPerson);
                        Console.WriteLine($"{newPerson.FirstName} {newPerson.LastName} successfully added to event {eventKVP.Key.Name}");
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            } while (true);
        }
        public static bool EditEvent(Dictionary<Event, List<Person>>data)
        {
            if(CheckIfEmpty(data))
                return false;
            
            if(!SelectEventKVPOrExit(out var eventKVP, data))
            {
                Console.WriteLine("Exitting");
                return false;
            }
            Event editedEvent = new Event();
            Console.WriteLine("Enter new value, or press enter to skip:\n");

            Console.WriteLine($"Edit events name: [{eventKVP.Key.Name}]");
            editedEvent.Name = StringInputValidation(out var newName)
                ? newName
                : eventKVP.Key.Name;

            Console.WriteLine($"\nEdit event's start date: {eventKVP.Key.StartTime}");
            editedEvent.StartTime = DateInputVerification(out var newDate)
                ? newDate
                : eventKVP.Key.StartTime;

            Console.WriteLine($"\nEdit event's end date: {eventKVP.Key.EndTime}");
            editedEvent.EndTime = DateInputVerification(out newDate)
                ? newDate
                : eventKVP.Key.EndTime;

            Console.WriteLine($"Edit event's type: {eventKVP.Key.EventType}");
            editedEvent.EventType = SelectEventType();
            Console.WriteLine("Do you want to save chanegs to this event?");
            if(StringDecisonYes())
            {
                var persons = eventKVP.Value;
                data.Remove(eventKVP.Key);
                data.Add(editedEvent, persons);
                Console.WriteLine("Changes have been saved!");
                return true;
            }
            Console.WriteLine("Editting canceled");
            return false;
        }
        public static bool StringInputValidation(out string input)
        {
            var userInput = Console.ReadLine();
            if (string.IsNullOrEmpty(userInput))
            {
                input = null;
                return false;
            }
            input = userInput;
            return true;
        }
        public static bool DeleteEvent(Dictionary<Event, List<Person>> data)
        {
            if (CheckIfEmpty(data))
                return false;

            if(!SelectEventKVPOrExit(out var kvp, data))
            {
                Console.WriteLine("Exitting");
                return false;
            }
                if (kvp.Value.Count > 0)
                {
                    var personCount = 0;
                    foreach (var person in kvp.Value)
                    {
                        Console.WriteLine(person);
                        personCount++;
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine($"Are you sure you want to delete event {kvp.Key.Name} with {personCount} attendants?");
                    if (StringDecisonYes())
                    {
                        data.Remove(kvp.Key);
                    }
                }
                else
                {
                    Console.WriteLine($"Are you sure you want to delete event {kvp.Key.Name} with no attendants?");
                    if (StringDecisonYes())
                    {
                        data.Remove(kvp.Key);
                    }
                }
                return true;
        }
        public static bool StringDecisonYes()
        {
            Console.Write("Enter 'yes', or leave empty to cancel: ");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return false;
            else if (string.Compare(input.ToLower(), "yes") == 0 || string.Compare(input.ToLower(), "y") == 0)
                return true;
            else
                Console.Write("Enter yes for confirmation: ");
            return StringDecisonYes();
        }
        public static bool AddNewEvent(Dictionary<Event, List<Person>> data)
        {
            Console.Clear();
            Console.WriteLine("** Add New Event ** \n");
            Console.WriteLine("Enter new event's name: ");
            string eventName;
            var valid = StringInputVerificationBoolean(out eventName);
            if(!valid)
            {
                Console.WriteLine("Exitting");
                return false;
            }
            Console.WriteLine("\nStart Date:");
            valid = DateInputVerification(out var startDateTime);
            if(!valid)
            {
                return false;
            }
            DateTime endDateTime = default;
            while(true)
            {
                Console.WriteLine("\nEnd Date:");
                valid = DateInputVerification(out endDateTime);
                if (!valid)
                {
                    return false;
                }
                if (endDateTime < startDateTime)
                {
                    Console.WriteLine($"End Date must be after {startDateTime} !");
                }
                else
                    break;
            }
            _EventType eventType = SelectEventType();
            var newEvent = new Event(eventName, startDateTime, endDateTime, eventType);
            data.Add(newEvent, new List<Person>());
            Console.WriteLine($"Event {eventName} saved!");
            return true;
        }
        public static _EventType SelectEventType()
        {
            Console.WriteLine("Enter index of event type: \n");
            Console.WriteLine("0 - Coffe");
            Console.WriteLine("1 - Lecture");
            Console.WriteLine("2 - Concert");
            Console.WriteLine("3 - Study Session");
            int userInput;
            while (int.TryParse(Console.ReadLine(),out userInput))
            {
                Console.WriteLine("Enter number!");
            }
            if(userInput == (int)_EventType.Coffe)
            {
                return _EventType.Coffe;
            }
            if (userInput == (int)_EventType.Concert)
            {
                return _EventType.Concert;
            }
            if (userInput == (int)_EventType.Lecture)
            {
                return _EventType.Lecture;
            }
            if (userInput == (int)_EventType.StudySession)
            {
                return _EventType.StudySession;
            }
            else
                return _EventType.Other;
        }
        public static bool DateInputVerification(out DateTime dateTime)
        {
            dateTime = default;
            while(true)
            {
                try
                {
                    Console.WriteLine("\nLeave empty to exit");
                    Console.WriteLine("Enter date in format [yyyy mm dd hh min]");
                    var input = Console.ReadLine();
                    if(string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Exitting");
                        return false;
                    }
                    string[] separated = input.Split(' ');
                    dateTime = new DateTime(int.Parse(separated[0]), int.Parse(separated[1]),
                        int.Parse(separated[2]), int.Parse(separated[3]), int.Parse(separated[4]), 0);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wrong format! \n");
                    continue;
                }
            }
            return true;
     
        }
            
        public static int IntVerification()
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Input must be a number!!, Try again");
            }
            return value;
        }
        static void Showmenu()
        {
            Console.WriteLine("1 - Add a new event");
            Console.WriteLine("2 - Delete event");
            Console.WriteLine("3 - Edit event");
            Console.WriteLine("4 - Add person to event");
            Console.WriteLine("5 - Remove preson from event");
            Console.WriteLine("6 - Show event details");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("99 - Initialize 2 events");
            Console.WriteLine();
        }
        public static void ShowEventDetails(Dictionary<Event, List<Person>> data)
        {
            Console.Clear();
            var decision = -1;
            Console.WriteLine("Select details: \n");
            Console.WriteLine("1 - Events");
            Console.WriteLine("2 - Event participants");
            Console.WriteLine("3 - Events and participants");
            Console.WriteLine("0 - Exit");
            while (!int.TryParse(Console.ReadLine(), out decision))
            {
                Console.WriteLine("Enter number!!");
            }
            Console.Clear();
            switch(decision)
            {
                case 1:
                    Console.WriteLine("*** 1 - Events ***\n");
                    foreach (var kvp in data)
                    {
                        Console.WriteLine(kvp.Key);
                        Console.WriteLine("\n");
                    }
                    break;
                case 2:
                    Console.WriteLine("*** 2 - Event participants ***");
                    var selectedKVP = SelectEventKVP(data);
                    if(selectedKVP.Value.Count == 0)
                        Console.WriteLine("No particiants!");
                    else
                    {
                        foreach(var person in selectedKVP.Value)
                        {
                            Console.WriteLine(person);
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine("*** 3 - Events and participants ***");
                    foreach (var kvp in data)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine(kvp.Key);
                        if(kvp.Value.Count > 0)
                        {
                            Console.WriteLine("\n------PARTICIPANTS-----\n");
                            foreach(var person in kvp.Value)
                                Console.WriteLine(person);
                        }
                        else
                            Console.WriteLine($"\nNo participants for event {kvp.Key.Name}\n");
                        Console.WriteLine("\n***************************\n");
                    }
                    break;
                case 0:
                    Console.WriteLine("Exitting");
                    break;
            }
        }
        static void Initialize(IDictionary<Event, List<Person>> eventData)
        {
            //Adding 2 events and 2 attendants
            var startDate = new DateTime(2020, 01, 24, 12, 30, 0);
            var endDate = new DateTime(2020, 01, 24, 14, 0, 0);
            var _event1 = new Event("Party na fesbu",startDate,endDate,(_EventType)2);

            startDate.AddDays(1);
            endDate.AddDays(1);
            var _event2 = new Event("Kava sa Antom",startDate,endDate,(_EventType)0);

            var osoba1 = new Person();
            osoba1.FirstName = "Mislav";
            osoba1.LastName = "Lesin";
            osoba1.OIB = "123897";
            osoba1.PhoneNumber = "091xxxxxxx";

            var osoba2 = new Person();
            osoba2.FirstName = "Ante";
            osoba2.LastName = "Jerkov";
            osoba2.OIB = "124652";
            osoba2.PhoneNumber = "091xxxxxxx";

            List<Person> posjetitelji = new List<Person>();
            posjetitelji.Add(osoba1);
            posjetitelji.Add(osoba2);

            eventData.Add(_event1, posjetitelji);

            List<Person> posjetitelji2 = new List<Person>();
            posjetitelji2.Add(osoba2);
            posjetitelji2.Add(osoba1);
            eventData.Add(_event2, posjetitelji2);
        }
        public static KeyValuePair<Event, List<Person>> SelectEventKVP(Dictionary<Event, List<Person>> data)
        {
            Console.WriteLine("\nEnter event index: \n");
            var index = 1;
            foreach(var kvp in data)
            {
                Console.WriteLine($"[{index}] - {kvp.Key.ToString()}");
                index++;
            }
            var selectedIndex = GetIntFromRange(1, index - 1) - 1;
            return data.ElementAt(selectedIndex);
            
        }
        public static bool SelectEventKVPOrExit(out KeyValuePair<Event, List<Person>> keyValuePair, Dictionary<Event, List<Person>> data)
        {
            Console.WriteLine("\nEnter event index or 0 to cancel \n");
            var index = 1;
            foreach (var kvp in data)
            {
                Console.WriteLine($"[{index}] - {kvp.Key.ToString()}");
                index++;
            }
            var selectedIndex = GetIntFromRange(0, index - 1) - 1;
            if(selectedIndex < 0)
            {
                keyValuePair = default;
                return false;
            }
            keyValuePair = data.ElementAt(selectedIndex);
            return true;

        }
        public static int GetIntFromRange(int min, int max)
        {
            var index = 0;
            while(!int.TryParse(Console.ReadLine(),out index) || index > max || index < min)
            {
                Console.WriteLine($"Enter number between {min} and {max}!");
            }
            return index;
        }
        public static bool StringInputVerificationBoolean(out string returnString)
        {
            returnString = Console.ReadLine().Trim();
            if(string.IsNullOrEmpty(returnString))
            {
                return false;
            }
            return true;
        }
    }
}
