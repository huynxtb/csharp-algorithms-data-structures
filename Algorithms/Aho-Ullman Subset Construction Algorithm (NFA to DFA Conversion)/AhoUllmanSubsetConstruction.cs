using System;
using System.Collections.Generic;
using System.Linq;

namespace SubsetConstruction
{
    /// <summary>
    /// Represents a state in a Non-deterministic Finite Automaton (NFA).
    /// </summary>
    public class NfaState
    {
        public int Id { get; set; }
        public bool IsAccepting { get; set; }
        public Dictionary<char, HashSet<NfaState>> Transitions { get; set; }

        public NfaState(int id, bool isAccepting = false)
        {
            Id = id;
            IsAccepting = isAccepting;
            Transitions = new Dictionary<char, HashSet<NfaState>>();
        }

        public void AddTransition(char symbol, NfaState nextState)
        {
            if (!Transitions.ContainsKey(symbol))
            {
                Transitions[symbol] = new HashSet<NfaState>();
            }
            Transitions[symbol].Add(nextState);
        }
    }

    /// <summary>
    /// Represents a Non-deterministic Finite Automaton (NFA).
    /// </summary>
    public class Nfa
    {
        public NfaState StartState { get; set; }
        public HashSet<NfaState> States { get; set; }

        public Nfa(NfaState startState)
        {
            StartState = startState;
            States = new HashSet<NfaState>();
        }

        public void AddState(NfaState state)
        {
            States.Add(state);
        }
    }

    /// <summary>
    /// Represents a state in a Deterministic Finite Automaton (DFA).
    /// </summary>
    public class DfaState
    {
        public int Id { get; set; }
        public bool IsAccepting { get; set; }
        public Dictionary<char, DfaState> Transitions { get; set; }

        public DfaState(int id, bool isAccepting = false)
        {
            Id = id;
            IsAccepting = isAccepting;
            Transitions = new Dictionary<char, DfaState>();
        }

        public void AddTransition(char symbol, DfaState nextState)
        {
            Transitions[symbol] = nextState;
        }
    }

    /// <summary>
    /// Represents a Deterministic Finite Automaton (DFA).
    /// </summary>
    public class Dfa
    {
        public DfaState StartState { get; set; }
        public HashSet<DfaState> States { get; set; }

        public Dfa(DfaState startState)
        {
            StartState = startState;
            States = new HashSet<DfaState>();
        }

        public void AddState(DfaState state)
        {
            States.Add(state);
        }
    }

    /// <summary>
    /// Implements the Aho-Ullman subset construction algorithm for NFA to DFA conversion.
    /// </summary>
    public static class SubsetConstructionAlgorithm
    {
        private const char EpsilonSymbol = '\0';

        /// <summary>
        /// Converts an NFA to an equivalent DFA using the subset construction algorithm.
        /// </summary>
        /// <param name="nfa">The input NFA to be converted.</param>
        /// <returns>An equivalent DFA.</returns>
        public static Dfa ConvertNfaToDfa(Nfa nfa)
        {
            var alphabet = GetAlphabet(nfa);
            var dfaStates = new Dictionary<HashSet<NfaState>, DfaState>(new SubsetEqualityComparer());
            var queue = new Queue<HashSet<NfaState>>();
            var dfaStateCounter = 0;

            // Compute the epsilon closure of the NFA's start state
            var initialSubset = EpsilonClosure(new HashSet<NfaState> { nfa.StartState });
            var initialDfaState = new DfaState(dfaStateCounter++, IsAcceptingSubset(initialSubset));
            
            dfaStates[initialSubset] = initialDfaState;
            queue.Enqueue(initialSubset);

            var dfa = new Dfa(initialDfaState);
            dfa.AddState(initialDfaState);

            // Worklist algorithm: process all reachable DFA states
            while (queue.Count > 0)
            {
                var currentSubset = queue.Dequeue();
                var currentDfaState = dfaStates[currentSubset];

                foreach (var symbol in alphabet)
                {
                    // Compute the move operation
                    var nextSubset = Move(currentSubset, symbol);
                    
                    // Compute the epsilon closure of the result
                    var nextSubsetWithEpsilon = EpsilonClosure(nextSubset);

                    // Skip empty subsets (dead state)
                    if (nextSubsetWithEpsilon.Count == 0)
                        continue;

                    // Check if this subset has been seen before
                    if (!dfaStates.ContainsKey(nextSubsetWithEpsilon))
                    {
                        var newDfaState = new DfaState(dfaStateCounter++, IsAcceptingSubset(nextSubsetWithEpsilon));
                        dfaStates[nextSubsetWithEpsilon] = newDfaState;
                        dfa.AddState(newDfaState);
                        queue.Enqueue(nextSubsetWithEpsilon);
                    }

                    // Add transition in the DFA
                    var nextDfaState = dfaStates[nextSubsetWithEpsilon];
                    currentDfaState.AddTransition(symbol, nextDfaState);
                }
            }

            return dfa;
        }

