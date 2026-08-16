# LRU Cache

### 1. Introduction
An LRU (Least Recently Used) Cache is a cache eviction algorithm that organizes items in order of use. When the cache reaches its capacity limit, it discards the least recently accessed items first. This data structure is ideal for scenarios requiring fast access to recently used data, such as database query caching, web page rendering, and session management.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        var cache = new LRUCache<string, int>(3);

        cache.Put("A", 1);
        cache.Put("B", 2);
        cache.Put("C", 3);

        // Accessing "A" makes it the most recently used
        int valA = cache.Get("A"); 

        // Adding "D" evicts "B" (least recently used)
        cache.Put("D", 4);

        bool hasB = cache.TryGetValue("B", out _);
        Console.WriteLine($"Contains B: {hasB}"); // Output: False
    }
}
```

### 3. Detailed Explanation
This implementation achieves $O(1)$ average time complexity for all operations by combining two data structures:
- **Dictionary<TKey, Node>**: Maps keys to nodes in the doubly-linked list, allowing $O(1)$ lookup.
- **Doubly-Linked List**: Tracks the access order. The head of the list represents the most recently used (MRU) item, and the tail represents the least recently used (LRU) item.

When an item is accessed via `Get` or `TryGetValue`, or updated via `Put`, its corresponding node is moved to the head of the list. When a new item is added via `Put` and the cache exceeds its capacity, the node at the tail is removed from both the list and the dictionary.

### 4. Complexity Analysis
- **Time Complexity**:
  - `Get`: $O(1)$ average (dictionary lookup and pointer updates).
  - `TryGetValue`: $O(1)$ average.
  - `Put`: $O(1)$ average (dictionary insertion/update and pointer updates).
- **Space Complexity**: $O(C)$ where $C$ is the capacity of the cache, as it stores at most $C$ nodes and dictionary entries.