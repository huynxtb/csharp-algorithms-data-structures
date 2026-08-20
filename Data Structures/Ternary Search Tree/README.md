# Ternary Search Tree (TST)

### 1. Introduction
A Ternary Search Tree (TST) is a specialized trie-like data structure where each node has up to three children: a left child, a middle child, and a right child. TSTs combine the space efficiency of binary search trees with the prefix-search capabilities of tries. They are highly effective for auto-complete systems, spell checkers, and IP routing tables.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        var tst = new TernarySearchTree<int>();

        // Insert values
        tst.Insert("cat", 1);
        tst.Insert("cats", 2);
        tst.Insert("cab", 3);

        // Retrieve values
        if (tst.TryGetValue("cat", out int val))
        {
            Console.WriteLine($"Found 'cat' with value: {val}");
        }

        // Prefix search
        var matches = tst.KeysWithPrefix("ca");
        foreach (var key in matches)
        {
            Console.WriteLine($"Prefix match: {key}");
        }

        // Delete key
        bool deleted = tst.Delete("cats");
        Console.WriteLine($"Deleted 'cats': {deleted}");
    }
}
```

### 3. Detailed Explanation
- **Node Structure**: Each node stores a single character, pointers to three subtrees (`Left`, `Mid`, `Right`), a generic value, and a boolean flag (`IsEndOfKey`) indicating if the node completes a valid key.
- **Branching Logic**:
  - `Left`: Traversed when the target character is smaller than the current node's character.
  - `Right`: Traversed when the target character is larger than the current node's character.
  - `Mid`: Traversed when the target character matches the current node's character, moving to the next character in the key.
- **Deletion & Cleanup**: The `Delete` method removes the key flag and value. It then performs a post-order traversal to prune nodes that no longer lead to any valid keys, preventing memory leaks.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Insert**: $O(L)$ average, where $L$ is the length of the key. Worst case is $O(L + N)$ if the tree becomes unbalanced.
  - **Search / Contains**: $O(L)$ average, $O(L + N)$ worst case.
  - **Delete**: $O(L)$ average, including post-order node reclamation.
  - **Prefix Search**: $O(L + K)$ where $L$ is the prefix length and $K$ is the number of matching keys in the subtree.
- **Space Complexity**: $O(N \cdot L)$ worst case, but significantly lower in practice due to shared prefixes among keys.