using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Tools.App.Sample
{
    /// <summary>
    /// Represents a single person.
    /// </summary>
    public class Person
    {
        public string FirstName
        {
            get;
        }

        public string LastName
        {
            get;
        }

        public string Position
        {
            get;
        }

        public Person(string firstName, string lastName, string position)
        {
            FirstName = firstName;
            LastName = lastName;
            Position = position;
        }
    }
}
