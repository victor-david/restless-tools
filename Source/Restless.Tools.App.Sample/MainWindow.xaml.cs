using System;
using System.Collections.Generic;
using System.Windows;

namespace Restless.Tools.App.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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

        public List<DateTime?> Dates
        {
            get;
        }
    }
}
