using System.Windows.Input;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Represents a list box item that ignores the right mouse button.
    /// </summary>
    public class ListBoxItemOverride : System.Windows.Controls.ListBoxItem
    {
        /// <summary>
        /// Occurs when the right mouse button is pressed. To surpress right click
        /// selection of the item, this method does nothing.
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
        }
    }
}
