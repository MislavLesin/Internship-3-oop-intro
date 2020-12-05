using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUMP_treci_domaci
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary < Event,List < Osoba >> data = new Dictionary<Event, List<Osoba>>();
            var userInput = -1;

            while(userInput != 0)
            {
                Showmenu();
                while(!int.TryParse(Console.ReadLine(),out userInput))
                    Console.WriteLine("Wrong input! Try again");

                switch (userInput)
                {
                    case 0:
                        Console.WriteLine("Exiting");
                        break;
                    case 1:
                        AddEvent(data);      
                        break;
                    case 2:                   
                        if(data.Count == 0)
                        {
                            Console.WriteLine("\nNo events yet!");
                            PressAnyKeyToContinue();
                        }
                        else
                            RemoveEvent(data);
                        break;
                    case 3:                    
                        if (data.Count == 0)
                        {
                            Console.WriteLine("\nNo events yet!");
                            PressAnyKeyToContinue();
                        }
                        else if(!EditEvent(data))
                            Console.WriteLine("Canceling");
                          
                        PressAnyKeyToContinue();
                        break;
                    case 4:                    
                        AddPersonToEvent(data);
                        break;
                    case 5:
                        if (RemovePersonFromEvent(data))
                        {
                            Console.WriteLine("Person removed !");
                            PressAnyKeyToContinue();
                        }
                        break;
                    case 6:
                        ShowEventDetails(data);
                        break;
                    case 99:
                        Initialize(data);
                        Console.WriteLine("Data inserted");
                        PressAnyKeyToContinue();
                        break;
                         
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input");
                        Console.WriteLine();
                        break;
                }
            }
        }
        static bool AddEvent(IDictionary<Event, List<Osoba>> eventData)
        {
            Console.Clear();
            Console.WriteLine("==== Add event ====\n");
            Console.WriteLine("Type 'exit' to cancel");
            Console.WriteLine("Or press enter to continue");
            var userInput = "";
            try
            {
                userInput = Console.ReadLine().Trim();
            }
            catch
            {
                Console.WriteLine("Try again");
            }
            if(string.Compare(userInput.ToLower(),"exit") == 0)
            {
                Console.WriteLine("\nExiting ");
                PressAnyKeyToContinue();
                return false;
            }
            var _newEvent = new Event();
            List<Osoba> _newList = new List<Osoba>();
            if(!EventDataVerification(_newEvent, eventData))
            {
                Console.WriteLine("Canceled"); 
                PressAnyKeyToContinue();
                return false;
            }
            else
                eventData.Add(_newEvent, _newList);
            Console.WriteLine("Event added");
            PressAnyKeyToContinue();
            return true;
        }
        static bool RemovePersonFromEvent(IDictionary<Event, List<Osoba>> eventData)
        {
            var indexCounter = 0;
            var personCounter = 0;
            Console.Clear();
            if(eventData.Count == 0)
            {
                Console.WriteLine("No events yet, exiting");
                PressAnyKeyToContinue();
                return false;
            }

            Console.WriteLine("Enter event index to edit");
            Console.WriteLine("'-1' to exit");
            var index = SelectEvents(eventData);
            if(index == -1)
            {
                Console.WriteLine("Exiting");
                PressAnyKeyToContinue();
                return false;
            }

            foreach(var kvp in eventData)
            {
                if (indexCounter == index)         
                {
                    foreach(var person in kvp.Value)
                    {
                        Console.WriteLine("\n========================\n");
                        Console.WriteLine("Index - [ "+personCounter+ " ] ");
                        Console.WriteLine("First Name - " +  person.FirstName);
                        Console.WriteLine("Last Name - " + person.LastName);
                        Console.WriteLine("Phone Number - " + person.PhoneNumber);
                        Console.WriteLine();
                        personCounter++;
                    }
                }
                else
                    indexCounter++;
            }
            Console.WriteLine("Enter person index to remove, or '-1' to exit");
            var finalDecision = UserInputVerification(0, personCounter - 1);
            if(finalDecision == -1)
            {
                Console.WriteLine("\nExiting");
                PressAnyKeyToContinue();
                return false;
            }
            else
            {
                indexCounter = 0;
                personCounter = 0;
                foreach (var kvp in eventData)
                {
                    if (indexCounter == index)         
                    {
                        foreach (var person in kvp.Value)
                        {
                            if (personCounter == finalDecision)
                            {
                                kvp.Value.Remove(person);
                                return true;
                            }
                            personCounter++;
                        }
                    }
                    else
                        indexCounter++;
                }
            }
            Console.WriteLine("Something went wrong");
            return false;
        }
        static void PrintEventData(Event _event)
        {
            
                
                Console.WriteLine("Event name - "+ _event.Name);
                Console.WriteLine();
                Console.WriteLine("Event type - "+ _event.EventType);
                Console.WriteLine();
                Console.WriteLine("Start date - "+ _event.StartTime);
                Console.WriteLine();
                Console.WriteLine("End date - "+ _event.EndTime);
                Console.WriteLine();
                var duration = _event.EndTime.Subtract(_event.StartTime);
                Console.WriteLine("Duration -  {0} Days, {1} Hours and {2} Minutes",duration.Days, duration.Hours, duration.Minutes);
                Console.WriteLine();
        }
        static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
        static bool EditEvent(IDictionary<Event, List<Osoba>> eventData)
        {
            var eventBuffer = new Event();
            var eventCounter = 0;
            List<Osoba> _list ;
            List<Osoba> _listbuffer = new List<Osoba>();
            Console.Clear();
            
            Console.WriteLine("Enter event index to edit");
            Console.WriteLine("'-1' to exit");
            var index = SelectEvents(eventData);

            if (index == -1)
                PressAnyKeyToContinue();
            else
            {
                foreach (var eData in eventData)
                {
                 
                    if (eventCounter == index)      
                    {
                        var duration = eData.Key.EndTime - eData.Key.StartTime;
                        _listbuffer = eData.Value;
                        Console.WriteLine();
                        Console.WriteLine("Name - " + eData.Key.Name);
                        Console.WriteLine("Type of event - " + eData.Key.EventType);
                        Console.WriteLine("Event start date and time - " + eData.Key.StartTime);
                        Console.WriteLine("Event end date and time - " + eData.Key.EndTime);
                        Console.WriteLine("Event duration - " + duration);
                        Console.WriteLine("Number of participants - " + eData.Value.Count);
                        Console.WriteLine();
                        Console.WriteLine("Do you want to keep participant data?");
                        Console.WriteLine("Yes to confirm, exit to exit");
                        var potvrda = Console.ReadLine();
                        if (string.Compare(potvrda.ToLower().Trim(), "yes") == 0)
                            _list = eData.Value;
                        else if (string.Compare(potvrda.ToLower().Trim(), "exit") == 0)
                        {
                            return false;
                        }
                        else
                            _list = new List<Osoba>();

                        var _newEvent = new Event();
                        eventBuffer = eData.Key;
                        eventData.Remove(eData.Key);

                        if (!EventDataVerification(_newEvent, eventData)) 
                        {
                            eventData.Add(eventBuffer, _listbuffer);
                            return false; ;
                        }
                        eventData.Add(_newEvent, _list);
                        Console.WriteLine("Executed successfully");
                        PressAnyKeyToContinue();
                        return true;
                    }
                    eventCounter++;
                }
            }
            Console.WriteLine("Something went wrong");
            return false;
        }
        static void RemoveEvent(IDictionary<Event, List<Osoba>> eventData)
        {
            var index = 0;
            Console.Clear();
            Console.WriteLine("Events: \n");
            foreach(var podatak in eventData)
            {
                Console.WriteLine("[ "+index+" ] - "+   podatak.Key.Name);
                index++;
            }
            Console.WriteLine();
            Console.WriteLine("Enter event index to delete");
            Console.WriteLine("Enter '-1' to exit");
            var userInput = UserInputVerification(0,eventData.Count -1);

            if (userInput == -1)
            {
                PressAnyKeyToContinue();
            }
            else
            {
                index = 0;
                foreach(var podatak in eventData)
                {
                    if(index == userInput )      
                    {
                        eventData.Remove(podatak.Key);
                        break;
                    }
                    index++;
                }
                Console.WriteLine("\nEvent removed!");
                PressAnyKeyToContinue();
            }
        }
        static void Initialize(IDictionary<Event, List<Osoba>> eventData)
        {
            //Adding 2 events and 2 attendants
            var startDate = new DateTime(2020, 01, 24, 12, 30, 0);
            var endDate = new DateTime(2020, 01, 24, 14, 0, 0);
            var _event1 = new Event();
            _event1.Name = "Party na fesbu";
            _event1.EventType = (Event._EventType) 2;
            _event1.StartTime = startDate;
            _event1.EndTime = endDate;

            var _event2 = new Event();
            _event2.Name = "Kava sa Antom";
            _event2.EventType = (Event._EventType)0;
            startDate.AddDays(1);
            endDate.AddDays(1);
            _event2.StartTime = startDate;
            _event2.EndTime = endDate;

            var osoba1 = new Osoba();
            osoba1.FirstName = "Mislav";
            osoba1.LastName = "Lesin";
            osoba1.OIB = 123;
            osoba1.PhoneNumber = "091xxxxxxx";

            var osoba2 = new Osoba();
            osoba2.FirstName = "Ante";
            osoba2.LastName = "Jerkov";
            osoba2.OIB = 124;
            osoba2.PhoneNumber = "091xxxxxxx";

            List<Osoba> posjetitelji = new List<Osoba>();
            posjetitelji.Add(osoba1);
            posjetitelji.Add(osoba2);

            eventData.Add(_event1, posjetitelji);

            posjetitelji.Clear();
            posjetitelji.Add(osoba2);
            posjetitelji.Add(osoba1);

            eventData.Add(_event2, posjetitelji);

        }
        static int SelectEvents(IDictionary<Event, List<Osoba>> eventData)
        {
            var index = 0;
            Console.Clear();
            Console.WriteLine("==== Select Event ====\n");
            Console.WriteLine("Enter event index or '-1' to exit");
            foreach (var podatak in eventData)
            {
                Console.WriteLine("[ "+index+" ] - " + podatak.Key.Name);
                index++;
            }
            
            Console.WriteLine();
            return (UserInputVerification(0, index - 1));
        }
        static void PrintPersonInfo(Osoba Person)
        {
            Console.WriteLine();
            Console.WriteLine("First Name - " + Person.FirstName + "\n");
            Console.WriteLine("Last Name - " + Person.LastName + " \n");
            Console.WriteLine("Phone Number - " + Person.PhoneNumber);
        }
        static bool ShowEventDetails(IDictionary<Event, List<Osoba>> eventData)
        {
            Console.Clear();
            Console.WriteLine("Select print format: ");
            Console.Write("\n1 - Event Details \n");
            Console.WriteLine("\n2 - Person details per event \n");
            Console.WriteLine("\n3 - Print all details (1 i 2) \n");
            Console.WriteLine("\n '-1' to exit");
            var unos = UserInputVerification(0, 3);
            if (unos == -1)
                return false;
            else if (unos == 1)
            {
                Console.Clear();
                Console.WriteLine("\n++++++++++++++++++\n");
                foreach (var kvp in eventData)
                {
                    PrintEventData(kvp.Key);
                    Console.WriteLine("Attendants - " + kvp.Value.Count);
                    Console.WriteLine("\n==========================\n");
                }
            }
            else if (unos == 2)
            {
                Console.Clear();
                var eventIndex = SelectEvents(eventData);
                if (eventIndex == -1)
                    return false;
                var eventCounter = 0;
                foreach (var kvp in eventData)
                {
                    if (eventCounter == eventIndex)
                    {
                        if(kvp.Value.Count == 0)
                        {
                            Console.WriteLine("There are no attendatns to this event\n");
                        }
                        else
                        {
                            foreach (var Person in kvp.Value)
                            {
                                PrintPersonInfo(Person);
                                Console.WriteLine("==========================================");
                            }
                        }
                    }
                    eventCounter++;
                }
            }
            else if (unos == 3)
            {
                Console.Clear();
                foreach (var kvp in eventData)
                {
                    PrintEventData(kvp.Key);
                    Console.WriteLine("Attendants - " + kvp.Value.Count);
                    Console.WriteLine("\n==========================\n");
                    Console.WriteLine("Persons attending : \n");
                    foreach (var person in kvp.Value)
                    {
                        Console.WriteLine();
                        PrintPersonInfo(person);
                        Console.WriteLine("====================================================");

                    }
                    Console.WriteLine("\n++++++++++++++++++++++++++++++++++++\n");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Press any key to ocntinue");
            Console.ReadKey();
            Console.Clear();
            return true;
        }
        static void AddPersonToEvent(IDictionary<Event, List<Osoba>> eventData)
        {
            if (eventData.Count == 0)
            {
                Console.WriteLine("No events yet!");
                PressAnyKeyToContinue();
            }
            else
            {
                Console.Clear();
                var counter = 0;
                var index = SelectEvents(eventData);
                if (index == -1)
                {
                    Console.WriteLine("\nExiting");
                    PressAnyKeyToContinue();
                }
                else
                {
                    foreach (var kvp in eventData)
                    {
                        if (counter == index)
                        {
                            PersonDataInput(kvp.Value);
                        }
                        counter++;
                    }
                }
            }
        }
        static string StringInputVerification()
        {
            var returnValue = ""; 
            try
            {
                returnValue = Console.ReadLine().Trim();
            }
            catch
            {
                Console.WriteLine("Wrong input, try again");
            }

            if(returnValue.Length == 0)
            {
                Console.WriteLine("The field must have a value! \n");
                returnValue = StringInputVerification();
            }
            return returnValue;
        }
        static void PersonDataInput(List<Osoba> PersonList)
        {
            var personAlreadyAdded = false;
            var personData = new Osoba();
            Console.Clear();
            Console.WriteLine("==== Person Data Input ====\n");

            Console.Write("Fist Name - ");
            personData.FirstName = StringInputVerification();
            Console.WriteLine();

            Console.Write("Last Name - ");
            personData.LastName = StringInputVerification();
            Console.WriteLine();

            Console.Write("Pbone Number - ");
            personData.PhoneNumber = StringInputVerification();
            Console.WriteLine();

            Console.Write("OIB - ");
            int OIB;
            while (!int.TryParse(Console.ReadLine(), out OIB) || OIB < 1)
                Console.WriteLine("Wrong input! Try again");
            personData.OIB = OIB;

            foreach(var person in PersonList)
            {
                if (person.OIB == personData.OIB)
                {
                    Console.WriteLine("\n" + person.FirstName + " is allready on event list!\n");
                    personAlreadyAdded = true;
                }    
            }
            if (personAlreadyAdded == false)
            {
                PersonList.Add(personData);
                Console.WriteLine("\nPerson Added\n");
                PressAnyKeyToContinue();
            } 
        }
        static bool EventDataVerification(Event InputEvent, IDictionary<Event, List<Osoba>> Podaci)
        {
            var bufferEvent = InputEvent;
            Console.Clear();
            if (Podaci.ContainsKey(InputEvent))
                Podaci.Remove(InputEvent);

            var dateValidFlag = true;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            int userInput;
            Console.WriteLine("\n==== Event Data Input ====\n");
            Console.WriteLine("Type 'exit' to exit\n");
            Console.Write("Enter event name: ");
            var name = StringInputVerification();
            
            if (string.Compare(name.ToLower().Trim(), "exit") == 0)
            {
                return false;     
            }
            foreach(var _Event in Podaci)
            {
                if(string.Compare(name.ToLower(),_Event.Key.Name.ToLower()) == 0)
                {
                    Console.WriteLine("There is already an event with that name!");
                    return false;
                }
            }

            InputEvent.Name = name;
            Console.WriteLine("");
            Console.WriteLine("Enter event type: ");
            Console.WriteLine("1 - Coffe");
            Console.WriteLine("2 - Lecture");
            Console.WriteLine("3 - Concert");
            Console.WriteLine("4 - Study session");

            userInput = UserInputVerification(1,4);

            InputEvent.EventType = (Event._EventType)userInput - 1;
            try
            {
                Console.WriteLine("\nStart date: \n");
                Console.WriteLine("Enter year");
                var godina = UserInputVerification(1999, 2100);

                Console.WriteLine("Enter month");
                var mjesec = UserInputVerification(1, 12);

                Console.WriteLine("Enter day");
                var dan = UserInputVerification(1, 31);

                Console.WriteLine("Enter start hour");
                var sat = UserInputVerification(0, 23);

                Console.WriteLine("Enter start minute");
                var minute = UserInputVerification(0, 60);

                startDate = new DateTime(godina, mjesec, dan, sat, minute, 0);
            }
            catch
            {
                Console.WriteLine("This date does not exist, try again");
            }

            try
            {
                do
                {
                    Console.Write("\nEnd date: \n");
                    Console.WriteLine("Enter year");
                    var godina = UserInputVerification(1999, 2100);

                    Console.WriteLine("Emter month");
                    var mjesec = UserInputVerification(1, 12);

                    Console.WriteLine("Enter day");
                    var dan = UserInputVerification(1, 31);

                    Console.WriteLine("Enter hour");
                    var sat = UserInputVerification(0, 23);

                    Console.WriteLine("Enter minute");
                    var minute = UserInputVerification(0, 60);

                    endDate = new DateTime(godina, mjesec, dan, sat, minute, 0);

                    foreach(var _event in Podaci)      
                    {
                        if(((_event.Key.StartTime <= startDate) && (_event.Key.EndTime >= startDate)) || 
                            ((_event.Key.StartTime <= endDate) && (_event.Key.EndTime >= endDate)) || 
                            ((_event.Key.StartTime >= startDate) && (_event.Key.EndTime <= endDate)))
                        
                        {
                            Console.WriteLine("\nThere is an event allready on this date!");
                            Console.WriteLine("Event name - "+ _event.Key.Name);
                            Console.WriteLine("Start Date - "+_event.Key.StartTime);
                            Console.WriteLine("End Date - "+ _event.Key.EndTime);
                            PressAnyKeyToContinue();
                            Console.WriteLine("Press any key to enter End Date, or type 'exit'");
                            var exitQuerry = "";
                            try
                            {
                                exitQuerry = Console.ReadLine().Trim();
                            }
                            catch
                            {
                                Console.WriteLine("Try again");
                            }
                            if(string.Compare(exitQuerry.ToLower().Trim(),"exit") == 0 )
                                return false;
                            dateValidFlag = false;
                            break;
                        }
                        else if (DateTime.Compare(startDate, endDate) >= 0)
                        {
                            Console.WriteLine("\nEnd date has to be after start date!!");
                            Console.WriteLine("Try again");
                            dateValidFlag = false;
                            PressAnyKeyToContinue();
                            Console.WriteLine("Start date is : " + startDate);
                        }
                        else
                        {
                            dateValidFlag = true;
                            Console.WriteLine();
                            Console.WriteLine("Date valid ");
                            break;
                        }
                    }

                    

                } while (dateValidFlag == false);
            }
            catch
            {
                Console.WriteLine("This date does not exist, try again");
            }

            InputEvent.StartTime = startDate;
            InputEvent.EndTime = endDate;
            return true;
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
        static int UserInputVerification(int min, int max)      //returns -1 for exit
        {
            var userInput = 0;
            while (!int.TryParse(Console.ReadLine(), out userInput) || userInput > max || userInput < min || userInput == -1)
            {
                if (userInput == -1)
                    return userInput;

                Console.WriteLine("Wrong input! Try again");
            }
            return userInput;
        }
    }
}
