using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a Wavelet Tree data structure for efficient range queries on a sequence of comparable elements.
/// </summary>
/// <typeparam name="T">The type of elements in the sequence. Must implement IComparable<T>.</typeparam>
public class WaveletTree<T> where T : IComparable<T>
{
    private readonly T _minVal;
    private readonly T _maxVal;
    private readonly int _alphabetSize;
    private readonly int _midVal;

    private WaveletTree<T> _left;
    private WaveletTree<T> _right;
    private readonly int[] _rank;
    private readonly int _totalElements;

    /// <summary>
    /// Initializes a new instance of the <see cref="WaveletTree{T}"/> class.
    /// </summary>
    /// <param name="sequence">The input sequence of elements.</param>
    /// <param name="minVal">The minimum possible value in the alphabet.</param>
    /// <param name="maxVal">The maximum possible value in the alphabet.</param>
    public WaveletTree(IEnumerable<T> sequence, T minVal, T maxVal)
    {
        _minVal = minVal;
        _maxVal = maxVal;
        _alphabetSize = GetAlphabetSize(minVal, maxVal);
        _midVal = _alphabetSize / 2;
        _totalElements = sequence.Count();

        if (_totalElements == 0 || _alphabetSize == 0)
        {
            _rank = new int[0];
            return;
        }

        _rank = new int[_totalElements + 1];
        var leftSequence = new List<T>();
        var rightSequence = new List<T>();

        int currentRank = 0;
        foreach (var item in sequence)
        {
            if (item.CompareTo(_minVal) < 0 || item.CompareTo(_maxVal) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequence), "Sequence contains elements outside the specified alphabet range.");
            }

