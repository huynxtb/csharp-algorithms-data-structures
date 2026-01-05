public class FenwickTreeRangeUpdatePointQuery
{
    private readonly int[] fenw;
    private readonly int size;

    public FenwickTreeRangeUpdatePointQuery(int n)
    {
        size = n;
        fenw = new int[n + 1];
    }

    private void InternalUpdate(int idx, int delta)
    {
        for (++idx; idx <= size; idx += idx & (-idx))
        {
            fenw[idx] += delta;
        }
    }

    private int InternalPrefixSum(int idx)
    {
        int result = 0;
        for (++idx; idx > 0; idx -= idx & (-idx))
        {
            result += fenw[idx];
        }
        return result;
    }

    public void RangeUpdate(int left, int right, int value)
    {
        if (left < 0 || right >= size || left > right)
            throw new System.ArgumentOutOfRangeException();
        InternalUpdate(left, value);
        if (right + 1 < size) InternalUpdate(right + 1, -value);
    }

    public int PointQuery(int index)
    {
        if (index < 0 || index >= size)
            throw new System.ArgumentOutOfRangeException();
        return InternalPrefixSum(index);
    }
}