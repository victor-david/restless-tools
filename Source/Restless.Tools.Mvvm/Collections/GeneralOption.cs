namespace Restless.Tools.Mvvm.Collections
{
    /// <summary>
    /// Represents a general purpose helper class that can be used when binding.
    /// </summary>
    public class GeneralOption : SelectableItem
    {
        #region Private
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets a string value associated with this option.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an integer value associated with this option.
        /// </summary>
        public long IntValue
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralOption"/> class.
        /// </summary>
        public GeneralOption()
        {
        }
        #endregion
    }
}
