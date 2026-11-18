using System;
using System.Collections.Generic;

public class JumpTable
{
    private List<int> elements;
    private int[] jumpIndices;
    private int jumpSize;

    // Constructor to initialize the Jump Table with a sorted array.
    public JumpTable(List<int> sortedElements)
    {
        elements = sortedElements;
        jumpSize = (int)Math.Sqrt(elements.Count);
        InitializeJumpIndices();
    }

    // Method to initialize the jump indices based on the jump size.
    private void InitializeJumpIndices()
    {
        int jumpCount = (elements.Count + jumpSize - 1) / jumpSize;
        jumpIndices = new int[jumpCount];
        for (int i = 0; i < jumpCount; i++)
        {
            jumpIndices[i] = elements[i * jumpSize];
        }
    }

    // Method to search for an element in the Jump Table.
    public int Search(int target)
    {
        int n = elements.Count;
        int i = 0;

        // Jump through the jump indices to find the block.
        while (i < jumpIndices.Length && target > jumpIndices[i])
        {
            i++;
        }

        // If we have gone past the last jump index, search the last block.
        if (i > 0)
        {
            i--;
        }

        // Perform a linear search within the identified block.
        int start = i * jumpSize;
        int end = Math.Min(start + jumpSize, n);
        for (int j = start; j < end; j++)
        {
            if (elements[j] == target)
            {
                return j; // Return the index if found.
            }
        }

        return -1; // Return -1 if the element is not found.
    }

    // Method to insert a new element into the Jump Table.
    public void Insert(int newElement)
    {
        elements.Add(newElement);
        elements.Sort(); // Keep the array sorted after insertion.
        InitializeJumpIndices(); // Reinitialize jump indices after insertion.
    }
}