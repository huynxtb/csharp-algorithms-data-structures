using System;

public class FenwickTreeFrequency
{
    private readonly int[] fenwickTree;
    private readonly int size;

    /// <summary>
    /// Initializes a Fenwick Tree for frequency counting with a given size.
    /// </summary>
    /// <param name="size">The size of the input array (number of elements).</param>
    public FenwickTreeFrequency(int size)
    {
        this.size = size;
        this.fenwickTree = new int[size + 1]; // 1-based indexing
    }

    /// <summary>
    /// Updates the frequency count of the element at the specified 1-based index by adding delta.
    /// </summary>
    /// <param name="index">1-based index of the element to update.</param>
    /// <param name="delta">Amount to add to the frequency (can be positive or negative).</param>
    public void Update(int index, int delta)
    {
        if (index <= 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 1 and the size of the Fenwick Tree.");

        while (index <= size)
        {
            fenwickTree[index] += delta;
            index += index & (-index);
        }
    }

    /// <summary>
    /// Queries the cumulative frequency (prefix sum) from the start up to the given 1-based index.
    /// </summary>
    /// <param name="index">1-based index up to which to compute the prefix sum.</param>
    /// <returns>The cumulative frequency sum up to the given index.</returns>
    public int Query(int index)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and the size of the Fenwick Tree.");

        int sum = 0;
        while (index > 0)
        {
            sum += fenwickTree[index];
            index -= index & (-index);
        }
        return sum;
    }
}