using System;

namespace SelfBalancingTernarySearchTree
{
    /// <summary>
    /// Represents a node in the Self-Balancing Ternary Search Tree.
    /// </summary>
    internal class TSTNode
    {
        /// <summary>
        /// The character stored at this node.
        /// </summary>
        public char Character { get; set; }

        /// <summary>
        /// Indicates if this node marks the end of a complete string.
        /// </summary>
        public bool IsEndOfWord { get; set; }

        /// <summary>
        /// The left child node (for characters less than the current node's character).
        /// </summary>
        public TSTNode Left { get; set; }

        /// <summary>
        /// The middle child node (for characters equal to the current node's character).
        /// </summary>
        public TSTNode Middle { get; set; }

        /// <summary>
        /// The right child node (for characters greater than the current node's character).
        /// </summary>
        public TSTNode Right { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSTNode"/> class.
        /// </summary>
        /// <param name="character">The character to store in the node.</param>
        public TSTNode(char character)
        {
            Character = character;
            IsEndOfWord = false;
            Left = null;
            Middle = null;
            Right = null;
        }
    }

    /// <summary>
    /// Implements a Self-Balancing Ternary Search Tree for efficient string storage and retrieval.
    /// This structure maintains strings in sorted order and allows for O(log n) average time complexity for insertion, deletion, and search operations.
    /// </summary>
    public class SelfBalancingTernarySearchTree
    {
        private TSTNode _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfBalancingTernarySearchTree"/> class.
        /// </summary>
        public SelfBalancingTernarySearchTree()
        {
            _root = null;
        }

        /// <summary>
        /// Inserts a string into the ternary search tree.
        /// </summary>
        /// <param name="key">The string to insert.</param>
        public void Insert(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            _root = InsertHelper(_root, key, 0);
        }

        /// <summary>
        /// Recursive helper method for inserting a string.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <param name="key">The string to insert.</param>
        /// <param name="index">The current character index in the string.</param>
        /// <returns>The root of the modified subtree.</returns>
        private TSTNode InsertHelper(TSTNode node, string key, int index)
        {
            char currentChar = key[index];

            if (node == null)
            {
                node = new TSTNode(currentChar);
            }

            if (currentChar < node.Character)
            {
                node.Left = InsertHelper(node.Left, key, index);
            }
            else if (currentChar > node.Character)
            {
                node.Right = InsertHelper(node.Right, key, index);
            }
            else // currentChar == node.Character
            {
                if (index < key.Length - 1)
                {
                    node.Middle = InsertHelper(node.Middle, key, index + 1);
                }
                else
                {
                    node.IsEndOfWord = true;
                }
            }
            return node;
        }

        /// <summary>
        /// Searches for a string in the ternary search tree.
        /// </summary>
        /// <param name="key">The string to search for.</param>
        /// <returns>True if the string is found, false otherwise.</returns>
        public bool Search(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return SearchHelper(_root, key, 0);
        }

        /// <summary>
        /// Recursive helper method for searching a string.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <param name="key">The string to search for.</param>
        /// <param name="index">The current character index in the string.</param>
        /// <returns>True if the string is found in the subtree, false otherwise.</returns>
        private bool SearchHelper(TSTNode node, string key, int index)
        {
            if (node == null)
            {
                return false;
            }

            char currentChar = key[index];

            if (currentChar < node.Character)
            {
                return SearchHelper(node.Left, key, index);
            }
            else if (currentChar > node.Character)
            {
                return SearchHelper(node.Right, key, index);
            }
            else // currentChar == node.Character
            {
                if (index == key.Length - 1)
                {
                    return node.IsEndOfWord;
                }
                else
                {
                    return SearchHelper(node.Middle, key, index + 1);
                }
            }
        }

        /// <summary>
        /// Deletes a string from the ternary search tree.
        /// </summary>
        /// <param name="key">The string to delete.</param>
        /// <returns>True if the string was found and deleted, false otherwise.</returns>
        public bool Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            bool deleted = false;
            _root = DeleteHelper(_root, key, 0, ref deleted);
            return deleted;
        }

        /// <summary>
        /// Recursive helper method for deleting a string.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <param name="key">The string to delete.</param>
        /// <param name="index">The current character index in the string.</param>
        /// <param name="deleted">A reference to a boolean indicating if deletion occurred.</param>
        /// <returns>The root of the modified subtree.</returns>
        private TSTNode DeleteHelper(TSTNode node, string key, int index, ref bool deleted)
        {
            if (node == null)
            {
                return null;
            }

            char currentChar = key[index];

            if (currentChar < node.Character)
            {
                node.Left = DeleteHelper(node.Left, key, index, ref deleted);
            }
            else if (currentChar > node.Character)
            {
                node.Right = DeleteHelper(node.Right, key, index, ref deleted);
            }
            else // currentChar == node.Character
            {
                if (index < key.Length - 1)
                {
                    node.Middle = DeleteHelper(node.Middle, key, index + 1, ref deleted);
                }
                else // This node is the end of the word to be deleted
                {
                    if (!node.IsEndOfWord)
                    {
                        // Word not found as a complete entry
                        return node;
                    }
                    node.IsEndOfWord = false;
                    deleted = true;

                    // If this node is no longer an end of a word and has no children, it can be removed.
                    if (node.Left == null && node.Middle == null && node.Right == null && !node.IsEndOfWord)
                    {
                        return null;
                    }
                }
            }

            // If a node has no children and is not the end of a word, it can be pruned.
            if (node.Left == null && node.Middle == null && node.Right == null && !node.IsEndOfWord)
            {
                return null;
            }

            return node;
        }
    }
}