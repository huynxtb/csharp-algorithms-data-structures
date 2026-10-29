# Separate Chaining Hash Table

## Introduction

The Separate Chaining Hash Table is a dynamic data structure that stores key-value pairs. It uses hashing to map keys to indices in an array (often called buckets). When multiple keys hash to the same index (a collision), separate chaining resolves this by storing all colliding entries in a linked list (or another collection) at that index. This implementation provides efficient average-case time complexity for insertion, deletion, and retrieval operations.

**When to use it:**

*   When you need fast average-case performance for lookups, insertions, and deletions.
*   When the number of elements is not known in advance and the table needs to grow dynamically.
*   When dealing with potential hash collisions and requiring a robust collision resolution strategy.

## Usage

```csharp
// Example usage of SeparateChainingHashTable

// Create a new hash table with string keys and integer values
var hashTable = new SeparateChainingHashTable<string, int>(10);

// Add elements
hashTable.Add("apple", 1);
hashTable.Add("banana", 2);
hashTable.Add("cherry", 3);

// Access elements using the Get method
Console.WriteLine($"Value for 'banana': {hashTable.Get("banana")}"); // Output: Value for 'banana': 2

// Check if a key exists
Console.WriteLine($"Contains 'apple': {hashTable.ContainsKey("apple")}"); // Output: Contains 'apple': True
Console.WriteLine($"Contains 'grape': {hashTable.ContainsKey("grape")}"); // Output: Contains 'grape': False

// Use the indexer to get and set values
hashTable["date"] = 4;
Console.WriteLine($"Value for 'date' (via indexer): {hashTable["date"]}"); // Output: Value for 'date' (via indexer): 4

hashTable["apple"] = 10; // Update existing value
Console.WriteLine($"Updated value for 'apple': {hashTable["apple"]}"); // Output: Updated value for 'apple': 10

// Remove an element
bool removed = hashTable.Remove("banana");
Console.WriteLine($"Was 'banana' removed? {removed}"); // Output: Was 'banana' removed? True
Console.WriteLine($"Count after removal: {hashTable.Count}"); // Output: Count after removal: 3

// Attempt to get a non-existent key (will throw KeyNotFoundException)
// try
// {
//     Console.WriteLine(hashTable.Get("grape"));
// }
// catch (KeyNotFoundException ex)
// {
//     Console.WriteLine(ex.Message);
// }

// Attempt to add a duplicate key (will throw ArgumentException)
// try
// {
//     hashTable.Add("cherry", 5);
// }
// catch (ArgumentException ex)
// {
//     Console.WriteLine(ex.Message);
// }
```

## Detailed Explanation

### Internal Structure

The `SeparateChainingHashTable<TKey, TValue>` class uses a generic array of `List<Entry<TKey, TValue>>` called `buckets`. Each `Entry<TKey, TValue>` is a private struct holding a `Key` and its corresponding `Value`. When a collision occurs, multiple `Entry` objects are added to the `List` at the computed bucket index.

### Hashing and Indexing

1.  **`GetBucketIndex(TKey key)`**: This method computes the hash code for a given `key` using `key.GetHashCode()`. It then ensures the hash code is non-negative using a bitwise AND operation (`& 0x7FFFFFFF`) and maps it to a valid index within the `buckets` array using the modulo operator (`% capacity`).

### Core Operations

*   **`Add(TKey key, TValue value)`**: Calculates the bucket index for the `key`. If the bucket is empty, a new `List` is created. It then iterates through the existing entries in the bucket to check for duplicate keys. If a duplicate is found, an `ArgumentException` is thrown. Otherwise, a new `Entry` is added to the list, and `count` is incremented. Before adding, it checks the load factor and triggers a resize if necessary.
*   **`Remove(TKey key)`**: Calculates the bucket index. If the bucket is not null, it iterates through the `List` at that index. If the `key` is found, the corresponding `Entry` is removed from the `List`, `count` is decremented, and `true` is returned. If the key is not found, `false` is returned.
*   **`Get(TKey key)`**: Calculates the bucket index. If the bucket is not null, it iterates through the `List` at that index. If the `key` is found, its `Value` is returned. If the key is not found after checking the entire bucket, a `KeyNotFoundException` is thrown.
*   **`ContainsKey(TKey key)`**: Similar to `Get`, but returns `true` if the key is found and `false` otherwise, without throwing an exception.
*   **`Count` Property**: Returns the current number of elements stored in the hash table.
*   **Indexer (`this[TKey key]`)**: Provides convenient access. The `get` accessor calls `Get(key)`. The `set` accessor checks if the key exists. If it does, the value is updated. If not, it adds the new key-value pair, similar to `Add`, including a potential resize if the load factor is exceeded.

### Collision Resolution

Collisions are handled by appending new entries to the `List` at the computed bucket index. This is the essence of separate chaining.

### Resizing and Rehashing

*   **Load Factor**: The hash table monitors its load factor (`count / capacity`). When this exceeds `LoadFactorThreshold` (0.75), the `Resize` method is called.
*   **`Resize(int newCapacity)`**: A new, larger `buckets` array is created (with a capacity that is the next prime number roughly double the old capacity). All existing entries are then rehashed and inserted into the new `buckets` array. This ensures that the distribution of elements remains relatively even as the table grows.

### Prime Capacities

The `GetPrime` and `IsPrime` helper methods are used to ensure that the initial capacity and the capacities after resizing are prime numbers. Using prime capacities can help in distributing hash codes more evenly across the buckets, especially when the hash function is not perfectly uniform.

### Equality Comparison

`EqualityComparer<TKey>.Default.Equals()` is used for all key comparisons. This ensures that the hash table correctly handles keys that implement custom equality logic or use the default equality for their type.

## Complexity Analysis

Let N be the number of elements in the hash table and C be the capacity of the internal array.

*   **Time Complexity**:
    *   **Average Case**: For `Add`, `Remove`, `Get`, `ContainsKey`:
        *   Insertion: O(1)
        *   Deletion: O(1)
        *   Search: O(1)
        This assumes a good hash function and a low load factor, leading to short linked lists.
    *   **Worst Case**: O(N)
        This occurs when all keys hash to the same bucket, effectively turning the hash table into a single linked list.
    *   **Resizing**: O(N + C)
        When resizing, all N elements need to be rehashed and reinserted into a new array of size C'. This operation happens infrequently, so its amortized cost is still O(1) for insertions.

*   **Space Complexity**:
    *   O(N + C)
        This accounts for the storage of N key-value pairs and the C buckets in the internal array. The `List` objects within the buckets also contribute to the space usage.