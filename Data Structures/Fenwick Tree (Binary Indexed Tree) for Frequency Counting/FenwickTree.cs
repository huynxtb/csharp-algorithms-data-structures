public class FenwickTree
{
    private readonly int[] tree;
    private readonly int size;

    /// <summary>
    /// Initializes a Fenwick Tree (Binary Indexed Tree) for frequency counting with a given size.
    /// </summary>
    /// <param name="size">The size of the Fenwick Tree (number of elements).</param>
    public FenwickTree(int size)
    {
        this.size = size;
        tree = new int[size + 1]; // Using 1-based indexing internally
    }

    /// <summary>
    /// Initializes the Fenwick Tree with an input frequency array.
    /// </summary>
    /// <param name="frequencies">The input array of frequencies or values (0-based index).</param>
    public FenwickTree(int[] frequencies) : this(frequencies.Length)
    {
        for (int i = 0; i < frequencies.Length; i++)
        {
            Update(i, frequencies[i]);
        }
    }

    /// <summary>
    /// Updates the frequency at a given index by adding the given delta.
    /// </summary>
    /// <param name="index">The zero-based index where frequency is updated.</param>
    /// <param name="delta">The value to add to the frequency at the given index.</param>
    public void Update(int index, int delta)
    {
        if (index < 0 || index >= size)
            throw new System.ArgumentOutOfRangeException(nameof(index));

        index += 1; // convert to 1-based indexing
        while (index <= size)
        {
            tree[index] += delta;
            index += index & (-index);
        }
    }

    /// <summary>
    /// Computes the prefix sum of frequencies from the start up to and including the given index.
    /// </summary>
    /// <param name="index">The zero-based index up to which the prefix sum is computed.</param>
    /// <returns>The prefix sum of frequencies up to the given index.</returns>
    public int Query(int index)
    {
        if (index < 0 || index >= size)
            throw new System.ArgumentOutOfRangeException(nameof(index));

        int sum = 0;
        index += 1; // convert to 1-based indexing

        while (index > 0)
        {
            sum += tree[index];
            index -= index & (-index);
        }

        return sum;
    }

    /// <summary>
    /// Gets the total sum of all frequencies in the Fenwick Tree.
    /// </summary>
    public int TotalSum => Query(size - 1);
}