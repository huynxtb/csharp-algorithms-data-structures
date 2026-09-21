namespace DataStructures.VanEmdeBoas;

using System;

/// <summary>
/// Represents a Van Emde Boas (vEB) tree data structure for dynamic set operations on integer keys.
/// Universe sizes are powers of 2. Key operations (Insert, Delete, Contains, Successor, Predecessor)
/// run in O(log log U) time where U is the universe size.
/// </summary>
public class VanEmdeBoasTree
{
    private readonly int _universeSize;
    private readonly int _lowerSqrtShift;
    private readonly int _lowerSqrtMask;
    private readonly int _upperSqrtShift;

    private int? _min;
    private int? _max;

    private VanEmdeBoasTree? _summary;
    private readonly VanEmdeBoasTree?[]? _clusters;

    /// <summary>
    /// Gets the universe size U of this tree (always a power of 2).
    /// </summary>
    public int UniverseSize => _universeSize;

    /// <summary>
    /// Gets the minimum element currently stored, or <c>null</c> if empty.
    /// Time complexity: O(1).
    /// </summary>
    public int? Min => _min;

    /// <summary>
    /// Gets the maximum element currently stored, or <c>null</c> if empty.
    /// Time complexity: O(1).
    /// </summary>
    public int? Max => _max;

    /// <summary>
    /// Gets a value indicating whether the tree contains no elements.
    /// Time complexity: O(1).
    /// </summary>
    public bool IsEmpty => !_min.HasValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="VanEmdeBoasTree"/> class.
    /// If <paramref name="universeSize"/> is not a power of 2, it is automatically rounded up to the next power of 2.
    /// </summary>
    /// <param name="universeSize">The requested universe size (must be at least 2).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="universeSize"/> is less than 2 or too large for 32-bit integers.</exception>
    public VanEmdeBoasTree(int universeSize)
    {
        if (universeSize < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(universeSize), "Universe size must be at least 2.");
        }

        _universeSize = RoundUpPowerOf2(universeSize);

        int totalBits = 31 - System.Numerics.BitOperations.LeadingZeroCount((uint)_universeSize);
        _lowerSqrtShift = totalBits / 2;
        _upperSqrtShift = totalBits - _lowerSqrtShift;
        _lowerSqrtMask = (1 << _lowerSqrtShift) - 1;

        _min = null;
        _max = null;

