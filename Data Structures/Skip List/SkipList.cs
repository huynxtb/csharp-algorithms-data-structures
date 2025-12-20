using System;
using System.Collections.Generic;

public class SkipList<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Node[] Next { get; set; }

        public Node(int level, T value)
        {
            Next = new Node[level];
            Value = value;
        }
    }

    private const double Probability = 0.5;
    private readonly Random _random;
    private readonly int _maxLevel;
    private Node _head;
    private int _currentLevel;

    public SkipList(int maxLevel = 16)
    {
        if (maxLevel < 1)
            throw new ArgumentException("maxLevel must be at least 1");

        _maxLevel = maxLevel;
        _random = new Random();
        _head = new Node(_maxLevel, default(T));
        _currentLevel = 1;
    }

    // Insert a value into the skip list
    public void Insert(T value)
    {
        Node[] update = new Node[_maxLevel];
        Node current = _head;

        // Find the place to update
        for (int i = _currentLevel - 1; i >= 0; i--)
        {
            while (current.Next[i] != null && current.Next[i].Value.CompareTo(value) < 0)
            {
                current = current.Next[i];
            }
            update[i] = current;
        }

        current = current.Next[0];

        // If the value doesn't exist insert new node
        if (current == null || current.Value.CompareTo(value) != 0)
        {
            int newLevel = RandomLevel();
            if (newLevel > _currentLevel)
            {
                for (int i = _currentLevel; i < newLevel; i++)
                {
                    update[i] = _head;
                }
                _currentLevel = newLevel;
            }

            Node newNode = new Node(newLevel, value);
            for (int i = 0; i < newLevel; i++)
            {
                newNode.Next[i] = update[i].Next[i];
                update[i].Next[i] = newNode;
            }
        }
    }

    // Search for a value; returns true if found, false otherwise
    public bool Search(T value)
    {
        Node current = _head;
        for (int i = _currentLevel - 1; i >= 0; i--)
        {
            while (current.Next[i] != null && current.Next[i].Value.CompareTo(value) < 0)
            {
                current = current.Next[i];
            }
        }
        current = current.Next[0];
        return current != null && current.Value.CompareTo(value) == 0;
    }

    // Delete a value from the skip list
    public bool Delete(T value)
    {
        Node[] update = new Node[_maxLevel];
        Node current = _head;
        for (int i = _currentLevel - 1; i >= 0; i--)
        {
            while (current.Next[i] != null && current.Next[i].Value.CompareTo(value) < 0)
            {
                current = current.Next[i];
            }
            update[i] = current;
        }

        current = current.Next[0];

        if (current != null && current.Value.CompareTo(value) == 0)
        {
            for (int i = 0; i < _currentLevel; i++)
            {
                if (update[i].Next[i] != current)
                    break;
                update[i].Next[i] = current.Next[i];
            }

            // Adjust current level if needed
            while (_currentLevel > 1 && _head.Next[_currentLevel - 1] == null)
            {
                _currentLevel--;
            }
            return true;
        }

        return false;
    }

    // Generate a random level for node
    private int RandomLevel()
    {
        int level = 1;
        while (_random.NextDouble() < Probability && level < _maxLevel)
        {
            level++;
        }
        return level;
    }
}