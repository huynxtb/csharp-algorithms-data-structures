using System;
using System.Collections;
using System.Collections.Generic;

public class CuckooHashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private struct Entry
    {
        public TKey Key;
        public TValue Value;
        public bool Occupied;
    }

    private const int DefaultInitialCapacity = 16;
    private const double MaxLoadFactor = 0.5;

    private Entry[] _table1;
    private Entry[] _table2;
    private int _capacity;
    private int _count;
    private readonly IEqualityComparer<TKey> _comparer;

    public int Count => _count;
    public int Capacity => _capacity;

    public CuckooHashTable(int initialCapacity = DefaultInitialCapacity, IEqualityComparer<TKey>? comparer = null)
    {
        if (initialCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Capacity must be positive.");

        _capacity = initialCapacity;
        _table1 = new Entry[_capacity];
        _table2 = new Entry[_capacity];
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
        _count = 0;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
                return value;
            throw new KeyNotFoundException($"Key '{key}' was not found in the hash table.");
        }
        set
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (ContainsKey(key))
            {
                UpdateValue(key, value);
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        if (ContainsKey(key))
            throw new ArgumentException("An element with the same key already exists.", nameof(key));

        if ((_count + 1) > _capacity * 2 * MaxLoadFactor)
        {
            ResizeAndRehash();
        }

        InsertInternal(key, value);
    }

    public bool Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int idx1 = Hash1(key);
        if (_table1[idx1].Occupied && _comparer.Equals(_table1[idx1].Key, key))
        {
            _table1[idx1] = default;
            _count--;
            return true;
        }

        int idx2 = Hash2(key);
        if (_table2[idx2].Occupied && _comparer.Equals(_table2[idx2].Key, key))
        {
            _table2[idx2] = default;
            _count--;
            return true;
        }

        return false;
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int idx1 = Hash1(key);
        if (_table1[idx1].Occupied && _comparer.Equals(_table1[idx1].Key, key))
            return true;

        int idx2 = Hash2(key);
        if (_table2[idx2].Occupied && _comparer.Equals(_table2[idx2].Key, key))
            return true;

        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int idx1 = Hash1(key);
        if (_table1[idx1].Occupied && _comparer.Equals(_table1[idx1].Key, key))
        {
            value = _table1[idx1].Value;
            return true;
        }

        int idx2 = Hash2(key);
        if (_table2[idx2].Occupied && _comparer.Equals(_table2[idx2].Key, key))
        {
            value = _table2[idx2].Value;
            return true;
        }

        value = default!;
        return false;
    }

    public void Clear()
    {
        Array.Clear(_table1, 0, _capacity);
        Array.Clear(_table2, 0, _capacity);
        _count = 0;
    }

    private void UpdateValue(TKey key, TValue value)
    {
        int idx1 = Hash1(key);
        if (_table1[idx1].Occupied && _comparer.Equals(_table1[idx1].Key, key))
        {
            _table1[idx1].Value = value;
            return;
        }

        int idx2 = Hash2(key);
        if (_table2[idx2].Occupied && _comparer.Equals(_table2[idx2].Key, key))
        {
            _table2[idx2].Value = value;
        }
    }

    private void InsertInternal(TKey key, TValue value)
    {
        TKey currentKey = key;
        TValue currentValue = value;
        int maxDisplacements = _capacity * 2 + 1;

        for (int i = 0; i < maxDisplacements; i++)
        {
            int idx1 = Hash1(currentKey);
            if (!_table1[idx1].Occupied)
            {
                _table1[idx1] = new Entry { Key = currentKey, Value = currentValue, Occupied = true };
                _count++;
                return;
            }

            Entry temp1 = _table1[idx1];
            _table1[idx1] = new Entry { Key = currentKey, Value = currentValue, Occupied = true };
            currentKey = temp1.Key;
            currentValue = temp1.Value;

            int idx2 = Hash2(currentKey);
            if (!_table2[idx2].Occupied)
            {
                _table2[idx2] = new Entry { Key = currentKey, Value = currentValue, Occupied = true };
                _count++;
                return;
            }

            Entry temp2 = _table2[idx2];
            _table2[idx2] = new Entry { Key = currentKey, Value = currentValue, Occupied = true };
            currentKey = temp2.Key;
            currentValue = temp2.Value;
        }

        ResizeAndRehash();
        InsertInternal(currentKey, currentValue);
    }

    private void ResizeAndRehash()
    {
        Entry[] oldTable1 = _table1;
        Entry[] oldTable2 = _table2;
        int oldCapacity = _capacity;

        _capacity *= 2;
        _table1 = new Entry[_capacity];
        _table2 = new Entry[_capacity];
        _count = 0;

        for (int i = 0; i < oldCapacity; i++)
        {
            if (oldTable1[i].Occupied)
                InsertInternal(oldTable1[i].Key, oldTable1[i].Value);
            if (oldTable2[i].Occupied)
                InsertInternal(oldTable2[i].Key, oldTable2[i].Value);
        }
    }

    private int Hash1(TKey key)
    {
        int h = _comparer.GetHashCode(key!);
        return (h & 0x7FFFFFFF) % _capacity;
    }

    private int Hash2(TKey key)
    {
        int h = _comparer.GetHashCode(key!);
        h = (int)((uint)h * 2654435761u);
        return (h & 0x7FFFFFFF) % _capacity;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < _capacity; i++)
        {
            if (_table1[i].Occupied)
                yield return new KeyValuePair<TKey, TValue>(_table1[i].Key, _table1[i].Value);
            if (_table2[i].Occupied)
                yield return new KeyValuePair<TKey, TValue>(_table2[i].Key, _table2[i].Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}