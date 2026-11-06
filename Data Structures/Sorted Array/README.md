# Sorted Array

## Introduction
The Sorted Array is a data structure that maintains a collection of integers in a sorted order. It allows for efficient insertion, deletion, and searching of elements while ensuring that the order is preserved. This structure is particularly useful when frequent searches are required, and the dataset is relatively static.

## Usage
```csharp
SortedArray sortedArray = new SortedArray();
sortedArray.Insert(5);
sortedArray.Insert(3);
sortedArray.Insert(8);
Console.WriteLine(sortedArray.Search(3)); // Output: 0
sortedArray.Delete(3);
Console.WriteLine(sortedArray.Size()); // Output: 2
```

## Detailed Explanation
The `SortedArray` class uses an internal array to store integers. It starts with a default capacity and dynamically resizes when the capacity is reached. The `Insert` method uses binary search to find the correct index for the new element, shifting elements as necessary to maintain order. The `Delete` method finds the element and shifts the remaining elements to fill the gap. The `Search` method also uses binary search to find the index of a given integer. The `Size` method returns the current number of elements in the array.

## Complexity Analysis
- **Insertion:** O(n) in the worst case due to shifting elements, but O(log n) for finding the index using binary search.
- **Deletion:** O(n) due to shifting elements, but O(log n) for finding the index.
- **Search:** O(log n) due to binary search.
- **Space Complexity:** O(n) for storing the elements in the array.