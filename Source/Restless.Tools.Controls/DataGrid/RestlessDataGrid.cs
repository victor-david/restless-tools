using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Extends <see cref="System.Windows.Controls.DataGrid"/> to provide custom functionality.
    /// </summary>
    public class RestlessDataGrid : DataGrid
    {
        #region Private
        private ScrollViewer scrollViewer;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RestlessDataGrid"/> class.
        /// </summary>
        public RestlessDataGrid()
        {
            DataContextChanged += OnDataContextChanged;
            AddHandler(LoadedEvent, new RoutedEventHandler(OnLoaded));
        }
        #endregion

        /************************************************************************/

        #region CustomSort (attached property)
        /// <summary>
        /// Defines an attached dependency property that is used to provide custom sorting
        /// on a DataGridColumn by adding a secondary sort on another column.
        /// </summary>
        public static readonly DependencyProperty CustomSortProperty = DependencyProperty.RegisterAttached
            (
                "CustomSort", typeof(DataGridColumnSortSpec), typeof(RestlessDataGrid), new PropertyMetadata()
            );

        /// <summary>
        /// Gets the <see cref="CustomSortProperty"/> for the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to get the property for.</param>
        /// <returns>The attached property, or null if none.</returns>
        public static DataGridColumnSortSpec GetCustomSort(DependencyObject obj)
        {
            return (DataGridColumnSortSpec)obj.GetValue(CustomSortProperty);
        }

        /// <summary>
        /// Sets the <see cref="CustomSortProperty"/> on the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to set the property on.</param>
        /// <param name="value">The property to set.</param>
        public static void SetCustomSort(DependencyObject obj, DataGridColumnSortSpec value)
        {
            obj.SetValue(CustomSortProperty, value);
        }
        #endregion

        /************************************************************************/

        #region DoubleClick (attached property)
        /// <summary>
        /// Defines an attached dependency property that enables binding the mouse double-click 
        /// on a data grid row to a command.
        /// </summary>
        /// <remarks>
        /// See:
        /// http://stackoverflow.com/questions/17419570/bind-doubleclick-command-from-datagrid-row-to-vm
        /// </remarks>
        /// <AttachedPropertyComments>
        /// <summary>
        /// This attached property provides access to the command that is bound to the mouse double-click.
        /// </summary>
        /// </AttachedPropertyComments>
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached
           (
                "DoubleClickCommand", typeof(ICommand), typeof(RestlessDataGrid), new PropertyMetadata(OnDoubleClickPropertyChanged)
           );

        private static void OnDoubleClickPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridRow element)
            {
                if (e.NewValue != null)
                {
                    element.AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(DataGridMouseDoubleClick));
                }
                else
                {
                    element.RemoveHandler(MouseDoubleClickEvent, new RoutedEventHandler(DataGridMouseDoubleClick));
                }
            }
        }

        private static void DataGridMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (sender is DataGridRow element)
            {
                var cmd = GetDoubleClickCommand(element);
                if (cmd.CanExecute(element.Item))
                {
                    cmd.Execute(element.Item);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="DoubleClickCommandProperty"/> for the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to get the property for.</param>
        /// <returns>The attached property, or null if none.</returns>
        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleClickCommandProperty);
        }

        /// <summary>
        /// Sets the <see cref="DoubleClickCommandProperty"/> on the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to set the property on.</param>
        /// <param name="value">The property to set.</param>
        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickCommandProperty, value);
        }
        #endregion

        /************************************************************************/

        #region ContextMenuOpeningCommand
        /// <summary>
        /// Gets or sets a command to be executed when a context menu associated with the data grid is opening.
        /// </summary>
        public ICommand ContextMenuOpeningCommand
        {
            get => (ICommand)GetValue(ContextMenuOpeningCommandProperty);
            set => SetValue(ContextMenuOpeningCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ContextMenuOpeningCommand"/> dependency property.
        /// </summary>
        public static DependencyProperty ContextMenuOpeningCommandProperty = DependencyProperty.Register
             (
                nameof(ContextMenuOpeningCommand), typeof(ICommand), typeof(RestlessDataGrid), new PropertyMetadata()
             );
        #endregion

        /************************************************************************/

        #region SortingCommand
        /// <summary>
        /// Gets or sets a command to be executed when the data grid is sorting as defined by <see cref="SortingCommandProperty"/>.
        /// </summary>
        public ICommand SortingCommand
        {
            get => (ICommand)GetValue(SortingCommandProperty);
            set => SetValue(CustomSortProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that allows the consumer to run a command when the DataGrid is sorting.
        /// </summary>
        public static DependencyProperty SortingCommandProperty = DependencyProperty.Register
             (
                nameof(SortingCommand), typeof(ICommand), typeof(RestlessDataGrid), new PropertyMetadata()
             );
        #endregion

        /************************************************************************/

        #region SelectedItemsList
        /// <summary>
        /// Gets or sets the selected items list as defined by <see cref="SelectedItemsListProperty"/>.
        /// </summary>
        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set => SetValue(SelectedItemsListProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that enables the consumer to bind to multiple selected items
        /// </summary>
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register
            (
                nameof(SelectedItemsList), typeof(IList), typeof(RestlessDataGrid), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region AutoFocusOnMouseEnter
        /// <summary>
        /// Gets or sets a boolean value that indicates whether the control will recieve focus when the mouse enters as defined by the <see cref="AutoFocusOnMouseEnterProperty"/>.
        /// </summary>
        public bool AutoFocusOnMouseEnter
        {
            get => (bool)GetValue(AutoFocusOnMouseEnterProperty);
            set => SetValue(AutoFocusOnMouseEnterProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that specifies whether the control will recieve focus when the mouse enters.
        /// </summary>
        public static readonly DependencyProperty AutoFocusOnMouseEnterProperty = DependencyProperty.Register
            (
                "AutoFocusOnMouseEnter", typeof(bool), typeof(RestlessDataGrid), new PropertyMetadata(false)
            );
        #endregion

        /************************************************************************/

        #region RestoreStateBehavior
        /// <summary>
        /// Gets or sets a value that determines which actions will be taken to restore the control state as defined by the <see cref="RestoreStateBehaviorProperty"/>.
        /// </summary>
        public RestoreDataGridState RestoreStateBehavior
        {
            get => (RestoreDataGridState)GetValue(RestoreStateBehaviorProperty);
            set => SetValue(RestoreStateBehaviorProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that specifies which actions will be taken to restore the control state.
        /// </summary>
        public static readonly DependencyProperty RestoreStateBehaviorProperty = DependencyProperty.Register
            (
                nameof(RestoreStateBehavior), typeof(RestoreDataGridState), typeof(RestlessDataGrid), new PropertyMetadata(RestoreDataGridState.None)
            );
        #endregion

        /************************************************************************/

        #region ScrollViewerVerticalOffset
        /// <summary>
        /// Gets or sets a value that determines the vertical offset of the data grid scroll viewer
        /// </summary>
        public double ScrollViewerVerticalOffset
        {
            get => (double)GetValue(ScrollViewerVerticalOffsetProperty);
            set => SetValue(ScrollViewerVerticalOffsetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ScrollViewerVerticalOffset"/> dependency property
        /// </summary>
        public static readonly DependencyProperty ScrollViewerVerticalOffsetProperty = DependencyProperty.Register
            (
                nameof(ScrollViewerVerticalOffset), typeof(double), typeof(RestlessDataGrid),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnScrollViewerVerticalOffsetChanged)
            );

        private static void OnScrollViewerVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestlessDataGrid control)
            {
                var info = new ScrollInfo(control, (double)e.NewValue);
                if (control.scrollViewer == null)
                {
                    control.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(PerformScroll), info);
                }
                else
                {
                    PerformScroll(info);
                }
            }
        }

        private static object PerformScroll(object parm)
        {
            if (parm is ScrollInfo info && info.Grid.scrollViewer != null)
            {
                double currentOffset = info.Grid.scrollViewer.VerticalOffset;
                double newOffset = info.VerticalOffset;
                if (newOffset != currentOffset)
                {
                    info.Grid.scrollViewer.ScrollToVerticalOffset(newOffset);
                }
            }
            return null;
        }
        #endregion

        /************************************************************************/

        #region OnBeginningEditCommand
        /// <summary>
        /// Gets or sets a command to be executed when the grid begins to edit.
        /// </summary>
        public ICommand OnBeginningEditCommand
        {
            get => (ICommand)GetValue(OnBeginningEditCommandProperty);
            set => SetValue(OnBeginningEditCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OnBeginningEditCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnBeginningEditCommandProperty = DependencyProperty.Register
            (
                nameof(OnBeginningEditCommand), typeof(ICommand), typeof(RestlessDataGrid), new PropertyMetadata(null)
            );

        #endregion
        
        /************************************************************************/

        #region OnCellEditEndingCommand
        /// <summary>
        /// Gets or sets a command to be executed when a grid cell ends it edit as defined by the  <see cref="OnCellEditEndingCommandProperty"/>.
        /// </summary>
        public ICommand OnCellEditEndingCommand
        {
            get => (ICommand)GetValue(OnCellEditEndingCommandProperty);
            set => SetValue(OnCellEditEndingCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OnCellEditEndingCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnCellEditEndingCommandProperty = DependencyProperty.Register
            (
                nameof(OnCellEditEndingCommand), typeof(ICommand), typeof(RestlessDataGrid), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Occurs when the mouse enters
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (ReferenceEquals(e.Source, e.OriginalSource))
            {
                if (AutoFocusOnMouseEnter)
                {
                    Focus();
                }
            }
        }

        /// <summary>
        /// Occurs when the selected item changes, raises the SelectionChanged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SelectedItemsList = SelectedItems;
        }

        /// <summary>
        /// Occurs when the <see cref="RestlessDataGrid"/> is sorting, raises the Sorting event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSorting(DataGridSortingEventArgs e)
        {
            base.OnSorting(e);
            var view = CollectionViewSource.GetDefaultView(ItemsSource);
            // think this is unlikely
            if (view == null) return;

            // Get the custom sort.
            var colSort = e.Column.GetValue(CustomSortProperty) as DataGridColumnSortSpec;

            // If we have either a custom sort or a sorting command, clear the sort descriptions.
            if (colSort != null || SortingCommand != null)
            {
                view.SortDescriptions.Clear();
            }

            if (SortingCommand != null)
            {
                SortingCommand.Execute(e.Column);
                e.Handled = view.SortDescriptions.Count > 0;
            }

            
            if (colSort != null)
            {
                ListSortDirection primaryDirection = (e.Column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

                string primaryPath = e.Column.SortMemberPath;
                if (!string.IsNullOrEmpty(colSort.Column1))
                {
                    primaryPath = colSort.Column1;
                }
                view.SortDescriptions.Add(new SortDescription(primaryPath, primaryDirection));

                ListSortDirection secondaryDirection = primaryDirection;
                switch (colSort.Behavior)
                {
                    case DataGridColumnSortBehavior.AlwaysAscending:
                        secondaryDirection = ListSortDirection.Ascending;
                        break;
                    case DataGridColumnSortBehavior.AlwaysDescending:
                        secondaryDirection = ListSortDirection.Descending;
                        break;
                    case DataGridColumnSortBehavior.ReverseFollowPrimary:
                        secondaryDirection = (primaryDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                        break;
                    default:
                        secondaryDirection = primaryDirection;
                        break;
                }
                view.SortDescriptions.Add(new SortDescription(colSort.Column2, secondaryDirection));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when a context menu associated with this data grid is opening.
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <remarks>
        /// If the <see cref="ContextMenuOpeningCommand"/> has been set, this method calls the command that was established,
        /// passing event arguments <paramref name="e"/> to the command handler.
        /// </remarks>
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);
            ICommand cmd = ContextMenuOpeningCommand;
            if (cmd != null && cmd.CanExecute(e))
            {
                cmd.Execute(e);
            }
        }

        /// <summary>
        /// Called when the data grid begins edit.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
        {
            base.OnBeginningEdit(e);
            ICommand cmd = OnBeginningEditCommand;
            if (cmd != null && cmd.CanExecute(e))
            {
                cmd.Execute(e);
            }
        }

        /// <summary>
        /// Called when the data grid ends cell editing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            base.OnCellEditEnding(e);
            ICommand cmd = OnCellEditEndingCommand;
            if (cmd != null && cmd.CanExecute(e))
            {
                cmd.Execute(e);
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = GetVisualChild<ScrollViewer>(this);

            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += OnScrollChanged;
            }

            DispatcherRestoreState();
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            SetValue(ScrollViewerVerticalOffsetProperty, e.VerticalOffset);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DispatcherRestoreState();
        }

        private void DispatcherRestoreState()
        {
            if (RestoreStateBehavior != RestoreDataGridState.None)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback((p) =>
                {
                    RestoreState();
                    return null;
                }), null);
            }
        }

        /// <summary>
        /// Restores the state. This must be run from the Disptacher callback
        /// </summary>
        private void RestoreState()
        {
            object temp = SelectedItem;
            var b = RestoreStateBehavior;

            //  no item selected previously. 
            if (temp == null)
            {
                if (b.HasFlag(RestoreDataGridState.SelectFirst))
                {
                    Items.MoveCurrentToFirst();
                    temp = Items.CurrentItem;
                }

                if (b.HasFlag(RestoreDataGridState.SelectLast))
                {
                    Items.MoveCurrentToLast();
                    temp = Items.CurrentItem;
                }

                if (SelectionMode == DataGridSelectionMode.Extended)
                {
                    SelectedItems.Clear();
                }

                SelectedItem = temp;
            }

            // item selected previously
            else if (b.HasFlag(RestoreDataGridState.RestoreLastSelection))
            {
                if (SelectionMode == DataGridSelectionMode.Extended)
                {
                    SelectedItems.Clear();
                }
                SelectedItem = null;
                SelectedItem = temp;
            }

            // Now. If we have a selected item, scroll it into view
            if (SelectedItem != null)
            {
                ScrollIntoView(SelectedItem);
            }
        }

        private T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);

            int numvisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numvisuals; ++i)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                    child = GetVisualChild<T>(v);
                else
                    break;
            }

            return child;
        }
        #endregion

        /************************************************************************/

        #region Private helper class (ScrollInfo)
        private class ScrollInfo
        {
            public RestlessDataGrid Grid { get; private set; }
            public double VerticalOffset { get; private set; }
            public ScrollInfo(RestlessDataGrid grid, double verticalOffset)
            {
                Grid = grid;
                VerticalOffset = verticalOffset;
            }
        }
        #endregion
    }
}
