# Morris In-order Traversal

## 1. Introduction
Morris Traversal is an algorithm for binary tree traversal that does not rely on recursion or an explicit auxiliary stack. By utilizing the concept of threaded binary trees, Morris Traversal temporarily establishes back-links from the in-order predecessor back to the current node and removes them once the subtree has been processed.

This algorithm is especially useful in memory-constrained environments where the $O(h)$ space overhead (where $h$ is tree height) of standard recursive or stack-based traversals is prohibitive.

## 2. Usage

```csharp
// Construct a sample binary tree:
//       1
//      / \
//     2   3
//    / \
//   4   5
TreeNode root = new TreeNode(1,
    new TreeNode(2,
        new TreeNode(4),
        new TreeNode(5)
    ),
    new TreeNode(3)
);

MorrisInorderTraversal traverser = new MorrisInorderTraversal();
List<int> inOrderResult = traverser.MorrisInorder(root);

// Output: 4, 2, 5, 1, 3
foreach (var val in inOrderResult)
{
    Console.Write(val + " ");
}
```

## 3. Detailed Explanation
The algorithm iterates through the binary tree using a `current` pointer:
1. If `current.Left` is `null`, there is no left subtree to process. The algorithm records `current.Value` and steps into `current.Right`.
2. If `current.Left` is not `null`, it locates the in-order predecessor (the rightmost node of the left subtree):
   - If `predecessor.Right` is `null`, this is the first visit to this subtree. It sets `predecessor.Right = current` (creating the thread) and advances `current` to `current.Left`.
   - If `predecessor.Right` is `current`, the left subtree has already been fully explored via the thread. It breaks the cycle by setting `predecessor.Right = null`, records `current.Value`, and moves `current` to `current.Right`.
3. The algorithm terminates when `current` becomes `null`. The tree is completely restored to its original unmodified structure.

## 4. Complexity Analysis
- **Time Complexity:** $O(N)$ where $N$ is the number of nodes in the binary tree. Although finding predecessors traverses edges multiple times, every edge in the tree is traversed at most 3 times (once to find predecessor, once to traverse downward, once during the thread return).
- **Space Complexity:** $O(1)$ auxiliary space complexity (excluding the output collection), as no recursion stack or auxiliary stack data structure is used.