# Max-Ordered List

## Introduction
The Max-Ordered List is a dynamic data structure that maintains a list of integers in descending order. It allows for efficient insertion of new elements while ensuring that the maximum value can be quickly retrieved or removed. This structure is particularly useful in scenarios where the maximum value needs to be accessed frequently, such as in priority queues or scheduling algorithms.

## Usage
```csharp
MaxOrderedList maxList = new MaxOrderedList();
maxList.Insert(10);
maxList.Insert(5);
maxList.Insert(20);
int max = maxList.GetMax(); // max is 20
int removedMax = maxList.RemoveMax(); // removedMax is 20
bool isEmpty = maxList.IsEmpty(); // isEmpty is false
int count = maxList.Count(); // count is 2
```

## Detailed Explanation
The Max-Ordered List is implemented using a List<int> to store the elements. When a new value is inserted, it is added to the list, and the list is sorted in descending order. This ensures that the maximum value is always at the front of the list. The `RemoveMax` method retrieves and removes the first element of the list, which is the maximum value. The `GetMax` method simply returns the first element without removing it. The `IsEmpty` method checks if the list contains any elements, and the `Count` method returns the total number of elements in the list.

## Complexity Analysis
- **Insert:** O(n log n) due to sorting the list after each insertion.
- **RemoveMax:** O(n) as it involves removing the first element from the list.
- **GetMax:** O(1) since it directly accesses the first element.
- **IsEmpty:** O(1) as it checks the count.
- **Count:** O(1) as it returns the count of elements.