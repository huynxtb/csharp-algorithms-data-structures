# Pairing Heap

### 1. Introduction
A Pairing Heap is a type of heap-ordered multiway tree structure. It is simplified alternative to the Fibonacci Heap, offering excellent empirical performance. It is commonly used to implement priority queues, particularly in algorithms like Dijkstra's shortest path where `DecreaseKey` operations are frequent.

### 2. Usage
```csharp
// Instantiate the heap
var heap = new PairingHeap<int>();

// Insert elements and keep node references for DecreaseKey
PairingHeapNode<int> node1 = heap.Insert(15);
PairingHeapNode<int> node2 = heap.Insert(30);
PairingHeapNode<int> node3 = heap.Insert(5);

// Find minimum element
int min = heap.FindMin(); // Returns 5

// Decrease key of a node
heap.DecreaseKey(node2, 2);
int newMin = heap.FindMin(); // Returns 2

// Delete minimum element
int deleted = heap.DeleteMin(); // Returns 2
```

### 3. Detailed Explanation
- **Node Structure**: Each `PairingHeapNode<T>` contains a `Value`, a pointer to its first `Child`, and pointers to its `Next` and `Prev` siblings. For the first child, the `Prev` pointer points back to the parent node. This allows $O(1)$ time complexity to detach a node from its parent/siblings during a `DecreaseKey` operation.
- **Merge**: Compares the root values of two heaps. The heap with the larger root becomes the leftmost child of the heap with the smaller root.
- **DeleteMin**: Removes the root node and performs a two-pass merge on its children. The first pass merges pairs of siblings from left to right. The second pass merges the resulting heaps from right to left to form the new root.
- **DecreaseKey**: Decreases the value of a node. If the node is not the root, it is detached from its parent and siblings, and then merged back into the root heap.

### 4. Complexity Analysis
- **Insert**: $O(1)$
- **FindMin**: $O(1)$
- **DeleteMin**: $O(\log n)$ amortized
- **DecreaseKey**: $O(2^{2\sqrt{\log \log n}})$ amortized (practically $O(1)$ in empirical tests)
- **Merge**: $O(1)$
- **Space Complexity**: $O(n)$ auxiliary space