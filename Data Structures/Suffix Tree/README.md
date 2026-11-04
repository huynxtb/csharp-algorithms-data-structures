# Suffix Tree

## Introduction
A Suffix Tree is a compressed trie that represents all the suffixes of a given string. It allows for fast substring searches, longest common substring queries, and more. Suffix Trees are particularly useful in applications like bioinformatics, text processing, and data compression.

## Usage
```csharp
SuffixTree suffixTree = new SuffixTree("banana");
bool exists = suffixTree.Search("ana"); // returns true
List<int> occurrences = suffixTree.FindOccurrences("ana"); // returns list of starting indices
```

## Detailed Explanation
The Suffix Tree implementation consists of a root node and child nodes that represent the characters of the input string. Each node contains a dictionary of children, a start index, an end pointer, and the length of the edge. The `InsertSuffix` method adds each suffix of the string to the tree. The `Search` method checks if a given pattern exists in the tree, while `FindOccurrences` retrieves all starting indices of the pattern in the original string by traversing the tree.

## Complexity Analysis
- **Insertion:** O(n) for each suffix, where n is the length of the string. Overall complexity is O(n^2) for building the tree.
- **Search:** O(m), where m is the length of the pattern being searched.
- **Find Occurrences:** O(k + m), where k is the number of occurrences found and m is the length of the pattern.