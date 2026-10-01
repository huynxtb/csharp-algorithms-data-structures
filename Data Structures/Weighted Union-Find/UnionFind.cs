public class UnionFind {
    private int[] parent;
    private int[] size;

    public UnionFind(int n) {
        parent = new int[n];
        size = new int[n];
        for (int i = 0; i < n; i++) {
            parent[i] = i;
            size[i] = 1;
        }
    }

    public int Find(int x) {
        if (parent[x] != x) {
            parent[x] = Find(parent[x]); // Path compression
        }
        return parent[x];
    }

    public void Union(int x, int y) {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX != rootY) {
            // Union by size
            if (size[rootX] < size[rootY]) {
                parent[rootX] = rootY;
                size[rootY] += size[rootX];
            } else {
                parent[rootY] = rootX;
                size[rootX] += size[rootY];
            }
        }
    }
}