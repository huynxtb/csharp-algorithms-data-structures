# Linear Interpolation Search

## Introduction
Linear Interpolation Search estimated array probe indices based on the range of elements, linearly searching adjacent indices or narrowing bounds based on estimated values. Use this algorithm for searching uniformly distributed, sorted numeric arrays where linear proportional estimation yields faster convergence than mid-point division.

## Usage
```csharp
int[] numbers = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
int searchKey = 70;
int index = LinearInterpolationSearch.Search(numbers, searchKey);
// index is 6
```

## Detailed Explanation
1. Check bounds and edge cases (null, empty array, or key outside `array[low]` and `array[high]`).
2. Compute probe position `pos` using linear interpolation: `pos = low + ((key - array[low]) * (high - low)) / (array[high] - array[low])`.
3. Compare `array[pos]` with target key.
4. If equal, return `pos`.
5. Adjust `low` or `high` bounds linearly based on comparison result and recalculate.

## Complexity Analysis
- **Time Complexity**:
  - Best Case: O(1) when element is at predicted interpolation index.
  - Average Case (Uniformly distributed data): O(log log n).
  - Worst Case (Non-uniformly distributed data): O(n).
- **Space Complexity**: O(1) auxiliary space.