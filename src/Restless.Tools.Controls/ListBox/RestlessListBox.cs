using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Extends System.Windows.Controls.ListBox to provide addtional functionality.
    /// </summary>
    public class RestlessListBox : ListBox
    {
        #region Private
        private ScrollViewer outerScrollViewer;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RestlessListBox"/> class.
        /// </summary>
        public RestlessListBox()
        {
            // nothing yet
        }
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets a value that determines if right click can select an item.
        /// The default is false.
        /// </summary>
        public bool RightClickSelection
        {
            get => (bool)GetValue(RightClickSelectionProperty);
            set => SetValue(RightClickSelectionProperty, value);
        }

        /// <summary>
        /// Dependency property for <see cref="RightClickSelection"/>.
        /// </summary>
        public static readonly DependencyProperty RightClickSelectionProperty = DependencyProperty.Register
            (
                nameof(RightClickSelection), typeof(bool), typeof(RestlessListBox), new PropertyMetadata(false)
            );

        /// <summary>
        /// Gets or sets a boolean value that determines if the mouse wheel movement
        /// is handled by an outer scroll viewer. The default is false.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see cref="RestlessListBox"/> is contained within an outer scroll viewer
        /// with other content to scroll as a unit, the mouse wheel has no effect when the mouse
        /// is within the list box. Set this property to true to allow mouse wheel movement
        /// within the list box to manage the outer scroll viewer.
        /// </para>
        /// <para>
        /// If the list box has its Height property set to a value that does not allow all of its
        /// items to display, or is otherwise constrained, it will activate its built in scroll viewer.
        /// In these cases, you should not set this property to true.
        /// </para>
        /// </remarks>
        public bool UseOuterScrollViewer
        {
            get => (bool)GetValue(UseOuterScrollViewerProperty);
            set => SetValue(UseOuterScrollViewerProperty, value);
        }

        /// <summary>
        /// Dependency property for <see cref="UseOuterScrollViewer"/>
        /// </summary>
        public static readonly DependencyProperty UseOuterScrollViewerProperty = DependencyProperty.Register
            (
                nameof(UseOuterScrollViewer), typeof(bool), typeof(RestlessListBox), new PropertyMetadata(false)
            );
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Creates or identifies the element used to display a specified item.
        /// </summary>
        /// <returns>The element used to display a specified item.</returns>
        /// <remarks>
        /// This method is used to implement the behavior described by <see cref="RightClickSelection"/>.
        /// If <see cref="RightClickSelection"/> is false (the default), this method returns
        /// a <see cref="RestlessListBox"/> object that surpresses the right click. Otherwise,
        /// this method returns the object from the base implementation.
        /// </remarks>
        protected override DependencyObject GetContainerForItemOverride()
        {
            if (RightClickSelection)
            {
                return base.GetContainerForItemOverride();
            }
            return new ListBoxItemOverride();
        }

        /// <summary>
        /// Invoked when an unhandled System.Windows.Input.Mouse.PreviewMouseWheel attached
        /// event reaches an element in its route that is derived from this class.
        /// </summary>
        /// <param name="e">The System.Windows.Input.MouseWheelEventArgs that contains the event data.</param>
        /// <remarks>
        /// This method is used to implement the behavior described by <see cref="UseOuterScrollViewer"/>.
        /// </remarks>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (UseOuterScrollViewer)
            {
                var sv = GetOuterScrollViewer();
                if (sv != null)
                {
                    sv.ScrollToVerticalOffset(sv.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Gets the outer scroll viewer. If cached, returns the cached object. Otherwise, looks it up.
        /// </summary>
        /// <returns>The scroll viewer, or null if none.</returns>
        private ScrollViewer GetOuterScrollViewer()
        {
            if (outerScrollViewer == null)
            {
                outerScrollViewer = CoreHelper.GetVisualParent<ScrollViewer>(this);
            }
            return outerScrollViewer;
        }
        #endregion
    }
}
