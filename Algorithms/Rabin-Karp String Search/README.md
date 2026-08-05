# Rabin-Karp String Search

## Introduction

The Rabin-Karp algorithm is a string searching algorithm that uses hashing to find occurrences of a pattern within a text. It's particularly efficient when searching for multiple patterns in a text or when the alphabet size is large. The core idea is to compute a hash value for the pattern and then compute hash values for all substrings of the text of the same length as the pattern. If the hash values match, a character-by-character comparison is performed to confirm the match and handle potential hash collisions.

## Usage

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        string text = "ABABDABACDABABCABAB";
        string pattern = "ABABCABAB";

        IEnumerable<int> occurrences = RabinKarpSearch.Search(text, pattern);

        Console.WriteLine($"Pattern '{pattern}' found at indices:");
        foreach (int index in occurrences)
        {
            Console.WriteLine(index);
        }

        // Example with no occurrences
        string text2 = "This is a test string.";
        string pattern2 = "xyz";
        IEnumerable<int> occurrences2 = RabinKarpSearch.Search(text2, pattern2);
        Console.WriteLine($"\nPattern '{pattern2}' found at indices:");
        if (!occurrences2.Any())
        {
            Console.WriteLine("No occurrences found.");
        }
        else
        {
            foreach (int index in occurrences2)
            {
                Console.WriteLine(index);
            }
        }

        // Example with empty pattern
        string text3 = "abc";
        string pattern3 = "";
        IEnumerable<int> occurrences3 = RabinKarpSearch.Search(text3, pattern3);
        Console.WriteLine($"\nPattern '{pattern3}' found at indices:");
        foreach (int index in occurrences3)
        {
            Console.WriteLine(index);
        }
    }
}
```

## Detailed Explanation

The `RabinKarpSearch` class provides a static `Search` method that takes the `text` and `pattern` as input. It utilizes a rolling hash function to efficiently compare substrings.

1.  **Constants:**
    *   `PrimeMultiplier`: Typically set to the size of the alphabet (e.g., 256 for ASCII characters). It's used in the polynomial rolling hash calculation.
    *   `PrimeModulus`: A large prime number used to keep the hash values within a manageable range and reduce the probability of collisions.

2.  **Edge Case Handling:**
    *   The method first checks for `null` inputs and throws an `ArgumentNullException` if either `text` or `pattern` is null.
    *   If the `pattern` is empty, it's considered to match at every possible position in the `text`, including after the last character. Thus, it yields all indices from 0 to `text.Length`.
    *   If the `text` is empty or the `pattern` is longer than the `text`, no matches are possible, and the method returns an empty `IEnumerable`.

3.  **Hash Calculation:**
    *   `highestPower`: This variable stores `(PrimeMultiplier^(m-1)) % PrimeModulus`. It's pre-calculated to efficiently remove the contribution of the leading character when sliding the window.
    *   The initial hash values for the `pattern` (`patternHash`) and the first window of the `text` (`textHash`) are computed. This is done using a polynomial rolling hash function: `hash = (hash * PrimeMultiplier + character) % PrimeModulus`.

4.  **Sliding Window and Matching:**
    *   The algorithm iterates through the `text` using a sliding window of size `m` (the length of the `pattern`).
    *   In each iteration, it compares `patternHash` with `textHash`.
    *   **Hash Match:** If the hash values are equal, it indicates a potential match. To confirm, a character-by-character comparison is performed between the current window of `text` and the `pattern`. If all characters match, the current index `i` is yielded as a valid occurrence.
    *   **Rolling Hash Update:** After checking for a match, the `textHash` is updated for the next window. This is the "rolling" part: the hash of the previous window is modified by subtracting the contribution of the character leaving the window (`text[i]`) and adding the contribution of the new character entering the window (`text[i + m]`). The formula used is:
        `new_textHash = ( (textHash - text[i] * highestPower) * PrimeMultiplier + text[i + m] ) % PrimeModulus`.
        Care is taken to ensure `textHash` remains non-negative after subtraction by adding `PrimeModulus` if it becomes negative.

5.  **Yield Return:** The method uses `yield return` to return indices lazily, making it memory-efficient for large texts.

## Complexity Analysis

*   **Time Complexity:**
    *   **Best Case:** O(N + M), where N is the length of the text and M is the length of the pattern. This occurs when there are very few hash collisions, and the character-by-character verification is rarely needed.
    *   **Average Case:** O(N + M). The probability of hash collisions is low with a good hash function and a large prime modulus.
    *   **Worst Case:** O(N * M). This occurs in scenarios with a high number of hash collisions (e.g., if all characters in the text and pattern are the same, or if the hash function is poorly chosen), forcing character-by-character verification for almost every window.

*   **Space Complexity:** O(1) (excluding the space required to store the input strings and the output `IEnumerable`). The algorithm uses a constant amount of extra space for variables like hash values, `highestPower`, and loop counters.