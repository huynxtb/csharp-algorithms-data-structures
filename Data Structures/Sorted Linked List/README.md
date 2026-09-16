# Sorted Linked List

## Introduction
A Sorted Linked List is a data structure that maintains its elements in a sorted order as new elements are added. It is useful when you need to keep a collection of items sorted without the overhead of sorting after every insertion. This structure allows for efficient insertion, removal, and searching of elements.

## Usage
```csharp
SortedLinkedList list = new SortedLinkedList();
list.Insert(5);
list.Insert(3);
list.Insert(8);
list.Print(); // Outputs: 3 5 8 

list.Remove(5);
list.Print(); // Outputs: 3 8 

bool found = list.Search(3); // Returns true
found = list.Search(5); // Returns false
```

## Detailed Explanation
The `SortedLinkedList` class contains a private nested `Node` class that represents each element in the list. The `Insert` method adds a new node in the correct position to maintain sorted order. The `Remove` method traverses the list to find and remove a specified node. The `Search` method checks for the presence of a value by traversing the list. The `Print` method outputs all elements in the list.

## Complexity Analysis
- **Insert:** O(n) in the worst case, where n is the number of elements in the list, as we may need to traverse the entire list to find the correct insertion point.
- **Remove:** O(n) in the worst case, as we may need to traverse the entire list to find the node to remove.
- **Search:** O(n) in the worst case, as we may need to traverse the entire list to find the element.
- **Print:** O(n), as we need to visit each node to output its value.
