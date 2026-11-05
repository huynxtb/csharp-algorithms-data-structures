public class KaryTreeNode<T>
{
    /// <summary>
    /// Gets or sets the value stored in this node.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets the list of child nodes. Each node can have up to K children.
    /// </summary>
    public List<KaryTreeNode<T>> Children { get; private set; }

    /// <summary>
    /// Initializes a new instance of the KaryTreeNode class with the specified value.
    /// </summary>
    /// <param name="value">The value to store in the node.</param>
    public KaryTreeNode(T value)
    {
        Value = value;
        Children = new List<KaryTreeNode<T>>();
    }
}

public class KaryTree<T>
{
    /// <summary>
    /// Gets the root node of the K-ary tree.
    /// </summary>
    public KaryTreeNode<T> Root { get; private set; }

    /// <summary>
    /// Gets the maximum number of children (K) allowed for any node in the tree.
    /// </summary>
    public int K { get; private set; }

    /// <summary>
    /// Initializes a new instance of the KaryTree class with a specified maximum number of children (K).
    /// </summary>
    /// <param name="k">The maximum number of children a node can have. Must be a positive integer.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown if K is less than or equal to 0.</exception>
    public KaryTree(int k)
    {
        if (k <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(k), "K must be a positive integer.");
        }
        K = k;
    }

    /// <summary>
    /// Sets the root of the K-ary tree. This method can only be called once when the tree is empty.
    /// </summary>
    /// <param name="value">The value for the root node.</param>
    /// <exception cref="System.InvalidOperationException">Thrown if the root already exists.</exception>
    public void SetRoot(T value)
    {
        if (Root != null)
        {
            throw new System.InvalidOperationException("Root already exists. Use AddChild methods to add nodes.");
        }
        Root = new KaryTreeNode<T>(value);
    }

    /// <summary>
    /// Adds a new child node to a specified parent node object.
    /// This method is useful when you already have a reference to the parent node (e.g., from a Search operation).
    /// </summary>
    /// <param name="parentNode">The existing parent node to which the child will be added.</param>
    /// <param name="childValue">The value of the new child node.</param>
    /// <returns>True if the child was added successfully, false otherwise (e.g., parent is null, or parent already has K children).</returns>
    public bool AddChild(KaryTreeNode<T> parentNode, T childValue)
    {
        if (parentNode == null)
        {
            return false; // Cannot add child to a null parent node reference.
        }

        if (parentNode.Children.Count >= K)
        {
            return false; // Parent already has K children, cannot add more.
        }

        KaryTreeNode<T> newChild = new KaryTreeNode<T>(childValue);
        parentNode.Children.Add(newChild);
        return true;
    }

    /// <summary>
    /// Adds a new child node to a parent node identified by its value.
    /// This method will search for the parent node in the tree.
    /// </summary>
    /// <param name="parentValue">The value of the parent node.</param>
    /// <param name="childValue">The value of the new child node.</param>
    /// <returns>True if the child was added successfully, false otherwise (e.g., parent not found, or parent already has K children).</returns>
    public bool AddChild(T parentValue, T childValue)
    {
        if (Root == null)
        {
            return false; // Cannot add child if the tree is empty (no root).
        }

        KaryTreeNode<T> parentNode = Search(parentValue); // Use Search to find the parent
        return AddChild(parentNode, childValue); // Delegate to the other AddChild method
    }

    /// <summary>
    /// Performs a pre-order traversal of the K-ary tree.
    /// In pre-order traversal, the current node is visited first, then its children are visited recursively from left to right.
    /// </summary>
    /// <returns>A list of values in pre-order sequence. Returns an empty list if the tree is empty.</returns>
    public System.Collections.Generic.List<T> PreOrderTraversal()
    {
        System.Collections.Generic.List<T> result = new System.Collections.Generic.List<T>();
        PreOrderTraversalRecursive(Root, result);
        return result;
    }

    /// <summary>
    /// Recursive helper method for pre-order traversal.
    /// </summary>
    /// <param name="node">The current node being visited.</param>
    /// <param name="result">The list to accumulate the traversed values.</param>
    private void PreOrderTraversalRecursive(KaryTreeNode<T> node, System.Collections.Generic.List<T> result)
    {
        if (node == null)
        {
            return;
        }

        result.Add(node.Value); // Visit current node
        foreach (var child in node.Children)
        {
            PreOrderTraversalRecursive(child, result); // Recursively visit children
        }
    }

    /// <summary>
    /// Searches for a node with a specific value in the K-ary tree using Breadth-First Search (BFS).
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>The KaryTreeNode if a node with the specified value is found, otherwise null.</returns>
    public KaryTreeNode<T> Search(T value)
    {
        if (Root == null)
        {
            return null; // Tree is empty
        }

        System.Collections.Generic.Queue<KaryTreeNode<T>> queue = new System.Collections.Generic.Queue<KaryTreeNode<T>>();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            KaryTreeNode<T> current = queue.Dequeue();
            // Using object.Equals for generic type comparison.
            // This assumes T correctly implements Equals or is a value type.
            if (current.Value.Equals(value))
            {
                return current;
            }

            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }
        return null; // Value not found in the tree
    }
}