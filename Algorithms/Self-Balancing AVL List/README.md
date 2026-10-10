# Self-Balancing AVL List

## Introduction

The AVL List is a self-balancing binary search tree data structure that maintains elements in sorted order while providing efficient insertion, deletion, and search operations. The term "AVL" comes from its inventors, Adelson-Velsky and Landis. Unlike standard linked lists that require O(n) time for searches, or unbalanced binary search trees that can degrade to O(n) in worst cases, an AVL List guarantees O(log n) performance for these operations by maintaining a balance factor constraint: the height difference between left and right subtrees of any node cannot exceed 1.

The AVL List is particularly useful when you need:
- A dynamically ordered collection with automatic sorting
- Fast lookups, insertions, and deletions
- Guaranteed logarithmic time complexity for core operations
- A self-healing data structure that rebalances automatically

## Usage

```csharp
// Create an AVL List of integers
AVLList<int> avlList = new AVLList<int>();

// Insert elements
avlList.Insert(50);
avlList.Insert(25);
avlList.Insert(75);
avlList.Insert(10);
avlList.Insert(30);
avlList.Insert(60);
avlList.Insert(80);

// Search for an element
bool found = avlList.Search(30);
if (found)
{
    // Element found
}

// Get all elements in sorted order
List<int> sortedElements = avlList.TraverseInOrder();
foreach (int element in sortedElements)
{
    // Process element (will be in ascending order: 10, 25, 30, 50, 60, 75, 80)
}

// Get min and max values
int minValue = avlList.GetMin();  // Returns 10
int maxValue = avlList.GetMax();  // Returns 80

// Delete an element
bool deleted = avlList.Delete(25);
if (deleted)
{
    // Element was successfully deleted
}

// Check properties
int count = avlList.Count;  // Returns current number of elements
int height = avlList.GetHeight();  // Returns tree height
bool balanced = avlList.IsBalanced();  // Verifies balance property

// Clear all elements
avlList.Clear();
```

## Detailed Explanation

### Node Structure
Each node in the AVL List contains:
- **Data**: The value stored in the node
- **Left**: Reference to the left child node
- **Right**: Reference to the right child node
- **Height**: The height of the subtree rooted at this node (used for balancing)
- **Next**: A pointer for potential linked list operations (future extensibility)

### Core Operations

#### Insertion
1. Perform standard binary search tree insertion based on value comparison
2. Update the height of the current node
3. Calculate the balance factor (height difference between left and right subtrees)
4. If balance factor > 1 or < -1, perform rotations to restore balance
5. Four cases are handled:
   - Left-Left: Single right rotation
   - Right-Right: Single left rotation
   - Left-Right: Left rotation on left child, then right rotation
   - Right-Left: Right rotation on right child, then left rotation

#### Deletion
1. Find the node to delete using binary search
2. If node has no children, simply remove it
3. If node has one child, replace it with that child
4. If node has two children, replace it with the smallest value in the right subtree (in-order successor)
5. Recursively rebalance the tree after deletion

#### Search
1. Start at root node
2. Compare search value with current node
3. If equal, return true
4. If less, search left subtree
5. If greater, search right subtree
6. If reach null node, element not found

#### Balancing (Rotations)
Rotations restructure the tree to maintain the balance property:
- **Right Rotation**: Used when left subtree is too heavy
- **Left Rotation**: Used when right subtree is too heavy
- **Left-Right Rotation**: Combination of left and right rotations
- **Right-Left Rotation**: Combination of right and left rotations

#### In-Order Traversal
Visits nodes in the order: Left subtree → Current node → Right subtree. This produces elements in ascending sorted order.

### Balance Factor and Height
The balance factor of a node is calculated as: `height(left_subtree) - height(right_subtree)`
Valid balance factors are: -1, 0, or 1. Any other value triggers rebalancing.

## Complexity Analysis

### Time Complexity

| Operation | Average Case | Worst Case |
|-----------|--------------|------------|
| Insert | O(log n) | O(log n) |
| Delete | O(log n) | O(log n) |
| Search | O(log n) | O(log n) |
| Traverse (In-Order) | O(n) | O(n) |
| Get Min/Max | O(log n) | O(log n) |
| IsBalanced Check | O(n) | O(n) |

The guaranteed O(log n) for insert, delete, and search operations is the key advantage of AVL trees compared to unbalanced binary search trees that can degrade to O(n).

### Space Complexity

| Metric | Complexity |
|--------|------------|
| Storage | O(n) |
| Recursion Stack (Insert/Delete/Search) | O(log n) |
| In-Order Traversal Result | O(n) |

**Space Complexity Notes:**
- Each of the n elements requires one node object, resulting in O(n) space
- The recursive operations use a call stack with maximum depth equal to tree height, which is O(log n) for a balanced AVL tree
- In-order traversal returns a list containing all n elements, requiring O(n) additional space

The AVL List provides an optimal balance between time and space complexity, making it ideal for applications requiring frequent insertions, deletions, and searches on large datasets.