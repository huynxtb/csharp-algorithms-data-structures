# Patricia Trie (Radix Tree)

### 1. Introduction
A Patricia Trie (Practical Algorithm to Retrieve Information Coded in Alphanumeric) or Radix Tree is a space-optimized trie where every node that is the only child is merged with its parent. This reduces the number of nodes and edges, making it highly efficient for storing sparse keys, IP routing tables, and prefix-based lookups.

### 2. Usage
```csharp
using System;
using Algorithms.DataStructures;

class Program
{
    static void Main()
    {
        var trie = new PatriciaTrie<int>();
        
        trie.Insert("apple", 1);
        trie.Insert("app", 2);
        trie.Insert("apricot", 3);

        if (trie.TryGetValue("apple", out int val))
        {
            Console.WriteLine($"Found apple: {val}");
        }

        foreach (var kvp in trie.GetByPrefix("ap"))
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        trie.Remove("app");
    }
}
```

### 3. Detailed Explanation
- **Insert**: Traverses the tree matching edge labels. If a partial match occurs, the existing node is split into a parent node representing the common prefix and two child nodes representing the distinct suffixes.
- **TryGetValue**: Traverses down the tree matching edge labels. If the key is fully consumed and the final node has a value, it returns true.
- **Remove**: Deletes the value association. If a node becomes childless, it is removed from its parent. If the parent or the node itself is left with a single child and no value, they are merged to maintain the radix tree invariant.
- **GetByPrefix**: Traverses to the node matching the prefix, then performs a depth-first traversal to yield all descendant keys.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Insert**: $O(k)$ where $k$ is the length of the key.
  - **Search**: $O(k)$ where $k$ is the length of the key.
  - **Delete**: $O(k)$ where $k$ is the length of the key.
  - **Prefix Search**: $O(k + m)$ where $k$ is the prefix length and $m$ is the total size of the matched subtree.
- **Space Complexity**: $O(N \cdot k)$ worst-case, but significantly lower than a standard Trie due to path compression.