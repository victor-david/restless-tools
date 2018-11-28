# Restless Animal Development Tools Library (Office Automation)

Provides MS Office interaction features, such as .doc to .docx conversion.

This assembly provides a convenience wrapper for Microsoft.Office.Interop.Word for projects that require the ability to convert .doc to .docx.

### Document Conversion

```c#
/// <summary>
/// Demonstrates how to convert multiple documents.
/// </summary>
private void ConvertMultipleToDocX()
{
    ConvertToDocx(@"D:\Files\Doc1.doc");
    ConvertToDocx(@"D:\Files\Doc2.doc");
    ConvertToDocx(@"D:\Files\Doc3.doc");
    // You can convert multiple documents, but must shut down when through.
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
    }
    else
    {
        // Access result.Exception. It is non-null only when result.Success is false.
    }
}
```