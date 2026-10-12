# Odd-Even Transposition Sort (Brick Sort)

## Introduction

Odd-Even Transposition Sort, also known as Brick Sort, is a comparison-based sorting algorithm that is a variation of Bubble Sort. Unlike traditional Bubble Sort which compares consecutive adjacent elements in each pass, Odd-Even Transposition Sort alternates between two phases:

- **Odd Phase**: Compares and swaps elements at positions (1, 2), (3, 4), (5, 6), etc.
- **Even Phase**: Compares and swaps elements at positions (0, 1), (2, 3), (4, 5), etc.

This alternating pattern allows for better parallelization because elements in different phases can be processed independently without conflicts. The algorithm terminates when a complete pass (both odd and even phases) produces no swaps, indicating the array is fully sorted.

### When to Use It

- When you need a simple, in-place sorting algorithm with minimal auxiliary space requirements.
- When parallelization is available and important (each phase can be parallelized).
- When stability is not strictly required (it is unstable).
- When you need guaranteed O(n²) behavior and can tolerate it.
- In distributed or multi-core systems where the phase-based structure provides natural parallelization opportunities.

## Usage

```csharp
// Example 1: Sort integers in ascending order
int[] numbers = { 5, 2, 8, 1, 9, 3 };
OddEvenTranspositionSort.Sort(numbers);
// Result: { 1, 2, 3, 5, 8, 9 }

// Example 2: Sort strings in ascending order
string[] words = { "dog", "cat", "apple", "zebra" };
OddEvenTranspositionSort.Sort(words);
// Result: { "apple", "cat", "dog", "zebra" }

// Example 3: Sort in descending order
int[] descending = { 5, 2, 8, 1, 9, 3 };
OddEvenTranspositionSort.SortDescending(descending);
// Result: { 9, 8, 5, 3, 2, 1 }

// Example 4: Sort with custom comparer
var people = new[] {
    new { Name = "Alice", Age = 30 },
    new { Name = "Bob", Age = 25 },
    new { Name = "Charlie", Age = 35 }
};
OddEvenTranspositionSort.Sort(people, Comparer<dynamic>.Create((a, b) => a.Age.CompareTo(b.Age)));

// Example 5: Sort with comparison delegate
int[] data = { 3, 1, 4, 1, 5, 9 };
OddEvenTranspositionSort.Sort(data, (a, b) => a.CompareTo(b));
```

## Detailed Explanation

The algorithm works by repeatedly sweeping through the array with alternating phases:

1. **Initialization**: Start with a flag `swapped` set to `true`, indicating that at least one more pass is needed.

2. **Odd Phase**: Iterate through indices 1, 3, 5, 7, ... (odd-indexed elements paired with their right neighbors). Compare each pair and swap if they are out of order relative to the comparer. If any swap occurs, set `swapped` to `true`.

3. **Even Phase**: Iterate through indices 0, 2, 4, 6, ... (even-indexed elements paired with their right neighbors). Compare each pair and swap if they are out of order. If any swap occurs, set `swapped` to `true`.

4. **Termination**: Repeat both phases until a complete pass (odd + even phases) occurs with `swapped` remaining `false`, meaning no swaps were made and the array is sorted.

### Key Properties

- **In-Place**: The algorithm modifies the input array directly with O(1) auxiliary space.
- **Comparison-Based**: Uses element comparisons to determine order; can work with any comparable type.
- **Unstable**: Does not preserve the relative order of equal elements.
- **Parallelizable**: Each phase can be parallelized since elements in different pairs within a phase don't conflict.
- **Adaptive**: In the best case (already sorted array), it runs in O(n) time as only one pass is needed.

### Example Walkthrough

Sorting `[5, 2, 8, 1, 9, 3]`:

```
Initial: [5, 2, 8, 1, 9, 3]

Odd Phase 1 (compare indices 1-2, 3-4, 5-end):
  Compare 2, 8: 2 < 8, no swap → [5, 2, 8, 1, 9, 3]
  Compare 1, 9: 1 < 9, no swap → [5, 2, 8, 1, 9, 3]
  Result: [5, 2, 8, 1, 9, 3]

Even Phase 1 (compare indices 0-1, 2-3, 4-5):
  Compare 5, 2: 5 > 2, swap → [2, 5, 8, 1, 9, 3]
  Compare 8, 1: 8 > 1, swap → [2, 5, 1, 8, 9, 3]
  Compare 9, 3: 9 > 3, swap → [2, 5, 1, 8, 3, 9]
  Result: [2, 5, 1, 8, 3, 9]

... (continues alternating odd and even phases until sorted)

Final: [1, 2, 3, 5, 8, 9]
```

## Complexity Analysis

### Time Complexity

- **Worst Case**: O(n²) - When the array is sorted in reverse order, the algorithm requires approximately n² comparisons and swaps.
- **Average Case**: O(n²) - For randomly ordered arrays, the algorithm typically requires O(n²) operations.
- **Best Case**: O(n) - When the array is already sorted, only one complete pass (odd + even phases) is needed with n comparisons and no swaps.

### Space Complexity

- **O(1)** - The algorithm sorts in-place using only a constant amount of extra space for the temporary swap variable and loop counters.

### Comparison with Related Algorithms

- **vs. Bubble Sort**: Odd-Even Transposition Sort is more parallelizable but has similar worst-case complexity.
- **vs. QuickSort**: QuickSort is faster on average (O(n log n)) but has O(n²) worst case and uses more space.
- **vs. Merge Sort**: Merge Sort is faster (O(n log n)) but requires O(n) additional space.
- **vs. Insertion Sort**: Insertion Sort can be faster on small or nearly-sorted arrays but is harder to parallelize.

The main advantage of Odd-Even Transposition Sort is its suitability for parallel processing architectures, making it valuable in multi-core and distributed computing environments.