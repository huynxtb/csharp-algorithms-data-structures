# Range Sum Binary Search Tree

## Introduction

The Range Sum Binary Search Tree (BST) is a specialized BST data structure that supports dynamic insertion, deletion, and searching of integer values while also efficiently answering range sum queries. Specifically, it allows you to calculate the sum of all values stored in the BST that fall within a given range [low, high]. This makes it suitable for scenarios where you need to maintain a dynamic collection of numeric data and frequently retrieve the sum of a subset of that data based on value range constraints.

## Usage

// Create an instance of the RangeSumBST
var bst = new RangeSumBST();

// Insert values
bst.Insert(10);
bst.Insert(5);
bst.Insert(15);
bst.Insert(3);
bst.Insert(7);

// Search for a value
bool found = bst.Search(7); // true

// Compute the sum of values between 4 and 12 (inclusive)
int sumInRange = bst.RangeSum(4, 12); // returns 10 + 5 + 7 = 22

// Delete a value
bst.Delete(5);

// Query sum again after deletion
sumInRange = bst.RangeSum(4, 12); // returns 10 + 7 = 17

## Detailed Explanation

### Data Structure
Each node in the BST stores:
- An integer value.
- A sum of all node values in its subtree (`SubtreeSum`), which is the augmentation used to efficiently calculate range sums.
- References to its left and right children.

### Operations
- **Insertion**: When inserting a new value, the node is placed as per BST rules. On the way back up the recursion, the `SubtreeSum` of each node is updated to reflect the changes.

- **Deletion**: When deleting a node:
  - If it has no children, simply remove it.
  - If it has one child, replace it by that child.
  - If it has two children, replace the node's value with its inorder successor's value and delete the successor. After any deletion, update subtree sums on the way back up.

- **Search**: Standard BST search to find if a value is present.

- **RangeSum(low, high)**:
  - This method recursively traverses the tree pruning subtrees that cannot contain values within the range.
  - When a node's value is less than `low`, the method ignores its left subtree.
  - When a node's value is greater than `high`, it ignores its right subtree.
  - Otherwise, it adds the current node value and continues exploring its children.

Note that this implementation does not use self-balancing trees. Thus, performance depends on the shape of the BST (balanced trees yield the best performance).

## Complexity Analysis

- **Insertion:**
  - Average case: O(log n) when the tree is balanced.
  - Worst case: O(n) in skewed trees.

- **Deletion:**
  - Average case: O(log n) for balanced trees.
  - Worst case: O(n) in skewed trees.

- **Search:**
  - Average case: O(log n) for balanced trees.
  - Worst case: O(n) in skewed trees.

- **RangeSum(low, high):**
  - Average case: O(log n) due to pruning of subtrees outside the range.
  - Worst case: O(n) when range covers almost all nodes or tree is skewed.

- **Space Complexity:**
  - O(n) for storing n nodes in the tree.

This implementation provides efficient range sum queries on average, with extra bookkeeping per node for subtree sums to keep queries fast. However, the balance of the tree will impact the time efficiency of all operations.