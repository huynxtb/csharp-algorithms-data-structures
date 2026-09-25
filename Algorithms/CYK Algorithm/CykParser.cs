using System;
using System.Collections.Generic;
using System.Linq;

namespace CykAlgorithm
{
    /// <summary>
    /// Represents a production rule in Chomsky Normal Form (CNF).
    /// Either A -> BC (Binary rule with two non-terminals) or A -> a (Terminal rule with one terminal token).
    /// </summary>
    public sealed class CnfRule : IEquatable<CnfRule>
    {
        public string Left { get; }
        public string? FirstRight { get; }
        public string? SecondRight { get; }
        public string? Terminal { get; }
        public bool IsTerminalRule => Terminal != null;

        private CnfRule(string left, string? firstRight, string? secondRight, string? terminal)
        {
            if (string.IsNullOrWhiteSpace(left))
                throw new ArgumentException("Left-hand side non-terminal cannot be null or empty.", nameof(left));

            Left = left;
            FirstRight = firstRight;
            SecondRight = secondRight;
            Terminal = terminal;
        }

        public static CnfRule CreateBinary(string left, string firstRight, string secondRight)
        {
            if (string.IsNullOrWhiteSpace(firstRight))
                throw new ArgumentException("First right non-terminal cannot be null or empty.", nameof(firstRight));
            if (string.IsNullOrWhiteSpace(secondRight))
                throw new ArgumentException("Second right non-terminal cannot be null or empty.", nameof(secondRight));

            return new CnfRule(left, firstRight, secondRight, null);
        }

        public static CnfRule CreateTerminal(string left, string terminal)
        {
            if (terminal == null)
                throw new ArgumentNullException(nameof(terminal));

            return new CnfRule(left, null, null, terminal);
        }

        public bool Equals(CnfRule? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Left == other.Left &&
                   FirstRight == other.FirstRight &&
                   SecondRight == other.SecondRight &&
                   Terminal == other.Terminal;
        }

