# Dynamic Array (Resizable Array)

## Introduction

The `DynamicArray<T>` is a generic collection class that provides the functionality of a resizable array. Unlike a standard C# array, which has a fixed size, a `DynamicArray` automatically adjusts its underlying storage capacity as elements are added or removed. This makes it a flexible choice when the exact number of elements is not known beforehand or when the collection size changes frequently.

It offers efficient random access to elements (O(1) time complexity) and amortized constant time complexity for adding elements to the end. It is a fundamental data structure often used as the basis for other collections.

**When to use it:**
*   When you need an array-like structure but don't know the final size.
*   When you frequently add or remove elements from the end.
*   When you need fast access to elements by their index.
*   As a building block for more complex data structures.

## Usage

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        // Create a new DynamicArray of integers with default capacity
        DynamicArray<int> numbers = new DynamicArray<int>();

        // Add elements
        numbers.Add(10);
        numbers.Add(20);
        numbers.Add(30);
        Console.WriteLine($"Count: {numbers.Count}, Capacity: {numbers.Capacity}"); // Output: Count: 3, Capacity: 4

        numbers.Add(40);
        numbers.Add(50); // This will trigger a resize
        Console.WriteLine($"Count: {numbers.Count}, Capacity: {numbers.Capacity}"); // Output: Count: 5, Capacity: 8

        // Access elements using the indexer
        Console.WriteLine($"Element at index 2: {numbers[2]}"); // Output: Element at index 2: 30

        // Modify an element
        numbers[1] = 25;
        Console.WriteLine($"Element at index 1 after modification: {numbers[1]}"); // Output: Element at index 1 after modification: 25

        // Remove an element
        int removed = numbers.RemoveAt(0);
        Console.WriteLine($"Removed element: {removed}"); // Output: Removed element: 10
        Console.WriteLine($"Count after removal: {numbers.Count}, Capacity: {numbers.Capacity}"); // Output: Count after removal: 4, Capacity: 8 (no shrink yet)

        // Adding more elements to potentially trigger shrink
        numbers.Add(60);
        numbers.Add(70);
        numbers.Add(80);
        numbers.Add(90);
        numbers.Add(100);
        Console.WriteLine($"Count: {numbers.Count}, Capacity: {numbers.Capacity}"); // Output: Count: 9, Capacity: 16

        // Removing elements to trigger shrink
        numbers.RemoveAt(8);
        numbers.RemoveAt(7);
        numbers.RemoveAt(6);
        numbers.RemoveAt(5);
        numbers.RemoveAt(4);
        numbers.RemoveAt(3);
        numbers.RemoveAt(2);
        numbers.RemoveAt(1);
        Console.WriteLine($"Count after many removals: {numbers.Count}, Capacity: {numbers.Capacity}"); // Output: Count after many removals: 1, Capacity: 8 (shrink occurred)

        // Clear all elements
        numbers.Clear();
        Console.WriteLine($"Count after clear: {numbers.Count}, Capacity: {numbers.Capacity}"); // Output: Count after clear: 0, Capacity: 8 (or DEFAULT_CAPACITY if reset in Clear)
    }
}
```

## Detailed Explanation

### Internal Storage

The `DynamicArray<T>` uses a private array `T[] _items` to store its elements. A private integer `_count` keeps track of the number of elements currently in the array, which is distinct from the `Capacity` (the total size of the `_items` array).

### Constructors

*   **`DynamicArray()`**: Initializes the array with a `DEFAULT_CAPACITY` (set to 4). The `_count` is set to 0.
*   **`DynamicArray(int initialCapacity)`**: Allows specifying an initial capacity. If `initialCapacity` is negative, an `ArgumentOutOfRangeException` is thrown. If `initialCapacity` is 0, it defaults to `DEFAULT_CAPACITY` to ensure there's always some space. Otherwise, it creates an array of the specified size.

### Properties

*   **`Count`**: A read-only property that returns the current number of elements (`_count`).
*   **`Capacity`**: A read-only property that returns the total size of the internal `_items` array.

### Indexer (`this[int index]`) 

*   **Get**: Allows retrieving an element at a specific `index`. It performs bounds checking: if `index` is less than 0 or greater than or equal to `_count`, an `IndexOutOfRangeException` is thrown. Otherwise, it returns `_items[index]`.
*   **Set**: Allows modifying an element at a specific `index`. It also performs bounds checking. If the index is valid, it updates `_items[index]` with the new `value`.

### `Add(T item)` Method

1.  **Check Capacity**: It first checks if `_count` is equal to `Capacity`. If it is, the array is full and needs to be resized.
2.  **Resize**: The `Resize` method is called. If the current `Capacity` is 0 (which can happen if `initialCapacity` was 0 and `DEFAULT_CAPACITY` was also 0, though our implementation sets it to at least 1), the new capacity becomes 1. Otherwise, the capacity is doubled (`Capacity * 2`).
3.  **Add Item**: The new `item` is placed at the `_count` index in the `_items` array, and `_count` is incremented.

### `RemoveAt(int index)` Method

1.  **Bounds Checking**: Validates that the `index` is within the valid range (`0` to `_count - 1`). Throws `IndexOutOfRangeException` if invalid.
2.  **Store Removed Item**: The element at the specified `index` is stored in a temporary variable to be returned.
3.  **Decrement Count**: `_count` is decremented.
4.  **Shift Elements**: A loop shifts all elements from `index + 1` to the end one position to the left, effectively overwriting the removed element and filling the gap.
5.  **Clear Last Slot**: The element at the now-vacant `_count` index (which was the last element before shifting) is set to `default(T)`. This is important for garbage collection if `T` is a reference type, as it allows the object to be collected if no other references exist.
6.  **Shrink Capacity**: After removal, it checks if the array should shrink. If the `Capacity` is greater than `DEFAULT_CAPACITY` and `_count` is less than or equal to `Capacity / 4`, the `Resize` method is called with `Capacity / 2` to halve the array's size. This prevents the array from holding a much larger capacity than needed after many removals.
7.  **Return Removed Item**: The stored removed item is returned.

### `Clear()` Method

This method resets the `DynamicArray` to an empty state. The current implementation iterates through the existing elements and sets them to `default(T)` to aid garbage collection, then resets `_count` to 0. The `Capacity` remains unchanged. An alternative (commented out) is to reallocate the internal array to `DEFAULT_CAPACITY`, which might be more memory-efficient if the array is expected to be refilled soon.

### `Resize(int newCapacity)` Method

This private helper method is responsible for changing the capacity of the internal array. It creates a new array with the `newCapacity`, copies all existing elements from the old `_items` array to the new one using `Array.Copy`, and then replaces `_items` with the new array.

## Complexity Analysis

*   **`Add(T item)`**:
    *   **Time Complexity**: Amortized O(1). While a single `Add` operation might take O(N) time if a resize is needed (where N is the current number of elements), resizes happen infrequently. The cost of resizing is spread out over many `Add` operations, resulting in an average (amortized) time complexity of O(1).
    *   **Space Complexity**: O(N) for storing N elements.

*   **`RemoveAt(int index)`**:
    *   **Time Complexity**: O(N) in the worst case. This is because shifting elements to fill the gap can take time proportional to the number of elements remaining after the removed element.
    *   **Space Complexity**: O(1) (excluding the space for the array itself).

*   **`this[int index]` (Get/Set)**:
    *   **Time Complexity**: O(1). Accessing or modifying an element by its index is a direct operation on the underlying array.
    *   **Space Complexity**: O(1).

*   **`Clear()`**:
    *   **Time Complexity**: O(N) if elements are explicitly cleared (as in the provided code), or O(1) if only `_count` is reset and no reallocation occurs.
    *   **Space Complexity**: O(1).

*   **`Count` / `Capacity` Properties**: 
    *   **Time Complexity**: O(1).
    *   **Space Complexity**: O(1).
