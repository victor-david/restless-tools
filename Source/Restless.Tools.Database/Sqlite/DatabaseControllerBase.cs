using Restless.Tools.Database.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents the base class for a SQLite database controller. This class must be inherited.
    /// </summary>
    public abstract class DatabaseControllerBase
    {
        #region Private
        private const string DataSetName = "Main";
        private Dictionary<string, string> attached;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Defines the name of the main schema.
        /// This is the schema name used by the main database, that is:
        /// the first database associated with a connection. This name is defined by Sqlite.
        /// </summary>
        public const string MainSchemaName = "main";

        /// <summary>
        /// Defines the name of the temporary schema. This is the schema name
        /// defined by Sqlite for temporary tables.
        /// </summary>
        public const string TempSchemaName = "temp";

        /// <summary>
        /// Defines the name of the database that may be used to create an in-memory database.
        /// An in-memory database behaves like a disk-based database, but goes away when the connection is closed.
        /// </summary>
        public const string MemoryDatabase = ":memory:";
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the DataSet object for this controller
        /// </summary>
        public DataSet DataSet
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the full path of the database file.
        /// </summary>
        public string DatabaseFileName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a boolean value that indicates if the database file exists.
        /// </summary>
        public bool DatabaseExists
        {
            get => (!string.IsNullOrEmpty(DatabaseFileName) && File.Exists(DatabaseFileName));
        }

        /// <summary>
        /// Gets the connection object.
        /// This property is not available until the <see cref="CreateAndOpen(string)"/> method has been called.
        /// </summary>
        public SQLiteConnection Connection
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the execution object.
        /// This property is not available until the <see cref="CreateAndOpen(string)"/> method has been called.
        /// </summary>
        public ExecuteObject Execution
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the transaction adapeter for this controller.
        /// This property is not available until the <see cref="CreateAndOpen(string)"/> method has been called.
        /// </summary>
        public TransactionAdapter Transaction
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a boolean value that indicates if a transaction is currently active on <see cref="Connection"/>.
        /// </summary>
        public bool IsTransactionActive
        {
            get => Connection != null && !Connection.AutoCommit;
        }

        /// <summary>
        /// Gets a value that indicates if the database controller has been initialized.
        /// This property is set to true when the <see cref="CreateAndOpen(string)"/> method is called.
        /// </summary>
        public bool Initialized
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Protected properties
        #endregion

        /************************************************************************/

        #region Constructor (protected)
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseControllerBase"/> class.
        /// </summary>
        protected DatabaseControllerBase()
        {
            Initialized = false;
            DataSet = new DataSet(DataSetName);
            attached = new Dictionary<string, string>();
        }
        #endregion

        /************************************************************************/

        #region Public Methods
        /// <summary>
        /// Calls the Save() method of each table contained in the set of tables
        /// </summary>
        public void Save()
        {
            foreach (var table in DataSet.Tables.OfType<TableBase>())
            {
                table.Save();
            }
        }

        /// <summary>
        /// Gets a value that indicates if any table in the set of tables is dirty.
        /// </summary>
        /// <returns>true if a table is dirty; otherwise, false.</returns>
        public bool IsDirty()
        {
            foreach (var table in DataSet.Tables.OfType<TableBase>())
            {
                if (table.IsDirty()) return true;
            }
            return false;
        }

        /// <summary>
        /// Shutdowns the database connection.
        /// </summary>
        /// <param name="saveTables">true to save all tables</param>
        /// <remarks>
        /// <para>
        /// This method first calls the OnShuttingDown() method for each table. 
        /// If <paramref name="saveTables"/> is true, it then calls the Save() method
        /// to save all tables. If you pass <paramref name="saveTables"/> as false, it is
        /// the responsibility of each table to save or not, which they choose during
        /// OnShuttingDown().
        /// </para>
        /// <para>
        /// When this method finishes, the database connection is closed 
        /// and no more database operations are allowed.
        /// </para>
        /// </remarks>
        public void Shutdown(bool saveTables)
        {
            if (Initialized && Connection != null)
            {
                foreach (var table in DataSet.Tables.OfType<TableBase>())
                {
                    table.OnShuttingDown(saveTables);
                }
                if (saveTables) Save();
                Connection.Close();
                Connection = null;
                Execution = null;
                Transaction = null;
                DatabaseFileName = null;
                Initialized = false;
                foreach (var table in DataSet.Tables.OfType<TableBase>())
                {
                    table.Constraints.Clear();
                }
                DataSet.Relations.Clear();
                DataSet.Tables.Clear();
            }
        }

        /// <summary>
        /// Gets the table as specified by its type
        /// </summary>
        /// <typeparam name="T">The type of the table</typeparam>
        /// <returns>The table object</returns>
        public T GetTable<T>() where T : TableBase
        {
            try
            {
                var table = DataSet.Tables.OfType<T>().First();
                return table;
            }
            catch
            {
                throw new InvalidOperationException(Strings.InvalidOperationDataTableNotRegistered);
            }
        }

        /// <summary>
        /// Gets a boolean value that indicates if the specified schema is currently attached.
        /// </summary>
        /// <param name="schemaName">The name of the schema</param>
        /// <returns>true if <paramref name="schemaName"/> is currently attached; otherwise, false.</returns>
        public bool IsAttached(string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName)) throw new ArgumentNullException(nameof(schemaName));
            return attached.ContainsKey(schemaName);
        }

        /// <summary>
        /// Gets the name of the database file that is currently attached via the specified schema name.
        /// </summary>
        /// <param name="schemaName">The schema name.</param>
        /// <returns>The name of the database file associated with <paramref name="schemaName"/> or null if the schema isn't attached.</returns>
        public string GetAttachedFileName(string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName)) throw new ArgumentNullException(nameof(schemaName));
            if (attached.ContainsKey(schemaName))
            {
                return attached[schemaName];
            }
            return null;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Creates the database file if needed and initializes the database connection
        /// </summary>
        /// <param name="databaseFileName">The full path and file name of the database file</param>
        protected void CreateAndOpen(string databaseFileName)
        {
            if (!Initialized)
            {
                if (string.IsNullOrEmpty(databaseFileName)) throw new ArgumentNullException(nameof(databaseFileName));
                DatabaseFileName = databaseFileName;
                PrepareDatabaseFileAsNeeded(databaseFileName);
                Connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", databaseFileName));
                Connection.Open();
                Execution = new ExecuteObject(Connection);
                Transaction = new TransactionAdapter(Connection);
                Initialized = true;
            }
        }

        /// <summary>
        /// Attaches the specified database file using the specified schema name if the schema is not already attached.
        /// </summary>
        /// <param name="schemaName">The name of the schema</param>
        /// <param name="databaseFileName">The full path and file name of the database file</param>
        /// <param name="initAction">A callback method to initialize the attached schema; create tables, etc.</param>
        /// <remarks>
        /// If <paramref name="schemaName"/> is already attached, this method does nothing.
        /// </remarks>
        protected void Attach(string schemaName, string databaseFileName, Action initAction)
        {
            if (string.IsNullOrEmpty(schemaName)) throw new ArgumentNullException(nameof(schemaName));
            if (string.IsNullOrEmpty(databaseFileName)) throw new ArgumentNullException(nameof(databaseFileName));

            if (Initialized && Connection != null && !IsAttached(schemaName))
            {
                PrepareDatabaseFileAsNeeded(databaseFileName);
                Execution.NonQuery($"ATTACH DATABASE \"{databaseFileName}\" AS {schemaName}");
                initAction();
                attached.Add(schemaName, databaseFileName);
            }
        }

        /// <summary>
        /// Detaches the specified schema if it is attached.
        /// </summary>
        /// <param name="schemaName">The name of the schema.</param>
        /// <remarks>
        /// If <paramref name="schemaName"/> is not attached, this method does nothing.
        /// </remarks>
        protected void Detach(string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName)) throw new ArgumentNullException(nameof(schemaName));

            if (Initialized && Connection != null && IsAttached(schemaName))
            {
                List<TableBase> schemaTables = DataSet.Tables.OfType<TableBase>().Where((table) => table.Namespace == schemaName).ToList();
                foreach (var table in schemaTables)
                {
                    table.Save();
                    table.Constraints.Clear();
                }

                foreach (var relation in DataSet.Relations.OfType<DataRelation>().Where(r => r.ChildTable.Namespace == schemaName || r.ParentTable.Namespace == schemaName).ToList())
                {
                    DataSet.Relations.Remove(relation);
                }

                foreach (var table in schemaTables)
                {
                    DataSet.Tables.Remove(table);
                }

                Execution.NonQuery($"DETACH DATABASE {schemaName}");
                attached.Remove(schemaName);
            }
        }

        /// <summary>
        /// Instantiates the specified table object and adds it to the Tables collection.
        /// </summary>
        /// <typeparam name="T">The type derived from TableBase</typeparam>
        /// <returns>The table object</returns>
        /// <remarks>
        /// This method attempts to create the table structure within the database.
        /// </remarks>
        protected T CreateAndRegisterTable<T>() where T : TableBase, new()
        {
            var table = new T();

            if (!table.Exists())
            {
                table.CreateFromDdl();
            }

            if (table.Exists() && !table.HasRows())
            {
                table.Populate();
            }

            if (table.Exists())
            {
                table.Load();
            }

            DataSet.Tables.Add(table);
            return table;
        }

        /// <summary>
        /// Enables a derived class to signal that table registration is complete, allowing the base controller to begin table post registration operations.
        /// </summary>
        /// <param name="schemaName">The schema name. Only tables in this namespace will be processed.</param>
        protected void TableRegistrationComplete(string schemaName)
        {
            List<TableBase> failed = new List<TableBase>();

            IEnumerable<TableBase> tablesInSchema = DataSet.Tables.OfType<TableBase>().Where((table)=> table.Namespace == schemaName);

            foreach (var table in tablesInSchema)
            {
                table.SetDataRelations();
            }

            /* This enables us to retry any table that failed to use data relations
             * A table can fail to use relations because of a dependency on another table 
             * that needs to create a column first. This only happens if Table A
             * wants to create a column to Table B that Table B is creating to Table C.
             * This removes the need to put tables in the DataSet in a certain order.
             * Without this, we'd need to put Table B in the set before Table A.
             */
            foreach (var table in tablesInSchema)
            {
                try
                {
                    table.UseDataRelations();
                }
                catch
                {
                    failed.Add(table);
                }
            }

            foreach (var table in failed)
            {
                table.UseDataRelations();
            }


            foreach (var table in tablesInSchema)
            {
                table.OnInitializationComplete();
            }
        }

        /// <summary>
        /// Enables a derived class to signal that table registration is complete, allowing the base controller to begin table post registration operations.
        /// This overload uses <see cref="MainSchemaName"/>.
        /// </summary>
        protected void TableRegistrationComplete()
        {
            TableRegistrationComplete(MainSchemaName);
        }

        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Prepares for a database file. If the file doesn't exist, creates
        /// both the directory to the file and the file itself. When this method
        /// returns, the specified file exists, although it will be zero bytes
        /// if it wasn't there before.
        /// </summary>
        /// <param name="databaseFileName"></param>
        private void PrepareDatabaseFileAsNeeded(string databaseFileName)
        {
            if (databaseFileName != MemoryDatabase)
            {
                if (!File.Exists(databaseFileName))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(databaseFileName));
                    SQLiteConnection.CreateFile(databaseFileName);
                }
            }
        }
        #endregion
    }
}
