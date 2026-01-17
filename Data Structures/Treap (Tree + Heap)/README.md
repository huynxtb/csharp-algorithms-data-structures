# Treap (Tree + Heap)

## Introduction
A Treap is a randomized balanced binary search tree data structure that combines the properties of a binary search tree (BST) and a binary heap. Each node in a Treap contains a key and a randomly assigned priority, where the keys follow the BST property, and the priorities follow the heap property (max-heap in this implementation). The randomized priorities help keep the tree balanced with high probability, allowing efficient average-case operations.

Treaps are useful when you need a dynamic ordered set that supports quick insertion, deletion, and membership queries with expected logarithmic time complexity. They are particularly beneficial when you want balanced tree performance without complex balancing logic as in self-balancing trees like AVL or Red-Black trees.

## Usage
var treap = new Treap<int>();

// Insert keys
treap.Insert(10);
treap.Insert(5);
treap.Insert(15);

// Check if a key exists
bool contains5 = treap.Contains(5);  // true
bool contains7 = treap.Contains(7);  // false

// In-order traversal (sorted order)
foreach (var key in treap.InOrderTraversal())
{
    Console.WriteLine(key);
}

// Delete a key
treap.Delete(5);

// Check after deletion
bool contains5AfterDelete = treap.Contains(5); // false

## Detailed Explanation
- The Treap nodes have a `Key` and a randomly assigned `Priority` at insertion.
- It maintains the **BST property**: for any node, all keys in the left subtree are less, and all keys in the right subtree are greater.
- It maintains the **max-heap property on priorities**: each node's priority is greater than or equal to the priorities of its children.
- On insertion:
  - A new node is inserted as in a BST.
  - The priority is assigned as a random integer.
  - To maintain the heap property, rotations (left or right) are performed to "bubble up" the node if its priority is greater than its parent.
- On deletion:
  - The node to delete is located similarly to BST.
  - It is "rotated down" until it becomes a leaf by comparing and rotating with the higher priority child.
  - Finally, it is removed.
- Rotations preserve the BST property while adjusting the tree shape to maintain the heap property on priorities.
- This approach provides expected O(log n) operations due to randomized balancing.

## Complexity Analysis
| Operation     | Average Time Complexity | Worst-case Time Complexity |
|---------------|-------------------------|----------------------------|
| Insert        | O(log n)                | O(n)                      |
| Delete        | O(log n)                | O(n)                      |
| Contains      | O(log n)                | O(n)                      |
| InOrderTraversal | O(n)                  | O(n)                      |

- The average complexities are logarithmic because the random priorities probabilistically keep the tree balanced.
- In the worst case (rare, if random priorities cause skew), the tree could degenerate to a linked list, resulting in O(n) for operations.
- Space complexity is O(n), where n is the number of nodes in the Treap.

This Treap implementation provides a simple and effective balanced search tree solution without complex balancing algorithms, suitable for many applications needing ordered dynamic sets.