# 0/1 Knapsack Problem Solver

## 1. Introduction
The 0/1 Knapsack Problem is a combinatorial optimization problem. Given a set of items, each with a weight and a value, determine the subset of items to include in a knapsack such that the total weight does not exceed a given capacity and the total value is maximized. Each item may be chosen at most once (0 or 1).

## 2. Usage

```csharp
using System;
using System.Collections.Generic;
using KnapsackSolver;

var items = new List<KnapsackItem>
{
    new(Weight: 2, Value: 12, Id: "item-1"),
    new(Weight: 1, Value: 10, Id: "item-2"),
    new(Weight: 3, Value: 20, Id: "item-3"),
    new(Weight: 2, Value: 15, Id: "item-4")
};

int capacity = 5;
KnapsackResult result = Knapsack01.Solve(items, capacity);

// Output:
// Total Value: 37
// Total Weight: 5
// Selected: item-2, item-4, item-3
```

## 3. Detailed Explanation
The solver constructs a two-dimensional dynamic programming table `dp[n + 1, capacity + 1]` where `dp[i, w]` represents the maximum value achievable using a subset of the first `i` items with a maximum weight bound `w`.

- **Base Case**: `dp[0, w] = 0` for all `w`, and `dp[i, 0] = 0` for all `i`.
- **State Transition**:
  - If `items[i - 1].Weight <= w`:
    `dp[i, w] = max(dp[i - 1, w], dp[i - 1, w - items[i - 1].Weight] + items[i - 1].Value)`
  - Otherwise: `dp[i, w] = dp[i - 1, w]`
- **Path Reconstruction**: Starts at `dp[n, capacity]`. If `dp[i, w] != dp[i - 1, w]`, the `i`-th item was included. The item is appended to the result, capacity decreases by `items[i - 1].Weight`, and the traversal moves to `i - 1`.

## 4. Complexity Analysis
- **Time Complexity**:
  - Table Construction: $O(N \times W)$, where $N$ is the number of items and $W$ is the maximum knapsack capacity.
  - Backtracking / Path Reconstruction: $O(N)$.
  - Overall Time Complexity: $O(N \times W)$.
- **Space Complexity**:
  - DP Table: $O(N \times W)$ to enable deterministic backtrack reconstruction.
  - Result Collection: $O(N)$ auxiliary memory.