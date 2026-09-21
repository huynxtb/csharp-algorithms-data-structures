# Rope (Cord) Data Structure

## Introduction

A **Rope** (also called a **Cord**) is a binary-tree-based data structure designed for efficient manipulation of very large, mutable strings. Unlike traditional strings which can require O(n) time for concatenation, splitting, and insertion operations, Ropes achieve O(log n) complexity for these operations by organizing string fragments as leaves in a balanced binary tree.

Ropes are ideal for:
- Text editors (word processors, IDEs) that require efficient undo/redo and real-time editing
- Large document processing systems
- Scenarios with frequent string concatenations, insertions, and deletions
- Functional programming where immutable string operations are common

## Usage

```csharp
// Create a Rope from a string
Rope rope1 = new Rope("Hello ");
Rope rope2 = new Rope("World!");

// Concatenate two Ropes
Rope combined = rope1.Concat(rope2);
Console.WriteLine(combined.ToString()); // Output: "Hello World!"

// Get character at index
char ch = combined.Index(6); // 'W'

// Get total length
int len = combined.Length; // 12

// Extract substring
string sub = combined.Substring(0, 5); // "Hello"

// Split at position
var (left, right) = combined.Split(6);
Console.WriteLine(left.ToString());  // "Hello "
Console.WriteLine(right.ToString()); // "World!"

// Insert string at position
Rope inserted = combined.Insert(6, "Beautiful ");
Console.WriteLine(inserted.ToString()); // "Hello Beautiful World!"

// Delete characters
Rope deleted = combined.Delete(6, 6);
Console.WriteLine(deleted.ToString()); // "Hello !"

// Rebalance to maintain optimal tree structure
Rope balanced = deleted.Rebalance();
```

## Detailed Explanation

### Tree Structure

The Rope is implemented as a binary tree with two types of nodes:

1. **Leaf Nodes**: Store actual string fragments (up to `LeafMaxLength` characters each)
2. **Internal Nodes**: Have left and right child nodes and maintain a **weight** value

### Weight Invariant

The key to the Rope's efficiency is the **weight** property of internal nodes:
- **Weight = total length of all characters in the left subtree**
- This allows O(log n) indexing: to find the character at position `i`, we compare `i` with the weight to decide whether to traverse left or right

### Core Operations

#### 1. Index(i) - O(log n)
Traverse from root to leaf using weight comparisons:
- If `i < weight`, go left (position is in left subtree)
- Otherwise, go right and adjust index by subtracting weight

#### 2. Concat(other) - O(1)
Create a new internal node with current Rope as left child and other as right child. The weight is automatically set to the length of the left subtree.

#### 3. Split(i) - O(log n)
Recursively split nodes at position `i`:
- For leaf nodes: split the string at index `i`
- For internal nodes: if `i ≤ weight`, split the left child; otherwise, split the right child
- Return both resulting subtrees

#### 4. Insert(i, s) - O(log n)
- Split at position `i` to get `(left, right)`
- Create a new Rope from string `s`
- Concatenate: `left.Concat(new Rope(s)).Concat(right)`

#### 5. Delete(start, length) - O(log n)
- Split at `start + length` to get left part
- Split left part at `start` to isolate deleted region
- Concatenate remaining parts

#### 6. Substring(start, length) - O(log n + m)
Split operations to isolate the range, then convert to string (where m is substring length).

#### 7. Rebalance() - O(n)
Collect all leaf nodes in order, then rebuild a perfectly balanced tree using a divide-and-conquer approach. This prevents tree degeneration after many sequential operations.

### Tree Balancing

The initial tree is built as a perfectly balanced binary tree. However, after multiple insert/delete operations, the tree can become unbalanced. The `Rebalance()` method reconstructs an optimal tree by:
1. Collecting all leaf nodes
2. Recursively building a balanced tree by always splitting leaves at the midpoint

## Complexity Analysis

### Time Complexity

| Operation | Complexity | Notes |
|-----------|-----------|-------|
| **Rope(string)** | O(n) | Building initial balanced tree from string |
| **Index(i)** | O(log n) | Binary search through tree via weights |
| **Length** | O(1) | Stored in root node |
| **Concat(other)** | O(1) | Just creates one new internal node |
| **Split(i)** | O(log n) | Traverses tree depth, creates O(log n) nodes |
| **Insert(i, s)** | O(log n) | Split + Concat operations |
| **Delete(start, len)** | O(log n) | Two splits + concatenation |
| **Substring(start, len)** | O(log n + m) | Splits are O(log n), string construction is O(m) |
| **ToString()** | O(n) | Must visit all leaf nodes to reconstruct |
| **Rebalance()** | O(n) | Must visit all leaves and rebuild tree |

### Space Complexity

| Operation | Complexity | Notes |
|-----------|-----------|-------|
| **Storage (n chars)** | O(n) | Leaf nodes store all characters |
| **Tree Structure** | O(n / LeafMaxLength) | ~O(n) nodes in tree |
| **Concat** | O(1) | Creates one new internal node |
| **Split** | O(log n) | Creates O(log n) new nodes for split paths |
| **Rebalance** | O(n) | Reconstructs entire tree |

### Key Advantages

- **Concatenation**: Standard string `+` operator is O(n); Rope.Concat is O(1)
- **Insertion/Deletion**: Standard string operations are O(n); Rope operations are O(log n)
- **Indexing**: Both are O(1) for strings, O(log n) for Ropes, but Ropes avoid expensive copy operations
- **Memory Efficiency**: Shared subtrees between Ropes (structural sharing) can reduce memory for immutable operations

### Practical Considerations

- The `LeafMaxLength = 10` balances memory locality with tree depth
- For production use, larger leaf sizes (e.g., 512-1024 bytes) reduce tree overhead
- Call `Rebalance()` periodically if many sequential edits occur
- `ToString()` is expensive; avoid calling it frequently on very large Ropes