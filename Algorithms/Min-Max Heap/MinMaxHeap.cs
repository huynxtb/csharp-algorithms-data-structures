using System;
using System.Collections.Generic;

public class MinMaxHeap<T> where T : IComparable<T>
{
    private List<T> heap;

    public MinMaxHeap()
    {
        heap = new List<T>();
    }

    public void Insert(T item)
    {
        heap.Add(item);
        BubbleUp(heap.Count - 1);
    }

    public T ExtractMin()
    {
        if (IsEmpty) throw new InvalidOperationException("Heap is empty.");
        T min = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        if (!IsEmpty) PushDown(0);
        return min;
    }

    public T ExtractMax()
    {
        if (IsEmpty) throw new InvalidOperationException("Heap is empty.");
        T max;
        if (heap.Count == 1)
        {
            max = heap[0];
            heap.RemoveAt(0);
            return max;
        }
        else if (heap.Count == 2)
        {
            max = heap[1];
            heap.RemoveAt(1);
            return max;
        }
        else
        {
            max = heap[1];
            heap[1] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            PushDown(1);
            return max;
        }
    }

    public T PeekMin()
    {
        if (IsEmpty) throw new InvalidOperationException("Heap is empty.");
        return heap[0];
    }

    public T PeekMax()
    {
        if (IsEmpty) throw new InvalidOperationException("Heap is empty.");
        return heap.Count > 1 ? heap[1] : heap[0];
    }

    public int Count => heap.Count;

    public bool IsEmpty => heap.Count == 0;

    private int GetParentIndex(int index) => (index - 1) / 2;
    private int GetLeftChildIndex(int index) => 2 * index + 1;
    private int GetRightChildIndex(int index) => 2 * index + 2;
    private int GetGrandparentIndex(int index) => (index - 3) / 4;
    private int GetLevel(int index) => (int)Math.Floor(Math.Log2(index + 1));

    private void Swap(int index1, int index2)
    {
        T temp = heap[index1];
        heap[index1] = heap[index2];
        heap[index2] = temp;
    }

    private void BubbleUp(int index)
    {
        if (index == 0) return;
        int parentIndex = GetParentIndex(index);
        if (GetLevel(index) % 2 == 0) // Even level (min level)
        {
            if (heap[index].CompareTo(heap[parentIndex]) < 0)
            {
                Swap(index, parentIndex);
                BubbleUp(parentIndex);
            }
        }
        else // Odd level (max level)
        {
            if (heap[index].CompareTo(heap[parentIndex]) > 0)
            {
                Swap(index, parentIndex);
                BubbleUp(parentIndex);
            }
        }
    }

    private void PushDown(int index)
    {
        if (GetLevel(index) % 2 == 0) // Even level (min level)
        {
            int minChildIndex = MinChildGrandchild(index);
            if (minChildIndex != -1 && heap[minChildIndex].CompareTo(heap[index]) < 0)
            {
                Swap(index, minChildIndex);
                PushDown(minChildIndex);
            }
        }
        else // Odd level (max level)
        {
            int maxChildIndex = MaxChildGrandchild(index);
            if (maxChildIndex != -1 && heap[maxChildIndex].CompareTo(heap[index]) > 0)
            {
                Swap(index, maxChildIndex);
                PushDown(maxChildIndex);
            }
        }
    }

    private int MinChildGrandchild(int index)
    {
        int leftChild = GetLeftChildIndex(index);
        int rightChild = GetRightChildIndex(index);
        int minIndex = -1;
        if (leftChild < heap.Count)
        {
            minIndex = leftChild;
            int leftGrandchild = GetLeftChildIndex(leftChild);
            int rightGrandchild = GetRightChildIndex(leftChild);
            if (leftGrandchild < heap.Count && heap[leftGrandchild].CompareTo(heap[minIndex]) < 0)
                minIndex = leftGrandchild;
            if (rightGrandchild < heap.Count && heap[rightGrandchild].CompareTo(heap[minIndex]) < 0)
                minIndex = rightGrandchild;
        }
        if (rightChild < heap.Count)
        {
            if (minIndex == -1 || heap[rightChild].CompareTo(heap[minIndex]) < 0)
                minIndex = rightChild;
            int leftGrandchild = GetLeftChildIndex(rightChild);
            int rightGrandchild = GetRightChildIndex(rightChild);
            if (leftGrandchild < heap.Count && heap[leftGrandchild].CompareTo(heap[minIndex]) < 0)
                minIndex = leftGrandchild;
            if (rightGrandchild < heap.Count && heap[rightGrandchild].CompareTo(heap[minIndex]) < 0)
                minIndex = rightGrandchild;
        }
        return minIndex;
    }

    private int MaxChildGrandchild(int index)
    {
        int leftChild = GetLeftChildIndex(index);
        int rightChild = GetRightChildIndex(index);
        int maxIndex = -1;
        if (leftChild < heap.Count)
        {
            maxIndex = leftChild;
            int leftGrandchild = GetLeftChildIndex(leftChild);
            int rightGrandchild = GetRightChildIndex(leftChild);
            if (leftGrandchild < heap.Count && heap[leftGrandchild].CompareTo(heap[maxIndex]) > 0)
                maxIndex = leftGrandchild;
            if (rightGrandchild < heap.Count && heap[rightGrandchild].CompareTo(heap[maxIndex]) > 0)
                maxIndex = rightGrandchild;
        }
        if (rightChild < heap.Count)
        {
            if (maxIndex == -1 || heap[rightChild].CompareTo(heap[maxIndex]) > 0)
                maxIndex = rightChild;
            int leftGrandchild = GetLeftChildIndex(rightChild);
            int rightGrandchild = GetRightChildIndex(rightChild);
            if (leftGrandchild < heap.Count && heap[leftGrandchild].CompareTo(heap[maxIndex]) > 0)
                maxIndex = leftGrandchild;
            if (rightGrandchild < heap.Count && heap[rightGrandchild].CompareTo(heap[maxIndex]) > 0)
                maxIndex = rightGrandchild;
        }
        return maxIndex;
    }
}