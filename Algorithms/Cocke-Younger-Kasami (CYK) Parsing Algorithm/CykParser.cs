using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a production rule in Chomsky Normal Form (CNF).
/// Rules are of the form A -> BC or A -> a.
/// </summary>
public readonly struct CnfRule
{
    /// <summary>
    /// The left-hand side of the production rule (a non-terminal symbol).
    /// </summary>
    public string NonTerminal { get; }

    /// <summary>
    /// The right-hand side of the production rule. Can be a single terminal (for A -> a) 
    /// or two non-terminals (for A -> BC).
    /// </summary>
    public string RightHandSide { get; }

    /// <summary>
    /// Indicates if the rule is a terminal production (A -> a).
    /// </summary>
    public bool IsTerminalRule => RightHandSide.Length == 1 && char.IsLower(RightHandSide[0]);

    /// <summary>
    /// The terminal symbol if the rule is a terminal production.
    /// </summary>
    public char Terminal => RightHandSide[0];

    /// <summary>
    /// The first non-terminal symbol if the rule is a binary non-terminal production.
    /// </summary>
    public char LeftNonTerminal => RightHandSide[0];

    /// <summary>
    /// The second non-terminal symbol if the rule is a binary non-terminal production.
    /// </summary>
    public char RightNonTerminal => RightHandSide[1];

    /// <summary>
    /// Initializes a new instance of the <see cref="CnfRule"/> struct.
    /// </summary>
    /// <param name="nonTerminal">The left-hand side non-terminal.</param>
    /// <param name="rightHandSide">The right-hand side (terminal or two non-terminals).</param>
    /// <exception cref="ArgumentException">Thrown if the right-hand side is invalid for CNF.</exception>
    public CnfRule(string nonTerminal, string rightHandSide)
    {
        if (string.IsNullOrEmpty(nonTerminal) || !char.IsUpper(nonTerminal[0]))
            throw new ArgumentException("Non-terminal must be a single uppercase character.", nameof(nonTerminal));
        if (string.IsNullOrEmpty(rightHandSide))
            throw new ArgumentException("Right-hand side cannot be empty.", nameof(rightHandSide));

        if (rightHandSide.Length == 1)
        {
            if (!char.IsLower(rightHandSide[0]))
                throw new ArgumentException("Terminal rule right-hand side must be a single lowercase character.", nameof(rightHandSide));
        }
        else if (rightHandSide.Length == 2)
        {
            if (!char.IsUpper(rightHandSide[0]) || !char.IsUpper(rightHandSide[1]))
                throw new ArgumentException("Binary non-terminal rule right-hand side must be two uppercase characters.", nameof(rightHandSide));
        }
        else
        {
            throw new ArgumentException("Right-hand side must be a single terminal or two non-terminals for CNF.", nameof(rightHandSide));
        }

        NonTerminal = nonTerminal;
        RightHandSide = rightHandSide;
    }

    public override string ToString() => $"{NonTerminal} -> {RightHandSide}";
}

/// <summary>
/// Represents a context-free grammar in Chomsky Normal Form (CNF).
/// </summary>
public class CnfGrammar
{
    /// <summary>
    /// The set of all production rules in CNF.
    /// </summary>
    public IReadOnlyList<CnfRule> Rules { get; }

    /// <summary>
    /// The start non-terminal symbol of the grammar.
    /// </summary>
    public string StartSymbol { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CnfGrammar"/> class.
    /// </summary>
    /// <param name="rules">The list of CNF production rules.</param>
    /// <param name="startSymbol">The start non-terminal symbol.</param>
    /// <exception cref="ArgumentNullException">Thrown if rules or startSymbol is null.</exception>
    /// <exception cref="ArgumentException">Thrown if startSymbol is empty or not an uppercase character.</exception>
    public CnfGrammar(IEnumerable<CnfRule> rules, string startSymbol)
    {
        Rules = rules?.ToList() ?? throw new ArgumentNullException(nameof(rules));
        if (string.IsNullOrEmpty(startSymbol) || !char.IsUpper(startSymbol[0]))
            throw new ArgumentException("Start symbol must be a single uppercase character.", nameof(startSymbol));
        StartSymbol = startSymbol;
    }
}

/// <summary>
/// Represents a node in a parse tree.
/// </summary>
public class ParseTreeNode
{
    /// <summary>
    /// The symbol (terminal or non-terminal) represented by this node.
    /// </summary>
    public string Symbol { get; }

