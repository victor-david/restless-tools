using Restless.Tools.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Tools.App.Sample
{
    public class TabPersonController : ObservableObject
    {
        private Person selectedPerson;

        public PersonCollection Persons
        {
            get;
            private set;
        }

        public Person SelectedPerson
        {
            get => selectedPerson;
            set => SetProperty(ref selectedPerson, value);
        }

        public TabPersonController()
        {
            Persons = new PersonCollection();
        }

        public void AddPerson(Person p)
        {
            Persons.Add(p);
            SelectedPerson = p;
        }

        public void ClosePerson(Person person)
        {
            if (person != null && Persons.Contains(person.Id))
            {
                Persons.Remove(person);
            }
        }
    }
}
