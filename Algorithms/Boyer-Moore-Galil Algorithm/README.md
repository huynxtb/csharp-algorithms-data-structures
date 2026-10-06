# Boyer-Moore-Galil String Search Algorithm

## 1. Introduction
The **Boyer-Moore-Galil** algorithm is an optimization of the standard Boyer-Moore string search algorithm introduced by Zvi Galil in 1979. 

While standard Boyer-Moore executes in sub-linear time on average, its worst-case time complexity can degrade to $O(n \cdot m)$ when searching for periodic patterns (e.g., searching for `"aaaa"` in `"aaaaaaaaaaaa"`). The Galil rule exploits pattern periodicity: when a complete match is found, the search window shifts by the pattern's shortest period length $k$, and the first $m - k$ characters are known to match automatically. Skipping comparisons for this already-verified prefix reduces the worst-case time complexity to strictly $O(n)$ without compromising average-case sub-linear speed.

### When to Use:
- Searching for repeated or periodic sub-patterns in massive text files or genomic DNA sequences.
- Scenarios requiring strict worst-case $O(n)$ guarantees while maintaining standard Boyer-Moore performance on general alphabets.

---

## 2. Usage

```csharp
using System;
using BoyerMooreGalilAlgorithm;

public class Example
{
    public static void Run()
    {
        string text = "GCATCGCAGAGAGTATACAGTACG";
        string pattern = "GCAGAGAG";

        var searcher = new BoyerMooreGalilSearcher(pattern);

        // Find the first occurrence
        int firstIndex = searcher.FindFirst(text.AsSpan());
        Console.WriteLine($"First match at index: {firstIndex}");

        // Find all occurrences
        foreach (int matchIndex in searcher.FindAllOccurrences(text))
        {
            Console.WriteLine($"Match found at index: {matchIndex}");
        }
    }
}
```

---

## 3. Detailed Explanation

1. **Pattern Preprocessing**:
   - **Bad Character Rule**: Stores the rightmost occurrence of every character in the alphabet inside the pattern.
   - **Good Suffix Rule**: Precomputes shift distances based on suffix-prefix overlaps (suffix function).
   - **Period Computation**: Utilizes the KMP prefix function (failure function) over the pattern to find its longest proper border. If $m \pmod{m - \pi[m - 1]} = 0$, the pattern is periodic with period $k = m - \pi[m - 1]$; otherwise, its period is $m$.

2. **Search Phase with Galil Optimization**:
   - Characters are compared right-to-left from pattern position $m - 1$ down to boundary $l$ (initially $l = 0$).
   - When a full match occurs at shift $s$:
     - The shift is advanced by the period $k$ ($s \leftarrow s + k$).
     - The left skip bound $l$ is set to $m - k$, since the first $m - k$ characters of the new alignment align identically with the suffix of the previous match.
   - On mismatch, the algorithm shifts by $\max(\text{BadCharShift}, \text{GoodSuffixShift})$ and resets $l = 0$.

---

## 4. Complexity Analysis

| Metric | Complexity |
| :--- | :--- |
| **Preprocessing Time** | $O(m + |\Sigma|)$ where $m$ is pattern length and $|\Sigma|$ is alphabet size |
| **Preprocessing Space** | $O(m + |\Sigma|)$ |
| **Search Time (Worst-Case)** | $O(n)$ where $n$ is text length (guaranteed linear via Galil rule) |
| **Search Time (Average-Case)** | $O(n / m)$ sub-linear |
| **Auxiliary Search Space** | $O(1)$ |