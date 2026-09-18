# R-Tree Spatial Index (Quadratic Split)

An R-Tree is a balanced, tree-like data structure designed for indexing multi-dimensional information, such as geographical coordinates, bounding boxes, or polygons in 2D space. It is widely used in spatial databases, GIS software, and game physics engines for fast range queries, spatial intersection tests, and nearest-neighbor searches.

This implementation uses **Guttman's Quadratic Split** algorithm to balance nodes when overflow occurs.

---

## Usage Example

```csharp
// Create an RTree instance with min capacity 2 and max capacity 4
var tree = new RTree<string>(minEntries: 2, maxEntries: 4);

// Insert items with their 2D bounding boxes (minX, minY, maxX, maxY)
tree.Insert(new Rectangle2D(0.0, 0.0, 2.0, 2.0), "Building A");
tree.Insert(new Rectangle2D(3.0, 3.0, 6.0, 6.0), "Building B");
tree.Insert(new Rectangle2D(1.0, 1.0, 4.0, 4.0), "Park C");

// Perform a range / bounding box search
var queryWindow = new Rectangle2D(0.5, 0.5, 2.5, 2.5);
List<string> found = tree.Search(queryWindow);
// Result: contains "Building A" and "Park C"

// Find nearest neighbor to a point
var nearest = tree.NearestNeighbor(2.5, 2.5);
if (nearest.HasValue)
{
    string item = nearest.Value.Item;
    double distanceSq = nearest.Value.DistanceSquared;
}

// Remove an item
bool removed = tree.Remove(new Rectangle2D(0.0, 0.0, 2.0, 2.0), "Building A");
```

---

## Detailed Explanation

### 1. Structure
- **Bounding Box (`Rectangle2D`)**: Represents an axis-aligned 2D bounding rectangle defined by `(MinX, MinY, MaxX, MaxY)`.
- **Nodes (`RTreeNode<T>`)**: Can be leaf nodes (holding entries with spatial bounds and payload data `T`) or internal non-leaf nodes (holding child node pointers wrapped with bounding envelopes).

### 2. Quadratic Split Algorithm
When a node exceeds `maxEntries` ($M$), it is partitioned into two nodes:
- **`PickSeeds`**: Evaluates every pair of entries to find the pair $(E_1, E_2)$ that produces the maximum "wasted area" (i.e., `Area(Union(E1, E2)) - Area(E1) - Area(E2)`). These two entries become the initial seeds of Group 1 and Group 2.
- **`PickNext`**: For all remaining unassigned entries, it calculates the enlargement needed to incorporate each entry into Group 1 and Group 2. The entry with the highest difference in area enlargement is selected and added to the group that requires less enlargement.

### 3. Deletion & Re-balancing
- Locates the leaf node containing the item and removes it.
- If node count drops below `minEntries` ($m$), the node is eliminated, its remaining sibling entries are temporarily saved, and reinserted back into the tree starting from the root.

---

## Complexity Analysis

- **Search Query Time Complexity**: $O(\log_M N)$ average case for selective queries; $O(N)$ worst case if all bounding boxes overlap extensively.
- **Insertion Time Complexity**: $O(M^2 \log_M N)$ due to the quadratic split heuristic on node splits along the path of height $O(\log_M N)$.
- **Deletion Time Complexity**: $O(M^2 \log_M N)$ due to tree condensation and re-insertion of underflowing sibling nodes.
- **Space Complexity**: $O(N)$ where $N$ is the number of stored spatial items.