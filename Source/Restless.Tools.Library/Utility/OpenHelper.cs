using Restless.Tools.Resources;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides static helper methods to open files and web sites.
    /// </summary>
    public static class OpenHelper
    {
        /// <summary>
        /// Opens the specified web site.
        /// </summary>
        /// <param name="browserPath">The path to the browser executable, or null/empty to use the system default</param>
        /// <param name="urlToOpen">The url of the web site to open. Adds http if needed.</param>
        /// <remarks>
        /// See:
        /// http://stackoverflow.com/questions/2370732/how-to-find-all-the-browsers-installed-on-a-machine
        /// </remarks>
        public static void OpenWebSite(string browserPath, string urlToOpen)
        {
            try
            {
                string url = Format.MakeHttp(urlToOpen);
                Process process = new Process();
                if (!String.IsNullOrEmpty(browserPath))
                {
                    process.StartInfo.FileName = browserPath;
                    process.StartInfo.Arguments = url;
                }
                else
                {
                    process.StartInfo.FileName = url;
                }
                process.Start();
            }
            catch (Exception ex)
            {
                Messages.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="args">The arguments to the file.</param>
        public static void OpenFile(string fileName, string args = null)
        {
            try
            {
                Validations.ValidateNullEmpty(fileName, "fileName");
                var process = Process.Start(fileName, args);
            }

            catch (Win32Exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(fileName);
                sb.AppendLine();
                sb.AppendLine(Strings.InvalidOperation_CannotOpenFile);
                Messages.ShowError(sb.ToString());
            }

            catch (Exception ex)
            {
                Messages.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// Opens explorer to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void Explore(string path)
        {
            OpenFile("explorer.exe", String.Format(@"/e /root,{0}", path));
        }

        /// <summary>
        /// Opens explorer to the specified path and selects the specified file.
        /// </summary>
        /// <param name="fullPath">The full path that indcludes the file name.</param>
        public static void ExploreToFile(string fullPath)
        {
            OpenFile("explorer.exe", String.Format(@"/select,{0}", fullPath));
        }
    }
}
