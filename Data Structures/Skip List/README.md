# Skip List

## Introduction

A Skip List is a probabilistic data structure that offers efficient search, insertion, and deletion operations within a dynamically ordered sequence of elements. It achieves average-case O(log n) time complexity for these operations, making it an efficient alternative to balanced binary search trees. Skip Lists use multiple layers of linked lists with varying levels, allowing quick traversal across large sections of the list.

## Usage

Here's a basic example of how to use the generic Skip List in C#:

// Create a new SkipList instance for integers
var skipList = new SkipList<int>();

// Insert values
skipList.Insert(10);
skipList.Insert(20);
skipList.Insert(15);

// Search for a value
bool found = skipList.Search(15);  // true

// Delete a value
bool deleted = skipList.Delete(10);  // true

// Check if a deleted value still exists
bool stillExists = skipList.Search(10);  // false

## Detailed Explanation

### Design
- The `SkipList<T>` class is generic and constrained to types implementing `IComparable<T>`, ensuring that elements can be ordered.
- Internally, it uses a nested `Node` class that holds the data value and an array of forward pointers, one per level.
- The maximum level (`_maxLevel`) defines the maximum height of the skip list tower.

### Core Mechanics
- **Random Level Generation:** New nodes are assigned levels randomly with a geometric distribution (using the constant probability 0.5). Higher levels are less frequent, allowing faster traversal.
- **Insert:** To insert, the algorithm first searches forward at each level to find the correct insert position. It maintains an `update` array to hold nodes whose pointers will change.
- If the new node's randomly chosen level is higher than the current max level, the list's current level is updated and the head is prepared accordingly.
- Pointers are updated to insert the new node at appropriate places.

- **Search:** Starts from the highest level, moving forward while the next node's value is less than the target, then drops down a level to refine the search until level 0 is reached.

- **Delete:** Similar to search, it finds nodes pointing to the target node at each level and updates their pointers to bypass the deleted node. The current level is decreased if upper levels become empty.

### Encapsulation
- All internal details are encapsulated within the class; no direct node accesses from outside.
- The probabilistic balancing is hidden from the user.

## Complexity Analysis

| Operation | Average Time Complexity | Worst Time Complexity |
|-----------|-------------------------|-----------------------|
| Search    | O(log n)                | O(n)                  |
| Insert    | O(log n)                | O(n)                  |
| Delete    | O(log n)                | O(n)                  |

- The average time complexity is logarithmic because the skip list maintains a balanced probabilistic structure.
- The worst case occurs if all elements fall into a single level list (very unlikely).
- Space complexity is O(n), due to storing multiple forward pointers per node.

This Skip List implementation serves as a reusable component for scenarios needing ordered, dynamic collections with efficient access, insertion, and deletion.