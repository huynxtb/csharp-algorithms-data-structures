# Consistent Hash Ring with Virtual Nodes

## 1. Introduction
Consistent Hashing is a distributed hashing scheme that operates independently of the number of servers or caches in a distributed system. It minimizes key redistribution when nodes are added or removed. Virtual nodes (vnodes) solve the problem of non-uniform distribution (hotspots) by mapping multiple virtual positions on the ring to a single physical node.

## 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        // Initialize ring with 100 virtual nodes per physical node
        var ring = new ConsistentHashRing<string>(100);

        // Add physical nodes
        ring.AddNode("Server_A");
        ring.AddNode("Server_B");
        ring.AddNode("Server_C");

        // Route keys to nodes
        string node1 = ring.GetNode("user_session_98234");
        string node2 = ring.GetNode("user_session_11023");

        Console.WriteLine($"Key 1 routed to: {node1}");
        Console.WriteLine($"Key 2 routed to: {node2}");

        // Remove a node
        ring.RemoveNode("Server_B");
    }
}
```

## 3. Detailed Explanation
- **Virtual Nodes**: Each physical node is replicated `replicationFactor` times. Each replica is hashed using a unique identifier (e.g., `NodeName_Index`) to distribute it across the 32-bit integer space.
- **Binary Search Lookup**: The hashes of all virtual nodes are stored in a sorted list (`_sortedKeys`). When routing a key, its hash is computed, and `BinarySearch` finds the first virtual node hash greater than or equal to the key's hash. If no such hash exists, it wraps around to the first element (index 0).
- **Thread Safety**: All operations (`AddNode`, `RemoveNode`, `GetNode`) are synchronized using a standard lock to ensure thread safety in concurrent environments.

## 4. Complexity Analysis
Let $N$ be the number of physical nodes and $V$ be the replication factor (virtual nodes per physical node). Let $M = N \times V$ be the total number of virtual nodes on the ring.

- **Time Complexity**:
  - **AddNode**: $O(V \log M + V \cdot M)$ due to binary search insertion and array shifting in the sorted list.
  - **RemoveNode**: $O(V \log M + V \cdot M)$ due to binary search lookup and array shifting.
  - **GetNode**: $O(\log M)$ to perform binary search on the sorted keys.
- **Space Complexity**: $O(M)$ to store the virtual node hashes and their mappings.