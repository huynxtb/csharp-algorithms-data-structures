using System;
public class LinearProbingHashTable<TKey, TValue>
{
    private int size;
    private int threshold;
    private int[] keys;
    private TValue[] values;
    private const float loadFactorThreshold = 0.75f;
    public LinearProbingHashTable(int initialCapacity = 16)
    {
        size = 0;
        threshold = (int)(initialCapacity * loadFactorThreshold);
        keys = new int[initialCapacity];
        values = new TValue[initialCapacity];
        for (int i = 0; i < initialCapacity; i++)
        {
            keys[i] = -1;
        }
    }
    private int Hash(TKey key)
    {
        return Math.Abs(key.GetHashCode() % keys.Length);
    }
    public void Insert(TKey key, TValue value)
    {
        if (size >= threshold)
        {
            Resize();
        }
        int index = Hash(key);
        while (keys[index] != -1)
        {
            if (keys[index].Equals(key.GetHashCode()))
            {
                values[index] = value;
                return;
            }
            index = (index + 1) % keys.Length;
        }
        keys[index] = key.GetHashCode();
        values[index] = value;
        size++;
    }
    public TValue Search(TKey key)
    {
        int index = Hash(key);
        while (keys[index] != -1)
        {
            if (keys[index].Equals(key.GetHashCode()))
            {
                return values[index];
            }
            index = (index + 1) % keys.Length;
        }
        throw new KeyNotFoundException();
    }
    public void Delete(TKey key)
    {
        int index = Hash(key);
        while (keys[index] != -1)
        {
            if (keys[index].Equals(key.GetHashCode()))
            {
                keys[index] = -1;
                size--;
                return;
            }
            index = (index + 1) % keys.Length;
        }
        throw new KeyNotFoundException();
    }
    private void Resize()
    {
        int newCapacity = keys.Length * 2;
        int[] newKeys = new int[newCapacity];
        TValue[] newValues = new TValue[newCapacity];
        for (int i = 0; i < newCapacity; i++)
        {
            newKeys[i] = -1;
        }
        threshold = (int)(newCapacity * loadFactorThreshold);
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] != -1)
            {
                int index = Math.Abs(keys[i] % newCapacity);
                while (newKeys[index] != -1)
                {
                    index = (index + 1) % newCapacity;
                }
                newKeys[index] = keys[i];
                newValues[index] = values[i];
            }
        }
        keys = newKeys;
        values = newValues;
    }
}