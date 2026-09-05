# Trie (Prefix Tree)

## 1. Introduction
A Trie, or Prefix Tree, is a tree-like data structure used to efficiently store and retrieve string keys. Use it for prefix matching, autocomplete systems, and spell checking.

## 2. Usage
```csharp
Trie trie = new Trie();
trie.Insert("apple");
bool searchResult = trie.Search("apple"); // Returns true
bool prefixResult = trie.StartsWith("app"); // Returns true
```

## 3. Detailed Explanation
The structure uses nested `TrieNode` instances. Each node contains a dictionary mapping characters to child nodes and a boolean flag marking key termination. Operations traverse the tree character by character.

## 4. Complexity Analysis
- **Insert**: Time `O(m)`, Space `O(m)` where `m` is key length.
- **Search**: Time `O(m)`, Space `O(1)`.
- **StartsWith**: Time `O(m)`, Space `O(1)`.