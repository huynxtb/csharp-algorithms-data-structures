# Count-Unique Subarrays

## 1. Introduction

The **Count-Unique Subarrays** algorithm counts the number of distinct (unique) contiguous subarrays within a given array of integers. A contiguous subarray is any sequence of consecutive elements from the original array. Two subarrays are considered identical if they contain the same elements in the same order.

This algorithm is useful in scenarios such as:
- Analyzing patterns or repetitions in data sequences.
- Solving combinatorics problems involving subsets of ordered data.
- Pre-processing steps in string/sequence matching algorithms.
- Competitive programming problems involving subarray uniqueness.

---

## 2. Usage

```csharp
// Basic usage: count unique subarrays
int[] myArray = { 1, 2, 1, 3 };
int uniqueCount = CountUniqueSubarrays.Count(myArray);
Console.WriteLine($"Unique subarrays: {uniqueCount}");
// Output: Unique subarrays: 9

// Detailed usage: get count and the actual unique subarrays
int[] anotherArray = { 1, 1, 2 };
List<string> subarrayList;
int count = CountUniqueSubarrays.CountWithDetails(anotherArray, out subarrayList);
Console.WriteLine($"Count: {count}");
foreach (var sub in subarrayList)
{
    Console.WriteLine(sub);
}

// Handling validation
try
{
    CountUniqueSubarrays.Count(null); // Throws ArgumentNullException
}
catch (ArgumentNullException ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    CountUniqueSubarrays.Count(new int[] {}); // Throws ArgumentException
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}
```

---

## 3. Detailed Explanation

### Input Validation
Before processing, the method checks:
- If the array is `null`, an `ArgumentNullException` is thrown.
- If the array is empty (length 0), an `ArgumentException` is thrown.

### Core Algorithm

1. **Outer Loop (start index):** Iterates over each possible starting index of a subarray from `0` to `array.Length - 1`.

2. **Inner Loop (end index):** For each starting index, iterates over each possible ending index from `start` to `array.Length - 1`, progressively building longer subarrays.

3. **String Representation:** Each subarray is serialized into a comma-separated string (e.g., `[1, 2, 3]` becomes `"1,2,3"`). This string acts as a unique key for the subarray.

4. **HashSet Tracking:** Each serialized subarray string is inserted into a `HashSet<string>`. Since a `HashSet` only stores unique values, duplicate subarrays are automatically ignored.

5. **Result:** After all iterations, the size of the `HashSet` represents the total number of unique subarrays.

### CountWithDetails Method
This overload additionally outputs a `List<string>` containing all unique subarray representations, useful for debugging or further processing.

---

## 4. Complexity Analysis

### Time Complexity

| Operation | Complexity |
|---|---|
| Generating all subarrays | O(n²) |
| Building each subarray string | O(n) per subarray |
| HashSet insertion (average) | O(n) per key (string hashing) |
| **Overall** | **O(n³)** |

Where `n` is the number of elements in the input array. The cubic complexity arises from the two nested loops (O(n²) subarrays) and the string construction/hashing for each subarray (O(n) per subarray).

### Space Complexity

| Resource | Complexity |
|---|---|
| HashSet storage | O(n² × n) = O(n³) in the worst case |
| StringBuilder buffer | O(n) |
| **Overall** | **O(n³)** |

In the worst case (all elements are distinct), all O(n²) subarrays are unique, and each can have up to O(n) characters in its string representation, leading to O(n³) space usage.

### Notes
- For very large arrays (n > 10,000), consider alternative approaches using rolling hashes or suffix arrays for better performance.
- The algorithm is well-suited for small to medium-sized arrays where clarity and correctness are prioritized over raw performance.
