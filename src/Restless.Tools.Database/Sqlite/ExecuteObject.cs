using System;
using System.Data.SQLite;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represenst a sql execution object
    /// </summary>
    public class ExecuteObject
    {
        #region Private
        private SQLiteConnection connection;
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        internal ExecuteObject(SQLiteConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Executes a query that returns no result
        /// </summary>
        /// <param name="sql">The sql string to execute.</param>
        public void NonQuery(string sql)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql)); 

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a command that returns a result set.
        /// </summary>
        /// <param name="sql">The sql string to execute.</param>
        /// <returns>A dat reader object</returns>
        public SQLiteDataReader Query(string sql)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Executes a command that returns a single result.
        /// </summary>
        /// <param name="sql">The sql string to execute.</param>
        /// <returns>The return value.</returns>
        public object Scalar(string sql)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                return command.ExecuteScalar();
            }
        }
        #endregion
    }
}
