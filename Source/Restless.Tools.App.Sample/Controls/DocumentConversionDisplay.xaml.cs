using Microsoft.Win32;
using Restless.Tools.OfficeAutomation;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Tools.App.Sample
{
    /// <summary>
    /// Interaction logic for DocumentConversionDisplay.xaml
    /// </summary>
    public partial class DocumentConversionDisplay : UserControl
    {
        private StringBuilder result;
        private string fileToConvert;
        
        public DocumentConversionDisplay()
        {
            InitializeComponent();
            result = new StringBuilder(1024);
            AppendMessage("To get started, select a .doc file for conversion");
        }

        private void ButtonSelectFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog
            {
                Title = "Select a Microsoft Word Document (.doc) to convert",
                Filter = "Word Document (*.doc)|*.doc"
            };
            if (d.ShowDialog() == true)
            {
                fileToConvert = d.FileName;
                AppendMessage($"{fileToConvert} selected for conversion");
            }

        }

        private void ButtonConvertFileClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(fileToConvert))
            {
                ConvertToDocx(fileToConvert);
                // Must shut down or Word is still hanging around in memory.
                OfficeOperation.Instance.Shutdown();
            }
            else
            {
                AppendMessage("Select a file first");
            }
        }

        /// <summary>
        /// Demonstrates how to convert multiple documents.
        /// </summary>
        private void ConvertMultipleToDocX()
        {
            ConvertToDocx(@"D:\Files\Doc1.doc");
            ConvertToDocx(@"D:\Files\Doc2.doc");
            ConvertToDocx(@"D:\Files\Doc3.doc");
            // You can convert multiple documents, but must shut down when through
            OfficeOperation.Instance.Shutdown();
        }

        /// <summary>
        /// Converts the specified .doc document to .docx.
        /// </summary>
        private void ConvertToDocx(string fileName)
        {
            OfficeConversionResult result = OfficeOperation.Instance.ConvertToXmlDocument(fileName, deleteOriginal:false);
            if (result.Success)
            {
                // Do something
                AppendMessage($"Success. File converted to: {result.ConvertedInfo.FullName}");
            }
            else
            {
                // Access result.Exception. It is non-null only when result.Success is false.
                AppendMessage(result.Exception.Message);
            }
        }

        private void AppendMessage(string text)
        {
            result.AppendLine(text);
            ResultText.Text = result.ToString();
        }
    }
}
