# Binomial Heap

### 1. Introduction
A Binomial Heap is a priority queue data structure similar to a binary heap but supporting fast merging of two heaps. It is structured as a collection of binomial trees of distinct sizes, satisfying the heap property where the key of any node is greater than or equal to the key of its parent.

### 2. Usage
```csharp
// Create a binomial heap
var heap = new BinomialHeap<int, string>();

// Insert elements
heap.Insert(10, "Ten");
heap.Insert(5, "Five");
heap.Insert(20, "Twenty");

// Get minimum element
var minNode = heap.GetMinimum();
Console.WriteLine($"Min: {minNode.Key} - {minNode.Value}"); // Output: Min: 5 - Five

// Extract minimum element
var extracted = heap.ExtractMinimum();
Console.WriteLine($"Extracted: {extracted.Key}"); // Output: Extracted: 5

// Decrease key
var nodeToDecrease = heap.GetMinimum(); // Node with key 10
heap.DecreaseKey(nodeToDecrease, 2);
Console.WriteLine($"New Min: {heap.GetMinimum().Key}"); // Output: New Min: 2

// Delete node
heap.Delete(nodeToDecrease);
```

### 3. Detailed Explanation
- **Binomial Tree**: A binomial tree $B_k$ of order $k$ has $2^k$ nodes, height $k$, and exactly $\binom{k}{i}$ nodes at depth $i$.
- **Heap Structure**: The heap is represented as a singly linked list of binomial tree roots, ordered by degree. Each node maintains pointers to its parent, leftmost child, and immediate right sibling.
- **Union**: Merges two heaps by first merging their root lists in sorted order of degree, then traversing the list to link trees of equal degree. Linking makes the tree with the larger root key a child of the tree with the smaller root key.
- **Decrease Key & Delete**: `DecreaseKey` bubbles the key and value up to the parent until the heap property is restored. `Delete` bubbles the target node to the root of its tree, removes it from the root list, and merges its children back into the heap.

### 4. Complexity Analysis
- **Insert**: $O(\log n)$ amortized $O(1)$
- **GetMinimum**: $O(\log n)$
- **ExtractMinimum**: $O(\log n)$
- **Union**: $O(\log n)$
- **DecreaseKey**: $O(\log n)$
- **Delete**: $O(\log n)$
- **Space Complexity**: $O(n)$