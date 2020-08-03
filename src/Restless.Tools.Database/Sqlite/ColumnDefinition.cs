namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents a column definition.
    /// </summary>
    public class ColumnDefinition
    {
        #region Constructor
        internal ColumnDefinition(string name, ColumnType type, bool isPrimaryKey, bool isNullable, object defaultValue, IndexType index)
        {
            Name = name;
            Type = type;
            IsPrimaryKey = isPrimaryKey;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
            Index = index;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the column type.
        /// </summary>
        public ColumnType Type { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the column is the primary key.
        /// </summary>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the column is nullable.
        /// </summary>
        public bool IsNullable { get; }

        /// <summary>
        /// Gets the default value for the column, or null.
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets the index type.
        /// </summary>
        public IndexType Index { get; }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets DDL that creates the column.
        /// </summary>
        /// <returns>The DDL string.</returns>
        public override string ToString()
        {
            return $"\"{Name}\" {ColumnTypeToString()}{IsPrimaryKeyToString()}{IsNullableToString()}{DefaultValueToString()}";
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private string ColumnTypeToString()
        {
            switch (Type)
            {
                case ColumnType.Integer:
                    return "INTEGER";
                case ColumnType.Text:
                    return "TEXT";
                case ColumnType.Boolean:
                    return "BOOLEAN";
                case ColumnType.Timestamp:
                    return "TIMESTAMP";
                case ColumnType.Numeric:
                    return "NUMERIC";
                default:
                    return "TEXT";
            }
        }

        private string IsPrimaryKeyToString()
        {
            return IsPrimaryKey ? " PRIMARY KEY" : string.Empty;
        }

        private string IsNullableToString()
        {
            if (!IsNullable) return " NOT NULL";
            return string.Empty;
        }

        private string DefaultValueToString()
        {
            if (DefaultValue != null)
            {
                if (DefaultValue is string str)
                {
                    return $" DEFAULT \"{str}\"";
                }

                return $" DEFAULT {DefaultValue}";
            }
            return string.Empty;
        }
        #endregion
    }
}