    /// <summary>
    /// The left child node (if this is a non-terminal node derived from a binary rule).
    /// </summary>
    public ParseTreeNode? Left { get; }

    /// <summary>
    /// The right child node (if this is a non-terminal node derived from a binary rule).
    /// </summary>
    public ParseTreeNode? Right { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParseTreeNode"/> class for a terminal node.
    /// </summary>
    /// <param name="terminal">The terminal symbol.</param>
    public ParseTreeNode(char terminal) : this(terminal.ToString(), null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParseTreeNode"/> class for a non-terminal node.
    /// </summary>
    /// <param name="nonTerminal">The non-terminal symbol.</param>
    /// <param name="left">The left child node.</param>
    /// <param name="right">The right child node.</param>
    public ParseTreeNode(string nonTerminal, ParseTreeNode? left, ParseTreeNode? right)
    {
        Symbol = nonTerminal;
        Left = left;
        Right = right;
    }

    /// <summary>
    /// Checks if this node is a leaf (terminal) node.
    /// </summary>
    public bool IsLeaf => Left == null && Right == null;

    public override string ToString() => IsLeaf ? Symbol : $"{Symbol}({Left?.ToString() ?? ""}, {Right?.ToString() ?? ""})";
}

/// <summary>
/// Implements the Cocke-Younger-Kasami (CYK) parsing algorithm for context-free grammars in Chomsky Normal Form (CNF).
/// </summary>
public class CykParser
{
    private readonly CnfGrammar _grammar;
    private readonly Dictionary<string, List<CnfRule>> _terminalProductions;
    private readonly Dictionary<string, List<CnfRule>> _nonTerminalProductions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CykParser"/> class.
    /// </summary>
    /// <param name="grammar">The CNF grammar to use for parsing.</param>
    /// <exception cref="ArgumentNullException">Thrown if grammar is null.</exception>
    public CykParser(CnfGrammar grammar)
    {
        _grammar = grammar ?? throw new ArgumentNullException(nameof(grammar));
        _terminalProductions = new Dictionary<string, List<CnfRule>>();
        _nonTerminalProductions = new Dictionary<string, List<CnfRule>>();
        IndexProductions();
    }

    /// <summary>
    /// Indexes the grammar rules for efficient lookup.
    /// </summary>
    private void IndexProductions()
    {
        foreach (var rule in _grammar.Rules)
        {
            if (rule.IsTerminalRule)
            {
                if (!_terminalProductions.TryGetValue(rule.Terminal.ToString(), out var list))
                {
                    list = new List<CnfRule>();
                    _terminalProductions[rule.Terminal.ToString()] = list;
                }
                list.Add(rule);
            }
            else
            {
                var rhs = rule.RightHandSide;
                if (!_nonTerminalProductions.TryGetValue(rhs, out var list))
                {
                    list = new List<CnfRule>();
                    _nonTerminalProductions[rhs] = list;
                }
                list.Add(rule);
            }
        }
    }

    /// <summary>
    /// Parses a sequence of tokens using the CYK algorithm.
    /// </summary>
    /// <param name="tokens">The sequence of input tokens (terminals).</param>
    /// <returns>True if the tokens can be generated by the grammar, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if tokens is null.</exception>
    public bool Parse(IReadOnlyList<string> tokens)
    {
        if (tokens == null)
            throw new ArgumentNullException(nameof(tokens));
        if (tokens.Count == 0)
            return false; // Or true, depending on grammar definition for empty string

        int n = tokens.Count;
        // dpTable[i, j] stores the set of non-terminals that can generate the substring tokens[j...j+i]
        // where i is the length of the substring (0-indexed length, so i=0 is length 1, i=1 is length 2, etc.)
        // and j is the starting index of the substring.
        var dpTable = new HashSet<string>[n, n];

        // Initialize the table
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                dpTable[i, j] = new HashSet<string>();
            }
        }

        // Fill the diagonal (substrings of length 1)
        for (int j = 0; j < n; j++)
        {
            var token = tokens[j];
            if (_terminalProductions.TryGetValue(token, out var rules))
            {
                foreach (var rule in rules)
                {
                    dpTable[0, j].Add(rule.NonTerminal);
                }
            }
        }

        // Fill the rest of the table for substrings of length 2 to n
        for (int len = 1; len < n; len++) // len is the length of the substring - 1 (0-indexed)
        {
            for (int j = 0; j < n - len; j++) // j is the starting index
            {
                // Substring is tokens[j...j+len]
                for (int k = 0; k < len; k++) // k is the split point
                {
                    // Split into tokens[j...j+k] and tokens[j+k+1...j+len]
                    var leftSubstrNonTerminals = dpTable[k, j];
                    var rightSubstrNonTerminals = dpTable[len - 1 - k, j + k + 1];

                    foreach (var nt1 in leftSubstrNonTerminals)
                    {
                        foreach (var nt2 in rightSubstrNonTerminals)
                        {
                            var combinedRhs = nt1 + nt2;
                            if (_nonTerminalProductions.TryGetValue(combinedRhs, out var rules))
                            {
                                foreach (var rule in rules)
                                {
                                    dpTable[len, j].Add(rule.NonTerminal);
                                }
                            }
                        }
                    }
                }
            }
        }

        // The sentence is accepted if the start symbol is in the top-right cell (dpTable[n-1, 0])
        return dpTable[n - 1, 0].Contains(_grammar.StartSymbol);
    }

