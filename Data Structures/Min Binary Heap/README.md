# Min Binary Heap

## 1. Introduction

A Min Binary Heap is a complete binary tree where the value of each parent node is less than or equal to the values of its child nodes. This property makes the root node the smallest element in the entire heap, allowing efficient retrieval of the minimum element. Min Binary Heaps are commonly used to implement priority queues, which are essential in many algorithms such as Dijkstra's shortest path, Huffman encoding, and task scheduling.

This implementation uses a dynamic array (`List<int>`) to store the heap, where the tree structure is implicitly represented.

## 2. Usage

Insert elements using the `Insert` method and retrieve the minimum element using the `ExtractMin` method. The current minimum can be checked using the `Peek` method without removing it. The `Count` property returns the number of elements in the heap.

Example:

var heap = new MinBinaryHeap();
heap.Insert(10);
heap.Insert(5);
heap.Insert(7);

int min = heap.ExtractMin();  // min is 5
int peek = heap.Peek();       // peek is 7
int count = heap.Count;       // count is 2

## 3. Detailed Explanation

- **Internal Storage:** The heap uses a `List<int>` to represent the binary tree. The parent-child relationships are determined by indices:
  - Parent index: (childIndex - 1) / 2
  - Left child index: 2 * parentIndex + 1
  - Right child index: 2 * parentIndex + 2

- **Insert:** 
  1. Add the new element to the end of the list.
  2. "Sift up" the element by comparing it with its parent and swapping if less, until the heap property is restored.

- **ExtractMin:**
  1. Store the root value (minimum) to return later.
  2. Replace root with the last element in the list and remove the last element.
  3. "Sift down" this element by swapping it with the smaller of its children until the min-heap property is restored or it becomes a leaf.

- **Peek:**
  Simply return the root element (`heap[0]`) which is the minimum.

- **Count:**
  Returns the current number of elements in the heap.

- **Helper Methods:**
  - `SiftUp` and `SiftDown` maintain the heap property efficiently after insertions and deletions.
  - `Swap` exchanges elements at two positions in the list.

## 4. Complexity Analysis

| Operation    | Time Complexity | Space Complexity |
|--------------|-----------------|------------------|
| Insert       | O(log n)        | O(1) (ignoring list resizing)          |
| ExtractMin   | O(log n)        | O(1)             |
| Peek         | O(1)            | O(1)             |
| Count        | O(1)            | O(1)             |

- **Explanation:** Both insertion and extraction require logarithmic time due to the sift operations that navigate the height of the tree (~log n). Peek and Count are constant-time operations since they simply access the root element or return the list size without modification.

This implementation provides an efficient and reusable min-heap structure tailored for integer values, suitable as a base for many algorithmic applications.