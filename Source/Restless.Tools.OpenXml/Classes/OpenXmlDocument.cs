using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Restless.Tools.OpenXml.Resources;
using System;
using System.IO;
using System.IO.Packaging;
using System.Text;

namespace Restless.Tools.OpenXml
{
    /// <summary>
    /// Provides static methods to manage OpenXML data
    /// </summary>
    public static class OpenXmlDocument
    {
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

        #region public static class Reader
        /// <summary>
        /// Provides static methods for reading information from Open XML documents.
        /// </summary>
        public static class Reader
        {
            #region Public methods
            /// <summary>
            /// Gets the text of the specified document
            /// </summary>
            /// <param name="fileName">The file name to get the text from.</param>
            /// <returns>The text of the document</returns>
            public static string GetText(string fileName)
            {
                if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(nameof(fileName));
                if (!File.Exists(fileName)) throw new InvalidOperationException($"{Strings.InvalidOperationFileDoesNotExist} {fileName}");

                try
                {
                    string result = null;
                    Package wordPackage = Package.Open(fileName, FileMode.Open, FileAccess.Read);
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(wordPackage))
                    {
                        Body body = wordDocument.MainDocumentPart.Document.Body;
                        if (body != null)
                        {
                            result = GetPlainText(body);
                        }
                    }
                    wordPackage.Close();
                    return result;
                }
                catch (IOException)
                {
                    throw;
                }
                catch
                {
                    throw new InvalidOperationException(Strings.InvalidOperationOpenXmlReader);
                }
            }

            /// <summary>
            /// Gets the word count of the specified document.
            /// </summary>
            /// <param name="fileName">The file name to get the word count from.</param>
            /// <returns>The word count</returns>
            /// <exception cref="InvalidOperationException">
            /// The file does not exist or is not an OpenXML file.
            /// </exception>
            public static int GetWordCount(string fileName)
            {
                string text = GetText(fileName);
                // The "–" character does not count as a word.
                text = text.Replace(Environment.NewLine, " ").Replace("–", " ");
                char[] splitter = new char[1] { ' ' };
                string[] words = text.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                return words.Length;
            }

            /// <summary>
            /// Gets the word count of the specified document.
            /// </summary>
            /// <param name="fileName">The file name to get the word count from.</param>
            /// <returns>The word count</returns>
            /// <remarks>
            /// Unlike the <see cref="GetWordCount"/> method, this method does not throw an exception
            /// if there's a problem with the file (doesn't exist or isn't an OpenXML file). Instead,
            /// it returns zero.
            /// </remarks>
            public static int TryGetWordCount(string fileName)
            {
                int wordCount = 0;
                try
                {
                    wordCount = GetWordCount(fileName);
                }
                catch
                {
                }
                return wordCount;
            }

            /// <summary>
            /// Gets core and extended properties from the specified document.
            /// </summary>
            /// <param name="fileName">The file name to get the properties from.</param>
            /// <returns>The properties</returns>
            public static PropertiesAdapter GetProperties(string fileName)
            {
                if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(nameof(fileName));
                if (!File.Exists(fileName)) throw new InvalidOperationException($"{Strings.InvalidOperationFileDoesNotExist} {fileName}");

                try
                {
                    PropertiesAdapter result = null;
                    Package wordPackage = Package.Open(fileName, FileMode.Open, FileAccess.Read);
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(wordPackage))
                    {
                        result = new PropertiesAdapter(fileName, wordDocument);
                    }
                    wordPackage.Close();
                    return result;
                }
                catch (IOException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException(Strings.InvalidOperationOpenXmlReader);
                }
            }
            #endregion

            /************************************************************************/

