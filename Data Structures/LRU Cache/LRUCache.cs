using System;
using System.Collections.Generic;

public class LRUCache<TKey, TValue> where TKey : notnull
{
    private class Node
    {
        public TKey Key { get; }
        public TValue Value { get; set; }
        public Node? Prev { get; set; }
        public Node? Next { get; set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    private readonly int _capacity;
    private readonly Dictionary<TKey, Node> _cache;
    private Node? _head;
    private Node? _tail;

    public int Count => _cache.Count;
    public int Capacity => _capacity;

    public LRUCache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }
        _capacity = capacity;
        _cache = new Dictionary<TKey, Node>(capacity);
    }

    public TValue Get(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (!_cache.TryGetValue(key, out var node))
        {
            throw new KeyNotFoundException($"The key '{key}' was not found in the cache.");
        }
        MoveToHead(node);
        return node.Value;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (_cache.TryGetValue(key, out var node))
        {
            MoveToHead(node);
            value = node.Value;
            return true;
        }
        value = default!;
        return false;
    }

    public void Put(TKey key, TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (_cache.TryGetValue(key, out var node))
        {
            node.Value = value;
            MoveToHead(node);
        }
        else
        {
            if (_cache.Count >= _capacity)
            {
                var tail = RemoveTail();
                if (tail != null)
                {
                    _cache.Remove(tail.Key);
                }
            }
            var newNode = new Node(key, value);
            AddToHead(newNode);
            _cache[key] = newNode;
        }
    }

    private void AddToHead(Node node)
    {
        node.Next = _head;
        node.Prev = null;
        if (_head != null)
        {
            _head.Prev = node;
        }
        _head = node;
        if (_tail == null)
        {
            _tail = node;
        }
    }

    private void RemoveNode(Node node)
    {
        if (node.Prev != null)
        {
            node.Prev.Next = node.Next;
        }
        else
        {
            _head = node.Next;
        }

        if (node.Next != null)
        {
            node.Next.Prev = node.Prev;
        }
        else
        {
            _tail = node.Prev;
        }
        node.Prev = null;
        node.Next = null;
    }

    private void MoveToHead(Node node)
    {
        if (node == _head) return;
        RemoveNode(node);
        AddToHead(node);
    }

    private Node? RemoveTail()
    {
        if (_tail == null) return null;
        var res = _tail;
        RemoveNode(res);
        return res;
    }
}