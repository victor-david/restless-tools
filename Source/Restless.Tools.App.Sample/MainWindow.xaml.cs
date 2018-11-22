using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Restless.Tools.Controls;
using Restless.Tools.Mvvm;

namespace Restless.Tools.App.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        /// Gets the dictionary of commands
        /// </summary>
        public CommandDictionary Commands
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

        public ObservableCollection<Person> Persons
        {
            get;
            private set;
        }

        #endregion

        /************************************************************************/

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeCommands();
            InitializeSections();
            InitializeDates();
            InitializeColumns();
            InitializePersons();
        }
        #endregion

        /************************************************************************/


        #region Private methods
        private void InitializeCommands()
        {
            Commands = new CommandDictionary();
            Commands.Add("S0", (o) => ActivateSection(0));
            Commands.Add("S1", (o) => ActivateSection(1));
            Commands.Add("S2", (o) => ActivateSection(2));
            Commands.Add("Open", (o) => MessageBox.Show("This is the row double click action."));
        }

        private void InitializeSections()
        {
            Sections = new ObservableCollection<Visibility>
            {
                Visibility.Visible,
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
            Persons = new ObservableCollection<Person>
            {
                new Person("Gail", "Ludwig", "VP Of Earth"),
                new Person("Mike", "Popeye", "Developer"),
                new Person("Kate", "Peabody", "Undersecretary of Secrets"),
                new Person("James", "Rascal", "All around helper guy"),
                new Person("Michael", "Post", "VP Of Other Planets"),
                new Person("Nancy", "West", "California Sales Rep"),
                new Person("Pao", "Sween", "Senior Lead"),
            };
        }

        private void ActivateSection(int section)
        {
            for (int k=0; k < Sections.Count; k++)
            {
                Sections[k] = (k == section) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion
    }
}
