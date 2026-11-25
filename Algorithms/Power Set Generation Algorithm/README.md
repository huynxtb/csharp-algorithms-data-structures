# Power Set Generation Algorithm

## Introduction
The Power Set Generation Algorithm is used to find all possible subsets of a given set of distinct elements. This includes the empty set and the original set itself. For a set with 'n' elements, its power set will contain 2^n distinct subsets. This algorithm is particularly useful in combinatorial problems, where all combinations of a set are needed.

## Usage
Here is an example of how to use the Power Set Generation Algorithm:
```csharp
List<int> inputSet = new List<int> { 1, 2, 3 };
List<List<int>> powerSet = PowerSetGenerator.GeneratePowerSet(inputSet);
```

## Detailed Explanation
The algorithm uses an iterative approach based on bit manipulation. For a set of 'n' elements, there are 2^n subsets. Each subset can be represented by an 'n'-bit binary number, where the 'i'-th bit indicates the presence (1) or absence (0) of the 'i'-th element from the original set. The algorithm iterates through all possible binary numbers from 0 to 2^n - 1, constructing subsets based on the bits that are set.

## Complexity Analysis
- **Time Complexity:** O(n * 2^n), where n is the number of elements in the input set. This is because we generate 2^n subsets and each subset can take up to O(n) time to construct.
- **Space Complexity:** O(n * 2^n) for storing the power set, as we need to store all subsets generated.