        if (_universeSize > 2)
        {
            int upperSqrt = 1 << _upperSqrtShift;
            _clusters = new VanEmdeBoasTree?[upperSqrt];
            _summary = null; // Lazily initialized
        }
        else
        {
            _clusters = null;
            _summary = null;
        }
    }

    /// <summary>
    /// Returns the high bits (cluster index) for the given element <paramref name="x"/>.
    /// </summary>
    private int High(int x) => x >> _lowerSqrtShift;

    /// <summary>
    /// Returns the low bits (offset within cluster) for the given element <paramref name="x"/>.
    /// </summary>
    private int Low(int x) => x & _lowerSqrtMask;

    /// <summary>
    /// Combines a cluster index and an offset back into the universe element index.
    /// </summary>
    private int Index(int high, int low) => (high << _lowerSqrtShift) | low;

    /// <summary>
    /// Determines whether the tree contains the specified integer key.
    /// </summary>
    /// <param name="x">The integer key to locate.</param>
    /// <returns><c>true</c> if the element is present; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="x"/> is not in the range [0, UniverseSize - 1].</exception>
    /// <remarks>Time complexity: O(log log U).</remarks>
    public bool Contains(int x)
    {
        ValidateElement(x);
        return ContainsInternal(x);
    }

    private bool ContainsInternal(int x)
    {
        if (x == _min || x == _max)
        {
            return true;
        }

        if (_universeSize == 2 || !_min.HasValue)
        {
            return false;
        }

        int high = High(x);
        var cluster = _clusters?[high];
        if (cluster == null)
        {
            return false;
        }

        return cluster.ContainsInternal(Low(x));
    }

    /// <summary>
    /// Inserts the specified integer key into the tree. Idempotent (no-op if already present).
    /// </summary>
    /// <param name="x">The integer key to insert.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="x"/> is not in the range [0, UniverseSize - 1].</exception>
    /// <remarks>Time complexity: O(log log U).</remarks>
    public void Insert(int x)
    {
        ValidateElement(x);
        InsertInternal(x);
    }

    private void InsertInternal(int x)
    {
        if (!_min.HasValue)
        {
            _min = x;
            _max = x;
            return;
        }

        if (x == _min)
        {
            return;
        }

        if (x < _min)
        {
            int temp = _min.Value;
            _min = x;
            x = temp;
        }

        if (_universeSize > 2)
        {
            int high = High(x);
            int low = Low(x);

            var cluster = GetOrCreateCluster(high);

            if (!cluster._min.HasValue)
            {
                GetOrCreateSummary().InsertInternal(high);
                cluster._min = low;
                cluster._max = low;
            }
            else
            {
                cluster.InsertInternal(low);
            }
        }

        if (x > _max)
        {
            _max = x;
        }
    }

    /// <summary>
    /// Removes the specified integer key from the tree. Idempotent (no-op if not present).
    /// </summary>
    /// <param name="x">The integer key to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="x"/> is not in the range [0, UniverseSize - 1].</exception>
    /// <remarks>Time complexity: O(log log U).</remarks>
    public void Delete(int x)
    {
        ValidateElement(x);
        DeleteInternal(x);
    }

    private void DeleteInternal(int x)
    {
        if (!_min.HasValue)
        {
            return;
        }

        if (_min == _max)
        {
            if (x == _min)
            {
                _min = null;
                _max = null;
            }
            return;
        }

        if (_universeSize == 2)
        {
            if (x == 0)
            {
                _min = 1;
            }
            else if (x == 1)
            {
                _min = 0;
            }
            _max = _min;
            return;
        }

        if (x == _min)
        {
            if (_summary == null || !_summary._min.HasValue)
            {
                _min = _max;
                return;
            }

            int firstClusterIndex = _summary._min.Value;
            x = Index(firstClusterIndex, _clusters![firstClusterIndex]!._min!.Value);
            _min = x;
        }

        int high = High(x);
        int low = Low(x);
        var cluster = _clusters?[high];

        if (cluster != null)
        {
            cluster.DeleteInternal(low);

            if (!cluster._min.HasValue)
            {
                _summary?.DeleteInternal(high);
            }
        }

        if (x == _max)
        {
            if (_summary == null || !_summary._max.HasValue)
            {
                _max = _min;
            }
            else
            {
                int lastClusterIndex = _summary._max.Value;
                _max = Index(lastClusterIndex, _clusters![lastClusterIndex]!._max!.Value);
            }
        }
    }

    /// <summary>
    /// Finds the smallest element in the tree strictly greater than <paramref name="x"/>.
    /// </summary>
    /// <param name="x">The reference key.</param>
    /// <returns>The successor of <paramref name="x"/>, or <c>null</c> if no such element exists.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="x"/> is not in the range [0, UniverseSize - 1].</exception>
    /// <remarks>Time complexity: O(log log U).</remarks>
    public int? Successor(int x)
    {
        ValidateElement(x);
        return SuccessorInternal(x);
    }

    private int? SuccessorInternal(int x)
    {
        if (_universeSize == 2)
        {
            if (x == 0 && _max == 1)
            {
                return 1;
            }
            return null;
        }

        if (_min.HasValue && x < _min.Value)
        {
            return _min;
        }

        int high = High(x);
        int low = Low(x);
        var cluster = _clusters?[high];

        int? maxLow = cluster?.Max;
        if (maxLow.HasValue && low < maxLow.Value)
        {
            int? offset = cluster!.SuccessorInternal(low);
            return offset.HasValue ? Index(high, offset.Value) : null;
        }

        int? nextCluster = _summary?.SuccessorInternal(high);
        if (!nextCluster.HasValue)
        {
            return null;
        }

        int? offsetInNext = _clusters![nextCluster.Value]!._min;
        return offsetInNext.HasValue ? Index(nextCluster.Value, offsetInNext.Value) : null;
    }

    /// <summary>
    /// Finds the largest element in the tree strictly less than <paramref name="x"/>.
    /// </summary>
    /// <param name="x">The reference key.</param>
    /// <returns>The predecessor of <paramref name="x"/>, or <c>null</c> if no such element exists.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="x"/> is not in the range [0, UniverseSize - 1].</exception>
    /// <remarks>Time complexity: O(log log U).</remarks>
    public int? Predecessor(int x)
    {
        ValidateElement(x);
        return PredecessorInternal(x);
    }

    private int? PredecessorInternal(int x)
    {
        if (_universeSize == 2)
        {
            if (x == 1 && _min == 0)
            {
                return 0;
            }
            return null;
        }

        if (_max.HasValue && x > _max.Value)
        {
            return _max;
        }

        int high = High(x);
        int low = Low(x);
        var cluster = _clusters?[high];

        int? minLow = cluster?.Min;
        if (minLow.HasValue && low > minLow.Value)
        {
            int? offset = cluster!.PredecessorInternal(low);
            return offset.HasValue ? Index(high, offset.Value) : null;
        }

        int? prevCluster = _summary?.PredecessorInternal(high);
        if (!prevCluster.HasValue)
        {
            if (_min.HasValue && x > _min.Value)
            {
                return _min;
            }
            return null;
        }

        int? offsetInPrev = _clusters![prevCluster.Value]!._max;
        return offsetInPrev.HasValue ? Index(prevCluster.Value, offsetInPrev.Value) : null;
    }

    private VanEmdeBoasTree GetOrCreateCluster(int high)
    {
        if (_clusters![high] == null)
        {
            _clusters[high] = new VanEmdeBoasTree(1 << _lowerSqrtShift);
        }
        return _clusters[high]!;
    }

    private VanEmdeBoasTree GetOrCreateSummary()
    {
        if (_summary == null)
        {
            _summary = new VanEmdeBoasTree(1 << _upperSqrtShift);
        }
        return _summary;
    }

    private void ValidateElement(int x)
    {
        if (x < 0 || x >= _universeSize)
        {
            throw new ArgumentOutOfRangeException(nameof(x), $"Element {x} is out of bounds for universe [0, {_universeSize - 1}].");
        }
    }

    private static int RoundUpPowerOf2(int v)
    {
        if (v <= 2) return 2;
        if (v > (1 << 30))
        {
            throw new ArgumentOutOfRangeException(nameof(v), "Universe size exceeds maximum supported 32-bit power of 2.");
        }
        return 1 << (32 - System.Numerics.BitOperations.LeadingZeroCount((uint)(v - 1)));
    }
}