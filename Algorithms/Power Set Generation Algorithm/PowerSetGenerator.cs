using System;
using System.Collections.Generic;

public static class PowerSetGenerator
{
    public static List<List<T>> GeneratePowerSet<T>(List<T> inputSet)
    {
        if (inputSet == null)
        {
            throw new ArgumentNullException(nameof(inputSet));
        }

        List<List<T>> powerSet = new List<List<T>>();
        int setCount = inputSet.Count;
        int powerSetCount = 1 << setCount; // 2^n

        for (int i = 0; i < powerSetCount; i++)
        {
            List<T> subset = new List<T>();
            for (int j = 0; j < setCount; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    subset.Add(inputSet[j]);
                }
            }
            powerSet.Add(subset);
        }

        return powerSet;
    }
}