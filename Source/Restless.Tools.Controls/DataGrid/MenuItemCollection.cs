using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Represents a bindable collection of menu items with convienence methods for adding items.
    /// </summary>
    /// <remarks>
    /// See also <see cref="MenuItemExtensions"/>
    /// </remarks>
    public class MenuItemCollection : ObservableCollection<Control>
    {
        /// <summary>
        /// Adds a new menu item to the collection.
        /// </summary>
        /// <param name="header">The item header, that which is displayed in the UI</param>
        /// <param name="command">The command associated with this item.</param>
        /// <returns>The newly added item.</returns>
        public MenuItem AddItem(string header, ICommand command)
        {
            var item = new MenuItem
            {
                Header = header,
                Command = command,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Add(item);
            return item;
        }

        /// <summary>
        /// Adds a menu separator to the collection.
        /// </summary>
        public void AddSeparator()
        {
            Add(new Separator());
        }
    }
}
