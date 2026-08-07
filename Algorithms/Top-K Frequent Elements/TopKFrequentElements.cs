using System;
using System.Collections.Generic;
using System.Linq;

public class TopKFrequentElements
{
    public int[] TopKFrequent(int[] nums, int k)
    {
        var frequencyMap = new Dictionary<int, int>();
        foreach (var num in nums)
        {
            if (frequencyMap.ContainsKey(num))
            {
                frequencyMap[num]++;
            }
            else
            {
                frequencyMap[num] = 1;
            }
        }
        var priorityQueue = new SortedSet<(int frequency, int num)>(Comparer<(int frequency, int num)>.Create((x, y) => y.frequency.CompareTo(x.frequency)));
        foreach (var pair in frequencyMap)
        {
            priorityQueue.Add((pair.Value, pair.Key));
        }
        var result = new int[k];
        for (int i = 0; i < k; i++)
        {
            result[i] = priorityQueue.Min.num;
            priorityQueue.Remove(priorityQueue.Min);
        }
        return result;
    }
}