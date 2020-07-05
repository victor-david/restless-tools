using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides static helper methods for a Window object.
    /// </summary>
    public static class WindowHelper
    {
        #region Public Methods
        /// <summary>
        /// Gets a Rectangle that describes the current position and size of the window.
        /// </summary>
        /// <param name="window">The window to get the position and size for.</param>
        /// <returns>A Rect structure that describes the current position and size of the window.</returns>
        public static Rect GetWindowRectangle(this Window window)
        {
            RECT nativeRect;
            var ih = new WindowInteropHelper(window);
            GetWindowRect(ih.Handle, out nativeRect);
            Point location = new Point(nativeRect.Left, nativeRect.Top);
            Size size = new Size(nativeRect.Right - nativeRect.Left, nativeRect.Bottom - nativeRect.Top);
            return new Rect(location, size);
        }
        #endregion

        /************************************************************************/

        #region Private Structs and Methods
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        #endregion
    }
}
