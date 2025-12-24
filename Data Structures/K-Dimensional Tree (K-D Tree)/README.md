# K-Dimensional Tree (K-D Tree)

## Introduction

The K-Dimensional Tree (K-D Tree) is a space-partitioning data structure for organizing points in a k-dimensional space. It is widely used in applications involving multi-dimensional data, such as computer graphics, spatial databases, and machine learning. This structure facilitates efficient multidimensional search queries, including nearest neighbor search and range search.

It recursively partitions the space into half-spaces along one dimension at a time. This enables rapid elimination of large portions of the search space during queries.

## Usage

Below is an example of how to use the K-D Tree in C#:

// Create a 3-dimensional k-d tree
KdTree tree = new KdTree(3);

// Insert points
tree.Insert(new double[] { 2.0, 3.0, 5.0 });
tree.Insert(new double[] { 5.0, 4.0, 7.0 });
tree.Insert(new double[] { 9.0, 6.0, 1.0 });
tree.Insert(new double[] { 4.0, 7.0, 2.0 });
tree.Insert(new double[] { 8.0, 1.0, 5.0 });
tree.Insert(new double[] { 7.0, 2.0, 6.0 });

// Nearest neighbor search
double[] queryPoint = new double[] { 9.0, 2.0, 6.0 };
double[] nearest = tree.NearestNeighbor(queryPoint);

// Range search
double[] minPoint = new double[] { 3.0, 2.0, 1.0 };
double[] maxPoint = new double[] { 7.0, 7.0, 7.0 };
List<double[]> pointsInRange = tree.RangeSearch(minPoint, maxPoint);

// Note: The above actions might be in a function or test, this implementation file contains only the KdTree class.

## Detailed Explanation

- Construction and Dimensions:
  The tree is constructed by specifying the dimension `k` for the space. Each node stores a k-dimensional point (array of doubles).

- Insertion:
  Points are inserted recursively. At each tree level (depth), the points are compared on a cyclic axis (dimension) `depth % k`.
  Points less on that axis go to the left subtree; otherwise, to the right.

- Nearest Neighbor Search:
  The nearest neighbor search is performed recursively:
  1. Traverse the tree down to the leaf node that would contain the query point.
  2. Backtrack: Check distances to update the closest point found so far.
  3. For each node, if the hypersphere (based on the best distance so far) intersects the splitting plane, recurse to the opposite subtree as well.

- Range Search:
  This method finds all points inside a multi-dimensional rectangular box (given by min and max points).
  Traversal skips subtrees that cannot possibly intersect the search box.

- Distance Calculation:
  Distance is computed using the squared Euclidean distance for efficiency (avoiding square root computation until necessary).

## Complexity Analysis

- Insertion: O(log n) on average for balanced trees, but worst-case O(n) for unbalanced trees.
- Nearest Neighbor Search: O(log n) average case; worst case O(n).
- Range Search: O(m + log n), where m is the number of points in the result set.
- Space Complexity: O(n), where n is the number of points stored.

This implementation allows efficient multi-dimensional spatial queries ideal for many applications in computational geometry and spatial data processing.