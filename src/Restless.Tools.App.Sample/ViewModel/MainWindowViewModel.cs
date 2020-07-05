using Restless.Tools.Controls;
using Restless.Tools.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Restless.Tools.App.Sample
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties
        /// <summary>
        /// Gets the sections
        /// </summary>
        public ObservableCollection<Visibility> Sections
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of sample dates for the calendar
        /// </summary>
        public List<DateTime?> Dates
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the columns for the data grid
        /// </summary>
        public DataGridColumnCollection Columns
        {
            get;
            private set;
        }

        public PersonCollection Persons
        {
            get;
            private set;
        }

        public TabPersonController TabPerson
        {
            get;
        }
        #endregion

        /************************************************************************/

        public MainWindowViewModel()
        {
            TabPerson = new TabPersonController();
            InitializeCommands();
            InitializeSections();
            InitializeDates();
            InitializeColumns();
            InitializePersons();
            Default.Format.PopupCalendarDate = "g";
        }

        private void InitializeCommands()
        {
            Commands.Add("S0", (o) => ActivateSection(0));
            Commands.Add("S1", (o) => ActivateSection(1));
            Commands.Add("S2", (o) => ActivateSection(2));
            Commands.Add("S3", (o) => ActivateSection(3));
            Commands.Add("S4", (o) => ActivateSection(4));
            Commands.Add("S5", (o) => ActivateSection(5));
            Commands.Add("S6", (o) => ActivateSection(6));
            Commands.Add("S7", (o) => ActivateSection(7));
            Commands.Add("S8", (o) => ActivateSection(8));
            Commands.Add("S9", (o) => ActivateSection(9));
            Commands.Add("Open", (o) => MessageBox.Show("This is the row double click action."));
            Commands.Add("AddTabPerson", RunAddTabPerson, CanRunAddTabPerson);
        }

        private void InitializeSections()
        {
            Sections = new ObservableCollection<Visibility>
            {
                Visibility.Visible,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
            };
        }

        private void InitializeDates()
        {
            Dates = new List<DateTime?>()
            {
                new DateTime(2018,7,5,4,3,2),
                new DateTime(2018,6,7,14,4,55),
                new DateTime(2018,5,21,0,53,21),
                new DateTime(2018,4,29,21,13,43),
                new DateTime(2018,4,12,4,53,53),
                new DateTime(1952,7,7,2,20,33),
            };
        }

        private void InitializeColumns()
        {
            Columns = new DataGridColumnCollection();
            Columns.Create("First Name (Fixed 200)", nameof(Person.FirstName)).MakeFixedWidth(200);
            Columns.Create("Last Name (Fixed 150)", nameof(Person.LastName)).MakeFixedWidth(150);
            Columns.Create("Position", nameof(Person.Position)).MakeFlexWidth(1);
        }

        private void InitializePersons()
        {
            Persons = new PersonCollection()
            {
                new Person(1, "Gail", "Ludwig", "VP Of Earth"),
                new Person(2, "Mike", "Popeye", "Developer"),
                new Person(3, "Kate", "Peabody", "Undersecretary of Secrets"),
                new Person(4, "James", "Rascal", "All around helper guy"),
                new Person(5, "Michael", "Post", "VP Of Other Planets"),
                new Person(6, "Nancy", "West", "California Sales Rep"),
                new Person(7, "Pao", "Sween", "Senior Lead"),
                new Person(8, "Jack", "Bailey", "VP of Doughnuts"),
                new Person(9, "Sally", "Frost", "Rally Lead"),
                new Person(10, "Betty", "Smitherson", "Senior Lead"),
                new Person(11, "George", "Pal", "All Around"),
                new Person(12, "Freedy", "Haliburton", "Secratary of Interior"),
                new Person(13, "Nick", "Blast", "Head Tester"),
                new Person(14, "Velma", "Jackson", "Another VP"),
                new Person(15, "Hellen", "Wait", "Account Exec"),
                new Person(16, "Mikey", "York", "Friend of the Family"),
                new Person(17, "Boss", "Chuck", "Senior Lead"),
                new Person(18, "Charles", "Benson", "Poet"),
                new Person(19, "Wally", "Kruger", "Lead Dev"),
                new Person(20, "Max", "Boat", "Fellowship Cooridinator"),
                new Person(21, "Mary", "Lemberstein", "Office Boss"),

            };

            foreach (var person in Persons)
            {
                person.Closed += PersonClosed;
            }
        }

        private void ActivateSection(int section)
        {
            for (int k = 0; k < Sections.Count; k++)
            {
                Sections[k] = (k == section) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void RunAddTabPerson(object parm)
        {
            if (CanRunAddTabPerson(null))
            {
                for (int id = 1; id <= 7; id++)
                {
                    if (!TabPerson.Persons.Contains(id))
                    {
                        TabPerson.AddPerson(Persons.GetWithId(id));
                        return;
                    }
                }
            }
        }

        private bool CanRunAddTabPerson(object parm)
        {
            return TabPerson.Persons.Count < 7;
        }

        private void PersonClosed(object sender, EventArgs e)
        {
            if (sender is Person person)
            {
                TabPerson.ClosePerson(person);
            }
        }
    }
}