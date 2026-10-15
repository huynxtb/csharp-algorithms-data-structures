# Radix Sort Algorithm (Least Significant Digit)

## 1. Introduction

Radix Sort is a non-comparative sorting algorithm that sorts numbers by processing individual digits. Unlike comparison-based algorithms like Quick Sort or Merge Sort, Radix Sort distributes elements into buckets based on their digits, making it exceptionally efficient for sorting large collections of integers.

### When to Use Radix Sort:
- Sorting large arrays of non-negative integers
- When you need linear time complexity (O(n*k) where k is the number of digits)
- When the number of digits is relatively small and known
- For sorting strings lexicographically
- In applications requiring stable sorting with predictable performance

### Key Characteristics:
- **Non-comparative**: Does not compare elements directly
- **Stable**: Maintains the relative order of equal elements
- **LSD Approach**: Processes digits from the Least Significant Digit (rightmost) to the Most Significant Digit (leftmost)
- **Counting Sort Subroutine**: Uses Counting Sort for each digit position

## 2. Usage

```csharp
// Example 1: Sorting a simple array
int[] numbers = { 170, 45, 75, 90, 2, 802, 24, 2, 66 };
int[] sortedNumbers = RadixSort.Sort(numbers);
// Result: { 2, 2, 24, 45, 66, 75, 90, 170, 802 }

// Example 2: Sorting an already sorted array
int[] alreadySorted = { 1, 2, 3, 4, 5 };
int[] result = RadixSort.Sort(alreadySorted);
// Result: { 1, 2, 3, 4, 5 }

// Example 3: Sorting reverse ordered array
int[] reversed = { 100, 50, 30, 20, 10 };
int[] sortedReversed = RadixSort.Sort(reversed);
// Result: { 10, 20, 30, 50, 100 }

// Example 4: Single element or empty array
int[] single = { 42 };
int[] singleResult = RadixSort.Sort(single);
// Result: { 42 }
```

## 3. Detailed Explanation

### How Radix Sort Works (LSD Approach)

The algorithm processes integers digit by digit, starting from the least significant digit (rightmost) and moving towards the most significant digit (leftmost). Here's a step-by-step breakdown:

#### Step 1: Find Maximum Number
First, identify the maximum number in the array to determine how many digit positions need to be processed.

#### Step 2: Process Each Digit Position
For each digit position (ones, tens, hundreds, etc.):
- Extract the digit at that position from each number
- Use Counting Sort to arrange elements based on that digit
- Maintain stability so previously sorted positions remain intact

#### Step 3: Counting Sort Subroutine
For each digit position, Counting Sort performs three operations:

**3a. Count Phase**: Create a frequency array to count how many numbers have each digit (0-9) at the current position.

**3b. Cumulative Count Phase**: Convert the frequency array into cumulative counts. This tells us the starting position for each digit group in the output array.

**3c. Place Elements Phase**: Iterate through the original array in reverse order and place each element at its correct position in the output array based on its digit. Processing in reverse maintains stability.

#### Example Walkthrough
```
Array: [170, 45, 75, 90, 2, 802, 24, 2, 66]

Digit Position 1 (Ones place):
  Group by ones digit:
  0: [170, 90]
  2: [2, 802, 2]
  4: [24]
  5: [45, 75]
  6: [66]
  Result: [170, 90, 2, 802, 2, 24, 45, 75, 66]

Digit Position 10 (Tens place):
  Applied to result from above
  Result: [2, 2, 24, 45, 66, 75, 90, 170, 802]

Digit Position 100 (Hundreds place):
  Applied to result from above
  Result: [2, 2, 24, 45, 66, 75, 90, 170, 802]
```

### Key Implementation Details

1. **Edge Case Handling**: The algorithm handles null, empty, and single-element arrays gracefully.

2. **Digit Extraction**: Uses the formula `(number / digitPlace) % 10` to extract the digit at a specific position.

3. **Stability**: Achieved by processing elements in reverse order during placement, ensuring that elements with the same digit maintain their relative order.

4. **Array Cloning**: The original array is not modified; a copy is created instead.

## 4. Complexity Analysis

### Time Complexity

- **Best Case**: O(n*k) where n is the number of elements and k is the number of digits in the maximum number
- **Average Case**: O(n*k)
- **Worst Case**: O(n*k)

The time complexity is linear with respect to both the number of elements and the number of digits. For a fixed number of digits, the algorithm runs in linear time O(n).

### Space Complexity

- **O(n + d)** where n is the number of elements and d is the number of unique digits (usually 10 for decimal numbers)
- The algorithm requires:
  - O(n) space for the output array
  - O(d) space for the digit count array (constant O(10) for base-10 numbers)
  - O(n) space for the cloned input array

### Comparison with Other Sorting Algorithms

| Algorithm | Time Complexity | Space Complexity | Stable | In-Place |
|-----------|-----------------|------------------|--------|----------|
| Radix Sort | O(n*k) | O(n+d) | Yes | No |
| Quick Sort | O(n log n) | O(log n) | No | Yes |
| Merge Sort | O(n log n) | O(n) | Yes | No |
| Heap Sort | O(n log n) | O(1) | No | Yes |
| Counting Sort | O(n+k) | O(n+k) | Yes | No |

### When Radix Sort Outperforms Comparison-Based Sorts

Radix Sort is more efficient than comparison-based algorithms when:
- k (number of digits) is small relative to log(n)
- You have large datasets with a known fixed digit range
- You need guaranteed linear-time performance
- The overhead of comparison-based sorting dominates the execution time