        public override bool Equals(object? obj) => Equals(obj as CnfRule);

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, FirstRight, SecondRight, Terminal);
        }

        public override string ToString()
        {
            return IsTerminalRule
                ? $"{Left} -> '{Terminal}'"
                : $"{Left} -> {FirstRight} {SecondRight}";
        }
    }

    /// <summary>
    /// Represents a Context-Free Grammar in Chomsky Normal Form (CNF).
    /// </summary>
    public class Grammar
    {
        public string StartSymbol { get; }
        private readonly HashSet<CnfRule> _rules = new();
        private readonly Dictionary<string, List<string>> _terminalToNonTerminals = new();
        private readonly Dictionary<(string, string), List<string>> _binaryToNonTerminals = new();

        public IReadOnlyCollection<CnfRule> Rules => _rules;

        public Grammar(string startSymbol)
        {
            if (string.IsNullOrWhiteSpace(startSymbol))
                throw new ArgumentException("Start symbol cannot be null or empty.", nameof(startSymbol));

            StartSymbol = startSymbol;
        }

        public void AddRule(CnfRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            if (_rules.Add(rule))
            {
                if (rule.IsTerminalRule)
                {
                    if (!_terminalToNonTerminals.TryGetValue(rule.Terminal!, out var list))
                    {
                        list = new List<string>();
                        _terminalToNonTerminals[rule.Terminal!] = list;
                    }
                    list.Add(rule.Left);
                }
                else
                {
                    var key = (rule.FirstRight!, rule.SecondRight!);
                    if (!_binaryToNonTerminals.TryGetValue(key, out var list))
                    {
                        list = new List<string>();
                        _binaryToNonTerminals[key] = list;
                    }
                    list.Add(rule.Left);
                }
            }
        }

        public IReadOnlyList<string> GetNonTerminalsForTerminal(string terminal)
        {
            return _terminalToNonTerminals.TryGetValue(terminal, out var list)
                ? list
                : Array.Empty<string>();
        }

        public IReadOnlyList<string> GetNonTerminalsForPair(string left, string right)
        {
            return _binaryToNonTerminals.TryGetValue((left, right), out var list)
                ? list
                : Array.Empty<string>();
        }
    }

    /// <summary>
    /// Represents a node in the generated concrete syntax/parse tree.
    /// </summary>
    public class ParseTreeNode
    {
        public string Symbol { get; }
        public string? TerminalValue { get; }
        public ParseTreeNode? LeftChild { get; }
        public ParseTreeNode? RightChild { get; }
        public bool IsLeaf => LeftChild == null && RightChild == null;

        public ParseTreeNode(string symbol, string terminalValue)
        {
            Symbol = symbol;
            TerminalValue = terminalValue;
            LeftChild = null;
            RightChild = null;
        }

        public ParseTreeNode(string symbol, ParseTreeNode leftChild, ParseTreeNode rightChild)
        {
            Symbol = symbol;
            TerminalValue = null;
            LeftChild = leftChild ?? throw new ArgumentNullException(nameof(leftChild));
            RightChild = rightChild ?? throw new ArgumentNullException(nameof(rightChild));
        }
    }

    /// <summary>
    /// Implements the Cocke-Younger-Kasami (CYK) dynamic programming parsing algorithm.
    /// </summary>
    public class CykParser
    {
        public Grammar Grammar { get; }

        public CykParser(Grammar grammar)
        {
            Grammar = grammar ?? throw new ArgumentNullException(nameof(grammar));
        }

        private class BacktrackPointer
        {
            public int Split { get; }
            public string LeftSymbol { get; }
            public string RightSymbol { get; }

            public BacktrackPointer(int split, string leftSymbol, string rightSymbol)
            {
                Split = split;
                LeftSymbol = leftSymbol;
                RightSymbol = rightSymbol;
            }
        }

        /// <summary>
        /// Parses an input string (tokenized character-by-character) to verify grammatical membership.
        /// </summary>
        public bool Parse(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var tokens = input.Select(c => c.ToString()).ToList();
            return Parse(tokens);
        }

        /// <summary>
        /// Parses an array/list of token strings to verify grammatical membership.
        /// </summary>
        public bool Parse(IReadOnlyList<string> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            int n = tokens.Count;
            if (n == 0) return false;

            // table[length - 1, start, symbol]
            var table = new HashSet<string>[n, n];
            for (int l = 0; l < n; l++)
            {
                for (int s = 0; s < n; s++)
                {
                    table[l, s] = new HashSet<string>();
                }
            }

            // Step 1: Base cases (substring length = 1)
            for (int i = 0; i < n; i++)
            {
                var nonTerminals = Grammar.GetNonTerminalsForTerminal(tokens[i]);
                foreach (var nt in nonTerminals)
                {
                    table[0, i].Add(nt);
                }
            }

            // Step 2: Inductive steps (substring lengths from 2 to n)
            for (int length = 2; length <= n; length++)
            {
                int lenIdx = length - 1;
                for (int start = 0; start <= n - length; start++)
                {
                    for (int split = 1; split < length; split++)
                    {
                        int leftLenIdx = split - 1;
                        int rightLenIdx = length - split - 1;
                        int rightStart = start + split;

                        var leftSet = table[leftLenIdx, start];
                        var rightSet = table[rightLenIdx, rightStart];

                        if (leftSet.Count == 0 || rightSet.Count == 0) continue;

                        foreach (var b in leftSet)
                        {
                            foreach (var c in rightSet)
                            {
                                var candidates = Grammar.GetNonTerminalsForPair(b, c);
                                foreach (var a in candidates)
                                {
                                    table[lenIdx, start].Add(a);
                                }
                            }
                        }
                    }
                }
            }

            return table[n - 1, 0].Contains(Grammar.StartSymbol);
        }

        /// <summary>
        /// Parses an input string (tokenized character-by-character) and returns the parse tree, or null if parsing fails.
        /// </summary>
        public ParseTreeNode? ParseTree(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var tokens = input.Select(c => c.ToString()).ToList();
            return ParseTree(tokens);
        }

        /// <summary>
        /// Parses an array/list of token strings and returns the parse tree, or null if parsing fails.
        /// </summary>
        public ParseTreeNode? ParseTree(IReadOnlyList<string> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            int n = tokens.Count;
            if (n == 0) return null;

            // backtracks[length - 1, start][symbol] = BacktrackPointer
            var backtracks = new Dictionary<string, BacktrackPointer>[n, n];
            for (int l = 0; l < n; l++)
            {
                for (int s = 0; s < n; s++)
                {
                    backtracks[l, s] = new Dictionary<string, BacktrackPointer>();
                }
            }

            // Step 1: Base cases (terminal tokens)
            for (int i = 0; i < n; i++)
            {
                var nonTerminals = Grammar.GetNonTerminalsForTerminal(tokens[i]);
                foreach (var nt in nonTerminals)
                {
                    // Split = 0 indicates a leaf / terminal production
                    backtracks[0, i][nt] = new BacktrackPointer(0, tokens[i], string.Empty);
                }
            }

            // Step 2: Dynamic programming fill
            for (int length = 2; length <= n; length++)
            {
                int lenIdx = length - 1;
                for (int start = 0; start <= n - length; start++)
                {
                    for (int split = 1; split < length; split++)
                    {
                        int leftLenIdx = split - 1;
                        int rightLenIdx = length - split - 1;
                        int rightStart = start + split;

                        var leftKeys = backtracks[leftLenIdx, start].Keys;
                        var rightKeys = backtracks[rightLenIdx, rightStart].Keys;

                        foreach (var b in leftKeys)
                        {
                            foreach (var c in rightKeys)
                            {
                                var candidates = Grammar.GetNonTerminalsForPair(b, c);
                                foreach (var a in candidates)
                                {
                                    if (!backtracks[lenIdx, start].ContainsKey(a))
                                    {
                                        backtracks[lenIdx, start][a] = new BacktrackPointer(split, b, c);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!backtracks[n - 1, 0].ContainsKey(Grammar.StartSymbol))
            {
                return null;
            }

            return BuildTree(backtracks, tokens, n - 1, 0, Grammar.StartSymbol);
        }

        private ParseTreeNode BuildTree(
            Dictionary<string, BacktrackPointer>[,] backtracks,
            IReadOnlyList<string> tokens,
            int lenIdx,
            int start,
            string symbol)
        {
            var bp = backtracks[lenIdx, start][symbol];

            if (lenIdx == 0)
            {
                return new ParseTreeNode(symbol, tokens[start]);
            }

            int split = bp.Split;
            int leftLenIdx = split - 1;
            int rightLenIdx = (lenIdx + 1) - split - 1;
            int rightStart = start + split;

            var leftTree = BuildTree(backtracks, tokens, leftLenIdx, start, bp.LeftSymbol);
            var rightTree = BuildTree(backtracks, tokens, rightLenIdx, rightStart, bp.RightSymbol);

            return new ParseTreeNode(symbol, leftTree, rightTree);
        }
    }
}