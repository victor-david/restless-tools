using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Provides flags that affect the behavior of a database controller
    /// </summary>
    [Flags]
    public enum DatabaseControllerBehavior
    {
        /// <summary>
        /// No automatic operations will be used
        /// </summary>
        None = 0,

        /// <summary>
        /// At table registration, if the table doesn't exists, the schema for an added table will be automatically created within the database from the definition sql provided by the table object.
        /// </summary>
        AutoDdlCreation = 1,

        /// <summary>
        /// At table registration, if the table doesn't have any rows, data will be automatically populated from sql provided by the table object.
        /// </summary>
        AutoPopulate = 2,

        /// <summary>
        /// At table registration, existing data will be automatically loaded from the database
        /// </summary>
        AutoDataLoad = 4,

        /// <summary>
        /// All of the above options
        /// </summary>
        All = AutoDdlCreation + AutoDataLoad + AutoPopulate
        
    }
}
