using System;
using System.Collections.Generic;
public class DStarAlgorithm
{
    private int[,] grid;
    private int rows;
    private int cols;
    private int startX;
    private int startY;
    private int endX;
    private int endY;
    private bool[,] obstacles;
    private List<Node> openList;
    private List<Node> closedList;
    private Dictionary<string, Node> allNodes;
    private int k_m;
    private int rhe;
    
    public DStarAlgorithm(int[,] grid, int startX, int startY, int endX, int endY)
    {
        this.grid = grid;
        this.rows = grid.GetLength(0);
        this.cols = grid.GetLength(1);
        this.startX = startX;
        this.startY = startY;
        this.endX = endX;
        this.endY = endY;
        this.obstacles = new bool[rows, cols];
        this.openList = new List<Node>();
        this.closedList = new List<Node>();
        this.allNodes = new Dictionary<string, Node>();
        this.k_m = 0;
        this.rhe = 0;
    }
    
    public void AddObstacle(int x, int y)
    {
        obstacles[x, y] = true;
    }
    
    public List<Node> Replan()
    {
        // Initialize the start and end nodes
        Node startNode = new Node(startX, startY, 0);
        Node endNode = new Node(endX, endY, 0);
        
        // Initialize the open and closed lists
        openList.Clear();
        closedList.Clear();
        allNodes.Clear();
        
        // Add the start node to the open list
        openList.Add(startNode);
        allNodes.Add(startNode.Key, startNode);
        
        while (openList.Count > 0)
        {
            // Get the node with the lowest cost from the open list
            Node currentNode = openList[0];
            openList.RemoveAt(0);
            closedList.Add(currentNode);
            
            // Check if the current node is the end node
            if (currentNode.X == endX && currentNode.Y == endY)
            {
                // Reconstruct the path
                List<Node> path = new List<Node>();
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.Parent;
                }
                path.Reverse();
                return path;
            }
            
            // Explore the neighbors of the current node
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (Math.Abs(dx) + Math.Abs(dy) != 1) continue;
                    int x = currentNode.X + dx;
                    int y = currentNode.Y + dy;
                    
                    // Check if the neighbor is within the grid boundaries and not an obstacle
                    if (x >= 0 && x < rows && y >= 0 && y < cols && !obstacles[x, y])
                    {
                        // Calculate the cost of the neighbor
                        int cost = currentNode.Cost + 1;
                        
                        // Check if the neighbor is already in the allNodes dictionary
                        if (allNodes.ContainsKey(x + "," + y))
                        {
                            Node neighborNode = allNodes[x + "," + y];
                            
                            // Check if the calculated cost is less than the stored cost
                            if (cost < neighborNode.Cost)
                            {
                                // Update the cost and parent of the neighbor node
                                neighborNode.Cost = cost;
                                neighborNode.Parent = currentNode;
                                
                                // Check if the neighbor node is in the closed list
                                if (closedList.Contains(neighborNode))
                                {
                                    closedList.Remove(neighborNode);
                                }
                                
                                // Add the neighbor node to the open list
                                openList.Add(neighborNode);
                            }
                        }
                        else
                        {
                            // Create a new neighbor node
                            Node neighborNode = new Node(x, y, cost);
                            neighborNode.Parent = currentNode;
                            allNodes.Add(x + "," + y, neighborNode);
                            openList.Add(neighborNode);
                        }
                    }
                }
            }
        }
        
        // If the end node is not reachable, return an empty list
        return new List<Node>();
    }
}

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Cost { get; set; }
    public Node Parent { get; set; }
    public string Key { get { return X + "," + Y; } }
    
    public Node(int x, int y, int cost)
    {
        X = x;
        Y = y;
        Cost = cost;
    }
}