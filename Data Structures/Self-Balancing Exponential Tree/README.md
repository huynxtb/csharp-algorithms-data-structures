# Self-Balancing Exponential Tree

## Introduction

The Self-Balancing Exponential Tree is a dynamic data structure designed to maintain a collection of items in a sorted order. It ensures that operations like insertion, deletion, and searching are performed efficiently by automatically rebalancing itself. This balancing mechanism prevents the tree from becoming skewed, which could degrade performance to linear time in the worst case. The 'exponential' aspect refers to the underlying principle of maintaining a balanced height, analogous to how exponential growth is managed to stay within bounds, ensuring logarithmic time complexity for key operations.

This data structure is particularly useful when dealing with large, dynamic datasets where frequent modifications and lookups are expected, and maintaining optimal performance is critical. It's an alternative to other balanced tree structures like AVL trees or Red-Black trees, offering a similar performance profile.

## Usage

Here's a C# code example demonstrating how to use the `SelfBalancingExponentialTree`:

```csharp
// Assuming the SelfBalancingExponentialTree class is defined in the same scope or imported.

// Create a new instance of the tree
SelfBalancingExponentialTree tree = new SelfBalancingExponentialTree();

// Insert elements into the tree
tree.Insert(10);
tree.Insert(20);
tree.Insert(30);
tree.Insert(40);
tree.Insert(50);
tree.Insert(25);

// Search for elements
bool found20 = tree.Search(20);
Console.WriteLine($"Is 20 in the tree? {found20}"); // Output: Is 20 in the tree? True

bool found100 = tree.Search(100);
Console.WriteLine($"Is 100 in the tree? {found100}"); // Output: Is 100 in the tree? False

// Delete an element
tree.Delete(30);

// Search again after deletion
bool found30AfterDelete = tree.Search(30);
Console.WriteLine($"Is 30 in the tree after deletion? {found30AfterDelete}"); // Output: Is 30 in the tree after deletion? False

tree.Delete(20);
tree.Delete(40);
```

## Detailed Explanation

The `SelfBalancingExponentialTree` is implemented using a node-based structure where each `ExponentialTreeNode` contains an integer `Value`, references to its `Left` and `Right` children, and an integer `Height` representing the height of the subtree rooted at that node. The `Height` property is crucial for determining the balance factor.

### Core Components:

1.  **`ExponentialTreeNode` Class:**
    *   `Value`: The integer data stored in the node.
    *   `Left`, `Right`: Pointers to the left and right child nodes.
    *   `Height`: The height of the subtree rooted at this node. This is used to calculate the balance factor.

2.  **`SelfBalancingExponentialTree` Class:**
    *   `root`: A reference to the root node of the tree.

### Key Methods:

*   **`GetHeight(ExponentialTreeNode node)`:** A helper method to safely get the height of a node (returns 0 if the node is null).
*   **`GetBalanceFactor(ExponentialTreeNode node)`:** Calculates the balance factor of a node, which is the difference between the height of its left subtree and the height of its right subtree.
*   **`UpdateHeight(ExponentialTreeNode node)`:** Recalculates and updates the height of a given node based on the heights of its children.
*   **`RightRotate(ExponentialTreeNode y)` and `LeftRotate(ExponentialTreeNode x)`:** These are the fundamental rotation operations used to rebalance the tree. They restructure the tree locally to resolve imbalances while maintaining the binary search tree property.
    *   A **right rotation** is performed when a node is left-heavy and its left child is also left-heavy or balanced.
    *   A **left rotation** is performed when a node is right-heavy and its right child is also right-heavy or balanced.
*   **`Balance(ExponentialTreeNode node)`:** This method checks the balance factor of a node. If the node is unbalanced (balance factor > 1 or < -1), it performs the necessary rotations (single or double rotations) to restore balance. It also updates the height of the node after balancing.
*   **`Insert(int value)`:** The public method to insert a new value. It calls the recursive `InsertRecursive` method.
*   **`InsertRecursive(ExponentialTreeNode node, int value)`:** This method recursively finds the correct position for the new value, inserts it as a new leaf node, and then, as the recursion unwinds, calls `Balance` on each node along the insertion path to ensure the tree remains balanced.
*   **`Search(int value)`:** The public method to search for a value. It calls the recursive `SearchRecursive` method.
*   **`SearchRecursive(ExponentialTreeNode node, int value)`:** Performs a standard binary search tree traversal to find the value. It returns `true` if found, `false` otherwise.
*   **`Delete(int value)`:** The public method to delete a value. It calls the recursive `DeleteRecursive` method.
*   **`DeleteRecursive(ExponentialTreeNode node, int value)`:** This method finds the node to be deleted. It handles three cases:
    1.  **Node with no children:** Simply remove the node.
    2.  **Node with one child:** Replace the node with its child.
    3.  **Node with two children:** Find the inorder successor (smallest node in the right subtree), copy its value to the node being deleted, and then recursively delete the inorder successor. After deletion, it calls `Balance` on the affected nodes to maintain balance.
*   **`GetMinValueNode(ExponentialTreeNode node)`:** A helper method used during deletion to find the node with the minimum value in a given subtree (used to find the inorder successor).

The "exponential" nature of the balancing comes from the fact that the height of the tree is kept logarithmic with respect to the number of nodes. This is achieved by ensuring that the height difference between the left and right subtrees of any node is at most 1. This property guarantees that the tree's height grows at most logarithmically, similar to how an exponential function's growth is managed to stay within certain bounds, thus enabling efficient operations.

## Complexity Analysis

*   **Time Complexity:**
    *   **Insertion:** O(log n) on average and in the worst case. The tree is balanced after each insertion, ensuring a logarithmic height.
    *   **Deletion:** O(log n) on average and in the worst case. Similar to insertion, balancing operations maintain the logarithmic height.
    *   **Search:** O(log n) on average and in the worst case. This is because the tree's height is always logarithmic.

*   **Space Complexity:**
    *   **Overall:** O(n), where n is the number of nodes in the tree, to store the nodes and their values.
    *   **Auxiliary Space (for operations):** O(log n) due to the recursion stack depth during insertion, deletion, and search operations in a balanced tree.