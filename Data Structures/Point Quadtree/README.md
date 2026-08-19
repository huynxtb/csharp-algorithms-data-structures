# Point Quadtree

### 1. Introduction
A Point Quadtree is a 2D spatial partitioning tree structure where each internal node has exactly four children. It is used to index points in a two-dimensional space, enabling efficient spatial queries such as range searches and nearest neighbor lookups.

### 2. Usage
```csharp
// Define the boundary of the quadtree (center X, center Y, half-width, half-height)
Boundary boundary = new Boundary(0, 0, 100, 100);
Quadtree<string> quadtree = new Quadtree<string>(boundary, capacity: 4);

// Insert points with associated data
quadtree.Insert(new Point2D(10, 10), "Point A");
quadtree.Insert(new Point2D(-20, 30), "Point B");
quadtree.Insert(new Point2D(50, -50), "Point C");

// Query points within a specific range
Boundary queryRange = new Boundary(0, 0, 25, 25);
List<QuadtreeItem<string>> results = quadtree.QueryRange(queryRange);

foreach (var item in results)
{
    Console.WriteLine($"Found {item.Data} at ({item.Point.X}, {item.Point.Y})");
}
```

### 3. Detailed Explanation
- **Point2D**: A lightweight struct representing a coordinate pair `(X, Y)`.
- **Boundary**: An Axis-Aligned Bounding Box (AABB) defined by its center coordinates and half-dimensions. It provides methods to check if a point lies within its bounds (`Contains`) and if it overlaps with another boundary (`Intersects`).
- **QuadtreeItem<T>**: A container class holding a `Point2D` and its associated generic data payload.
- **Quadtree<T>**: The core class managing spatial partitioning. When the number of items in a node exceeds its capacity, the node subdivides into four child quadrants (North-West, North-East, South-West, South-East) and redistributes its items to the appropriate children.

### 4. Complexity Analysis
- **Space Complexity**: $O(N)$ where $N$ is the number of points stored.
- **Time Complexity**:
  - **Insertion**: Average $O(\log N)$, Worst $O(N)$ if the tree becomes highly unbalanced (e.g., all points clustered in one spot).
  - **Range Query**: Average $O(\log N + K)$ where $K$ is the number of points matching the query, Worst $O(N)$ if the query covers the entire tree or the tree is unbalanced.