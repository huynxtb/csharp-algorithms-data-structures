# Knapsack Problem Solver using Branch and Bound

## Introduction

The Branch and Bound algorithm is a sophisticated optimization technique used to solve the 0/1 Knapsack Problem. Unlike brute-force approaches that examine all 2^n possible combinations, Branch and Bound systematically explores the solution space while intelligently pruning branches that cannot possibly lead to a better solution than the current best.

The algorithm works by:
1. Building a decision tree where each node represents a choice about including or excluding an item
2. Calculating an upper bound on the maximum profit achievable from each node using fractional knapsack
3. Pruning branches where the bound is less than or equal to the current best solution
4. Tracking which items are selected to provide the complete optimal solution

This approach is particularly efficient when the problem instance has good pruning characteristics, making it practical for many real-world knapsack problems while maintaining optimality guarantees.

## Usage

```csharp
KnapsackBranchAndBound solver = new KnapsackBranchAndBound();

int[] weights = { 2, 3, 4, 5 };
int[] values = { 3, 4, 5, 6 };
int capacity = 8;

KnapsackResult result = solver.Solve(weights, values, capacity);

Console.WriteLine($"Maximum Value: {result.MaxValue}");
Console.WriteLine($"Total Weight: {result.TotalWeight}");
Console.WriteLine("Selected Items (indices):");
foreach (int itemIndex in result.SelectedItems)
{
    Console.WriteLine($"  Item {itemIndex}: Weight={weights[itemIndex]}, Value={values[itemIndex]}");
}
```

## Detailed Explanation

### Algorithm Structure

The implementation consists of several key components:

**Item Class**: Represents each item with its weight, value, original index, and value-to-weight ratio. The ratio is crucial for sorting items to improve pruning efficiency.

**Node Class**: Represents a node in the decision tree, containing:
- Level: Which item is currently being considered
- Profit: Accumulated value from selected items
- Weight: Accumulated weight from selected items
- Bound: Upper bound on maximum profit achievable from this node
- ItemsIncluded: List of indices of items selected along this path

**KnapsackResult Class**: Encapsulates the final solution with maximum value, selected item indices, and total weight.

### Core Algorithm Steps

1. **Initialization**: 
   - Create an Item array from input weights and values
   - Calculate value-to-weight ratio for each item
   - Sort items by ratio in descending order (this improves pruning)
   - Create root node and calculate its bound

2. **Tree Exploration**:
   - Use a queue to process nodes (level-by-level or breadth-first exploration)
   - For each node at level i (considering item i):
     - Generate two child nodes: one including item i, one excluding it
     - Only add child nodes to the queue if their bound exceeds the current best solution

3. **Bound Calculation**:
   - Uses fractional knapsack to compute an upper bound
   - For items at and after the current level, includes them greedily by ratio
   - The last item may be partially included (fractional knapsack)
   - If bound ≤ current best, the entire subtree is pruned

4. **Solution Tracking**:
   - Maintains ItemsIncluded list at each node to track selected items
   - When a complete solution is found (level == n), updates best solution if profit is higher
   - Returns the best solution found

### Time Complexity

**Best Case**: O(n log n)
- When significant pruning occurs early (e.g., tight capacity constraint)
- Primarily spent on sorting items

**Average Case**: O(n·2^k)
- Where k is typically much smaller than n due to effective pruning
- Depends heavily on problem structure and pruning effectiveness

**Worst Case**: O(n·2^n)
- When pruning is ineffective (e.g., items with similar ratios, loose capacity constraints)
- Approaches brute force in degenerate cases
- Space: O(n·2^n) for storing the entire search tree in worst case

### Space Complexity

**Auxiliary Space**: O(n·d)
- Where d is the depth of the search tree being explored
- Queue stores nodes, each maintaining a list of selected items
- In worst case, O(n·2^n) to store all unexplored nodes

### Key Optimizations

1. **Sorting by Ratio**: Items are pre-sorted by value-to-weight ratio, which improves bound tightness and pruning effectiveness

2. **Upper Bound Calculation**: The fractional knapsack heuristic provides tight upper bounds, enabling aggressive pruning

3. **Early Termination**: Nodes with bounds not exceeding the current best are immediately discarded

4. **Efficient Path Tracking**: ItemsIncluded lists are copied when branching, ensuring correct solution reconstruction

## Complexity Analysis

**Time Complexity**:
- **Preprocessing**: O(n log n) for sorting items by ratio
- **Main Algorithm**: O(n × B) where B is the number of nodes explored
  - In practice, B ≪ 2^n due to pruning
  - Can approach O(n·2^n) in worst case without effective pruning
  - Often performs much better than dynamic programming for sparse or tightly-constrained problems

**Space Complexity**:
- **Item Storage**: O(n)
- **Queue and Node Storage**: O(Q) where Q is maximum queue size
  - Can be O(2^n) in worst case
  - Typically much smaller due to pruning
- **Total**: O(n + Q), worst case O(n·2^n)

**Comparison with Dynamic Programming**:
- DP: Always O(n·W) time and space where W is capacity
- B&B: Better when W is large relative to pruning effectiveness
- B&B: Worse when pruning is ineffective (similar items, loose constraints)

**Edge Cases**:
- Empty input: Returns 0 value, 0 weight
- Zero capacity: Returns 0 value, 0 weight
- All items exceed capacity: Returns 0 value, 0 weight
- Single item fitting: Returns that item's value
- Multiple optimal solutions: Returns one optimal solution (the first found)