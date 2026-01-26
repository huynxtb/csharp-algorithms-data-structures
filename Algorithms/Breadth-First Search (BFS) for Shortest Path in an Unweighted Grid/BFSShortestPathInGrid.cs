using System.Collections.Generic;

public static class BFSShortestPathInGrid
{
    /// <summary>
    /// Finds the shortest path length from start to target in an unweighted 2D grid using BFS.
    /// </summary>
    /// <param name="grid">2D integer array representing the grid (0 = open cell, 1 = blocked cell)</param>
    /// <param name="start">Tuple (row, col) for the start position</param>
    /// <param name="target">Tuple (row, col) for the target position</param>
    /// <returns>Length of the shortest path or -1 if no path exists</returns>
    public static int FindShortestPath(int[,] grid, (int row, int col) start, (int row, int col) target)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Validate start and target positions
        if (!IsInBounds(start.row, start.col, rows, cols) || !IsInBounds(target.row, target.col, rows, cols))
            return -1;

        if (grid[start.row, start.col] == 1 || grid[target.row, target.col] == 1)
            return -1;

        bool[,] visited = new bool[rows, cols];
        var queue = new Queue<(int row, int col)>();

        queue.Enqueue(start);
        visited[start.row, start.col] = true;

        // Directions: up, down, left, right
        int[] dRow = new int[] { -1, 1, 0, 0 };
        int[] dCol = new int[] { 0, 0, -1, 1 };

        int pathLength = 0;

        while (queue.Count > 0)
        {
            int size = queue.Count;

            for (int i = 0; i < size; i++)
            {
                var current = queue.Dequeue();

                if (current == target)
                    return pathLength;

                for (int dir = 0; dir < 4; dir++)
                {
                    int newRow = current.row + dRow[dir];
                    int newCol = current.col + dCol[dir];

                    if (IsInBounds(newRow, newCol, rows, cols) && !visited[newRow, newCol] && grid[newRow, newCol] == 0)
                    {
                        visited[newRow, newCol] = true;
                        queue.Enqueue((newRow, newCol));
                    }
                }
            }

            pathLength++;
        }

        return -1; // no path found
    }

    private static bool IsInBounds(int row, int col, int totalRows, int totalCols)
    {
        return row >= 0 && row < totalRows && col >= 0 && col < totalCols;
    }
}