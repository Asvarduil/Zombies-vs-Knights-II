using System;
using System.Collections.Generic;

public static class ICloneableExtensions
{
    /// <summary>
    /// Performs a deep-clone of a source, cloneable object.
    /// </summary>
    /// <typeparam name="T">Type of the object ot clone; must implement ICloneable.</typeparam>
    /// <param name="source">ICloneable source object</param>
    /// <returns>A deep-copy of the source object.</returns>
    public static T DeepCopy<T>(this T source)
        where T : ICloneable
    {
        return (T)source.Clone();
    }

    /// <summary>
    /// Clones a list of cloneable items into a cloned list.
    /// </summary>
    /// <typeparam name="T">The ICloneable type of the list.</typeparam>
    /// <param name="sourceList">The list of items to clone.</param>
    /// <returns>
    ///     A <c>List</c> of items of type T, that is a deep copy of the source list.
    /// </returns>
    public static List<T> DeepCopyList<T>(this IList<T> sourceList)
        where T : ICloneable
    {
        List<T> clonedList = new List<T>();

        for (int i = 0; i < sourceList.Count; i++)
        {
            T clonedItem = (T)sourceList[i].Clone();
            clonedList.Add(clonedItem);
        }

        return clonedList;
    }
}
