namespace Restless.Tools.Mvvm.Collections
{
    /// <summary>
    /// Represents a <see cref="SelectableItemList{T}"/> of <see cref="GeneralOption"/> objects.
    /// </summary>
    public class GeneralOptionList : SelectableItemList<GeneralOption>
    {
        /// <summary>
        /// Gets a boolean value that indicates the collection contains a <see cref="GeneralOption"/> object
        /// with its <see cref="GeneralOption.IntValue"/> property set to <paramref name="intValue"/>.
        /// </summary>
        /// <param name="intValue">The value to check.</param>
        /// <returns>true or false.</returns>
        public bool Contains(long intValue)
        {
            foreach (GeneralOption op in this)
            {
                if (op.IntValue == intValue)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
