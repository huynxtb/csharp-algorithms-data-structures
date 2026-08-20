# LFU (Least Frequently Used) Cache

### 1. Introduction
An LFU (Least Frequently Used) Cache is a specialized memory caching algorithm that discards the items that are least frequently used first. When a tie occurs (multiple items share the same lowest access frequency), the cache evicts the least recently used (LRU) item among them. This data structure is ideal for scenarios with static or semi-static access patterns where frequency of access is a better predictor of future use than recency.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        // Initialize LFU Cache with capacity of 2
        var cache = new LfuCache<string, int>(2);

        cache.Put("A", 1);
        cache.Put("B", 2);

        // Returns 1, increments frequency of "A" to 2
        int valA = cache.Get("A"); 

        // Evicts "B" (freq 1) instead of "A" (freq 2) to insert "C"
        cache.Put("C", 3); 

        bool hasB = cache.TryGetValue("B", out _); // Returns false
        bool hasC = cache.TryGetValue("C", out _); // Returns true
    }
}
```

### 3. Detailed Explanation
This implementation achieves $O(1)$ average time complexity for both `Get` and `Put` operations by combining two primary data structures:
- **Key Map (`_cache`)**: A hash map mapping keys to their corresponding node wrapper. This provides $O(1)$ lookup.
- **Frequency Map (`_frequencyLists`)**: A hash map mapping access frequencies to a custom `DoublyLinkedList` containing all nodes with that frequency. 

When an item is accessed or updated:
1. It is removed from its current frequency list.
2. Its frequency counter is incremented.
3. It is appended to the end of the list corresponding to the new frequency (acting as the Most Recently Used item for that frequency).
4. If the old frequency list becomes empty and it was the minimum frequency (`_minFrequency`), `_minFrequency` is incremented.

When the cache reaches capacity and a new item is inserted:
1. The node at the head of the list corresponding to `_minFrequency` is evicted (representing the Least Recently Used item among the Least Frequently Used items).
2. The evicted node's key is removed from the key map.
3. The new node is inserted with a frequency of 1, and `_minFrequency` is reset to 1.

### 4. Complexity Analysis
- **Time Complexity**:
  - `Get(key)`: $O(1)$ average time complexity for hash map lookup and pointer updates.
  - `Put(key, value)`: $O(1)$ average time complexity for hash map insertion, deletion, and pointer updates.
- **Space Complexity**: $O(N)$ where $N$ is the capacity of the cache, storing up to $N$ nodes in the hash map and doubly linked lists.