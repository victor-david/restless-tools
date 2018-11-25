using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Extends TabControl to prevent content unloading during tab switch and to provide tab reordering via drag and drop.
    /// </summary>
    /// <remarks>
    /// See:
    /// http://stackoverflow.com/questions/9794151/stop-tabcontrol-from-recreating-its-children
    /// </remarks>
    [TemplatePart(Name = PartItemsHolder, Type = typeof(Panel))]
    public class RestlessTabControl : TabControl
    {
        #region Private
        private Panel itemsHolderPanel = null;
        private const string PartItemsHolder = "PART_ItemsHolder";
        private Window dragCursor;
        private Point startPoint;
        private bool dragging = false;
        #endregion

        /************************************************************************/
        
        #region Public properties
        /// <summary>
        /// Gets or sets a value that indicates if the tabs of this control can be reordered using drag and drop.
        /// </summary>
        public bool AllowTabReorder
        {
            get => (bool)GetValue(AllowTabReorderProperty);
            set => SetValue(AllowTabReorderProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the <see cref="AllowTabReorder"/> property.
        /// </summary>
        public static readonly DependencyProperty AllowTabReorderProperty = DependencyProperty.Register
            (
                nameof(AllowTabReorder), typeof(bool), typeof(RestlessTabControl), new UIPropertyMetadata(false)
            );

        /// <summary>
        /// Gets or sets a command to be executed after a drag / drop operation to perform the reordering.
        /// If this property is not set, the tabs can be reordered internally if the ItemsSource is bound
        /// to an ObservableCollection or to an object type that derives directly from ObservableCollection.
        /// </summary>
        public ICommand ReorderTabsCommand
        {
            get => (ICommand)GetValue(ReorderTabsCommandProperty);
            set => SetValue(ReorderTabsCommandProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the <see cref="ReorderTabsCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ReorderTabsCommandProperty = DependencyProperty.Register
            (
                nameof(ReorderTabsCommand), typeof(ICommand), typeof(RestlessTabControl), new UIPropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets a brush that will be used in the tab drag cursor
        /// </summary>
        public Brush DragCursorBrush
        { 
            get => (Brush)GetValue(DragCursorBrushProperty);
            set => SetValue(DragCursorBrushProperty, value);
        }

        /// <summary>
        /// Identifies the dependency property for <see cref="DragCursorBrush"/>.
        /// </summary>
        public static readonly DependencyProperty DragCursorBrushProperty = DependencyProperty.Register
            (
                nameof(DragCursorBrush), typeof(Brush), typeof(RestlessTabControl), new PropertyMetadata(new SolidColorBrush(Colors.DarkRed))
            );

        /// <summary>
        /// Gets a boolean value that indicates if reordering is available.
        /// </summary>
        private bool IsReorderAvailable
        {
            get => AllowTabReorder && Items.Count > 1;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RestlessTabControl"/> class.
        /// </summary>
        public RestlessTabControl() : base()
        {
            // This is necessary so that we get the initial databound selected item
            ItemContainerGenerator.StatusChanged += ItemContainerGeneratorStatusChanged;
            BorderThickness = new Thickness(1);
            BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        static RestlessTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RestlessTabControl), new FrameworkPropertyMetadata(typeof(RestlessTabControl)));
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Get the ItemsHolder and generate any children
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            itemsHolderPanel = GetTemplateChild(PartItemsHolder) as Panel;
            UpdateSelectedItem();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// When the items change we remove any generated panel children and add any new ones as necessary
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (itemsHolderPanel == null)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    itemsHolderPanel.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            ContentPresenter cp = FindChildContentPresenter(item);
                            if (cp != null)
                            {
                                itemsHolderPanel.Children.Remove(cp);
                            }
                        }
                    }

                    // Don't do anything with new items because we don't want to
                    // create visuals that aren't being shown
                    UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace not implemented yet");
            }
        }

        /// <summary>
        /// Called when the selection on the control changes.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateSelectedItem();
        }

        /// <summary>
        /// Called when the preview mouse left button down event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tab)
            {
                startPoint = e.GetPosition(null);
                SetAllowDrop(excludedTab: tab);
            }
        }

        /// <summary>
        /// Called when the preview mouse move event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (IsReorderAvailable && !dragging && e.LeftButton == MouseButtonState.Pressed &&  GetRoutedEventTabItem(e) is TabItem tabSource)
            {
                Point pos = e.GetPosition(null);

                if (Math.Abs(pos.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(pos.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    tabSource.AllowDrop = false;
                    dragCursor = CreateDragCursor(tabSource);
                    dragCursor.Show();
                    dragging = true;
                    DragDrop.DoDragDrop(tabSource, tabSource, DragDropEffects.Move);
                    tabSource.AllowDrop = true;
                    dragging = false;
                    dragCursor.Close();
                    dragCursor = null;
                }
            }
        }

        /// <summary>
        /// Called when the DragEnter event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tab)
            {
                tab.Opacity = 0.475;
            }
        }

        /// <summary>
        /// Called when the give feedback event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            e.UseDefaultCursors = false;
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            dragCursor.Left = w32Mouse.X + 8;
            dragCursor.Top = w32Mouse.Y - (dragCursor.ActualHeight / 2);
            dragCursor.Opacity = (e.Effects == DragDropEffects.Move) ? 1.0 : 0.35;
            e.Handled = true;
        }

        /// <summary>
        /// Called when the DragLeave event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tab)
            {
                tab.Opacity = 1.0;
            }
        }

        /// <summary>
        /// Called when an item has been dropped on the control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tabTarget)
            {
                var tabSource = e.Data.GetData(typeof(TabItem)) as TabItem;

                if (!tabTarget.Equals(tabSource))
                {
                    if (ReorderTabsCommand != null)
                    {
                        ReorderTabsCommand.Execute(new TabItemDragDrop(tabSource, tabTarget));
                    }
                    else if (ItemsSource != null)
                    {
                        MoveByItemsSource(tabSource, tabTarget);
                    } else
                    {
                        MoveByItems(tabSource, tabTarget);
                    }
                    tabSource.Opacity = 1.0;
                    tabTarget.Opacity = 1.0;
                    e.Handled = true;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// If containers are done, generate the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorStatusChanged;
                UpdateSelectedItem();
            }
        }

        private void UpdateSelectedItem()
        {
            if (itemsHolderPanel == null)
            {
                return;
            }

            // Generate a ContentPresenter if necessary
            TabItem item = GetSelectedTabItem();
            if (item != null)
            {
                CreateChildContentPresenter(item);
            }

            // show the right child
            foreach (ContentPresenter child in itemsHolderPanel.Children)
            {
                child.Visibility = ((child.Tag as TabItem).IsSelected) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private ContentPresenter CreateChildContentPresenter(object item)
        {
            if (item == null)
            {
                return null;
            }

            ContentPresenter cp = FindChildContentPresenter(item);

            if (cp != null)
            {
                return cp;
            }

            // the actual child to be added.  cp.Tag is a reference to the TabItem
            cp = new ContentPresenter
            {
                Content = (item is TabItem) ? (item as TabItem).Content : item,
                ContentTemplate = SelectedContentTemplate,
                ContentTemplateSelector = SelectedContentTemplateSelector,
                ContentStringFormat = SelectedContentStringFormat,
                Visibility = Visibility.Collapsed,
                Tag = (item is TabItem) ? item : (ItemContainerGenerator.ContainerFromItem(item))
            };
            itemsHolderPanel.Children.Add(cp);
            return cp;
        }

        private ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is TabItem)
            {
                data = (data as TabItem).Content;
            }

            if (data == null || itemsHolderPanel == null)
            {
                return null;
            }

            foreach (ContentPresenter cp in itemsHolderPanel.Children)
            {
                if (cp.Content == data)
                {
                    return cp;
                }
            }

            return null;
        }

        private TabItem GetSelectedTabItem()
        {
            object selectedItem = SelectedItem;
            if (selectedItem == null)
            {
                return null;
            }

            if (!(selectedItem is TabItem item))
            {
                item = ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
            }

            return item;
        }

        private TabItem GetTabItem(object item)
        {
            if (!(item is TabItem tabItem))
            {
                tabItem = ItemContainerGenerator.ContainerFromItem(item) as TabItem;
            }

            return tabItem;
        }

        private void MoveByItemsSource(TabItem source, TabItem target)
        {
            var sourceType = ItemsSource.GetType();

            if (!sourceType.IsGenericType)
            {
                sourceType = sourceType.BaseType;
            }

            // sourceType being null is highly unlikely
            if (sourceType != null && sourceType.IsGenericType)
            {
                var sourceDef = sourceType.GetGenericTypeDefinition();

                if (sourceDef == typeof(ObservableCollection<>))
                {
                    int sourceIdx = ItemContainerGenerator.IndexFromContainer(source);
                    int targetIdx = ItemContainerGenerator.IndexFromContainer(target);
                    if (sourceIdx >= 0 && targetIdx >= 0)
                    {
                        var method = sourceType.GetMethod("Move");
                        method.Invoke(ItemsSource, new object[] { sourceIdx, targetIdx });
                    }
                }
            }
        }

        private void MoveByItems(TabItem source, TabItem target)
        {
            int sourceIdx = Items.IndexOf(source);
            int targetIdx = Items.IndexOf(target);
            if (sourceIdx >=0 && targetIdx >= 0)
            {
                Items.Remove(source);
                Items.Insert(targetIdx, source);
            }
        }

        /// <summary>
        /// Gets a tab item during a routed event
        /// </summary>
        /// <param name="e">The routed event args</param>
        /// <returns>The tab item, or null.</returns>
        /// <remarks>
        /// This method find the tabitem for the event. When tabs are defined directly in xaml,
        /// e.Source is the tab item. However, when items are bound via ItemsSource,
        /// e.Source is the tab control, and e.OriginalSource is some piece that's a child of the
        /// actual TabItem. This method examines all posibilities and returns the actual TabItem.
        /// </remarks>
        private TabItem GetRoutedEventTabItem(RoutedEventArgs e)
        {
            if (e.Source is TabItem tab1)
            {
                return GetValidatedChildTabItem(tab1);
            }
            if (e.OriginalSource is TabItem tab2)
            {
                return GetValidatedChildTabItem(tab2);
            }
            if (e.OriginalSource is DependencyObject dp)
            {
                return GetValidatedChildTabItem(GetParent<TabItem>(dp));
            }
            return null;
        }

        /// <summary>
        /// Gets the validated tab item
        /// </summary>
        /// <param name="tab">The tab item</param>
        /// <returns><paramref name="tab"/> if it belongs to this instance of the tab control</returns>
        private TabItem GetValidatedChildTabItem(TabItem tab)
        {
            if (GetParent<RestlessTabControl>(tab) == this)
            {
                return tab;
            }
            return null;
        }

        private T GetParent<T>(DependencyObject child) where T:DependencyObject
        {
            if (child == null) return null;
            var parent = VisualTreeHelper.GetParent(child);
            if (parent is T)
            {
                return parent as T;
            }
            return GetParent<T>(parent);
        }
        #endregion

        /************************************************************************/

        #region Private methods (drag and drop support)
        /// <summary>
        /// Sets all tabs to allow drop except for the excluded one, the one being dragged.
        /// </summary>
        /// <param name="excludedTab">The tab to exclude. This is the tab that's being dragged.</param>
        private void SetAllowDrop(TabItem excludedTab)
        {
            foreach (var item in Items)
            {
                TabItem tab = GetTabItem(item);
                if (tab != null && tab != excludedTab)
                {
                    tab.AllowDrop = true;
                }
            }
        }

        private Window CreateDragCursor(FrameworkElement dragElement)
        {
            dragCursor = new Window()
            {
                Background = DragCursorBrush,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Topmost = true,
                ShowInTaskbar = false,
                Width = dragElement.ActualWidth,
                Height = dragElement.ActualHeight + 6.0,
                Content = new Rectangle()
                {
                    Width = dragElement.ActualWidth,
                    Height = dragElement.ActualHeight,
                    Fill = new VisualBrush(dragElement)
                },
            };

            return dragCursor;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };
        #endregion
    }
}
