using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class ConsistentHashRing<TNode>
{
    private readonly int _replicationFactor;
    private readonly Func<string, uint> _hashFunction;
    private readonly List<uint> _sortedKeys = new List<uint>();
    private readonly Dictionary<uint, TNode> _ring = new Dictionary<uint, TNode>();
    private readonly object _lock = new object();

    public ConsistentHashRing(int replicationFactor, Func<string, uint>? hashFunction = null)
    {
        if (replicationFactor <= 0)
            throw new ArgumentOutOfRangeException(nameof(replicationFactor), "Replication factor must be greater than 0.");

        _replicationFactor = replicationFactor;
        _hashFunction = hashFunction ?? DefaultHash;
    }

    private static uint DefaultHash(string key)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToUInt32(hashBytes, 0);
        }
    }

    public void AddNode(TNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        string? nodeString = node.ToString();
        if (string.IsNullOrEmpty(nodeString))
            throw new ArgumentException("Node ToString() representation cannot be null or empty.", nameof(node));

        lock (_lock)
        {
            for (int i = 0; i < _replicationFactor; i++)
            {
                string virtualNodeKey = $"{nodeString}_{i}";
                uint hash = _hashFunction(virtualNodeKey);

                if (!_ring.ContainsKey(hash))
                {
                    _ring[hash] = node;
                    int index = _sortedKeys.BinarySearch(hash);
                    if (index < 0)
                    {
                        _sortedKeys.Insert(~index, hash);
                    }
                }
            }
        }
    }

    public void RemoveNode(TNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        string? nodeString = node.ToString();
        if (string.IsNullOrEmpty(nodeString))
            throw new ArgumentException("Node ToString() representation cannot be null or empty.", nameof(node));

        lock (_lock)
        {
            for (int i = 0; i < _replicationFactor; i++)
            {
                string virtualNodeKey = $"{nodeString}_{i}";
                uint hash = _hashFunction(virtualNodeKey);

                if (_ring.Remove(hash))
                {
                    int index = _sortedKeys.BinarySearch(hash);
                    if (index >= 0)
                    {
                        _sortedKeys.RemoveAt(index);
                    }
                }
            }
        }
    }

    public TNode GetNode(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        lock (_lock)
        {
            if (_sortedKeys.Count == 0)
            {
                throw new InvalidOperationException("Hash ring is empty.");
            }

            uint hash = _hashFunction(key);
            int index = _sortedKeys.BinarySearch(hash);

            if (index < 0)
            {
                index = ~index;
                if (index >= _sortedKeys.Count)
                {
                    index = 0;
                }
            }

            return _ring[_sortedKeys[index]];
        }
    }
}