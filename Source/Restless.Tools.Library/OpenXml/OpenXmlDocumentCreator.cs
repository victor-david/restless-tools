using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Restless.Tools.Utility;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Restless.Tools.OpenXml
{
    /// <summary>
    /// Provides document creation services
    /// </summary>
    public class OpenXmlDocumentCreator
    {
        #region Public properties
        /// <summary>
        /// Gets or sets the file name used for the create process.
        /// </summary>
        public string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the template file used for the create process.
        /// If this file is specified and it exists, it will be used to copy styles
        /// into the new document.
        /// </summary>
        public string TemplateFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the header text used for the create process
        /// </summary>
        public string HeaderText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the footer text used for the create process
        /// </summary>
        public string FooterText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that specifies whether to place page numbers in the header.
        /// </summary>
        public bool HeaderPageNumbers
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets a value that specifies whether to place page numbers in the footer.
        /// </summary>
        public bool FooterPageNumbers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the paragraphs to place into the created document.
        /// </summary>
        public string[] Paragraphs
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the author that will be placed into the created document.
        /// </summary>
        public string Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description that will be placed into the created document.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the company that will be placed into the created document.
        /// </summary>
        public string Company
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the title that will be placed into the created document.
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        #endregion
        
        /************************************************************************/
        
        #region Constructor
        /// <summary>
        /// Creates a new instance of this object
        /// </summary>
        public OpenXmlDocumentCreator()
        {

        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Creates the document
        /// </summary>
        public void Create()
        {
            Validations.ValidateNullEmpty(Filename, "Filename", "Filename property must be set before calling this method");
            var doc = WordprocessingDocument.Create(Filename, WordprocessingDocumentType.Document);
            // adds the extended if needed. From here, it's always needed.
            AddExtendedFilePropertiesPartIfNeeded(doc);
            doc.ExtendedFilePropertiesPart.Properties.Company.Text = Company;

            // Create a main document part and add it to the package.
            MainDocumentPart mainPart = doc.AddMainDocumentPart();
            AddSettingsToMainDocumentPart(mainPart);
            mainPart.Document = new Document();

            // Add a style part
            AddStylesToMainDocumentPart(mainPart);

            Body body = new Body();

            if (Paragraphs != null)
            {
                foreach (string paraText in Paragraphs)
                {
                    body.Append(new Paragraph(new Run(new Text(paraText))));
                }
            }

            var footerPart = doc.MainDocumentPart.AddNewPart<FooterPart>("rId1");
            GetPageFooterPart(FooterText).Save(footerPart);
            var headerPart = doc.MainDocumentPart.AddNewPart<HeaderPart>("rId2");
            GetPageHeaderPart(HeaderText).Save(headerPart);

            body.Append(new SectionProperties
                (
                    new FooterReference() { Type = HeaderFooterValues.Default, Id = "rId1" },
                    new HeaderReference() { Type = HeaderFooterValues.Default, Id = "rId2" }
                ));

            mainPart.Document.Append(body);

            /* Save and close */
            mainPart.Document.Save();
            doc.PackageProperties.Created = DateTime.UtcNow;
            doc.PackageProperties.Modified = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(Author))
            {
                doc.PackageProperties.Creator = Author;
            }

            if (!string.IsNullOrEmpty(Description))
            {
                doc.PackageProperties.Description = Description;
            }

            if (!string.IsNullOrEmpty(Title))
            {
                doc.PackageProperties.Title = Title;
            }

            doc.Close();
            if (!string.IsNullOrEmpty(TemplateFile) && File.Exists(TemplateFile))
            {
                try
                {
                    ReplaceStyles(TemplateFile, Filename);
                }
                catch { }
            }
        }

        /// <summary>
        /// Adds an ExtendedFilePropertiesPart to the specified document if it's not already there.
        /// </summary>
        /// <param name="doc">The document object</param>
        /// <remarks>
        /// This method is used when creating a new document. At that time, the freshly created doc does
        /// not have extended properties. This method is also used when saving extended properties; if
        /// the extended property element doesn't exist, this creates it and allows the extended property
        /// to be saved.
        /// </remarks>
        internal static void AddExtendedFilePropertiesPartIfNeeded(WordprocessingDocument doc)
        {
            Validations.ValidateNull(doc, "AddExtendedFilePropertiesPart.Doc");

            if (doc.ExtendedFilePropertiesPart == null)
            {
                doc.AddExtendedFilePropertiesPart();
            }

            if (doc.ExtendedFilePropertiesPart.Properties == null)
            {
                doc.ExtendedFilePropertiesPart.Properties = new DocumentFormat.OpenXml.ExtendedProperties.Properties()
                {
                    Company = new DocumentFormat.OpenXml.ExtendedProperties.Company(),
                };
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void AddSettingsToMainDocumentPart(MainDocumentPart part)
        {
            DocumentSettingsPart settingsPart = part.DocumentSettingsPart;
            if (settingsPart == null)  settingsPart = part.AddNewPart<DocumentSettingsPart>();
            settingsPart.Settings = new Settings
                (
                    new Compatibility
                        (
                            new CompatibilitySetting()
                                {
                                    Name = new EnumValue<CompatSettingNameValues>(CompatSettingNameValues.CompatibilityMode),
                                    Val = new StringValue("14"),
                                    Uri = new StringValue("http://schemas.microsoft.com/office/word")
                                }
                        )
                );
            settingsPart.Settings.Save();
        }

        private Footer GetPageFooterPart(string text)
        {
            return new Footer
            (
                new Paragraph
                    (
                        new ParagraphProperties
                        (
                            new ParagraphStyleId() { Val = "Footer" },
                            new Justification() { Val = JustificationValues.Right }
                        ),
                        GetRun(text, FooterPageNumbers)
                    )
            );
        }

        private Header GetPageHeaderPart(string text)
        {
            return new Header
            (
                new Paragraph
                    (
                        new ParagraphProperties
                        (
                            new ParagraphStyleId() { Val = "Header" },
                            new Justification() { Val = JustificationValues.Right }
                        ),
                        GetRun(text, HeaderPageNumbers)
                    )
            );
        }

        /// <summary>
        /// Gets a Run for a header or a footer, adding in the page number if requested
        /// </summary>
        /// <param name="text"></param>
        /// <param name="addPageNumber"></param>
        /// <returns></returns>
        private Run GetRun(string text, bool addPageNumber)
        {
            Run r = new Run(new Text(text));
            if (addPageNumber)
            {
                r.Append(new Text(" p.") { Space = SpaceProcessingModeValues.Preserve }, new PageNumber());
            }
            return r;
        }

        /// <summary>
        /// Adds a style definition part if it doesn't exist.
        /// </summary>
        /// <param name="mainPart">The document main part</param>
        /// <remarks>
        /// In this context (document creation), the style does not exist.
        /// When a document is created programatically, it does not automatically
        /// add a style part such as when creating with MS Word.
        /// </remarks>
        private void AddStylesToMainDocumentPart(MainDocumentPart mainPart)
        {
            StyleDefinitionsPart part = mainPart.StyleDefinitionsPart;
            if (part == null)
            {
                part = mainPart.AddNewPart<StyleDefinitionsPart>();
                Styles root = new Styles();
                root.Save(part);
            }
        }

        /// <summary>
        /// Attempts to replace the styles in <paramref name="dest"/> with those in <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source document.</param>
        /// <param name="dest">The destination document</param>
        private void ReplaceStyles(string source, string dest)
        {
            // Extract and replace the styles part.
            var node = ExtractStylesPart(source, false);
            if (node != null)
            {
                ReplaceStylesPart(dest, node, false);
            }

            // Extract and replace the stylesWithEffects part. To fully support 
            // round-tripping from Word 2010 to Word 2007, you should 
            // replace this part, as well.
            node = ExtractStylesPart(source, true);
            if (node != null)
            {
                ReplaceStylesPart(dest, node, true);
            }
        }


        // Given a file and an XDocument instance that contains the content of 
        // a styles or stylesWithEffects part, replace the styles in the file 
        // with the styles in the XDocument.
        private void ReplaceStylesPart(string fileName, XDocument newStyles, bool setStylesWithEffectsPart)
        {
            // Open the document for write access and get a reference.
            using (var document = WordprocessingDocument.Open(fileName, true))
            {
                // Get a reference to the main document part.
                var docPart = document.MainDocumentPart;

                // Assign a reference to the appropriate part to the
                // stylesPart variable.
                StylesPart stylesPart = null;
                if (setStylesWithEffectsPart)
                    stylesPart = docPart.StylesWithEffectsPart;
                else
                    stylesPart = docPart.StyleDefinitionsPart;

                // If the part exists, populate it with the new styles.
                if (stylesPart != null)
                {
                    newStyles.Save(new StreamWriter(stylesPart.GetStream(FileMode.Create, FileAccess.Write)));
                }
            }
        }

        // Extract the styles or stylesWithEffects part from a 
        // word processing document as an XDocument instance.
        private XDocument ExtractStylesPart(string fileName, bool getStylesWithEffectsPart)
        {
            XDocument styles = null;

            // Open the document for read access and get a reference.
            using (var document = WordprocessingDocument.Open(fileName, false))
            {
                // Get a reference to the main document part.
                var docPart = document.MainDocumentPart;

                // Assign a reference to the appropriate part to the
                // stylesPart variable.
                StylesPart stylesPart = null;

                if (getStylesWithEffectsPart)
                    stylesPart = docPart.StylesWithEffectsPart;
                else
                    stylesPart = docPart.StyleDefinitionsPart;

                // If the part exists, read it into the XDocument.
                if (stylesPart != null)
                {
                    using (var reader = XmlNodeReader.Create(stylesPart.GetStream(FileMode.Open, FileAccess.Read)))
                    {
                        styles = XDocument.Load(reader);
                    }
                }
            }
            return styles;
        }
        #endregion
    }
}
