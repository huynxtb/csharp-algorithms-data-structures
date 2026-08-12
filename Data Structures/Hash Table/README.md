# Introduction
The Hash Table is a fundamental data structure that stores key-value pairs in an array using a hash function to map keys to indices of the array. It is particularly useful for fast lookups, insertions, and deletions.

# Usage
```csharp
HashTable hashTable = new HashTable(10);
hashTable.Insert("apple", 5);
hashTable.Insert("banana", 7);
Console.WriteLine(hashTable.Get("apple")); // Output: 5
hashTable.Delete("apple");
Console.WriteLine(hashTable.Get("apple")); // Output: 
```

# Detailed Explanation
The implementation uses separate chaining (linked lists) to handle collisions. The `HashFunction` method calculates the index for a given key using the sum of the ASCII values of the characters in the key modulo the size of the table. The `Insert` method inserts a new key-value pair into the table, handling collisions by appending to the linked list at the calculated index. The `Get` method retrieves the value associated with a given key by traversing the linked list at the calculated index. The `Delete` method removes a key-value pair from the table by finding the node with the matching key and updating the linked list accordingly.

# Complexity Analysis
* Time complexity:
 + Insert: O(1 + n) where n is the number of collisions
 + Get: O(1 + n) where n is the number of collisions
 + Delete: O(1 + n) where n is the number of collisions
* Space complexity: O(n) where n is the number of key-value pairs