# Self-Balancing Ternary Search Tree

## Introduction

The Self-Balancing Ternary Search Tree (TST) is a specialized tree data structure used for storing and efficiently searching strings. Unlike a standard Trie or a binary search tree, a TST node contains a character and has three children: a left child (for characters less than the current node's character), a middle child (for characters equal to the current node's character, moving to the next character in the string), and a right child (for characters greater than the current node's character). This structure allows for efficient prefix searching and storage of strings. The "self-balancing" aspect in this implementation refers to the inherent balancing properties of the ternary search tree structure itself, which, when implemented correctly, leads to logarithmic time complexity for operations on average, similar to balanced binary search trees.

This data structure is particularly useful in scenarios where:

*   You need to store a large dictionary of words.
*   You frequently perform prefix searches (e.g., autocomplete functionality).
*   You need efficient insertion, deletion, and lookup of strings.
*   Memory efficiency is a concern, as it can be more space-efficient than a Trie for sparse datasets.

## Usage

Here's a C# example demonstrating how to use the `SelfBalancingTernarySearchTree`:

```csharp
using System;

// Assuming the SelfBalancingTernarySearchTree class is in the same scope or imported.
// For a single-file implementation, you would place the class definition above this usage.

public class ExampleUsage
{
    public static void Main(string[] args)
    {
        var tst = new SelfBalancingTernarySearchTree.SelfBalancingTernarySearchTree();

        // Inserting strings
        tst.Insert("apple");
        tst.Insert("apricot");
        tst.Insert("banana");
        tst.Insert("bandana");
        tst.Insert("apply");

        Console.WriteLine("--- Insertion Complete ---");

        // Searching for strings
        Console.WriteLine($"Search 'apple': {tst.Search("apple")}");     // Expected: True
        Console.WriteLine($"Search 'apricot': {tst.Search("apricot")}"); // Expected: True
        Console.WriteLine($"Search 'banana': {tst.Search("banana")}");   // Expected: True
        Console.WriteLine($"Search 'apply': {tst.Search("apply")}");     // Expected: True
        Console.WriteLine($"Search 'app': {tst.Search("app")}");       // Expected: False (unless 'app' was inserted)
        Console.WriteLine($"Search 'orange': {tst.Search("orange")}");   // Expected: False

        Console.WriteLine("--- Search Complete ---");

        // Deleting strings
        Console.WriteLine($"Delete 'banana': {tst.Delete("banana")}");   // Expected: True
        Console.WriteLine($"Search 'banana' after delete: {tst.Search("banana")}"); // Expected: False
        Console.WriteLine($"Delete 'grape': {tst.Delete("grape")}");     // Expected: False (not present)
        Console.WriteLine($"Delete 'apply': {tst.Delete("apply")}");     // Expected: True
        Console.WriteLine($"Search 'apply' after delete: {tst.Search("apply")}"); // Expected: False

        Console.WriteLine("--- Deletion Complete ---");

        // Inserting after deletion
        tst.Insert("application");
        Console.WriteLine($"Search 'application': {tst.Search("application")}"); // Expected: True
    }
}
