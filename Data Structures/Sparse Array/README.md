# Sparse Array

## Introduction
The Sparse Array is a data structure designed to efficiently handle and store large arrays that contain a significant number of default or empty values (typically zeros or nulls). It is particularly useful when dealing with datasets where only a few elements are non-default, allowing for memory-efficient storage.

## Usage
```csharp
SparseArray<int> sparseArray = new SparseArray<int>(100);
sparseArray.Set(10, 5);
sparseArray.Set(20, 15);
int valueAt10 = sparseArray.Get(10); // returns 5
int valueAt30 = sparseArray.Get(30); // returns 0 (default value)
int nonDefaultCount = sparseArray.CountNonDefaultElements(); // returns 2
int[] fullArray = sparseArray.ToArray(); // returns an array of size 100 with 5 at index 10 and 15 at index 20
```

## Detailed Explanation
The Sparse Array class utilizes a dictionary to store only the non-default values along with their indices. The constructor initializes the array size and the default value. The `Set` method allows setting a value at a specific index, while the `Get` method retrieves the value at an index, returning the default if not set. The `CountNonDefaultElements` method counts the number of non-default entries, and `ToArray` converts the sparse representation back into a full array.

## Complexity Analysis
- **Set:** O(1) on average for inserting into the dictionary, O(n) in the worst case if resizing occurs.
- **Get:** O(1) on average for retrieving from the dictionary.
- **CountNonDefaultElements:** O(1) since it returns the count of the dictionary.
- **ToArray:** O(n) as it iterates through the dictionary to construct the full array.