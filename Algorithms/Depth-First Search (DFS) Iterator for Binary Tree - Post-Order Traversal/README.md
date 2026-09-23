# Depth-First Search (DFS) Iterator for Binary Tree - Post-Order Traversal

## Introduction

The DFS Post-Order Iterator is a production-ready implementation that provides lazy, iterator-based depth-first search traversal of a binary tree using the post-order strategy. Post-order traversal visits nodes in the following sequence: **left subtree → right subtree → root**. This traversal order is particularly useful for scenarios such as:

- **Deleting trees**: Child nodes must be deleted before parent nodes
- **Evaluating expressions**: Operands must be evaluated before operators
- **Computing tree properties**: Values depending on subtrees must be computed bottom-up
- **Postfix notation parsing**: Converting infix to postfix expressions

The implementation uses an iterative approach with explicit stack management rather than recursion, enabling efficient pause-and-resume behavior. It fully implements the `IEnumerable<T>` and `IEnumerator<T>` interfaces, allowing seamless integration with foreach loops and LINQ operations.

## Usage

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// Create a binary tree
TreeNode<int> root = new TreeNode<int>(1)
{
    Left = new TreeNode<int>(2)
    {
        Left = new TreeNode<int>(4),
        Right = new TreeNode<int>(5)
    },
    Right = new TreeNode<int>(3)
    {
        Left = new TreeNode<int>(6),
        Right = new TreeNode<int>(7)
    }
};

// Method 1: Using foreach loop
var iterator = new DfsPostOrderIterator<int>(root);
foreach (var value in iterator)
{
    Console.WriteLine(value);
}
// Output: 4, 5, 2, 6, 7, 3, 1

// Method 2: Using LINQ
var iterator2 = new DfsPostOrderIterator<int>(root);
var result = iterator2.Where(x => x > 2).ToList();
// result: [4, 5, 3, 6, 7]

// Method 3: Using MoveNext directly for fine-grained control
var iterator3 = new DfsPostOrderIterator<int>(root);
while (iterator3.MoveNext())
{
    Console.WriteLine(iterator3.Current);
}

// Method 4: Handling empty tree
TreeNode<string> emptyRoot = null;
var emptyIterator = new DfsPostOrderIterator<string>(emptyRoot);
int count = 0;
foreach (var value in emptyIterator)
{
    count++;
}
// count: 0 (no nodes visited)

// Method 5: Single node tree
TreeNode<double> singleNode = new TreeNode<double>(42.5);
var singleIterator = new DfsPostOrderIterator<double>(singleNode);
foreach (var value in singleIterator)
{
    Console.WriteLine(value);
}
// Output: 42.5
```

## Detailed Explanation

### Architecture

The implementation consists of two main classes:

#### TreeNode<T> Class
A generic binary tree node with three properties:
- **Value**: Stores the data of type `T`
- **Left**: Reference to the left child node
- **Right**: Reference to the right child node

#### DfsPostOrderIterator<T> Class
Implements both `IEnumerable<T>` and `IEnumerator<T>` interfaces to provide full iterator functionality.

### State Management

The iterator maintains the following state:

1. **_traversalStack**: A stack of tuples containing:
   - `node`: The tree node to process
   - `childrenProcessed`: A boolean flag indicating whether the node's children have been visited

2. **_initialized**: Tracks whether `MoveNext()` has been called at least once

3. **_current**: Stores the current node's value being enumerated

### Post-Order Traversal Algorithm

The iterative post-order traversal works as follows:

1. **Initialization**: On the first call to `MoveNext()`, the root node is pushed to the stack with `childrenProcessed = false`

2. **Processing Phase**:
   - Pop a node from the stack
   - If `childrenProcessed == true`: The node has already had its children processed, so return this node's value
   - If `childrenProcessed == false`: 
     - Push the same node back onto the stack with `childrenProcessed = true`
     - Push the right child (if exists) with `childrenProcessed = false`
     - Push the left child (if exists) with `childrenProcessed = false`

3. **Traversal Order**: Because stacks are LIFO (Last In First Out), pushing left child after right child ensures left child is processed before right child, maintaining post-order semantics

### Key Features

- **Lazy Evaluation**: Nodes are only computed when `MoveNext()` is called, not upfront
- **Memory Efficient**: Uses stack space proportional to tree height, not tree size
- **Stateful**: Maintains exact position in traversal, allowing pause-and-resume
- **Interface Compliance**: Full implementation of standard .NET enumeration interfaces
- **Exception Handling**: Throws appropriate exceptions for invalid states (e.g., accessing `Current` before first `MoveNext()` call)
- **Resource Management**: Implements `IDisposable` for proper cleanup

### Edge Cases Handled

- **Empty trees** (null root): Iterator immediately returns false from `MoveNext()`
- **Single-node trees**: Correctly visits the single node
- **Unbalanced trees**: Works correctly regardless of balance
- **Null children**: Gracefully skips null left or right children
- **Reset functionality**: Can reset the iterator to traverse again

## Complexity Analysis

### Time Complexity

- **MoveNext()**: O(1) amortized
  - Each node is visited exactly twice in the traversal (once when first encountered, once when children are processed)
  - The total number of push and pop operations is O(n) for n nodes
  - Amortized over all calls, each operation is O(1)

- **Complete traversal of n nodes**: O(n)
  - Each of the n nodes is pushed and popped from the stack exactly once
  - Each push and pop is O(1)

- **GetEnumerator()**: O(1)
  - Simply creates a new iterator instance

### Space Complexity

- **Stack space**: O(h) where h is the height of the tree
  - In the worst case (skewed tree), h = n, resulting in O(n) space
  - In the best case (balanced tree), h = log(n), resulting in O(log n) space
  - The maximum stack depth occurs on the longest path from root to leaf

- **Per-iterator overhead**: O(1)
  - Each iterator instance uses constant extra space for state variables

### Summary Table

| Operation | Time Complexity | Space Complexity |
|-----------|-----------------|------------------|
| MoveNext() for each node | O(1) amortized | O(h) |
| Complete traversal | O(n) | O(h) |
| Reset() | O(1) | O(h) |
| GetEnumerator() | O(1) | O(1) |
| Dispose() | O(1) | O(1) |