using System;
public class HashTable
{
    private class Node
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public Node Next { get; set; }
    }
    private Node[] table;
    private int size;
    public HashTable(int size)
    {
        this.size = size;
        table = new Node[size];
    }
    private int HashFunction(string key)
    {
        int hash = 0;
        foreach (char c in key)
        {
            hash += c;
        }
        return hash % size;
    }
    public void Insert(string key, int value)
    {
        int index = HashFunction(key);
        if (table[index] == null)
        {
            table[index] = new Node { Key = key, Value = value };
        }
        else
        {
            Node current = table[index];
            while (current.Next != null)
            {
                if (current.Key == key)
                {
                    current.Value = value;
                    return;
                }
                current = current.Next;
            }
            if (current.Key == key)
            {
                current.Value = value;
            }
            else
            {
                current.Next = new Node { Key = key, Value = value };
            }
        }
    }
    public int? Get(string key)
    {
        int index = HashFunction(key);
        Node current = table[index];
        while (current != null)
        {
            if (current.Key == key)
            {
                return current.Value;
            }
            current = current.Next;
        }
        return null;
    }
    public void Delete(string key)
    {
        int index = HashFunction(key);
        Node current = table[index];
        Node previous = null;
        while (current != null)
        {
            if (current.Key == key)
            {
                if (previous == null)
                {
                    table[index] = current.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }
                return;
            }
            previous = current;
            current = current.Next;
        }
    }
}