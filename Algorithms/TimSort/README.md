# TimSort in C#

## 1. Introduction
`TimSort` is a hybrid, stable data sorting algorithm derived from Merge Sort and Insertion Sort, designed to perform exceptionally well on real-world data containing existing ordered subsequences (natural runs). Originally invented by Tim Peters for Python in 2002, it is also the standard sorting algorithm in Java, Android, and V8 (Chromium JavaScript engine).

Use TimSort when:
- Stability is required (the relative order of equal elements must remain unchanged).
- Real-world data often exhibits partially sorted patterns (ascending or descending).
- Consistent $O(N \log N)$ worst-case performance is required with $O(N)$ best-case capabilities on already ordered collections.

## 2. Usage

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        // Sorting primitive arrays
        int[] numbers = { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
        TimSorter<int>.Sort(numbers);
        Console.WriteLine(string.Join(", ", numbers));

        // Sorting custom objects with a custom comparer
        Person[] people = new Person[]
        {
            new Person("Alice", 30),
            new Person("Bob", 25),
            new Person("Charlie", 25),
            new Person("Diana", 35)
        };

        TimSorter<Person>.Sort(people, Comparer<Person>.Create((x, y) => x.Age.CompareTo(y.Age)));
        // Stable sort guarantees Bob appears before Charlie
        foreach (var person in people)
        {
            Console.WriteLine($"{person.Name}: {person.Age}");
        }
    }
}

public record Person(string Name, int Age);
```

## 3. Detailed Explanation

1. **MinRun Selection (`GetMinRunLength`)**:
   Computes a threshold between 32 and 64 such that `N / minRun` is close to a power of 2, ensuring balanced binary merges.
2. **Run Detection & Extension**:
   Iterates through the input array finding natural runs. Non-descending runs are left intact; strictly descending runs are reversed in-place to ensure stability and ascending order. If a run is shorter than `minRun`, it is extended using an optimized binary insertion sort (`BinarySort`).
3. **Run Stack Invariants**:
   Runs are pushed to a tracking stack. The stack maintains two invariants:
   - `len(A) > len(B) + len(C)`
   - `len(B) > len(C)`
   If either invariant is violated, adjacent runs are merged until equilibrium is restored.
4. **Galloping Mode Merge**:
   During standard two-way merge operations, if consecutive items are consistently picked from one run (exceeding `MIN_GALLOP`), TimSort switches to "Galloping Mode". It uses exponential/binary search (`GallopLeft` / `GallopRight`) to find larger slice insertion points and bulk-copies contiguous chunks, significantly reducing pairwise comparison overhead.

## 4. Complexity Analysis

| Case | Time Complexity | Auxiliary Space |
| :--- | :--- | :--- |
| **Best Case** | $O(N)$ | $O(1)$ |
| **Average Case** | $O(N \log N)$ | $O(N)$ |
| **Worst Case** | $O(N \log N)$ | $O(N)$ |
| **Stability** | **Stable** | - |