    /// <summary>
    /// Attempts to construct a parse tree for the given tokens.
    /// </summary>
    /// <param name="tokens">The sequence of input tokens (terminals).</param>
    /// <returns>A <see cref="ParseTreeNode"/> representing the parse tree if the tokens are valid, otherwise null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if tokens is null.</exception>
    public ParseTreeNode? GetParseTree(IReadOnlyList<string> tokens)
    {
        if (tokens == null)
            throw new ArgumentNullException(nameof(tokens));
        if (tokens.Count == 0)
            return null;

        int n = tokens.Count;
        // dpTable[i, j] stores a list of tuples: (NonTerminal, LeftChild, RightChild)
        // where NonTerminal can generate tokens[j...j+i], and LeftChild/RightChild are the parse trees
        // for the two sub-problems that produced NonTerminal.
        var dpTable = new List<Tuple<string, ParseTreeNode?, ParseTreeNode?>>[n, n];

        // Initialize the table
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                dpTable[i, j] = new List<Tuple<string, ParseTreeNode?, ParseTreeNode?>>();
            }
        }

        // Fill the diagonal (substrings of length 1)
        for (int j = 0; j < n; j++)
        {
            var token = tokens[j];
            if (_terminalProductions.TryGetValue(token, out var rules))
            {
                foreach (var rule in rules)
                {
                    dpTable[0, j].Add(Tuple.Create(rule.NonTerminal, new ParseTreeNode(token[0]), (ParseTreeNode?)null));
                }
            }
        }

        // Fill the rest of the table for substrings of length 2 to n
        for (int len = 1; len < n; len++) // len is the length of the substring - 1 (0-indexed)
        {
            for (int j = 0; j < n - len; j++) // j is the starting index
            {
                // Substring is tokens[j...j+len]
                for (int k = 0; k < len; k++) // k is the split point
                {
                    // Split into tokens[j...j+k] and tokens[j+k+1...j+len]
                    var leftSubstrEntries = dpTable[k, j];
                    var rightSubstrEntries = dpTable[len - 1 - k, j + k + 1];

                    foreach (var leftEntry in leftSubstrEntries)
                    {
                        foreach (var rightEntry in rightSubstrEntries)
                        {
                            var nt1 = leftEntry.Item1;
                            var nt2 = rightEntry.Item1;
                            var combinedRhs = nt1 + nt2;

                            if (_nonTerminalProductions.TryGetValue(combinedRhs, out var rules))
                            {
                                foreach (var rule in rules)
                                {
                                    var leftNode = leftEntry.Item2;
                                    var rightNode = rightEntry.Item2;
                                    var newNode = new ParseTreeNode(rule.NonTerminal, leftNode, rightNode);
                                    dpTable[len, j].Add(Tuple.Create(rule.NonTerminal, newNode, (ParseTreeNode?)null));
                                }
                            }
                        }
                    }
                }
            }
        }

        // Find the start symbol in the top-right cell and reconstruct the tree
        var finalEntries = dpTable[n - 1, 0];
        foreach (var entry in finalEntries)
        {
            if (entry.Item1 == _grammar.StartSymbol)
            {
                // We need to find the specific entry that corresponds to the start symbol
                // and reconstruct the tree from its children. The current structure stores the full node.
                // We need to find the correct node that was built for the start symbol.
                // The `Item2` of the tuple already holds the constructed ParseTreeNode.
                return entry.Item2;
            }
        }

        return null; // Start symbol not found, parsing failed.
    }
}