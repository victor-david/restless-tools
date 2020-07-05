using System.Data;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Defines keys that are used to set <see cref="DataColumn.ExtendedProperties"/>.
    /// </summary>
    public enum DataColumnPropertyKey
    {
        /// <summary>
        /// Indicates that the column will be excluded from insert operations
        /// </summary>
        ExcludeFromInsert,
        /// <summary>
        /// Indicates that the column will be excluded from update operations
        /// </summary>
        ExcludeFromUpdate,
        /// <summary>
        /// Indicates that the column will receive the newly inserted id during an insert operation
        /// </summary>
        ReceiveInsertedId,
    }
}
