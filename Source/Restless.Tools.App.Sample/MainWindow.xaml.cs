using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
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

        public ObservableCollection<Visibility> Sections
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
        }

        private void InitializeSections()
        {
            Sections = new ObservableCollection<Visibility>
            {
                Visibility.Visible,
                Visibility.Collapsed
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
