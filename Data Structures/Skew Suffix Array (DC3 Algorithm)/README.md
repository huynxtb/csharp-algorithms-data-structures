## Skew Suffix Array (DC3 Algorithm)

### Introduction

The Skew Suffix Array (also known as the DC3 - Difference Cover modulo 3 algorithm) is a highly efficient algorithm for constructing suffix arrays. A suffix array is a sorted array of all suffixes of a given string. It's a fundamental data structure in string processing, enabling fast pattern searching, longest common substring queries, and other string-related operations. The DC3 algorithm achieves a linear time complexity of O(n), making it suitable for very large texts where quadratic or n log n algorithms would be too slow.

### Usage

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        string text = "banana";
        SkewSuffixArray suffixArrayBuilder = new SkewSuffixArray(text);

        Console.WriteLine($"Text: {suffixArrayBuilder.Text}");

        Console.Write("Suffix Array: [");
        for (int i = 0; i < suffixArrayBuilder.SuffixArray.Length; i++)
        {
            Console.Write(suffixArrayBuilder.SuffixArray[i] + (i == suffixArrayBuilder.SuffixArray.Length - 1 ? "" : ", "));
        }
        Console.WriteLine("]");

        // Example: Get LCP Array
        int[] lcpArray = suffixArrayBuilder.GetLCPArray();
        Console.Write("LCP Array: [");
        for (int i = 0; i < lcpArray.Length; i++)
        {
            Console.Write(lcpArray[i] + (i == lcpArray.Length - 1 ? "" : ", "));
        }
        Console.WriteLine("]");

        // Example: Check for pattern containment
        string pattern1 = "ana";
        bool containsAna = suffixArrayBuilder.Contains(pattern1);
        Console.WriteLine($"Does text contain '{pattern1}'? {containsAna}"); // Output: True

        string pattern2 = "apple";
        bool containsApple = suffixArrayBuilder.Contains(pattern2);
        Console.WriteLine($"Does text contain '{pattern2}'? {containsApple}"); // Output: False

        // Example: Find all occurrences
        string pattern3 = "ana";
        List<int> occurrences = suffixArrayBuilder.FindAllOccurrences(pattern3);
        Console.Write($"Occurrences of '{pattern3}': [");
        for (int i = 0; i < occurrences.Count; i++)
        {
            Console.Write(occurrences[i] + (i == occurrences.Count - 1 ? "" : ", "));
        }
        Console.WriteLine("]"); // Output: [1, 3]
    }
}
```

### Detailed Explanation

The Skew (DC3) algorithm constructs a suffix array in three main phases:

1.  **Divide and Conquer (Recursive Step):**
    *   The algorithm divides the suffixes into three groups based on their starting positions modulo 3:
        *   Group 0: Suffixes starting at indices `i` where `i % 3 == 0`.
        *   Group 1: Suffixes starting at indices `i` where `i % 3 == 1`.
        *   Group 2: Suffixes starting at indices `i` where `i % 3 == 2`.
    *   The core idea is to recursively sort the suffixes in Group 1 and Group 2 (i.e., those starting at `i % 3 != 0`). This is done by considering triplets of characters `(text[i], text[i+1], text[i+2])` for each suffix in these groups.
    *   These triplets are then sorted using radix sort. If all triplets are unique, their sorted order directly gives the relative order of suffixes in Group 1 and 2. If there are duplicate triplets, a new, smaller string is formed where each character represents the rank of a unique triplet. The algorithm then recursively calls itself on this new string.
    *   The result of the recursive call is a sorted suffix array for the combined Group 1 and 2 suffixes (let's call this `sa12`).

2.  **Sort Group 0 Suffixes:**
    *   Once `sa12` is computed, the suffixes in Group 0 (starting at `i % 3 == 0`) can be sorted. A suffix `i` from Group 0 can be compared with a suffix `j` from Group 1 or 2 by comparing `(text[i], rank(i+1))` with `(text[j], rank(j+1))`. The `rank(k)` here refers to the rank of the suffix starting at index `k` within the already sorted `sa12` array. This comparison is efficient because we have `sa12` and can quickly find the rank of `i+1` and `j+1` using an inverse suffix array (rank array).
    *   This sorting of Group 0 suffixes is also done efficiently, often using radix sort on pairs `(text[i], rank(i+1))`, resulting in `sa0`.

3.  **Merge:**
    *   Finally, the two sorted arrays, `sa0` and `sa12`, are merged into a single, complete suffix array for the entire text. This merge operation is similar to the merge step in merge sort and takes linear time. The comparison logic during the merge needs to handle cases where one suffix starts at `i % 3 == 0` and the other at `j % 3 == 1` or `j % 3 == 2`, using the character values and ranks derived from `sa12`.

**Helper Methods:**
*   **`MapToRanks`**: Converts characters to integer ranks, starting from 1 to reserve 0 for padding/sentinels. This is crucial for radix sort.
*   **`RadixSortTriplets` / `RadixSortPairs` / `RadixSortHelper`**: Implementations of radix sort, which is essential for the linear time complexity. They sort based on characters at specific offsets or ranks.
*   **`AreTripletsEqual`**: A utility to check if two triplets are identical.
*   **`ComputeRankArray`**: Creates the inverse suffix array, mapping suffix starting positions to their rank in the sorted suffix array. This is vital for efficient comparisons during the sorting of Group 0 suffixes and the final merge.
*   **`GetLCPArray`**: Implements Kasai's algorithm to compute the Longest Common Prefix (LCP) array in O(n) time after the suffix array is built. It leverages the suffix array and its inverse.
*   **`Contains` / `FindAllOccurrences`**: Use binary search on the constructed suffix array to efficiently find pattern occurrences.

**Padding:** The text is padded with three sentinel characters (represented by 0 after rank mapping) to simplify boundary conditions when forming triplets and comparing suffixes that extend to the end of the string.

### Complexity Analysis

*   **Time Complexity:**
    *   **Suffix Array Construction:** O(n) - The DC3 algorithm is designed to run in linear time. Each step (recursive calls, radix sorts, merging) is performed in linear time with respect to the size of the input at that stage. The recursive calls reduce the problem size by a factor of approximately 3/2, leading to a recurrence relation that resolves to O(n).
    *   **LCP Array Construction (Kasai's Algorithm):** O(n) - Kasai's algorithm computes the LCP array in linear time given the suffix array and the original text.
    *   **`Contains` (Pattern Search):** O(m log n) - Standard binary search on the suffix array, where `m` is the pattern length and `n` is the text length. The string comparisons within the binary search take O(m) time.
    *   **`FindAllOccurrences` (Pattern Search):** O(m log n + k) - Binary search to find the first occurrence (O(m log n)), followed by linear scans to find all contiguous occurrences (O(k)), where `k` is the number of occurrences.

*   **Space Complexity:**
    *   **Suffix Array Construction:** O(n) - Requires auxiliary arrays for storing intermediate suffix arrays, ranks, and temporary sorting buffers. The depth of recursion is logarithmic, but the total space used across all recursive calls is linear.
    *   **LCP Array Construction:** O(n) - For storing the LCP array and auxiliary rank array.
    *   **`Contains` / `FindAllOccurrences`:** O(1) auxiliary space (excluding the space for the pattern and the result list).