            #region Private methods
            /// <summary> 
            ///  Read Plain Text in all XmlElements of word document 
            /// </summary> 
            /// <param name="element">XmlElement in document</param> 
            /// <returns>Plain Text in XmlElement</returns> 
            private static string GetPlainText(OpenXmlElement element)
            {
                StringBuilder builder = new StringBuilder();
                foreach (OpenXmlElement section in element.Elements())
                {
                    switch (section.LocalName)
                    {
                        // Text 
                        case "t":
                            builder.Append(section.InnerText);
                            break;

                        // Carriage return / page break
                        case "cr":
                        case "br":
                            builder.Append(Environment.NewLine);
                            break;

                        // Tab 
                        case "tab":
                            builder.Append("\t");
                            break;

                        // Paragraph 
                        case "p":
                            builder.Append(GetPlainText(section));
                            builder.AppendLine(Environment.NewLine);
                            break;

                        default:
                            builder.Append(GetPlainText(section));
                            break;
                    }
                }

                return builder.ToString();
            }
            #endregion
        }
        #endregion

        /************************************************************************/

        #region Public static class Writer
        /// <summary>
        /// Provides static methods for writing data to Open XML documents.
        /// </summary>
        public static class Writer
        {
            #region Public methods
            /// <summary>
            /// Sets core and extended properties for the specified document.
            /// </summary>
            /// <param name="fileName">The file name to set the properties for.</param>
            /// <param name="props">The properties object.</param>
            /// <remarks>
            /// This method does not preserve the original created and modification dates.
            /// To do so, call the <see cref="PropertiesAdapter.Save"/> method on the object
            /// returned from the <see cref="Reader.GetProperties"/> method instead. For example:
            /// <code>
            /// var props = OpenXmlDocument.Reader.GetProperties(@"D:\MyDocument.docx");
            /// props.Core.Title = "The New Title";
            /// props.Core.Description= "This title has a new description";
            /// props.Save();
            /// </code>
            /// <para>
            /// If you don't want to preserve the dates, or want to handle it yourself, use the <see cref="SetProperties"/> method:
            /// </para>
            /// <code>
            /// var props = OpenXmlDocument.Reader.GetProperties(@"D:\MyDocument.docx");
            /// props.Core.Title = "The New Title";
            /// props.Core.Description= "This title has a new description";
            /// OpenXmlDocument.Writer.SetProperties(@"D:\MyDocument.docx", props);
            /// </code>
            /// </remarks>
            public static void SetProperties(string fileName, PropertiesAdapter props)
            {
                if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(nameof(fileName));
                if (!File.Exists(fileName)) throw new InvalidOperationException($"{Strings.InvalidOperationFileDoesNotExist} {fileName}");
                if (props == null) throw new ArgumentNullException(nameof(props));

                try
                {
                    Package wordPackage = Package.Open(fileName, FileMode.Open, FileAccess.ReadWrite);
                    using (WordprocessingDocument doc = WordprocessingDocument.Open(wordPackage))
                    {
                        // core properties
                        doc.PackageProperties.Category = props.Core.Category;
                        doc.PackageProperties.ContentStatus = props.Core.ContentStatus;
                        doc.PackageProperties.ContentType = props.Core.ContentType;
                        doc.PackageProperties.Created = props.Core.Created;
                        doc.PackageProperties.Creator = props.Core.Creator;
                        doc.PackageProperties.Description = props.Core.Description;
                        doc.PackageProperties.Identifier = props.Core.Identifier;
                        doc.PackageProperties.Keywords = props.Core.Keywords;
                        doc.PackageProperties.Language = props.Core.Language;
                        doc.PackageProperties.LastModifiedBy = props.Core.LastModifiedBy;
                        doc.PackageProperties.LastPrinted = props.Core.LastPrinted;
                        doc.PackageProperties.Modified = props.Core.Modified;
                        doc.PackageProperties.Revision = props.Core.Revision;
                        doc.PackageProperties.Subject = props.Core.Subject;
                        doc.PackageProperties.Title = props.Core.Title;
                        doc.PackageProperties.Version = props.Core.Version;

                        // extended properties
                        OpenXmlDocumentCreator.AddExtendedFilePropertiesPartIfNeeded(doc);
                        doc.ExtendedFilePropertiesPart.Properties.Company.Text = props.Extended.Company;
                    }

                    wordPackage.Close();
                }
                catch (IOException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException(Strings.InvalidOperationOpenXmlReader);
                }
            }
            #endregion
        }
        #endregion
    }
}