        /// <summary>
        /// Computes the epsilon closure of a set of NFA states.
        /// </summary>
        /// <param name="states">The set of NFA states.</param>
        /// <returns>The epsilon closure of the input states.</returns>
        private static HashSet<NfaState> EpsilonClosure(HashSet<NfaState> states)
        {
            var closure = new HashSet<NfaState>(states);
            var stack = new Stack<NfaState>(states);

            while (stack.Count > 0)
            {
                var state = stack.Pop();

                if (state.Transitions.ContainsKey(EpsilonSymbol))
                {
                    foreach (var nextState in state.Transitions[EpsilonSymbol])
                    {
                        if (closure.Add(nextState))
                        {
                            stack.Push(nextState);
                        }
                    }
                }
            }

            return closure;
        }

        /// <summary>
        /// Computes the move operation: states reachable from a set of states via a given symbol.
        /// </summary>
        /// <param name="states">The set of NFA states.</param>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The set of states reachable via the symbol.</returns>
        private static HashSet<NfaState> Move(HashSet<NfaState> states, char symbol)
        {
            var result = new HashSet<NfaState>();

            foreach (var state in states)
            {
                if (state.Transitions.ContainsKey(symbol))
                {
                    foreach (var nextState in state.Transitions[symbol])
                    {
                        result.Add(nextState);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the input alphabet from an NFA (excluding epsilon transitions).
        /// </summary>
        /// <param name="nfa">The input NFA.</param>
        /// <returns>A set of all input symbols.</returns>
        private static HashSet<char> GetAlphabet(Nfa nfa)
        {
            var alphabet = new HashSet<char>();

            foreach (var state in nfa.States)
            {
                foreach (var symbol in state.Transitions.Keys)
                {
                    if (symbol != EpsilonSymbol)
                    {
                        alphabet.Add(symbol);
                    }
                }
            }

            return alphabet;
        }

        /// <summary>
        /// Determines if a subset of NFA states contains at least one accepting state.
        /// </summary>
        /// <param name="states">The subset of NFA states.</param>
        /// <returns>True if the subset contains an accepting state; otherwise, false.</returns>
        private static bool IsAcceptingSubset(HashSet<NfaState> states)
        {
            return states.Any(state => state.IsAccepting);
        }

        /// <summary>
        /// Custom equality comparer for HashSet<NfaState> to enable use as dictionary keys.
        /// </summary>
        private class SubsetEqualityComparer : IEqualityComparer<HashSet<NfaState>>
        {
            public bool Equals(HashSet<NfaState> x, HashSet<NfaState> y)
            {
                if (x == null || y == null)
                    return x == y;
                return x.SetEquals(y);
            }

            public int GetHashCode(HashSet<NfaState> obj)
            {
                if (obj == null)
                    return 0;
                int hash = 17;
                foreach (var state in obj.OrderBy(s => s.Id))
                {
                    hash = hash * 31 + state.Id.GetHashCode();
                }
                return hash;
            }
        }
    }
}