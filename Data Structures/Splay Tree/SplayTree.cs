using System;

public class SplayTree<TKey, TValue> where TKey : IComparable<TKey>
{
    private class Node
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Parent { get; set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    private Node root;
    private int count;

    public int Count => count;
    public bool IsEmpty => count == 0;

    public void Insert(TKey key, TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        if (root == null)
        {
            root = new Node(key, value);
            count++;
            return;
        }

        Node current = root;
        Node parent = null;
        int compare = 0;

        while (current != null)
        {
            parent = current;
            compare = key.CompareTo(current.Key);
            if (compare < 0)
            {
                current = current.Left;
            }
            else if (compare > 0)
            {
                current = current.Right;
            }
            else
            {
                current.Value = value;
                Splay(current);
                return;
            }
        }

        Node newNode = new Node(key, value) { Parent = parent };
        if (compare < 0)
        {
            parent.Left = newNode;
        }
        else
        {
            parent.Right = newNode;
        }

        count++;
        Splay(newNode);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        Node node = FindNode(key);
        if (node != null)
        {
            value = node.Value;
            return true;
        }

        value = default;
        return false;
    }

    public bool Delete(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        Node node = FindNode(key);
        if (node == null || node.Key.CompareTo(key) != 0)
        {
            return false;
        }

        // Node is now root due to FindNode splay
        if (node.Left == null)
        {
            root = node.Right;
            if (root != null)
            {
                root.Parent = null;
            }
        }
        else
        {
            Node rightSubtree = node.Right;
            Node leftSubtree = node.Left;
            leftSubtree.Parent = null;
            root = leftSubtree;

            Node maxLeft = GetMaximumNode(leftSubtree);
            Splay(maxLeft);

            root.Right = rightSubtree;
            if (rightSubtree != null)
            {
                rightSubtree.Parent = root;
            }
        }

        count--;
        return true;
    }

    public TKey GetMinimum()
    {
        if (root == null) throw new InvalidOperationException("Tree is empty.");
        Node minNode = GetMinimumNode(root);
        Splay(minNode);
        return minNode.Key;
    }

    public TKey GetMaximum()
    {
        if (root == null) throw new InvalidOperationException("Tree is empty.");
        Node maxNode = GetMaximumNode(root);
        Splay(maxNode);
        return maxNode.Key;
    }

    private Node FindNode(TKey key)
    {
        Node current = root;
        Node lastAccessed = null;

        while (current != null)
        {
            lastAccessed = current;
            int compare = key.CompareTo(current.Key);
            if (compare < 0)
            {
                current = current.Left;
            }
            else if (compare > 0)
            {
                current = current.Right;
            }
            else
            {
                Splay(current);
                return current;
            }
        }

        if (lastAccessed != null)
        {
            Splay(lastAccessed);
        }
        return null;
    }

    private void Splay(Node x)
    {
        if (x == null) return;

        while (x.Parent != null)
        {
            Node parent = x.Parent;
            Node grandparent = parent.Parent;

            if (grandparent == null)
            {
                // Zig
                if (x == parent.Left)
                {
                    RotateRight(parent);
                }
                else
                {
                    RotateLeft(parent);
                }
            }
            else if (x == parent.Left && parent == grandparent.Left)
            {
                // Zig-Zig
                RotateRight(grandparent);
                RotateRight(parent);
            }
            else if (x == parent.Right && parent == grandparent.Right)
            {
                // Zig-Zig
                RotateLeft(grandparent);
                RotateLeft(parent);
            }
            else if (x == parent.Left && parent == grandparent.Right)
            {
                // Zig-Zag
                RotateRight(parent);
                RotateLeft(grandparent);
            }
            else
            {
                // Zig-Zag
                RotateLeft(parent);
                RotateRight(grandparent);
            }
        }
        root = x;
    }

    private void RotateLeft(Node x)
    {
        Node y = x.Right;
        if (y == null) return;

        x.Right = y.Left;
        if (y.Left != null)
        {
            y.Left.Parent = x;
        }

        y.Parent = x.Parent;
        if (x.Parent == null)
        {
            root = y;
        }
        else if (x == x.Parent.Left)
        {
            x.Parent.Left = y;
        }
        else
        {
            x.Parent.Right = y;
        }

        y.Left = x;
        x.Parent = y;
    }

    private void RotateRight(Node x)
    {
        Node y = x.Left;
        if (y == null) return;

        x.Left = y.Right;
        if (y.Right != null)
        {
            y.Right.Parent = x;
        }

        y.Parent = x.Parent;
        if (x.Parent == null)
        {
            root = y;
        }
        else if (x == x.Parent.Left)
        {
            x.Parent.Left = y;
        }
        else
        {
            x.Parent.Right = y;
        }

        y.Right = x;
        x.Parent = y;
    }

    private Node GetMinimumNode(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    private Node GetMaximumNode(Node node)
    {
        while (node.Right != null)
        {
            node = node.Right;
        }
        return node;
    }
}