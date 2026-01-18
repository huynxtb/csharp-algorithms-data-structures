using System;
using System.Collections.Generic;

/// <summary>
/// Class representing a Suffix Automaton (SAM) data structure.
/// It allows building the SAM incrementally by adding characters,
/// checking substring existence, counting distinct substrings, and
/// obtaining the length of the longest substring.
/// </summary>
public class SuffixAutomaton
{
    /// <summary>
    /// Internal structure representing a state in the suffix automaton.
    /// </summary>
    private class State
    {
        /// <summary>
        /// Length of the longest substring represented by this state.
        /// </summary>
        public int Length;

        /// <summary>
        /// Suffix link to a smaller suffix state.
        /// </summary>
        public int Link;

        /// <summary>
        /// Transitions from this state by characters.
        /// Key: character, Value: next state index
        /// </summary>
        public Dictionary<char, int> Next = new Dictionary<char, int>();

        public State()
        {
            Length = 0;
            Link = -1;
        }
    }

    // Array of states
    private State[] states;
    // Number of states currently in the SAM
    private int size;
    // The last added state (representing the entire string so far)
    private int last;

    // Upper bound for initial state array size.
    // For a string of length n, maximum states needed is 2 * n - 1.
    private readonly int maxStates;

    /// <summary>
    /// Constructs a suffix automaton for a string with maximum length capacity.
    /// Initialize for maximum string size you expect to process.
    /// </summary>
    /// <param name="maxLength">Maximum length of the string to build SAM for.</param>
    public SuffixAutomaton(int maxLength)
    {
        maxStates = 2 * maxLength;
        states = new State[maxStates];
        for (int i = 0; i < maxStates; i++)
            states[i] = new State();

        size = 1; // The initial state 0
        last = 0;
    }

    /// <summary>
    /// Adds a single character to the suffix automaton,
    /// extending all suffixes with this character.
    /// </summary>
    /// <param name="c">The character to add</param>
    public void AddCharacter(char c)
    {
        int cur = size++;
        states[cur].Length = states[last].Length + 1;

        int p = last;
        // Try to append edge from last state
        while (p != -1 && !states[p].Next.ContainsKey(c))
        {
            states[p].Next[c] = cur;
            p = states[p].Link;
        }

        if (p == -1)
        {
            // No suffix to link to, link to root (state 0)
            states[cur].Link = 0;
        }
        else
        {
            int q = states[p].Next[c];
            if (states[p].Length + 1 == states[q].Length)
            {
                // Suffix link directly to q
                states[cur].Link = q;
            }
            else
            {
                // Clone state q
                int clone = size++;
                states[clone].Length = states[p].Length + 1;
                states[clone].Next = new Dictionary<char, int>(states[q].Next);
                states[clone].Link = states[q].Link;

                while (p != -1 && states[p].Next.TryGetValue(c, out int target) && target == q)
                {
                    states[p].Next[c] = clone;
                    p = states[p].Link;
                }

                states[q].Link = clone;
                states[cur].Link = clone;
            }
        }

        last = cur;
    }

    /// <summary>
    /// Checks if a given substring exists in the original string
    /// represented by this suffix automaton.
    /// </summary>
    /// <param name="substring">Substring to check</param>
    /// <returns>True if substring exists; otherwise false.</returns>
    public bool Contains(string substring)
    {
        int currentState = 0;
        foreach (char c in substring)
        {
            if (!states[currentState].Next.TryGetValue(c, out int nextState))
                return false;
            currentState = nextState;
        }
        return true;
    }

    /// <summary>
    /// Counts the number of distinct substrings in the original string
    /// represented by this suffix automaton.
    /// </summary>
    /// <returns>Number of distinct substrings.</returns>
    public long CountDistinctSubstrings()
    {
        // Number of distinct substrings = sum over all states of
        // (Length[state] - Length[link[state]])
        // Because each substring corresponds to exactly one path in automaton.
        long count = 0;

        for (int i = 1; i < size; i++)
        {
            count += states[i].Length - states[states[i].Link].Length;
        }

        return count;
    }

    /// <summary>
    /// Gets the length of the longest substring processed so far.
    /// </summary>
    public int LongestSubstringLength
    {
        get
        {
            return states[last].Length;
        }
    }
}
