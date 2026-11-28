using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides a static method to merge K sorted lists into a single sorted list
/// using a min-heap for optimal performance.
/// </summary>
public static class KWayMerge
{
    /// <summary>
    /// Represents a node in the min-heap, holding an element's value along with
    /// its origin list index and its index within that list.
    /// </summary>
    /// <typeparam name="T">The type of the element, which must be comparable.</typeparam>
    private class HeapNode<T> : IComparable<HeapNode<T>> where T : IComparable<T>
    {
        public T Value { get; }
        public int ListIndex { get; } // The index of the list this value came from
        public int ElementIndex { get; } // The index of this value within its list

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapNode{T}"/> class.
        /// </summary>
        /// <param name="value">The actual value of the element.</param>
        /// <param name="listIndex">The zero-based index of the source list.</param>
        /// <param name="elementIndex">The zero-based index of the element within its source list.</param>
        public HeapNode(T value, int listIndex, int elementIndex)
        {
            Value = value;
            ListIndex = listIndex;
            ElementIndex = elementIndex;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns
        /// an integer that indicates whether the current instance precedes, follows, or
        /// occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(HeapNode<T> other)
        {   
            if (other == null) return 1; // Any non-null object is greater than null
            return Value.CompareTo(other.Value);
        }
    }

    /// <summary>
    /// Implements a generic min-heap data structure.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the heap, which must be comparable.</typeparam>
    private class MinHeap<T> where T : IComparable<T>
    {
        private List<T> _heap; // The underlying list storing heap elements

        /// <summary>
        /// Initializes a new instance of the <see cref="MinHeap{T}"/> class.
        /// </summary>
        public MinHeap()
        {   
            _heap = new List<T>();
        }

        /// <summary>
        /// Gets the number of elements contained in the heap.
        /// </summary>
        public int Count => _heap.Count;

        /// <summary>
        /// Adds an item to the min-heap, maintaining the heap property.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {   
            _heap.Add(item);
            HeapifyUp(_heap.Count - 1);
        }

        /// <summary>
        /// Removes and returns the minimum item from the min-heap, maintaining the heap property.
        /// </summary>
        /// <returns>The minimum item in the heap.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the heap is empty.</exception>
        public T ExtractMin()
        {   
            if (_heap.Count == 0)
            {   
                throw new InvalidOperationException("Heap is empty.");
            }

            T minItem = _heap[0];
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex]; // Move last element to root
            _heap.RemoveAt(lastIndex);   // Remove last element

            if (_heap.Count > 0)
            {   
                HeapifyDown(0); // Restore heap property from root
            }

            return minItem;
        }

        /// <summary>
        /// Restores the heap property by moving an element up the heap.
        /// </summary>
        /// <param name="index">The index of the element to heapify up.</param>
        private void HeapifyUp(int index)
        {   
            int parentIndex = (index - 1) / 2;
            while (index > 0 && _heap[index].CompareTo(_heap[parentIndex]) < 0)
            {   
                Swap(index, parentIndex);
                index = parentIndex;
                parentIndex = (index - 1) / 2;
            }
        }

        /// <summary>
        /// Restores the heap property by moving an element down the heap.
        /// </summary>
        /// <param name="index">The index of the element to heapify down.</param>
        private void HeapifyDown(int index)
        {   
            int leftChildIndex;
            int rightChildIndex;
            int smallestChildIndex;

            while (true)
            {   
                leftChildIndex = 2 * index + 1;
                rightChildIndex = 2 * index + 2;
                smallestChildIndex = index;

                // Find the smallest among parent, left child, and right child
                if (leftChildIndex < _heap.Count && _heap[leftChildIndex].CompareTo(_heap[smallestChildIndex]) < 0)
                {   
                    smallestChildIndex = leftChildIndex;
                }

                if (rightChildIndex < _heap.Count && _heap[rightChildIndex].CompareTo(_heap[smallestChildIndex]) < 0)
                {   
                    smallestChildIndex = rightChildIndex;
                }

                // If the smallest is not the current node, swap and continue heapifying down
                if (smallestChildIndex != index)
                {   
                    Swap(index, smallestChildIndex);
                    index = smallestChildIndex;
                }
                else
                {   
                    break; // Heap property restored
                }
            }
        }

        /// <summary>
        /// Swaps two elements in the heap's underlying list.
        /// </summary>
        /// <param name="i">The index of the first element.</param>
        /// <param name="j">The index of the second element.</param>
        private void Swap(int i, int j)
        {   
            T temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }
    }

    /// <summary>
    /// Merges K sorted lists into a single sorted list using a min-heap.
    /// This algorithm efficiently combines elements from multiple already sorted lists
    /// into one comprehensive sorted list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the lists, which must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="sortedLists">An enumerable collection of sorted lists to be merged.
    /// Each inner list must be sorted in ascending order. Null lists or empty lists are handled gracefully.</param>
    /// <returns>A new sorted list containing all elements from the input lists.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sortedLists"/> is null.</exception>
    public static List<T> Merge<T>(IEnumerable<IList<T>> sortedLists) where T : IComparable<T>
    {
        if (sortedLists == null)
        {   
            throw new ArgumentNullException(nameof(sortedLists), "Input collection of sorted lists cannot be null.");
        }

        List<T> result = new List<T>();
        MinHeap<HeapNode<T>> minHeap = new MinHeap<HeapNode<T>>();

        // Convert the input IEnumerable to a List for O(1) indexed access later.
        // This creates a list of references to the original lists, not copies of the lists themselves.
        List<IList<T>> actualLists = sortedLists.ToList();

        // Initialize the heap with the first element from each non-empty list.
        // Each element is wrapped in a HeapNode to track its origin.
        for (int listIndex = 0; listIndex < actualLists.Count; listIndex++)
        {   
            IList<T> list = actualLists[listIndex];
            if (list != null && list.Count > 0)
            {   
                minHeap.Add(new HeapNode<T>(list[0], listIndex, 0));
            }
        }

        // Continuously extract the minimum element from the heap and add the next element
        // from its source list until the heap is empty.
        while (minHeap.Count > 0)
        {   
            HeapNode<T> minNode = minHeap.ExtractMin();
            result.Add(minNode.Value);

            int nextElementIndex = minNode.ElementIndex + 1;
            // Check if there are more elements in the list from which minNode came
            if (nextElementIndex < actualLists[minNode.ListIndex].Count)
            {   
                // Add the next element from that list to the heap
                minHeap.Add(new HeapNode<T>(
                    actualLists[minNode.ListIndex][nextElementIndex],
                    minNode.ListIndex,
                    nextElementIndex
                ));
            }
        }

        return result;
    }
}