using System.Windows.Controls;

namespace Restless.Tools.App.Sample
{
    /// <summary>
    /// Interaction logic for ListBoxDisplay.xaml
    /// </summary>
    public partial class ListBoxDisplay : UserControl
    {
        public ListBoxDisplay()
        {
            InitializeComponent();
            MyListBox.UseOuterScrollViewer = true;
        }
    }
}
