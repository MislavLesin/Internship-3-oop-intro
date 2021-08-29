using System;
using System.Collections.Generic;
using System.Text;

namespace Dump_Assignment_3
{
    class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OIB { get; set; }

        public string PhoneNumber { get; set; }

        public override string ToString()
        {

            return $"{FirstName} - {LastName} - {OIB} - {PhoneNumber}";
        }

    }
    
    
}
