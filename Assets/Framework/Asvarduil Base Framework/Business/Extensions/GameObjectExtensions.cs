using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    #region Extension Methods

    public static T GetComponentInChildrenOnly<T>(this GameObject parent)
        where T : MonoBehaviour
    {
        T[] components = parent.GetComponentsInChildrenOnly<T>();
        if (components.IsNullOrEmpty())
            return null;

        return components[0];
    }

    public static T[] GetComponentsInChildrenOnly<T>(this GameObject parent)
        where T : MonoBehaviour
    {
        List<T> results = new List<T>();

        T[] components = parent.GetComponentsInChildren<T>();
        for(int i = 0; i < components.Length; i++)
        {
            T current = components[i];
            if (current.gameObject == parent)
                continue;

            results.Add(current);
        }

        return results.ToArray();
    }

    #endregion Extension Methods
}
