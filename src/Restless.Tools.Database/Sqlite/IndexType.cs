namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Provides an enumeration for column index types
    /// </summary>
    public enum IndexType
    {
        /// <summary>
        /// Not indexed
        /// </summary>
        None,
        /// <summary>
        /// Indexed
        /// </summary>
        Index,
        /// <summary>
        /// Indexed unique
        /// </summary>
        Unique
    }
}
