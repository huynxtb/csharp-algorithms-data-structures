## Cartesian Tree

### 1. Introduction
A Cartesian Tree is a binary tree derived from a sequence of numbers, satisfying two main properties:
1.  **Heap Property**: It is a min-heap (or max-heap) with respect to the values of the nodes. That is, the value of any node is less than (or greater than) the values of its children. This implementation constructs a min-heap Cartesian Tree.
2.  **In-order Traversal Property**: An in-order traversal of the tree yields the original sequence of numbers.

This unique combination makes Cartesian Trees useful in various algorithms, particularly for efficiently solving Range Minimum Query (RMQ) problems, as a component of implicit treaps, or for representing hierarchical structures where both value-based ordering and sequence-based ordering are important.

### 2. Usage
To build a Cartesian Tree and perform traversals, use the `CartesianTree<T>.Build` static factory method. The type `T` must implement `IComparable<T>`.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// Assume CartesianTree<T> and CartesianTreeNode<T> classes are defined here.

// To build a Cartesian Tree from an array of integers:
int[] data = { 3, 2, 6, 1, 5 };
CartesianTree<int> tree = CartesianTree<int>.Build(data);

// Access the root node:
CartesianTreeNode<int> root = tree.Root;
// For the example data, root.Value will be 1.

// Perform an in-order traversal (yields the original sequence):
IEnumerable<int> inOrder = tree.InOrderTraversal();
// inOrder will yield: 3, 2, 6, 1, 5

// Perform a pre-order traversal:
IEnumerable<int> preOrder = tree.PreOrderTraversal();
// preOrder will yield: 1, 2, 3, 6, 5

// Handle edge cases:
// Empty input:
CartesianTree<int> emptyTree = CartesianTree<int>.Build(new int[] { });
// emptyTree.Root will be null.

// Single item input:
CartesianTree<int> singleItemTree = CartesianTree<int>.Build(new int[] { 10 });
// singleItemTree.Root.Value will be 10.
```

### 3. Detailed Explanation
The `Build` method constructs the Cartesian Tree in linear time using a monotonic stack algorithm. It iterates through the input `items` array, processing each element to create a new `CartesianTreeNode`.

For each `newNode` created from an `item`:
1.  **Identify Left Child**: The algorithm pops nodes from the top of the `stack` as long as their values are greater than `newNode.Value`. These popped nodes, in reverse order of popping, form the right spine of a subtree that will eventually become the left child of `newNode`. The *last* node popped becomes the direct left child of `newNode`. This ensures the min-heap property: `newNode` is smaller than its left child (which was previously a parent or ancestor of `newNode` in the stack).
2.  **Identify Right Child**: If the `stack` is not empty after the popping phase, it means the `stack.Peek()` node has a value less than `newNode.Value`. In this scenario, `newNode` becomes the right child of `stack.Peek()`. This maintains the min-heap property and ensures that `newNode` is placed correctly relative to its parent on the right spine.
3.  **Push to Stack**: Finally, `newNode` is pushed onto the `stack`. The stack always maintains a sequence of nodes whose values are in increasing order from bottom to top, forming the rightmost path of the tree being constructed.

After processing all items, the `Root` of the Cartesian Tree is the single node remaining at the bottom of the stack. If the input was empty, the `Root` remains `null`.

The `InOrderTraversal` and `PreOrderTraversal` methods are standard recursive tree traversals implemented using `yield return` for efficient enumeration. The `InOrderTraversal` specifically validates the second property of a Cartesian Tree by returning the original sequence of items.

### 4. Complexity Analysis
*   **Time Complexity**:
    *   **Construction (`Build`)**: $O(N)$, where $N$ is the number of items in the input collection. Each item is pushed onto the stack and popped from the stack at most once. All other operations (comparisons, assignments) are constant time.
    *   **Traversals (`InOrderTraversal`, `PreOrderTraversal`)**: $O(N)$, where $N$ is the number of nodes in the tree. Each node is visited exactly once.
*   **Space Complexity**:
    *   **Construction (`Build`)**: $O(N)$ in the worst case. The stack can hold up to $N$ nodes (e.g., for a strictly decreasing input sequence like `[5, 4, 3, 2, 1]`). The tree itself also stores $N$ nodes.
    *   **Traversals (`InOrderTraversal`, `PreOrderTraversal`)**: $O(H)$ where $H$ is the height of the tree. This is due to the recursion stack. In the worst case (a skewed tree), $H$ can be $O(N)$.