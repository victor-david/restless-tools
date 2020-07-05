using System.Collections.ObjectModel;

namespace Restless.Tools.App.Sample
{
    public class PersonCollection : ObservableCollection<Person>
    {
        public bool Contains(int id)
        {
            foreach (var person in this)
            {
                if (person.Id == id)
                {
                    return true;
                }
            }
            return false;
        }


        public Person GetWithId(int id)
        {
            foreach (var person in this)
            {
                if (person.Id == id) return person;
            }
            return null;
        }
    }
}
