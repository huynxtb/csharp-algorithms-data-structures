# Introduction
The Fisher-Yates (Knuth) Shuffle is an algorithm used to generate a random permutation of a finite sequence. It is an efficient and unbiased method for shuffling the elements of an array.

# Usage
```csharp
int[] array = { 1, 2, 3, 4, 5 };
FisherYatesShuffle.Shuffle(array);
foreach (var item in array)
{
    Console.WriteLine(item);
}
```

# Detailed Explanation
The Fisher-Yates (Knuth) Shuffle algorithm works by iterating through the input array from the last element to the first. For each element at index `i`, it generates a random index `j` between 0 and `i` (inclusive) and swaps the elements at indices `i` and `j`. This process ensures that each element has an equal chance of being placed at any position in the array.

# Complexity Analysis
* Time complexity: O(n), where n is the number of elements in the array.
* Space complexity: O(1), as the algorithm only uses a constant amount of additional space to store the temporary swap variable and the random number generator.