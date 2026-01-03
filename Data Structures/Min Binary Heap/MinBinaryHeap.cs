using System.Collections.Generic;

public class MinBinaryHeap
{
    private List<int> heap;

    public MinBinaryHeap()
    {
        heap = new List<int>();
    }

    public int Count => heap.Count;

    public void Insert(int element)
    {
        heap.Add(element);
        SiftUp(heap.Count - 1);
    }

    public int ExtractMin()
    {
        if (heap.Count == 0)
            throw new System.InvalidOperationException("Heap is empty.");

        int min = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        if (heap.Count > 0)
            SiftDown(0);

        return min;
    }

    public int Peek()
    {
        if (heap.Count == 0)
            throw new System.InvalidOperationException("Heap is empty.");

        return heap[0];
    }

    private void SiftUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (heap[parentIndex] <= heap[index])
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void SiftDown(int index)
    {
        int lastIndex = heap.Count - 1;
        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int smallestIndex = index;

            if (leftChildIndex <= lastIndex && heap[leftChildIndex] < heap[smallestIndex])
                smallestIndex = leftChildIndex;

            if (rightChildIndex <= lastIndex && heap[rightChildIndex] < heap[smallestIndex])
                smallestIndex = rightChildIndex;

            if (smallestIndex == index)
                break;

            Swap(index, smallestIndex);
            index = smallestIndex;
        }
    }

    private void Swap(int i, int j)
    {
        int temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}