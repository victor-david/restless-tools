﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Restless.Tools.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Restless.Tools.Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The array {0} must have a minimum length of {1}.
        /// </summary>
        internal static string Argument_ValidateArray {
            get {
                return ResourceManager.GetString("Argument_ValidateArray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The object ({0}) is not allowed to be null..
        /// </summary>
        internal static string ArgumentNull_ValidateNull {
            get {
                return ResourceManager.GetString("ArgumentNull_ValidateNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The string ({0}) is not allowed to be null or empty..
        /// </summary>
        internal static string ArgumentNull_ValidateNullOrEmpty {
            get {
                return ResourceManager.GetString("ArgumentNull_ValidateNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value for {0} is out of range. Must be between {1} - {2}.
        /// </summary>
        internal static string ArgumentOutOfRange_ValidateInteger {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_ValidateInteger", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to open the specified file. Please make sure the path and file name is correct..
        /// </summary>
        internal static string InvalidOperation_CannotOpenFile {
            get {
                return ResourceManager.GetString("InvalidOperation_CannotOpenFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specifed column does not belong to the specified table.
        /// </summary>
        internal static string InvalidOperation_ColumnDoesNotBelongToTable {
            get {
                return ResourceManager.GetString("InvalidOperation_ColumnDoesNotBelongToTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The row does not belong to the {0} table.
        /// </summary>
        internal static string InvalidOperation_DataRowMustBeTable {
            get {
                return ResourceManager.GetString("InvalidOperation_DataRowMustBeTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The data row does not belong to the table type that owns this object..
        /// </summary>
        internal static string InvalidOperation_DataRowTableMismatch {
            get {
                return ResourceManager.GetString("InvalidOperation_DataRowTableMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified data table has not been registered.
        /// </summary>
        internal static string InvalidOperation_DataTableNotRegistered {
            get {
                return ResourceManager.GetString("InvalidOperation_DataTableNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The table returned an empty string for ddl creation.
        /// </summary>
        internal static string InvalidOperation_EmptyDdl {
            get {
                return ResourceManager.GetString("InvalidOperation_EmptyDdl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified file does not exist.
        /// </summary>
        internal static string InvalidOperation_FileDoesNotExist {
            get {
                return ResourceManager.GetString("InvalidOperation_FileDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The host table and the dependent table cannot be the same..
        /// </summary>
        internal static string InvalidOperation_HostAndDependentTablesSame {
            get {
                return ResourceManager.GetString("InvalidOperation_HostAndDependentTablesSame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified import file does not exist.
        /// </summary>
        internal static string InvalidOperation_ImportFileDoesNotExist {
            get {
                return ResourceManager.GetString("InvalidOperation_ImportFileDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No search scope has been specified..
        /// </summary>
        internal static string InvalidOperation_NoSearchScopeSpecified {
            get {
                return ResourceManager.GetString("InvalidOperation_NoSearchScopeSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot read the specified file. The file is not is the Open XML format..
        /// </summary>
        internal static string InvalidOperation_OpenXmlReader {
            get {
                return ResourceManager.GetString("InvalidOperation_OpenXmlReader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table is read only.
        /// </summary>
        internal static string InvalidOperation_TableIsReadOnly {
            get {
                return ResourceManager.GetString("InvalidOperation_TableIsReadOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The whois lookup type is unknown.
        /// </summary>
        internal static string InvalidOperation_WhoisLookupTypeIsUnknown {
            get {
                return ResourceManager.GetString("InvalidOperation_WhoisLookupTypeIsUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This method is not implemented.
        /// </summary>
        internal static string NotImplemented {
            get {
                return ResourceManager.GetString("NotImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unhandled Exception.
        /// </summary>
        internal static string UnhandledExceptionCaption {
            get {
                return ResourceManager.GetString("UnhandledExceptionCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unhandled exception occurred.
        ///
        ///{0}
        ///
        ///The application will now attempt to shutdown gracefully..
        /// </summary>
        internal static string UnhandledExceptionMessageFormat {
            get {
                return ResourceManager.GetString("UnhandledExceptionMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (unspecified).
        /// </summary>
        internal static string UnspecifiedName {
            get {
                return ResourceManager.GetString("UnspecifiedName", resourceCulture);
            }
        }
    }
}
