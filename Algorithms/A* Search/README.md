# A* Search Algorithm

## 1. Introduction

The A* (A-star) Search algorithm is a powerful and commonly used graph traversal and pathfinding algorithm. Its goal is to find the shortest (least costly) path from a start node to a goal node through a weighted graph. A* combines aspects of Dijkstra's algorithm and greedy best-first search by using a heuristic function to estimate the cost to reach the goal, which guides the search toward the target efficiently.

This implementation is designed for generic graphs represented with adjacency lists and supports a customizable heuristic function to accommodate different types of graphs and distance metrics, like Euclidean or Manhattan distances.

Use cases of A*: game development (navigation), robotics (path planning), network routing, and any scenario where efficient shortest pathfinding is needed.

## 2. Usage

Here's how to utilize this implementation:

// Define your node type, for instance string or int:
var graph = new Graph<string>();

var a = new Node<string>("A");
var b = new Node<string>("B");
var c = new Node<string>("C");
var d = new Node<string>("D");

// Add edges (directed), with weights:
graph.AddEdge(a, b, 1.0);
graph.AddEdge(b, c, 2.0);
graph.AddEdge(a, d, 4.0);
graph.AddEdge(d, c, 1.0);

// Define a heuristic function (for instance, zero heuristic, which degrades to Dijkstra):
Func<Node<string>, Node<string>, double> heuristic = (n, goal) => 0.0;

// Find the shortest path:
List<Node<string>> path = AStarSearch.FindPath(graph, a, c, heuristic);

// path now holds the nodes from A to C minimizing total weights

## 3. Detailed Explanation

- **Graph, Node, Edge:** The graph uses adjacency lists to store nodes and their outgoing edges with weights. Nodes are generic, requiring an equality contract.

- **Heuristic:** Passed as a delegate `Func<Node<T>, Node<T>, double>`, enabling the user to provide an admissible heuristic.

- **Priority Queue:** A min-heap-based priority queue is implemented internally to efficiently support the open set in A*.

- **A* Algorithm:** Keeps track of:
  - `gScore`: the cost from the start node to any node currently known.
  - `fScore`: the estimated total cost through that node (g + heuristic).
  - `cameFrom`: the best path to reconstruct the final route.

The algorithm iteratively explores the lowest fScore node, updating neighbors until reaching the goal or exhausting paths.

## 4. Complexity Analysis

- **Time Complexity:**
  - Each node potentially enqueued and dequeued once.
  - Priority queue operations are O(log n) where n is the number of nodes in the open set.
  - Overall: O(E log V) where E is edges and V is vertices, in typical sparse graphs.

- **Space Complexity:**
  - Requires storage for graph adjacency lists: O(V + E).
  - Additionally, dictionaries for `gScore`, `fScore`, and `cameFrom` use O(V).
  - Priority queue worst-case size up to O(V).

This implementation offers a reusable, efficient component for pathfinding in weighted graphs with customizable heuristics, suitable for many practical applications.