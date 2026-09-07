using System;
using System.Collections.Generic;

public class LruCache<TKey, TValue> where TKey : notnull
{
    private class CacheItem
    {
        public TKey Key { me; }
        public TValue Value { get; set; }

        public CacheItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<CacheItem>> _map;
    private readonly LinkedList<CacheItem> _list;

    public LruCache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        _capacity = capacity;
        _map = new Dictionary<TKey, LinkedListNode<CacheItem>>();
        _list = new LinkedList<CacheItem>();
    }

    public int Capacity => _capacity;
    public int Count => _map.Count;

    public bool TryGet(TKey key, out TValue value)
    {
        if (_map.TryGetValue(key, out var node))
        {
            _list.Remove(node);
            _list.AddFirst(node);
            value = node.Value.Value;
            return true;
        }

        value = default!;
        return false;
    }

    public TValue Get(TKey key)
    {
        if (TryGet(key, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Key '{key}' was not found in the cache.");
    }

    public void Put(TKey key, TValue value)
    {
        if (_map.TryGetValue(key, out var node))
        {
            node.Value.Value = value;
            _list.Remove(node);
            _list.AddFirst(node);
            return;
        }

        if (_map.Count >= _capacity)
        {
            var lastNode = _list.Last;
            if (lastNode != null)
            {
                _map.Remove(lastNode.Value.Key);
                _list.RemoveLast();
            }
        }

        var newItem = new CacheItem(key, value);
        var newNode = _list.AddFirst(newItem);
        _map[key] = newNode;
    }

    public bool Remove(TKey key)
    {
        if (_map.TryGetValue(key, out var node))
        {
            _map.Remove(key);
            _list.Remove(node);
            return true;
        }

        return false;
    }

    public void Clear()
    {
        _map.Clear();
        _list.Clear();
    }
}