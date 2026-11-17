public class SegmentTree {
    private int[] tree;
    private int[] arr;
    private int n;

    public SegmentTree(int[] input) {
        arr = input;
        n = arr.Length;
        tree = new int[4 * n];
        Build(0, 0, n - 1);
    }

    private void Build(int node, int start, int end) {
        if (start == end) {
            tree[node] = arr[start];
        } else {
            int mid = (start + end) / 2;
            Build(2 * node + 1, start, mid);
            Build(2 * node + 2, mid + 1, end);
            tree[node] = Math.Max(tree[2 * node + 1], tree[2 * node + 2]);
        }
    }

    public int RangeMaxQuery(int L, int R) {
        return RangeMaxQuery(0, 0, n - 1, L, R);
    }

    private int RangeMaxQuery(int node, int start, int end, int L, int R) {
        if (R < start || end < L) {
            return int.MinValue;
        }
        if (L <= start && end <= R) {
            return tree[node];
        }
        int mid = (start + end) / 2;
        int leftMax = RangeMaxQuery(2 * node + 1, start, mid, L, R);
        int rightMax = RangeMaxQuery(2 * node + 2, mid + 1, end, L, R);
        return Math.Max(leftMax, rightMax);
    }

    public void Update(int index, int value) {
        Update(0, 0, n - 1, index, value);
    }

    private void Update(int node, int start, int end, int index, int value) {
        if (start == end) {
            arr[index] = value;
            tree[node] = value;
        } else {
            int mid = (start + end) / 2;
            if (start <= index && index <= mid) {
                Update(2 * node + 1, start, mid, index, value);
            } else {
                Update(2 * node + 2, mid + 1, end, index, value);
            }
            tree[node] = Math.Max(tree[2 * node + 1], tree[2 * node + 2]);
        }
    }
}