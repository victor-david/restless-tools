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
    public class MenuItemCollection : ObservableCollection<Control>
    {
        /// <summary>
        /// Adds a new menu item to the collection.
        /// </summary>
        /// <param name="header">The item header, that which is displayed in the UI</param>
        /// <param name="command">The command associated with this item.</param>
        /// <param name="commandParm">A parameter for <paramref name="command"/> if needed, or null if none.</param>
        /// <param name="imageResource">The name of the image resource to use on this item, or null if none.</param>
        /// <param name="tag">An arbitrary object to attach to the menu item.</param>
        public void AddItem(string header, ICommand command, object commandParm = null, string imageResource = null, object tag = null)
        {
            var item = new MenuItem
            {
                Header = header,
                Command = command,
                CommandParameter = commandParm,
                Tag = tag,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            if (!string.IsNullOrEmpty(imageResource))
            {
                item.Icon = Application.Current.TryFindResource(imageResource);
            }
            Add(item);
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
