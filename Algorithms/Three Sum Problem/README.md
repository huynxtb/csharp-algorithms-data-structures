# Three Sum Problem

## Introduction
The Three Sum Problem is a classic algorithmic problem that involves finding all unique triplets in an array of integers that sum up to zero. This problem is particularly useful in various applications such as data analysis, financial modeling, and algorithm design where combinations of elements need to be evaluated.

## Usage
To use the `ThreeSum` method, create an instance of the `ThreeSumSolution` class and call the method with an integer array as follows:
```csharp
ThreeSumSolution solution = new ThreeSumSolution();
int[] nums = {-1, 0, 1, 2, -1, -4};
List<List<int>> triplets = solution.ThreeSum(nums);
```

## Detailed Explanation
The algorithm begins by sorting the input array, which allows for the use of the two-pointer technique. The outer loop iterates through each element in the array, treating it as a potential first element of a triplet. For each first element, two pointers are initialized: one starting just after the current element and the other at the end of the array. The sum of the three elements is calculated, and based on the result, the pointers are adjusted. Duplicate values are skipped to ensure that only unique triplets are added to the result list.

## Complexity Analysis
- **Time Complexity:** O(n^2) - The algorithm involves sorting the array (O(n log n)) and a nested loop with two pointers (O(n)).
- **Space Complexity:** O(1) - The space used is primarily for the output list, which is dependent on the number of unique triplets found, but no additional space is used for computation.