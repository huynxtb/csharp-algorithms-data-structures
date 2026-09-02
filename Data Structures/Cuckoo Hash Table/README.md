# Cuckoo Hash Table

## 1. Introduction
A Cuckoo Hash Table is a dictionary data structure offering worst-case constant time $O(1)$ lookups and deletions. It uses two separate hash tables and hash functions. When a collision occurs during insertion, the existing element is evicted to its alternative location, kicking off a chain of evictions until an empty slot is found or a cycle is detected.

Use Cuckoo Hashing when high-performance, guaranteed $O(1)$ worst-case lookup performance is required (e.g., networking systems, real-time caches, hardware routing tables).

## 2. Usage

```csharp
using System;

class Program
{
    static void Main()
    {
        var map = new CuckooHashTable<string, int>();

        // Adding elements
        map.Add("apple", 1);
        map.Add("banana", 2);
        map["cherry"] = 3;

        // Lookup
        if (map.TryGetValue("apple", out int val))
        {
            Console.WriteLine($"apple: {val}");
        }

        // Check containment
        Console.WriteLine($"Contains banana: {map.ContainsKey("banana")}");

        // Remove
        map.Remove("banana");

        // Enumeration
        foreach (var pair in map)
        {
            Console.WriteLine($"{pair.Key} -> {pair.Value}");
        }
    }
}
```

## 3. Detailed Explanation
- **Dual Tables & Hash Functions**: The hash table maintains two arrays (`_table1` and `_table2`). `Hash1` maps keys into `_table1` and `Hash2` maps keys into `_table2` using Knuth's multiplicative hashing algorithm.
- **Eviction Loop**: When inserting a key-value pair, it attempts to place it in `_table1`. If occupied, the existing key-value pair is evicted and inserted into its alternative position in `_table2`. This process continues until a free slot is located.
- **Cycle Detection**: If the eviction loop exceeds a maximum iteration depth ($2 \times \text{capacity} + 1$), an infinite cycle is assumed. The table automatically doubles its capacity and rehashes all elements.
- **Load Factor Control**: Capacity is automatically doubled if the load factor exceeds 50% across both tables.

## 4. Complexity Analysis
- **Lookup (`ContainsKey`, `TryGetValue`)**: $O(1)$ worst-case, inspecting at most 2 array locations.
- **Deletion (`Remove`)**: $O(1)$ worst-case, inspecting and clearing at most 2 array locations.
- **Insertion (`Add`)**: $O(1)$ amortized time. Worst-case triggers rehashing $O(N)$.
- **Space Complexity**: $O(N)$ auxiliary space across the dual underlying array buffers.