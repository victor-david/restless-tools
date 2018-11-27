using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides static helper methods
    /// </summary>
    public static class CoreHelper
    {
        /// <summary>
        /// Gets the visual parent of the specified type for the specified DependencyObject.
        /// </summary>
        /// <typeparam name="T">The parent type.</typeparam>
        /// <param name="child">The child object.</param>
        /// <returns>The parent of <paramref name="child"/> that is of type <typeparamref name="T"/>, or null if none.</returns>
        public static T GetVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            if (child == null) return null;
            var parent = VisualTreeHelper.GetParent(child);
            if (parent is T)
            {
                return parent as T;
            }
            return GetVisualParent<T>(parent);
        }

        /// <summary>
        /// Gets the visual child of the specified type for the specified DependencyObject.
        /// </summary>
        /// <typeparam name="T">The child type.</typeparam>
        /// <param name="parent">The parent object.</param>
        /// <returns></returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
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

    }
}
