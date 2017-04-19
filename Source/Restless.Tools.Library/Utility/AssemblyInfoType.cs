namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides an enumeration for instaniating an AssemblyInfo object.
    /// </summary>
    public enum AssemblyInfoType
    {
        /// <summary>
        /// Information is retrieved from the entry assembly.
        /// </summary>
        Entry,
        /// <summary>
        /// Information is retrieved from the calling assembly.
        /// </summary>
        Calling,
        /// <summary>
        /// Information is retrieved from the executing assembly.
        /// </summary>
        Executing
    }
}
