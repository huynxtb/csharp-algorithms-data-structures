using System;
using System.Collections.Generic;

public class OptimalMergePattern
{
    // Class to represent an entry in the priority queue
    private class QueueEntry
    {
        public int[] Array;
        public int CurrentIndex;

        public QueueEntry(int[] array, int currentIndex)
        {
            Array = array;
            CurrentIndex = currentIndex;
        }
    }

    // Method to merge multiple sorted arrays into a single sorted array
    public int[] MergeSortedArrays(int[][] arrays)
    {
        // Priority queue to manage the merging process
        List<QueueEntry> priorityQueue = new List<QueueEntry>();

        // Initialize the priority queue with the first element of each array
        foreach (var array in arrays)
        {
            if (array.Length > 0)
            {
                priorityQueue.Add(new QueueEntry(array, 0));
            }
        }

        // Result list to store the merged output
        List<int> mergedList = new List<int>();

        // While there are elements in the priority queue
        while (priorityQueue.Count > 0)
        {
            // Find the minimum element among the current heads of the arrays
            QueueEntry minEntry = GetMinEntry(priorityQueue);
            mergedList.Add(minEntry.Array[minEntry.CurrentIndex]);

            // Move to the next index in the array from which the minimum element was taken
            minEntry.CurrentIndex++;

            // If there are more elements in that array, add the next element to the priority queue
            if (minEntry.CurrentIndex < minEntry.Array.Length)
            {
                priorityQueue.Add(minEntry);
            }
        }

        // Convert the merged list to an array and return
        return mergedList.ToArray();
    }

    // Method to get the minimum entry from the priority queue
    private QueueEntry GetMinEntry(List<QueueEntry> queue)
    {
        // Assume the first entry is the minimum
        QueueEntry minEntry = queue[0];
        int minIndex = 0;

        // Iterate through the queue to find the actual minimum
        for (int i = 1; i < queue.Count; i++)
        {
            if (queue[i].Array[queue[i].CurrentIndex] < minEntry.Array[minEntry.CurrentIndex])
            {
                minEntry = queue[i];
                minIndex = i;
            }
        }

        // Remove the minimum entry from the queue
        queue.RemoveAt(minIndex);
        return minEntry;
    }
}