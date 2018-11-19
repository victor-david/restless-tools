using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using Restless.Tools.OfficeAutomation.Resources;

namespace Restless.Tools.OfficeAutomation
{
    /// <summary>
    /// Provides access to Microsoft Office operations.
    /// </summary>
    public sealed class OfficeOperation
    {
        #region Private Members
        private static OfficeOperation instance;
        private Word.Application wordApplication;
        private Word.Document wordDocument;
        private object isFalse = false;
        private object isTrue = true;
        private object m = System.Reflection.Missing.Value;
        private List<string> extensions;
        #endregion

        /************************************************************************/

        #region Public Fields Properties
        /// <summary>
        /// Defines the number of seconds to add to the original modified date
        /// when saving the file after an internal update.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When certain operations are performed, such as setting property value, or
        /// during a document conversion, the original modified date 
        /// (or in the case of a document conversion, the created date also)
        /// is reset back to its original value, plus the number of seconds defined here.
        /// </para>
        /// <para>
        /// By incrementing the dates this small amount, the original intention of the dates
        /// are retained while also enabling another process, such as a backup
        /// routine, to see that the files have changed.
        /// </para>
        /// </remarks>
        public const int SecondsToAdd = 15;
        #endregion

        /************************************************************************/

        #region Singleton Access, Constructor, Destructor
        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static OfficeOperation Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OfficeOperation();
                }
                return instance;
            }
        }

        private OfficeOperation()
        {
            PrepareApplication();
            wordDocument = null;
            extensions = new List<string>();
            extensions.Add(".doc");
            extensions.Add(".docx");
        }
        #endregion

        /************************************************************************/

        #region Public Methods
        /// <summary>
        /// Shuts the word application down. You must call this method after using this class.
        /// </summary>
        /// <remarks>
        /// After using this class to convert files, you must call this method to shut down the underlying Word application:
        /// <code>
        /// Restless.Tools.OfficeAutomation.OfficeOperation.Instance.ConvertToXmlDocument(@"D:\MyFiles\File1.doc");
        /// Restless.Tools.OfficeAutomation.OfficeOperation.Instance.ConvertToXmlDocument(@"D:\MyFiles\File2.doc");
        /// Restless.Tools.OfficeAutomation.OfficeOperation.Instance.ConvertToXmlDocument(@"D:\MyFiles\File3.doc");
        /// Restless.Tools.OfficeAutomation.OfficeOperation.Instance.Shutdown();
        /// </code>
        /// </remarks>
        public void Shutdown()
        {
            #pragma warning disable 0467
            if (wordApplication != null)
            {
                CloseFile();
                wordApplication.Quit(ref isFalse);
                wordApplication = null;
            }
            #pragma warning restore
        }

        /// <summary>
        /// Converts the specified document to .docx
        /// </summary>
        /// <param name="fileName">The original file name</param>
        /// <returns>an object that describes the result of the conversion operation</returns>
        public OfficeConversionResult ConvertToXmlDocument(string fileName)
        {
            #pragma warning disable 0467
            Exception opException = null;
            string newFileName = fileName;
            try
            {
                if (String.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException("ConvertToXmlDocument.FileName");
                }
                
                if (Path.GetExtension(fileName).ToLower() == ".doc")
                {
                    var origFile = OpenFile(fileName);
                    origFile.ThrowIfException();

                    string n = origFile.Info.Name.Substring(0, origFile.Info.Name.Length - origFile.Info.Extension.Length) + ".docx";
                    newFileName = Path.Combine(origFile.Info.DirectoryName, n);

                    wordDocument.SaveAs2(
                        newFileName,
                        Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXMLDocument,
                        m, m, m, m, m, m, m, m, m, m, m, m, m, m,
                        Microsoft.Office.Interop.Word.WdCompatibilityMode.wdCurrent);

                    CloseFile();
                    File.SetCreationTimeUtc(newFileName, origFile.Info.CreationTimeUtc.AddSeconds(SecondsToAdd));
                    File.SetLastWriteTimeUtc(newFileName, origFile.Info.LastWriteTimeUtc.AddSeconds(SecondsToAdd));
                    // TODO: Like to send to Recycle, but don't want the dependency on the other library.
                    File.Delete(origFile.Info.FullName);
                    // Restless.Tools.Win32.FileOperations.SendToRecycleSilent(origFile.Info.FullName);
                }
            }

            catch (Exception ex)
            {
                opException = ex;
            }

            return new OfficeConversionResult(fileName, newFileName, opException);

            #pragma warning restore
        }
        #endregion

        /************************************************************************/

        #region Private Methods
        private void PrepareApplication()
        {
            if (wordApplication == null)
            {
                wordApplication = new Microsoft.Office.Interop.Word.Application();
                wordApplication.Visible = false;
            }
        }

        /// <summary>
        /// Closes the previously opened file (if any) and opens the specified file in preparation for an office operation.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="openReadOnly">true to open in read only mode</param>
        private OfficeFileResult OpenFile(string fileName, bool openReadOnly)
        {
            CloseFile();
            Exception opException = null;
            try
            {
                if (String.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException("OpenFile.FileName");
                }
                string ext = Path.GetExtension(fileName).ToLower();
                if (!extensions.Contains(ext))
                {
                    throw new InvalidOperationException(Strings.InvalidOperation_IncorrectFileType);
                }
                PrepareApplication();
                object file = fileName;
                object ro = openReadOnly;
                wordDocument = wordApplication.Documents.Open(ref file, ref m, ref ro);
                if (wordDocument == null) throw new Exception(Strings.InvalidOperation_UnableToOpenDocument);
            }

            catch (Exception ex)
            {
                opException = ex;
                wordDocument = null;
            }

            return new OfficeFileResult(fileName, opException);
        }

        /// <summary>
        /// Closes the previously opened file (if any) and opens the specified file in preparation for an office operation. Opens in read-only mode.
        /// </summary>
        /// <param name="fileName">The file name</param>
        private OfficeFileResult OpenFile(string fileName)
        {
            return OpenFile(fileName, true);
        }

        /// <summary>
        /// Closes the file if one is open.
        /// </summary>
        private void CloseFile()
        {
            #pragma warning disable 0467
            if (wordDocument != null)
            {
                wordDocument.Close(ref isFalse);
                wordDocument = null;
            }
            #pragma warning restore
        }
        #endregion
    }
}
