using System.Linq;
using System.Collections.Generic;

public static class IEnumerableExtensions
{
    #region Methods

    /// <summary>
    /// Determines whether the collection is null or contains no elements.
    /// </summary>
    /// <typeparam name="T">The IEnumerable type.</typeparam>
    /// <param name="enumerable">The enumerable, which may be null or empty.</param>
    /// <returns>
    ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            return true;
        }

        /* If this is a list, use the Count property for efficiency. 
         * The Count property is O(1) while IEnumerable.Count() is O(N). */
        ICollection<T> collection = enumerable as ICollection<T>;
        if (collection != null)
        {
            return collection.Count < 1;
        }

        return !enumerable.Any();
    }

    /// <summary>
    /// Finds an item with the given name in the source list.
    /// </summary>
    /// <typeparam name="T">The INamed type.</typeparam>
    /// <param name="sourceList">A list of items that support the EntityName parameter.</param>
    /// <param name="nameToFind">The name of the item we want to find.</param>
    /// <returns>
    ///     <c>An item</c> if found; otherwise a <c>default object of the given type.</c>
    /// </returns>
    public static T FindItemByName<T>(this IList<T> sourceList, string nameToFind)
        where T : INamed
    {
        T result = default(T);

        for(int i = 0; i < sourceList.Count; i++)
        {
            T current = sourceList[i];
            if (current.EntityName != nameToFind)
                continue;

            result = current;
            break;
        }

        return result;
    }

    /// <summary>
    /// Does a list contain all values of another list?
    /// </summary>
    /// <remarks>Needs .NET 3.5 or greater.  Source:  http://stackoverflow.com/a/1520664/1037948 </remarks>
    /// <typeparam name="T">list value type</typeparam>
    /// <param name="containingList">the larger list we're checking in</param>
    /// <param name="lookupList">the list to look for in the containing list</param>
    /// <returns>true if it has everything</returns>
    public static bool ContainsAll<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
    {
        return !lookupList.Except(containingList).Any();
    }

    #endregion Methods
}
