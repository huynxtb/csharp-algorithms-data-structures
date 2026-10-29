using System;
using System.Collections.Generic;

public class SeparateChainingHashTable<TKey, TValue> where TKey : notnull
{
    private struct Entry<K, V>
    {
        public K Key;
        public V Value;

        public Entry(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }

    private List<Entry<TKey, TValue>>[] buckets;
    private int count;
    private const double LoadFactorThreshold = 0.75;
    private int capacity;

    public int Count { get { return count; } }
    public int Capacity { get { return capacity; } }

    public SeparateChainingHashTable(int initialCapacity = 11)
    {
        if (initialCapacity < 1)
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be positive.");

        capacity = GetPrime(initialCapacity);
        buckets = new List<Entry<TKey, TValue>>[capacity];
        count = 0;
    }

    private int GetBucketIndex(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int hashCode = key.GetHashCode();
        // Ensure non-negative hash code and map to bucket index
        return (hashCode & 0x7FFFFFFF) % capacity;
    }

    private void Resize(int newCapacity)
    {
        int newPrimeCapacity = GetPrime(newCapacity);
        List<Entry<TKey, TValue>>[] newBuckets = new List<Entry<TKey, TValue>>[newPrimeCapacity];
        int oldCapacity = capacity;
        capacity = newPrimeCapacity;

        for (int i = 0; i < oldCapacity; i++)
        {
            if (buckets[i] != null)
            {
                foreach (var entry in buckets[i])
                {
                    int newIndex = GetBucketIndex(entry.Key);
                    if (newBuckets[newIndex] == null)
                        newBuckets[newIndex] = new List<Entry<TKey, TValue>>();
                    newBuckets[newIndex].Add(entry);
                }
            }
        }
        buckets = newBuckets;
    }

    public void Add(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        if ((double)count / capacity > LoadFactorThreshold)
        {
            Resize(capacity * 2);
        }

        int index = GetBucketIndex(key);

        if (buckets[index] == null)
            buckets[index] = new List<Entry<TKey, TValue>>();
        else
        {
            foreach (var entry in buckets[index])
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                    throw new ArgumentException($"Key '{key}' already exists.");
            }
        }

        buckets[index].Add(new Entry<TKey, TValue>(key, value));
        count++;
    }

    public bool Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int index = GetBucketIndex(key);

        if (buckets[index] != null)
        {
            for (int i = 0; i < buckets[index].Count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(buckets[index][i].Key, key))
                {
                    buckets[index].RemoveAt(i);
                    count--;
                    return true;
                }
            }
        }
        return false;
    }

    public TValue Get(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int index = GetBucketIndex(key);

        if (buckets[index] != null)
        {
            foreach (var entry in buckets[index])
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                    return entry.Value;
            }
        }

        throw new KeyNotFoundException($"Key '{key}' not found.");
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int index = GetBucketIndex(key);

        if (buckets[index] != null)
        {
            foreach (var entry in buckets[index])
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                    return true;
            }
        }
        return false;
    }

    public TValue this[TKey key]
    {
        get { return Get(key); }
        set { 
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = GetBucketIndex(key);

            if (buckets[index] == null)
            {
                if ((double)count / capacity > LoadFactorThreshold)
                {
                    Resize(capacity * 2);
                    index = GetBucketIndex(key);
                }
                buckets[index] = new List<Entry<TKey, TValue>>();
                buckets[index].Add(new Entry<TKey, TValue>(key, value));
                count++;
            }
            else
            {
                bool found = false;
                for (int i = 0; i < buckets[index].Count; i++)
                {
                    if (EqualityComparer<TKey>.Default.Equals(buckets[index][i].Key, key))
                    {
                        buckets[index][i] = new Entry<TKey, TValue>(key, value);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if ((double)count / capacity > LoadFactorThreshold)
                    {
                        Resize(capacity * 2);
                        index = GetBucketIndex(key);
                    }
                    buckets[index].Add(new Entry<TKey, TValue>(key, value));
                    count++;
                }
            }
        }
    }

    // Helper to find the next prime number greater than or equal to n
    private static int GetPrime(int n)
    {
        if (n <= 1)
            return 2;
        if (n % 2 == 0)
            n++;
        while (!IsPrime(n))
        {
            n += 2;
        }
        return n;
    }

    // Helper to check if a number is prime
    private static bool IsPrime(int n)
    {
        if (n <= 1)
            return false;
        if (n <= 3)
            return true;
        if (n % 2 == 0 || n % 3 == 0)
            return false;
        for (int i = 5; i * i <= n; i = i + 6)
        {
            if (n % i == 0 || n % (i + 2) == 0)
                return false;
        }
        return true;
    }
}