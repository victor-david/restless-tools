using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using Restless.Tools.Utility;
using Restless.Tools.Resources;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents the base class for a SQLite database controller. This class must be inherited.
    /// </summary>
    public abstract class DatabaseControllerBase
    {
        #region Private
        private const string DataSetName = "Main";
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
            get
            {
                return (!String.IsNullOrEmpty(DatabaseFileName) && File.Exists(DatabaseFileName));
            }
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
        /// <summary>
        /// Gets or sets the flags that affect how the controller operates
        /// </summary>
        protected DatabaseControllerBehavior Behavior
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (protected)
        #pragma warning disable 1591
        protected DatabaseControllerBase()
        {
            Initialized = false;
            Behavior = DatabaseControllerBehavior.All;
            DataSet = new DataSet(DataSetName);
        }
        #pragma warning restore 1591
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
                throw new InvalidOperationException(Strings.InvalidOperation_DataTableNotRegistered);
            }
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
                Validations.ValidateNullEmpty(databaseFileName, "Init.DatabaseFileName");
                DatabaseFileName = databaseFileName;
                Directory.CreateDirectory(Path.GetDirectoryName(databaseFileName));
                CreateFile(databaseFileName);
                Connection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", databaseFileName));
                Connection.Open();
                Execution = new ExecuteObject(Connection);
                Transaction = new TransactionAdapter(Connection);
                Initialized = true;
            }
        }

        /// <summary>
        /// Instansiates the specified table object and adds it to the Tables collection.
        /// </summary>
        /// <typeparam name="T">The type derived from TableBase</typeparam>
        /// <returns>The table object</returns>
        /// <remarks>
        /// This method attempts to create the table structure within the database.
        /// </remarks>
        protected T CreateAndRegisterTable<T>()
            where T : TableBase, new()
        {
            var table = new T();

            if (!table.Exists() && Behavior.HasFlag(DatabaseControllerBehavior.AutoDdlCreation))
            {
                table.CreateFromDdl();
            }

            if (table.Exists() && !table.HasRows() && Behavior.HasFlag(DatabaseControllerBehavior.AutoPopulate))
            {
                table.Populate();
            }

            if (table.Exists() && Behavior.HasFlag(DatabaseControllerBehavior.AutoDataLoad))
            {
                table.Load();
            }

            DataSet.Tables.Add(table);
            return table;
        }

        /// <summary>
        /// Enables a derived class to signal that table registration is complete, allowing the base controller to begin table post registration operations.
        /// </summary>
        protected void TableRegistrationComplete()
        {
            List<TableBase> failed = new List<TableBase>();
            foreach (var table in DataSet.Tables.OfType<TableBase>())
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
            foreach (var table in DataSet.Tables.OfType<TableBase>())
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


            foreach (var table in DataSet.Tables.OfType<TableBase>())
            {
                table.OnInitializationComplete();
            }

        }

        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Creates the database file if it does not already exist.
        /// </summary>
        /// <param name="databaseFileName"></param>
        private void CreateFile(string databaseFileName)
        {
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);
            }
        }
        #endregion
    }
}
