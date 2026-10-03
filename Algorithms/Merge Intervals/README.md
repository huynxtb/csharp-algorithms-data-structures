# Merge Intervals

## 1. Introduction
The **Merge Intervals** algorithm consolidates a collection of intervals by combining any that overlap or are directly adjacent. It is widely used in:
- Calendar/meeting scheduling (finding free/busy slots).
- Range compression in computational geometry.
- Resource allocation and time-based analytics.
- Preprocessing step for many sweep-line algorithms.

Use this algorithm when you have a set of ranges (start, end) and need a canonical, non-overlapping, sorted representation of them.

## 2. Usage
```csharp
using System;
using System.Collections.Generic;

public static class Example
{
    public static void Demo()
    {
        var intervals = new List<Interval>
        {
            new Interval(1, 3),
            new Interval(2, 6),
            new Interval(8, 10),
            new Interval(15, 18)
        };

        var merger = new IntervalMerger();
        List<Interval> merged = merger.MergeIntervals(intervals);

        foreach (var interval in merged)
        {
            Console.WriteLine(interval); // [1, 6], [8, 10], [15, 18]
        }
    }
}
```

## 3. Detailed Explanation
The implementation is split across two classes:

- **`Interval`**: A simple data holder with `Start` and `End` integer properties, plus a convenient `ToString` override.
- **`IntervalMerger`**: Contains the merging logic.

The `MergeIntervals` method performs the following steps:
1. **Edge case handling**: If the input is `null` or empty, an empty list is returned. If only one interval exists, a copy is returned.
2. **Sorting** (`SortIntervals` helper): All intervals are ordered by `Start`, with `End` as a tie-breaker. This ensures each new interval can only overlap with the most recently merged one.
3. **Iterative merging**: The first interval becomes the current merge window. For every subsequent interval:
   - If it overlaps/adjoins the current window (`next.Start <= current.End`), the window's `End` is extended to `max(current.End, next.End)`. This naturally handles the case where one interval is fully contained inside another.
   - Otherwise, a new window is started and appended to the result list.
4. **Overlap detection** (`Overlaps` helper): Because the list is sorted by start time, we only need to check whether the next interval's start falls at or before the current interval's end.

Copies of input intervals are made before mutation to avoid side effects on the caller's data.

## 4. Complexity Analysis
- **Time Complexity**: `O(n log n)` dominated by the initial sort. The single linear scan to merge is `O(n)`.
- **Space Complexity**: `O(n)` for the sorted copy and the output list. If the sort is done in place and inputs may be mutated, the auxiliary space can be reduced to `O(log n)` (sort stack) plus the output.

| Operation          | Complexity |
|--------------------|------------|
| Sorting            | O(n log n) |
| Merging scan       | O(n)       |
| Overall            | O(n log n) |
| Auxiliary memory   | O(n)       |
