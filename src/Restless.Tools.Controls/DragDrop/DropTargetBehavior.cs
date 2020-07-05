using System.Windows;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// The behaviour for objects acting as drag sources
    /// </summary>
    public class DropTargetBehavior
    {
        /// <summary>
        ///  Defines an attached dependency property that describes the drop target.
        /// </summary>
        public static readonly DependencyProperty DropTargetProperty = DependencyProperty.RegisterAttached
            (
                "DropTarget", typeof(IDropTarget), typeof(DropTargetBehavior), new PropertyMetadata(null, OnPropertyChanged)
            );

        /// <summary>
        /// Gets the drop target property value.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <returns>The drop target, or null if none.</returns>
        public static IDropTarget GetDropTarget(DependencyObject d)
        {
            return (IDropTarget)d.GetValue(DropTargetProperty);
        }

        /// <summary>
        /// Sets the drop target property value.
        /// </summary>
        /// <param name="d">The The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetDropTarget(DependencyObject d, IDropTarget value)
        {
            d.SetValue(DropTargetProperty, value);
        }

        /// <summary>
        /// Called when the DropTarget property changes.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)d;

            if (e.NewValue != null)
            {
                element.DragOver += DragOver;
                element.Drop += Drop;
            }
            else
            {
                element.DragOver -= DragOver;
                element.Drop -= Drop;
            }
        }

        private static void Drop(object sender, DragEventArgs e)
        {
            var dropTarget = GetDropTarget((DependencyObject)sender);

            dropTarget.Drop(e.Data);
            e.Handled = true;
        }

        private static void DragOver(object sender, DragEventArgs e)
        {
            var dropTarget = GetDropTarget((DependencyObject)sender);

            e.Effects = dropTarget.GetDropEffects(e.Data);
            e.Handled = true;            
        }
    }
}