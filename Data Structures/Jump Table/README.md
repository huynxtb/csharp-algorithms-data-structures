# Jump Table

## Introduction
The Jump Table is a data structure used to improve the efficiency of searching for elements in a sorted array. It allows the search to 'jump' over sections of the array, thus reducing the number of comparisons needed to find an element. This is particularly useful for large datasets where traditional linear search methods would be inefficient.

## Usage
```csharp
List<int> sortedList = new List<int> { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
JumpTable jumpTable = new JumpTable(sortedList);
int index = jumpTable.Search(7); // Returns the index of the element 7
jumpTable.Insert(10); // Inserts 10 into the Jump Table
```

## Detailed Explanation
The Jump Table implementation consists of a list of sorted elements and an array of jump indices. The jump indices are calculated based on the square root of the number of elements, allowing the search to skip over sections of the array. When searching for an element, the algorithm first jumps through the jump indices to find the appropriate block, and then performs a linear search within that block. The insert method adds a new element to the list and reinitializes the jump indices to maintain the structure.

## Complexity Analysis
- **Search Operation:** Average time complexity is O(√n) due to the jump mechanism, with a worst-case scenario of O(n) when the element is not found. 
- **Insert Operation:** Time complexity is O(n log n) due to the need to sort the list after insertion. 
- **Space Complexity:** O(√n) for the jump indices storage.