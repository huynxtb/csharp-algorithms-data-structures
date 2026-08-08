# Huffman Coding Algorithm

## 1. Introduction

Huffman Coding is a lossless data compression algorithm. It assigns variable-length codes to input characters, with lengths based on the frequencies of corresponding characters. More frequent characters get shorter codes, and less frequent characters get longer codes. This method is particularly effective for data where the frequency distribution of characters is uneven.

## 2. Usage

```csharp
using System;
using System.Collections;
using System.Text;

public class Example
{
    public static void Main(string[] args)
    {
        string originalText = "this is an example for huffman encoding";

        // Encode the text
        (BitArray compressedData, HuffmanNode root) = HuffmanEncoder.Encode(originalText);

        Console.WriteLine($"Original Text: {originalText}");
        Console.WriteLine($"Compressed Data Length (bits): {compressedData.Length}");

        // Decode the text
        string decodedText = HuffmanEncoder.Decode(compressedData, root);
        Console.WriteLine($"Decoded Text: {decodedText}");

        // Example with empty string
        string emptyText = "";
        (BitArray compressedEmpty, HuffmanNode rootEmpty) = HuffmanEncoder.Encode(emptyText);
        string decodedEmpty = HuffmanEncoder.Decode(compressedEmpty, rootEmpty);
        Console.WriteLine($"\nEmpty Text Encoded: '{HuffmanEncoder.Decode(compressedEmpty, rootEmpty)}' (Length: {compressedEmpty.Length})");

        // Example with single unique character
        string singleCharText = "aaaaa";
        (BitArray compressedSingle, HuffmanNode rootSingle) = HuffmanEncoder.Encode(singleCharText);
        string decodedSingle = HuffmanEncoder.Decode(compressedSingle, rootSingle);
        Console.WriteLine($"\nSingle Char Text ('{singleCharText}') Encoded Length: {compressedSingle.Length}");
        Console.WriteLine($"Single Char Text Decoded: '{decodedSingle}'");
    }
}
```

## 3. Detailed Explanation

### `HuffmanNode` Class

-   **`Character`**: Stores the character represented by the node. For internal nodes, this is typically a null character (`'\0'`).
-   **`Frequency`**: Stores the frequency of the character or the sum of frequencies of its children.
-   **`Left`, `Right`**: References to the left and right child nodes, respectively. A '0' bit typically represents traversing left, and a '1' bit represents traversing right.
-   **`CompareTo`**: Implements `IComparable<HuffmanNode>` to allow nodes to be ordered by frequency, essential for the priority queue.
-   **`IsLeaf`**: A convenience property to check if a node is a leaf node (i.e., it represents a character).

### `HuffmanEncoder` Class

#### `Encode(string inputText)` Method

1.  **Input Validation**: Checks for null or empty input strings. Returns an empty `BitArray` and `null` root for an empty string.
2.  **Frequency Table Construction**: Iterates through the input string to build a `Dictionary<char, int>` mapping each character to its occurrence count.
3.  **Huffman Tree Construction**: 
    *   A `PriorityQueue<HuffmanNode, int>` (min-heap) is used. Each unique character is added as a leaf `HuffmanNode` to the queue, prioritized by its frequency.
    *   **Edge Case (Single Unique Character)**: If the input has only one distinct character, a dummy parent node is created to ensure a tree structure with two branches, allowing for a '0' or '1' code.
    *   The algorithm repeatedly extracts the two nodes with the lowest frequencies from the priority queue.
    *   A new internal `HuffmanNode` is created with these two nodes as its children. The frequency of this new node is the sum of its children's frequencies.
    *   The new internal node is inserted back into the priority queue.
    *   This process continues until only one node remains in the queue, which is the root of the Huffman tree.
4.  **Prefix Code Generation**: The `GenerateCodes` helper method performs a depth-first traversal of the Huffman tree. It assigns '0' for left branches and '1' for right branches, building the binary code string for each character. These codes are stored in a `Dictionary<char, string>`.
5.  **Data Encoding**: The input string is traversed again. For each character, its corresponding Huffman code is retrieved from the dictionary, and each bit of the code is added to a `List<bool>`.
6.  **Result**: A `BitArray` is created from the list of booleans, and the `BitArray` along with the `rootNode` of the Huffman tree are returned.

#### `GenerateCodes(HuffmanNode node, string currentCode, Dictionary<char, string> huffmanCodes)` Method

-   A recursive helper function that traverses the Huffman tree.
-   When it reaches a leaf node, it stores the `currentCode` (path from the root) associated with the character in the `huffmanCodes` dictionary.
-   For internal nodes, it appends '0' to `currentCode` and recurses on the left child, and appends '1' and recurses on the right child.

#### `Decode(BitArray compressedData, HuffmanNode root)` Method

1.  **Input Validation**: Checks for null `compressedData` or `root`. Handles empty `compressedData` by returning an empty string.
2.  **Edge Case (Single Unique Character)**: If the tree structure indicates a single unique character was encoded, it reconstructs the string by repeating that character for the length of the compressed data.
3.  **Tree Traversal**: 
    *   Starts at the `root` of the Huffman tree.
    *   Iterates through each bit in the `compressedData` `BitArray`.
    *   If the bit is '0', it moves to the left child; if '1', it moves to the right child.
    *   If the current node becomes `null` during traversal, it indicates corrupted data or an invalid tree, and an `ArgumentException` is thrown.
    *   When a leaf node is reached, the character stored in that node is appended to the `decodedString` `StringBuilder`, and the traversal resets to the `root` for the next character.
4.  **Final Check**: After processing all bits, if the `currentNode` is not back at the `root`, it implies the compressed data is incomplete or malformed, and an `ArgumentException` is thrown.
5.  **Result**: The `ToString()` method of the `StringBuilder` returns the fully decoded original string.

## 4. Complexity Analysis

Let N be the number of characters in the input string and U be the number of unique characters.

### `Encode` Method:

-   **Frequency Table Construction**: O(N) - Each character is processed once.
-   **Huffman Tree Construction**: O(U log U) - Inserting U unique characters into a priority queue takes O(U log U). The merging process involves 2*(U-1) extractions and insertions, each taking O(log U).
-   **Prefix Code Generation**: O(U) - Traverses the Huffman tree, which has at most 2U-1 nodes.
-   **Data Encoding**: O(N * L_avg) where L_avg is the average code length. In the worst case, L_avg can be O(U), making it O(N*U). However, for typical text data, L_avg is much smaller, often closer to O(log U).

**Overall Time Complexity for `Encode`**: O(N + U log U). If U is significantly smaller than N, it approaches O(N).
**Space Complexity for `Encode`**: O(N + U) - O(N) for the output `BitArray` (potentially smaller than N bytes if compression is effective) and O(U) for the frequency map, Huffman tree, and code dictionary.

### `Decode` Method:

-   **Tree Traversal**: O(M * L_avg) where M is the number of bits in the compressed data and L_avg is the average code length. Since M is roughly N * L_avg, this is effectively O(N * L_avg^2) in the worst case. However, each bit traversal leads to a node, and we reset to the root after finding a character. A more precise analysis is O(M), as each bit is processed once to traverse the tree until a leaf is found.

**Overall Time Complexity for `Decode`**: O(M), where M is the length of the compressed bitstream. This is typically proportional to the original data size N.
**Space Complexity for `Decode`**: O(U + N') - O(U) for storing the Huffman tree and O(N') for the decoded string, where N' is the length of the original string.
