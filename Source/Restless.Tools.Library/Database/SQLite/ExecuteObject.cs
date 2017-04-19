using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using Restless.Tools.Utility;

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
            Validations.ValidateNull(connection, "ExecuteObject.Connection");
            this.connection = connection;
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
            Validations.ValidateNullEmpty(sql, "NonQuery.Sql");
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
            Validations.ValidateNullEmpty(sql, "Query.Sql");
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
            Validations.ValidateNullEmpty(sql, "Scalar.Sql");
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                return command.ExecuteScalar();
            }
        }
        #endregion
    }
}