            if (item.CompareTo(_minVal) < _midVal)
            {
                leftSequence.Add(item);
                _rank[currentRank + 1] = _rank[currentRank];
            }
            else
            {
                rightSequence.Add(item);
                _rank[currentRank + 1] = _rank[currentRank] + 1;
            }
            currentRank++;
        }

        if (leftSequence.Count > 0 && _minVal.CompareTo(_maxVal) < 0)
        {
            _left = new WaveletTree<T>(leftSequence, _minVal, GetMidpointValue(_minVal, _maxVal, _midVal));
        }
        if (rightSequence.Count > 0 && _minVal.CompareTo(_maxVal) < 0)
        {
            _right = new WaveletTree<T>(rightSequence, GetMidpointValue(_minVal, _maxVal, _midVal), _maxVal);
        }
    }

    /// <summary>
    /// Counts the occurrences of a specific value within a given range.
    /// </summary>
    /// <param name="left">The starting index of the range (inclusive, 0-indexed).</param>
    /// <param name="right">The ending index of the range (inclusive, 0-indexed).</param>
    /// <param name="value">The value to count.</param>
    /// <returns>The number of occurrences of the value in the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if left or right indices are out of bounds.</exception>
    public int RangeFrequency(int left, int right, T value)
    {
        if (left < 0 || right >= _totalElements || left > right)
        {
            throw new ArgumentOutOfRangeException("Indices must be within the bounds of the sequence and left <= right.");
        }
        if (_alphabetSize == 0)
        {
            return 0;
        }

        return RangeFrequencyInternal(left, right + 1, value);
    }

    /// <summary>
    /// Finds the k-th smallest element within a given range.
    /// </summary>
    /// <param name="left">The starting index of the range (inclusive, 0-indexed).</param>
    /// <param name="right">The ending index of the range (inclusive, 0-indexed).</param>
    /// <param name="k">The rank of the element to find (1-indexed, i.e., k=1 for the smallest element).</param>
    /// <returns>The k-th smallest element in the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if left or right indices are out of bounds, or if k is invalid for the range.</exception>
    public T Quantile(int left, int right, int k)
    {
        if (left < 0 || right >= _totalElements || left > right)
        {
            throw new ArgumentOutOfRangeException("Indices must be within the bounds of the sequence and left <= right.");
        }
        if (k <= 0 || k > (right - left + 1))
        {
            throw new ArgumentOutOfRangeException(nameof(k), "k must be between 1 and the size of the range.");
        }
        if (_alphabetSize == 0)
        {
            throw new InvalidOperationException("Cannot find quantile in an empty tree.");
        }

        return QuantileInternal(left, right + 1, k);
    }

    /// <summary>
    /// Counts the number of elements less than or equal to a given value within a specified range.
    /// </summary>
    /// <param name="left">The starting index of the range (inclusive, 0-indexed).</param>
    /// <param name="right">The ending index of the range (inclusive, 0-indexed).</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns>The count of elements less than or equal to the value in the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if left or right indices are out of bounds.</exception>
    public int RangeCountLessOrEqual(int left, int right, T value)
    {
        if (left < 0 || right >= _totalElements || left > right)
        {
            throw new ArgumentOutOfRangeException("Indices must be within the bounds of the sequence and left <= right.");
        }
        if (_alphabetSize == 0)
        {
            return 0;
        }

        return RangeCountLessOrEqualInternal(left, right + 1, value);
    }

    private int RangeFrequencyInternal(int left, int right, T value)
    {
        if (_minVal.CompareTo(_maxVal) == 0)
        {
            return (value.CompareTo(_minVal) == 0) ? (right - left) : 0;
        }

        int mid = _alphabetSize / 2;
        T midValue = GetMidpointValue(_minVal, _maxVal, mid);

        if (value.CompareTo(_minVal) < 0 || value.CompareTo(_maxVal) > 0)
        {
            return 0;
        }

        if (value.CompareTo(midValue) < 0)
        {
            if (_left == null)
            {
                return 0;
            }
            int leftCount = _rank[left];
            int rightCount = _rank[right];
            return _left.RangeFrequencyInternal(leftCount, rightCount, value);
        }
        else
        {
            if (_right == null)
            {
                return 0;
            }
            int leftCount = left - _rank[left];
            int rightCount = right - _rank[right];
            return _right.RangeFrequencyInternal(leftCount, rightCount, value);
        }
    }

    private T QuantileInternal(int left, int right, int k)
    {
        if (_minVal.CompareTo(_maxVal) == 0)
        {
            return _minVal;
        }

        int leftCount = _rank[right] - _rank[left];
        int rightCount = (right - left) - leftCount;

        if (k <= leftCount)
        {
            if (_left == null)
            {
                throw new InvalidOperationException("Internal error: Left child expected but not found.");
            }
            return _left.QuantileInternal(_rank[left], _rank[right], k);
        }
        else
        {
            if (_right == null)
            {
                throw new InvalidOperationException("Internal error: Right child expected but not found.");
            }
            return _right.QuantileInternal(left - _rank[left], right - _rank[right], k - leftCount);
        }
    }

    private int RangeCountLessOrEqualInternal(int left, int right, T value)
    {
        if (_minVal.CompareTo(_maxVal) == 0)
        {
            return (value.CompareTo(_minVal) >= 0) ? (right - left) : 0;
        }

        int mid = _alphabetSize / 2;
        T midValue = GetMidpointValue(_minVal, _maxVal, mid);

        if (value.CompareTo(_minVal) < 0)
        {
            return 0;
        }
        if (value.CompareTo(_maxVal) >= 0)
        {
            return right - left;
        }

        if (value.CompareTo(midValue) < 0)
        {
            if (_left == null)
            {
                return 0;
            }
            return _left.RangeCountLessOrEqualInternal(_rank[left], _rank[right], value);
        }
        else
        {
            if (_right == null)
            {
                return 0;
            }
            int leftElementsCount = _rank[right] - _rank[left];
            return leftElementsCount + _right.RangeCountLessOrEqualInternal(left - _rank[left], right - _rank[right], value);
        }
    }

    // Helper to determine alphabet size. Assumes integer-like behavior for min/max.
    // This is a simplification. For arbitrary T, a more robust alphabet mapping might be needed.
    private int GetAlphabetSize(T minVal, T maxVal)
    {
        if (minVal.CompareTo(maxVal) > 0)
        {
            return 0;
        }
        if (minVal.CompareTo(maxVal) == 0)
        {
            return 1;
        }
        // This assumes T can be cast to a numeric type for size calculation.
        // For a truly generic solution, a mapping function or a different approach to alphabet size is needed.
        try
        {
            long minLong = Convert.ToInt64(minVal);
            long maxLong = Convert.ToInt64(maxVal);
            return (int)(maxLong - minLong + 1);
        }
        catch
        {
            // Fallback for types that don't directly convert to long.
            // This might not be accurate for all T, but provides a basic mechanism.
            // A more robust solution would involve a pre-defined alphabet or a mapping.
            return 1000; // Arbitrary large enough for many cases, but not ideal.
        }
    }

    // Helper to get the value corresponding to a midpoint index.
    // This is a simplification. For arbitrary T, a more robust mapping is needed.
    private T GetMidpointValue(T minVal, T maxVal, int midIndex)
    {
        if (minVal.CompareTo(maxVal) == 0)
        {
            return minVal;
        }
        try
        {
            long minLong = Convert.ToInt64(minVal);
            long midValueLong = minLong + midIndex;
            return (T)Convert.ChangeType(midValueLong, typeof(T));
        }
        catch
        {
            // Fallback for types that don't directly convert to long.
            // This is a heuristic and might not be accurate.
            // A better approach would be to pass a function to map index to value.
            // For simplicity, we'll assume a linear interpolation if possible.
            // This part is highly dependent on the nature of T.
            // For demonstration, we'll return a value that's conceptually between min and max.
            // This might require a custom comparer or a more complex structure.
            // For integer types, this is generally fine.
            throw new NotSupportedException("Generic T type requires a specific mapping function for midpoint calculation.");
        }
    }
}