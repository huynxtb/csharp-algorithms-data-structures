using System;
using System.Collections.Generic; 

public class LfuCache<TKey, TValue>
{
    private class Node
    {
        public TKey Key { get; }
        public TValue Value { get; set; }
        public int Frequency { get; set; }
        public Node Prev { get; set; }
        public Node Next { get; set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Frequency = 1;
        }
    }

    private class DoublyLinkedList
    {
        public Node Head { get; }
        public Node Tail { get; }
        public int Count { get; private set; }

        public DoublyLinkedList()
        {
            Head = new Node(default, default);
            Tail = new Node(default, default);
            Head.Next = Tail;
            Tail.Prev = Head;
            Count = 0;
        }

        public void AddLast(Node node)
        {
            Node prev = Tail.Prev;
            prev.Next = node;
            node.Prev = prev;
            node.Next = Tail;
            Tail.Prev = node;
            Count++;
        }

        public void Remove(Node node)
        {
            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;
            node.Prev = null;
            node.Next = null;
            Count--;
        }

        public Node RemoveFirst()
        {
            if (Count == 0) return null;
            Node first = Head.Next;
            Remove(first);
            return first;
        }
    }

    private readonly int _capacity;
    private readonly Dictionary<TKey, Node> _cache;
    private readonly Dictionary<int, DoublyLinkedList> _frequencyLists;
    private readonly object _lock = new object();
    private int _minFrequency;

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _cache.Count;
            }
        }
    }

    public LfuCache(int capacity)
    {
        if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative.");
        _capacity = capacity;
        _cache = new Dictionary<TKey, Node>();
        _frequencyLists = new Dictionary<int, DoublyLinkedList>();
        _minFrequency = 0;
    }

    public TValue Get(TKey key)
    {
        lock (_lock)
        {
            if (!_cache.TryGetValue(key, out Node node))
            {
                throw new KeyNotFoundException($"The key '{key}' was not found in the cache.");
            }
            UpdateFrequency(node);
            return node.Value;
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        lock (_lock)
        {
            if (!_cache.TryGetValue(key, out Node node))
            {
                value = default;
                return false;
            }
            UpdateFrequency(node);
            value = node.Value;
            return true;
        }
    }

    public void Put(TKey key, TValue value)
    {
        if (_capacity <= 0) return;

        lock (_lock)
        {
            if (_cache.TryGetValue(key, out Node node))
            {
                node.Value = value;
                UpdateFrequency(node); 
            }
            else
            {
                if (_cache.Count >= _capacity)
                {
                    Evict();
                }

                Node newNode = new Node(key, value);
                _cache[key] = newNode;
                AddToFrequencyList(1, newNode);
                _minFrequency = 1;
            }
        }
    }

    private void UpdateFrequency(Node node)
    {
        int oldFreq = node.Frequency;
        DoublyLinkedList oldList = _frequencyLists[oldFreq];
        oldList.Remove(node);

        if (oldList.Count == 0 && _minFrequency == oldFreq)
        {
            _minFrequency++;
        }

        node.Frequency++;
        AddToFrequencyList(node.Frequency, node);
    }

    private void AddToFrequencyList(int frequency, Node node)
    {
        if (!_frequencyLists.TryGetValue(frequency, out DoublyLinkedList list))
        {
            list = new DoublyLinkedList();
            _frequencyLists[frequency] = list;
        }
        list.AddLast(node);
    }

    private void Evict()
    {
        if (_frequencyLists.TryGetValue(_minFrequency, out DoublyLinkedList list) && list.Count > 0)
        {
            Node lruNode = list.RemoveFirst();
            if (lruNode != null)
            {
                _cache.Remove(lruNode.Key);
            }
        }
    }
}