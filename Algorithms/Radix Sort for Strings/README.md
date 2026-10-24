# Radix Sort for Strings

## Introduction
Radix Sort for Strings is a non-comparison-based sorting algorithm that sorts an array of strings based on their lexicographical order. It processes the strings character by character, starting from the least significant character to the most significant. This algorithm is particularly useful when sorting strings of varying lengths efficiently.

## Usage
```csharp
var sorter = new RadixSortStrings();
string[] strings = { "banana", "apple", "orange", "kiwi" };
sorter.Sort(strings);
// strings is now sorted: ["apple", "banana", "kiwi", "orange"]
```

## Detailed Explanation
The algorithm works by determining the maximum length of the strings in the array. It then iteratively applies a counting sort on each character position, starting from the last character to the first. The counting sort is used as a subroutine to sort the strings based on the current character position. The `GetCharAtPosition` method is used to handle strings of varying lengths by returning a default value for non-existent characters.

## Complexity Analysis
- **Time Complexity:** O(n * k), where n is the number of strings and k is the maximum length of the strings. Counting sort runs in O(n + m), where m is the range of the input (256 for ASCII characters).
- **Space Complexity:** O(n + m) for the output array and count array used in counting sort.