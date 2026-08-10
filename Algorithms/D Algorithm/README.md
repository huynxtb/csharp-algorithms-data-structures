# Introduction
The D* algorithm is a well-known pathfinding algorithm used for dynamic replanning in partially known or changing environments. It is an extension of the A* algorithm, designed to handle changes in the environment after the initial plan has been generated.

# Usage
```csharp
DStarAlgorithm algorithm = new DStarAlgorithm(new int[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, 0, 0, 2, 2);
algorithm.AddObstacle(1, 1);
List<Node> path = algorithm.Replan();
foreach (Node node in path)
{
    Console.WriteLine(node.X + ", " + node.Y);
}
```

# Detailed Explanation
The D* algorithm works by maintaining a list of open nodes and a list of closed nodes. The open list contains nodes that have been discovered but not yet explored, while the closed list contains nodes that have been fully explored. The algorithm starts by adding the start node to the open list and then iteratively explores the neighbors of the node with the lowest cost in the open list. If a neighbor is not in the allNodes dictionary, it is added to the open list. If a neighbor is already in the allNodes dictionary but has a higher cost than the calculated cost, its cost and parent are updated, and it is added to the open list. The algorithm continues until the end node is reached or the open list is empty.

# Complexity Analysis
The time complexity of the D* algorithm is O(b^d), where b is the branching factor and d is the depth of the search. The space complexity is O(b^d) as well, since in the worst case, the algorithm needs to store all nodes in the open and closed lists.