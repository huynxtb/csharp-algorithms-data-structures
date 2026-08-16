# Introduction
The Linear Probing Hash Table is a data structure that stores key-value pairs in an array using a hash function to map keys to indices. When a collision occurs, it uses linear probing to find the next available slot.

# Usage
```csharp
LinearProbingHashTable<string, int> hashTable = new LinearProbingHashTable<string, int>();
hashTable.Insert("apple", 5);
hashTable.Insert("banana", 7);
Console.WriteLine(hashTable.Search("apple")); // Output: 5
hashTable.Delete("apple");
try
{
    Console.WriteLine(hashTable.Search("apple"));
}
catch (KeyNotFoundException)
{
    Console.WriteLine("Key not found");
}
```

# Detailed Explanation
The implementation uses a simple modulo-based hash function for integer keys. It handles collisions using linear probing. The hash table manages its own internal array and resizes when the load factor exceeds a defined threshold (0.75).

# Complexity Analysis
*   Insert: O(1) average case, O(n) worst case
*   Search: O(1) average case, O(n) worst case
*   Delete: O(1) average case, O(n) worst case
*   Space complexity: O(n)