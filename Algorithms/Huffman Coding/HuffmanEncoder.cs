using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Represents a node in the Huffman tree.
/// </summary>
public class HuffmanNode : IComparable<HuffmanNode>
{
    public char Character { get; set; }
    public int Frequency { get; set; }
    public HuffmanNode Left { get; set; }
    public HuffmanNode Right { get; set; }

    public HuffmanNode(char character, int frequency, HuffmanNode left = null, HuffmanNode right = null)
    {
        Character = character;
        Frequency = frequency;
        Left = left;
        Right = right;
    }

    /// <summary>
    /// Compares nodes based on frequency for priority queue ordering.
    /// </summary>
    public int CompareTo(HuffmanNode other)
    {
        if (other == null) return 1;
        return Frequency.CompareTo(other.Frequency);
    }

    /// <summary>
    /// Checks if the node is a leaf node.
    /// </summary>
    public bool IsLeaf => Left == null && Right == null;
}

/// <summary>
/// Implements the Huffman Coding algorithm for lossless data compression.
/// </summary>
public static class HuffmanEncoder
{
    /// <summary>
    /// Encodes an input string using Huffman Coding.
    /// </summary>
    /// <param name="inputText">The string to encode.</param>
    /// <returns>A tuple containing the compressed data as a BitArray and the root of the Huffman tree.</returns>
    /// <exception cref="ArgumentNullException">Thrown if inputText is null.</exception>
    public static (BitArray compressedData, HuffmanNode root)
        Encode(string inputText)
    {
        if (inputText == null)
            throw new ArgumentNullException(nameof(inputText));
        if (string.IsNullOrEmpty(inputText))
            return (new BitArray(0), null);

        // 1. Build frequency table
        var frequencyMap = new Dictionary<char, int>();
        foreach (char c in inputText)
        {
            if (frequencyMap.ContainsKey(c))
                frequencyMap[c]++;
            else
                frequencyMap[c] = 1;
        }

        // 2. Construct Huffman Tree using a priority queue (min-heap)
        var priorityQueue = new PriorityQueue<HuffmanNode, int>();
        foreach (var pair in frequencyMap)
        {
            priorityQueue.Enqueue(new HuffmanNode(pair.Key, pair.Value), pair.Value);
        }

        // Handle single unique character case
        if (priorityQueue.Count == 1)
        {
            var singleNode = priorityQueue.Dequeue();
            var root = new HuffmanNode('\0', singleNode.Frequency, singleNode, null);
            priorityQueue.Enqueue(root, root.Frequency);
        }

        while (priorityQueue.Count > 1)
        {
            var left = priorityQueue.Dequeue();
            var right = priorityQueue.Dequeue();
            var combinedFrequency = left.Frequency + right.Frequency;
            var parentNode = new HuffmanNode('\0', combinedFrequency, left, right);
            priorityQueue.Enqueue(parentNode, parentNode.Frequency);
        }

        HuffmanNode rootNode = priorityQueue.Dequeue();

        // 3. Generate prefix codes
        var huffmanCodes = new Dictionary<char, string>();
        GenerateCodes(rootNode, "", huffmanCodes);

        // 4. Encode the input string
        var encodedBits = new List<bool>();
        foreach (char c in inputText)
        {
            string code = huffmanCodes[c];
            foreach (char bit in code)
            {
                encodedBits.Add(bit == '1');
            }
        }

        return (new BitArray(encodedBits.ToArray()), rootNode);
    }

    /// <summary>
    /// Recursively generates Huffman codes for each character.
    /// </summary>
    private static void GenerateCodes(HuffmanNode node, string currentCode, Dictionary<char, string> huffmanCodes)
    {
        if (node == null)
            return;

        if (node.IsLeaf)
        {
            huffmanCodes[node.Character] = currentCode;
            return;
        }

        GenerateCodes(node.Left, currentCode + "0", huffmanCodes);
        GenerateCodes(node.Right, currentCode + "1", huffmanCodes);
    }

    /// <summary>
    /// Decodes a BitArray using the provided Huffman tree root.
    /// </summary>
    /// <param name="compressedData">The compressed data as a BitArray.</param>
    /// <param name="root">The root of the Huffman tree used for encoding.</param>
    /// <returns>The original decoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if compressedData or root is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the compressed data is invalid or the tree is malformed.</exception>
    public static string Decode(BitArray compressedData, HuffmanNode root)
    {
        if (compressedData == null)
            throw new ArgumentNullException(nameof(compressedData));
        if (root == null)
            throw new ArgumentNullException(nameof(root));

        // Handle empty input case
        if (compressedData.Length == 0)
            return string.Empty;

        // Handle single unique character case
        if (root.Left != null && root.Right == null && root.Left.IsLeaf)
        {
            var decodedChars = new char[compressedData.Length];
            for (int i = 0; i < compressedData.Length; i++)
            {
                decodedChars[i] = root.Left.Character;
            }
            return new string(decodedChars);
        }

        var decodedString = new StringBuilder();
        HuffmanNode currentNode = root;
        for (int i = 0; i < compressedData.Length; i++)
        {
            bool bit = compressedData[i];
            if (bit)
            {
                currentNode = currentNode.Right;
            }
            else
            {
                currentNode = currentNode.Left;
            }

            if (currentNode == null)
                throw new ArgumentException("Invalid compressed data or malformed Huffman tree.");

            if (currentNode.IsLeaf)
            {
                decodedString.Append(currentNode.Character);
                currentNode = root; // Reset to root for the next character
            }
        }

        // Ensure we ended at a leaf node after processing all bits
        if (currentNode != root)
            throw new ArgumentException("Incomplete compressed data: did not end on a complete character code.");

        return decodedString.ToString();
    }
}