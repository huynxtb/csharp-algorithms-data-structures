# Breadth-First Search (BFS) for Shortest Path in an Unweighted Grid

## Introduction

This implementation provides a Breadth-First Search (BFS) algorithm to find the shortest path length in an unweighted 2D grid or maze. The grid is represented by a matrix where each cell can be open (0) or blocked (1). BFS is ideal for such problems since it explores the grid layer-by-layer, ensuring the shortest path is found efficiently when moving in four cardinal directions.

Use this code when you need to find the fewest steps required to move from a start position to a target position on a grid without weights—typical in puzzle games, robotics pathfinding in uniform-cost grids, and many grid-based simulations.

## Usage

int[,] grid = {
    { 0, 0, 1, 0 },
    { 0, 0, 0, 0 },
    { 1, 1, 0, 1 },
    { 0, 0, 0, 0 }
};

var start = (row: 0, col: 0);
var target = (row: 3, col: 3);

int shortestPathLength = BFSShortestPathInGrid.FindShortestPath(grid, start, target);

// shortestPathLength will be the length of the shortest path or -1 if none exists

## Detailed Explanation

- The grid is a 2D integer array where `0` represents an open cell (walkable), and `1` represents a blocked cell (obstacle).
- The BFS algorithm initializes a queue with the start cell and marks it visited.
- It proceeds by dequeuing cells, checking if the target cell is reached.
- From the current cell, it enqueues all unvisited, open neighbors (up, down, left, right).
- The search continues in waves (levels), counting each BFS layer as one step in the shortest path.
- If the target is reached, the path length is returned immediately.
- If the queue empties without reaching the target, `-1` is returned to indicate no path.

Helper method `IsInBounds` ensures that neighbors are within grid boundaries.

## Complexity Analysis

- **Time Complexity:** O(rows * cols), because every cell is visited at most once.
- **Space Complexity:** O(rows * cols) in the worst case due to the queue and visited tracking.

This implementation balances clarity and correctness, making it a foundational tool for shortest path search in grid-based puzzles or pathfinding scenarios with uniform movement cost.