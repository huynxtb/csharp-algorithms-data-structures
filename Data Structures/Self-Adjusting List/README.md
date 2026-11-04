# Self-Adjusting List

## Introduction
The Self-Adjusting List is a data structure that maintains an ordered list of elements while optimizing access times based on the frequency of access. Frequently accessed elements are moved towards the front of the list, making future accesses faster. This structure is useful in scenarios where certain elements are accessed more frequently than others, such as caching mechanisms or user preference lists.

## Usage
```csharp
SelfAdjustingList<int> sal = new SelfAdjustingList<int>();
sal.Insert(1);
sal.Insert(2);
sal.Insert(3);
sal.Access(2); // Moves 2 to the front
sal.PrintList(); // Output: 2 1 3
sal.Remove(1);
sal.PrintList(); // Output: 2 3
```

## Detailed Explanation
The Self-Adjusting List is implemented using a `LinkedList` to maintain the order of elements and a `HashSet` to ensure that duplicates are not allowed. The `Insert` method adds a new element to the front of the list if it does not already exist. The `Access` method moves an accessed element to the front, and the `Remove` method deletes an element from both the list and the set. The use of a `HashSet` allows for O(1) average time complexity for insertion, access, and removal operations.

## Complexity Analysis
- **Insert:** O(1) average time complexity due to the use of a `HashSet` for duplicate checking and `LinkedList` for insertion.
- **Access:** O(1) average time complexity for checking existence and O(n) for moving the element in the `LinkedList`.
- **Remove:** O(1) average time complexity for checking existence and O(n) for removal in the `LinkedList`.
- **Space Complexity:** O(n) for storing elements in both the `LinkedList` and `HashSet`.