# Linear Search

### 1. Introduction
Linear Search is a sequential search algorithm that starts at one end of a collection and checks every element until the desired element is found or the end of the collection is reached. It is best used for unsorted collections, small datasets, or when simplicity is preferred over performance.

### 2. Usage
```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        IList<string> fruits = new List<string> { "apple", "banana", "cherry", "date" };
        int index = LinearSearchAlgorithm.LinearSearch(fruits, "cherry");
        Console.WriteLine($"Found at index: {index}"); // Output: Found at index: 2
    }
}
```

### 3. Detailed Explanation
The implementation defines a static generic method `LinearSearch<T>` that accepts an `IList<T>` and a target element of type `T`:
- **Null Guard**: The method first checks if the input collection is null. If so, it returns `-1` to prevent a `NullReferenceException`.
- **Generic Comparison**: It retrieves the default equality comparer for type `T` using `EqualityComparer<T>.Default` to ensure safe comparison of both value and reference types.
- **Iteration**: A `for` loop iterates through the collection sequentially. If a match is found, the current index is returned immediately.
- **Fallback**: If the loop completes without finding the target, the method returns `-1`.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Best Case**: O(1) when the target element is at the first position.
  - **Worst Case**: O(n) when the target element is at the last position or not present in the collection.
  - **Average Case**: O(n).
- **Space Complexity**: O(1) auxiliary space as the search is performed in-place without allocating extra memory.