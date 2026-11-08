# Octree

## Introduction
An Octree is a tree data structure used to partition a three-dimensional space by recursively subdividing it into eight octants. It is commonly employed for spatial indexing, efficient collision detection, visible surface determination, and storing sparse 3D data in applications like game development, graphics rendering, and computational geometry.

## Usage
```csharp
var bounds = new AxisAlignedBoundingBox(new Point3D(0, 0, 0), new Point3D(100, 100, 100));
var octree = new Octree<Point3D>(bounds, 5, 10);
octree.Insert(new Point3D(10, 10, 10));
var queryBounds = new AxisAlignedBoundingBox(new Point3D(5, 5, 5), new Point3D(15, 15, 15));
var results = octree.Query(queryBounds);
```

## Detailed Explanation
The Octree is implemented with a generic node class `OctreeNode<T>` that contains a bounding box, a list of items, and an array of child nodes. The `Insert` method adds items to the tree, subdividing nodes when necessary based on the maximum items per node and maximum depth. The `Query` method retrieves items within a specified bounding box, ensuring efficient spatial partitioning by only checking relevant nodes.

## Complexity Analysis
- **Insertion:** Average O(log n), worst O(n) when all items are in one node.
- **Query:** Average O(log n), worst O(n) if all items are in the queried region.
- **Space Complexity:** O(n) for storing items in the tree.