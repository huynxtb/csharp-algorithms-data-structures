using System;
using System.Collections.Generic;
using System.Linq;

public class Item
{
    public int Weight { get; set; }
    public int Value { get; set; }
    public int Index { get; set; }
    public double Ratio { get; set; }
}

public class Node
{
    public int Level { get; set; }
    public int Profit { get; set; }
    public int Weight { get; set; }
    public double Bound { get; set; }
    public List<int> ItemsIncluded { get; set; }

    public Node()
    {
        ItemsIncluded = new List<int>();
    }
}

public class KnapsackResult
{
    public int MaxValue { get; set; }
    public List<int> SelectedItems { get; set; }
    public int TotalWeight { get; set; }

    public KnapsackResult()
    {
        SelectedItems = new List<int>();
    }
}

public class KnapsackBranchAndBound
{
    public KnapsackResult Solve(int[] weights, int[] values, int capacity)
    {
        if (weights == null || values == null || weights.Length == 0)
        {
            return new KnapsackResult { MaxValue = 0, TotalWeight = 0 };
        }

        if (capacity <= 0)
        {
            return new KnapsackResult { MaxValue = 0, TotalWeight = 0 };
        }

        int n = weights.Length;
        Item[] items = new Item[n];

        for (int i = 0; i < n; i++)
        {
            items[i] = new Item
            {
                Weight = weights[i],
                Value = values[i],
                Index = i,
                Ratio = weights[i] > 0 ? (double)values[i] / weights[i] : 0
            };
        }

        Array.Sort(items, (a, b) => b.Ratio.CompareTo(a.Ratio));

        KnapsackResult bestSolution = new KnapsackResult { MaxValue = 0, TotalWeight = 0 };
        Node rootNode = new Node { Level = 0, Profit = 0, Weight = 0, ItemsIncluded = new List<int>() };
        rootNode.Bound = CalculateBound(rootNode, capacity, items);

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(rootNode);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            if (currentNode.Level == items.Length)
            {
                if (currentNode.Profit > bestSolution.MaxValue)
                {
                    bestSolution.MaxValue = currentNode.Profit;
                    bestSolution.TotalWeight = currentNode.Weight;
                    bestSolution.SelectedItems = new List<int>(currentNode.ItemsIncluded);
                }
                continue;
            }

            if (currentNode.Bound <= bestSolution.MaxValue)
            {
                continue;
            }

            Item currentItem = items[currentNode.Level];

            if (currentNode.Weight + currentItem.Weight <= capacity)
            {
                Node includeNode = new Node
                {
                    Level = currentNode.Level + 1,
                    Profit = currentNode.Profit + currentItem.Value,
                    Weight = currentNode.Weight + currentItem.Weight,
                    ItemsIncluded = new List<int>(currentNode.ItemsIncluded) { currentItem.Index }
                };
                includeNode.Bound = CalculateBound(includeNode, capacity, items);

                if (includeNode.Bound > bestSolution.MaxValue)
                {
                    queue.Enqueue(includeNode);
                }
            }

            Node excludeNode = new Node
            {
                Level = currentNode.Level + 1,
                Profit = currentNode.Profit,
                Weight = currentNode.Weight,
                ItemsIncluded = new List<int>(currentNode.ItemsIncluded)
            };
            excludeNode.Bound = CalculateBound(excludeNode, capacity, items);

            if (excludeNode.Bound > bestSolution.MaxValue)
            {
                queue.Enqueue(excludeNode);
            }
        }

        return bestSolution;
    }

    private double CalculateBound(Node node, int capacity, Item[] items)
    {
        if (node.Weight >= capacity)
        {
            return node.Profit;
        }

        double bound = node.Profit;
        int remainingCapacity = capacity - node.Weight;

        for (int i = node.Level; i < items.Length; i++)
        {
            if (items[i].Weight <= remainingCapacity)
            {
                bound += items[i].Value;
                remainingCapacity -= items[i].Weight;
            }
            else
            {
                bound += (double)items[i].Value * remainingCapacity / items[i].Weight;
                break;
            }
        }

        return bound;
    }
}