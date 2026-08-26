using System;
using System.Collections.Generic;

public static class LinearSearchAlgorithm
{
    public static int LinearSearch<T>(IList<T> collection, T target)
    {
        if (collection == null)
        {
            return -1;
        }

        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < collection.Count; i++)
        {
            if (comparer.Equals(collection[i], target))
            {
                return i;
            }
        }

        return -1;
    }
}