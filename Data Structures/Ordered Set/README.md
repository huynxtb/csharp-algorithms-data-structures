# Ordered Set

## Introduction
The Ordered Set is a data structure that maintains a collection of unique elements in a sorted order. It is particularly useful when you need to ensure that all elements are unique and sorted, while still allowing for efficient insertion, deletion, and lookup operations.

## Usage
Here is an example of how to use the Ordered Set:
```csharp
var orderedSet = new OrderedSet<int>();
orderedSet.Add(5);
orderedSet.Add(3);
orderedSet.Add(8);

Console.WriteLine(orderedSet.Contains(3)); // Output: True
orderedSet.Remove(3);
Console.WriteLine(orderedSet.Contains(3)); // Output: False

var sortedElements = orderedSet.GetSortedElements(); // Output: [5, 8]
```

## Detailed Explanation
The Ordered Set is implemented using a `SortedSet<T>`, which automatically maintains the elements in sorted order. The `Add` method inserts an element into the set, ensuring that duplicates are not added. The `Remove` method deletes an element if it exists in the set. The `Contains` method checks for the presence of an element, returning a boolean value. Finally, `GetSortedElements` retrieves all elements in the set as an array, sorted in ascending order.

## Complexity Analysis
- **Add**: O(log n) - Inserting an element into a sorted set.
- **Remove**: O(log n) - Deleting an element from a sorted set.
- **Contains**: O(log n) - Checking for the presence of an element.
- **GetSortedElements**: O(n) - Retrieving all elements as an array.