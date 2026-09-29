# SkipArray

## Introduction
SkipArray is a data structure designed to maintain a sorted array while allowing for efficient search, insert, and delete operations. It leverages a skip list-like structure to provide average-case O(log n) time complexity for these operations, making it suitable for scenarios where frequent modifications and lookups are required.

## Usage
```csharp
SkipArray skipArray = new SkipArray();
skipArray.Insert(10);
skipArray.Insert(20);
skipArray.Insert(15);
Console.WriteLine(skipArray.Search(15)); // Output: True
skipArray.Delete(15);
Console.WriteLine(skipArray.Search(15)); // Output: False
```

## Detailed Explanation
The SkipArray maintains an internal array that is dynamically resized as needed. When inserting a new value, it uses binary search to find the correct index for the new element, ensuring that the array remains sorted. If the array reaches a certain load factor, it is resized to accommodate more elements. Deletion is also performed using binary search to locate the element, followed by shifting elements to fill the gap. The search operation utilizes binary search to quickly determine if an element exists in the array.

## Complexity Analysis
- **Insert:** O(n) in the worst case due to shifting elements, but average-case O(log n) due to binary search for the insertion point.
- **Delete:** O(n) in the worst case due to shifting elements, but average-case O(log n) due to binary search for locating the element.
- **Search:** O(log n) due to binary search.
- **Space Complexity:** O(n) for storing the elements in the array.