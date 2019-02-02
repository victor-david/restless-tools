using Restless.Tools.Mvvm;
using System;
using System.Windows.Input;

namespace Restless.Tools.App.Sample
{
    /// <summary>
    /// Represents a single person.
    /// </summary>
    public class Person : ObservableObject
    {
        public event EventHandler Closed;

        public int Id
        {
            get;
        }

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

        public ICommand CloseCommand
        {
            get;
        }

        public Person(int id, string firstName, string lastName, string position)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            CloseCommand = RelayCommand.Create((o) =>
            {
                Closed?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
