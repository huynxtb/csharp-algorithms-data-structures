# Implicit Treap (Treap with Implicit Keys)

## Introduction

An **Implicit Treap** is a balanced binary search tree that uses implicit keys (array indices) instead of explicit keys. It combines two key ideas:

1. **Implicit Keys**: The position of an element is determined by the size of its left subtree, rather than storing explicit key values.
2. **Random Priorities**: Each node has a random priority, and the tree maintains a max-heap property on these priorities to ensure balance.

This data structure is ideal for implementing dynamic sequences where elements can be inserted, deleted, or modified at arbitrary positions efficiently. Common use cases include:
- Text editors with undo/redo operations
- Sequence concatenation and manipulation
- Range updates and queries on dynamic arrays
- Implement efficient rope data structures

## Usage

```csharp
using System;

// Create a new implicit treap
ImplicitTreap<int> treap = new ImplicitTreap<int>();

// Add elements
treap.AddLast(10);
treap.AddLast(20);
treap.AddLast(30);

// Insert at specific position
treap.Insert(1, 15);  // [10, 15, 20, 30]

// Get element at index
int value = treap.GetAt(2);  // Returns 20

// Set element at index
treap.SetAt(0, 5);  // [5, 15, 20, 30]

// Remove element
treap.RemoveAt(2);  // [5, 15, 30]

// Get count
int count = treap.Count;  // Returns 3

// Convert to array
int[] array = treap.ToArray();  // [5, 15, 30]

// Extract subsequence
ImplicitTreap<int> subseq = treap.SubSequence(0, 2);  // [5, 15]

// Boundary checking
try
{
    treap.GetAt(10);  // Throws ArgumentOutOfRangeException
}
catch (ArgumentOutOfRangeException ex)
{
    // Handle error
}
```

## Detailed Explanation

### Core Components

#### Node Structure
Each node contains:
- **Value**: The stored element
- **Priority**: A random integer for heap property (max-heap)
- **Size**: The number of elements in the subtree rooted at this node
- **Left/Right**: Child node references

The size field is crucial for implicit keys. At any node, the implicit key is the size of its left subtree.

#### Split Operation
The `Split(node, key, out left, out right)` operation divides a treap into two parts:
- **Left part**: Contains all elements at indices [0, key)
- **Right part**: Contains all elements at indices [key, n)

The split works recursively:
1. If the split point falls in the left subtree, recursively split the left child and reconnect
2. If the split point falls in the right subtree, recursively split the right child and reconnect
3. Update sizes after each split to maintain implicit key correctness

#### Merge Operation
The `Merge(left, right)` operation combines two treaps:
- All elements in `left` have indices less than those in `right`
- The merge compares priorities of root nodes
- The node with higher priority becomes the new root
- The other node's subtree is recursively merged with the appropriate child
- This ensures both BST (by implicit keys) and heap (by priority) properties are maintained

#### Insert Operation
To insert a value at index `i`:
1. Split the treap at position `i` → (left, right)
2. Create a new node with the value and a random priority
3. Merge: first merge the new node with right, then merge left with the result

#### Remove Operation
To remove element at index `i`:
1. Split at position `i` → (left, temp)
2. Split temp at position 1 → (removed, right)
3. Merge left and right

#### GetAt and SetAt Operations
These operations navigate the tree using implicit keys:
- Compare the target index with the left subtree size
- If index equals left size, found the target
- Otherwise, recurse to the appropriate child and adjust the index

### Balancing and Complexity

The random priorities ensure that the treap remains balanced with high probability. The probability that the height exceeds O(log n) decreases exponentially. This is why all operations have expected O(log n) time complexity.

## Complexity Analysis

### Time Complexity

| Operation | Average | Worst Case |
|-----------|---------|------------|
| Insert(index, value) | O(log n) | O(n) |
| RemoveAt(index) | O(log n) | O(n) |
| GetAt(index) | O(log n) | O(n) |
| SetAt(index, value) | O(log n) | O(n) |
| AddFirst(value) | O(log n) | O(n) |
| AddLast(value) | O(log n) | O(n) |
| Count | O(1) | O(1) |
| ToArray() | O(n) | O(n) |
| SubSequence(start, length) | O(length + log n) | O(n) |

**Notes:**
- Average case assumes random priority assignment
- Worst case occurs when all priorities are in ascending/descending order (extremely unlikely with random generation)
- Split and Merge operations are O(log n) on average
- The tree height is O(log n) with high probability

### Space Complexity

| Aspect | Complexity |
|--------|------------|
| Storage for n elements | O(n) |
| Split/Merge operations | O(log n) auxiliary |
| Recursion depth | O(log n) average, O(n) worst |

**Notes:**
- Each node requires constant space
- Recursion depth determines stack usage
- No additional data structures are used beyond the tree nodes

### Why This Data Structure?

Compared to alternatives:
- **Array**: O(n) insertion/deletion at arbitrary positions
- **Linked List**: O(n) random access
- **Standard Treap**: Requires explicit key comparisons; less suitable for index-based operations
- **Implicit Treap**: O(log n) for all index-based operations with simple comparisons

The implicit treap is particularly effective for sequences where both positional access and modifications at arbitrary positions are common operations.