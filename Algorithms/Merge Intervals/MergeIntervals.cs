using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents an interval with a start and end value.
/// </summary>
public class Interval
{
    /// <summary>
    /// Gets or sets the start of the interval.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    /// Gets or sets the end of the interval.
    /// </summary>
    public int End { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval"/> class.
    /// </summary>
    public Interval()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval"/> class with the specified start and end values.
    /// </summary>
    /// <param name="start">The start value of the interval.</param>
    /// <param name="end">The end value of the interval.</param>
    public Interval(int start, int end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Returns a string representation of the interval.
    /// </summary>
    /// <returns>A string in the format [Start, End].</returns>
    public override string ToString()
    {
        return $"[{Start}, {End}]";
    }
}

/// <summary>
/// Provides functionality to merge overlapping intervals.
/// </summary>
public class IntervalMerger
{
    /// <summary>
    /// Merges all overlapping or adjacent intervals in the supplied list and returns a sorted list of non-overlapping intervals.
    /// </summary>
    /// <param name="intervals">An unsorted list of intervals to be merged.</param>
    /// <returns>A sorted list of merged, non-overlapping intervals. Returns an empty list if the input is null or empty.</returns>
    public List<Interval> MergeIntervals(List<Interval> intervals)
    {
        var result = new List<Interval>();

        if (intervals == null || intervals.Count == 0)
        {
            return result;
        }

        if (intervals.Count == 1)
        {
            result.Add(new Interval(intervals[0].Start, intervals[0].End));
            return result;
        }

        var sorted = SortIntervals(intervals);

        var current = new Interval(sorted[0].Start, sorted[0].End);
        result.Add(current);

        for (int i = 1; i < sorted.Count; i++)
        {
            var next = sorted[i];

            if (Overlaps(current, next))
            {
                current.End = Math.Max(current.End, next.End);
            }
            else
            {
                current = new Interval(next.Start, next.End);
                result.Add(current);
            }
        }

        return result;
    }

    /// <summary>
    /// Sorts intervals by start time, then by end time to break ties.
    /// </summary>
    /// <param name="intervals">The intervals to sort.</param>
    /// <returns>A new list containing the intervals sorted ascending by start then end.</returns>
    private List<Interval> SortIntervals(List<Interval> intervals)
    {
        return intervals
            .Where(i => i != null)
            .OrderBy(i => i.Start)
            .ThenBy(i => i.End)
            .ToList();
    }

    /// <summary>
    /// Determines whether two intervals overlap or are adjacent. Assumes <paramref name="a"/> starts at or before <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first interval (expected to be the earlier interval).</param>
    /// <param name="b">The second interval.</param>
    /// <returns>True if the intervals overlap or are adjacent; otherwise false.</returns>
    private bool Overlaps(Interval a, Interval b)
    {
        return b.Start <= a.End;
    }
}