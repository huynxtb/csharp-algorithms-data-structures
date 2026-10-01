# Aho-Ullman Subset Construction Algorithm

## Introduction

The Aho-Ullman subset construction algorithm is a foundational technique in formal language theory and compiler construction that converts a Non-deterministic Finite Automaton (NFA) into an equivalent Deterministic Finite Automaton (DFA). This algorithm is essential for:

- **Lexical Analysis**: Building efficient lexical analyzers from regular expressions
- **Pattern Matching**: Implementing fast string matching engines
- **Compiler Design**: Converting regular expression patterns into efficient scanners
- **Formal Language Processing**: Automatically generating DFAs from NFAs

The key insight is that each DFA state represents a subset of NFA states, effectively combining multiple non-deterministic paths into single deterministic transitions.

## Usage

```csharp
using SubsetConstruction;

// Create NFA states
var nfa0 = new NfaState(0, false);
var nfa1 = new NfaState(1, false);
var nfa2 = new NfaState(2, false);
var nfa3 = new NfaState(3, true);

// Build NFA for pattern: a*b (zero or more 'a' followed by 'b')
nfa0.AddTransition('a', nfa0);        // self-loop on 'a'
nfa0.AddTransition('\0', nfa1);       // epsilon transition to nfa1
nfa0.AddTransition('b', nfa1);        // 'b' transition
nfa1.AddTransition('b', nfa2);        // 'b' transition
nfa2.AddTransition('\0', nfa3);       // epsilon transition to accepting state

// Create and populate NFA
var nfa = new Nfa(nfa0);
nfa.AddState(nfa0);
nfa.AddState(nfa1);
nfa.AddState(nfa2);
nfa.AddState(nfa3);

// Convert NFA to DFA using Aho-Ullman algorithm
var dfa = SubsetConstructionAlgorithm.ConvertNfaToDfa(nfa);

// Now the DFA can be used for efficient pattern matching
// DFA states correspond to subsets of NFA states
// DFA transitions are deterministic (only one next state per input)
```

## Detailed Explanation

### Algorithm Components

1. **NfaState Class**: Represents a state in an NFA with:
   - Unique integer ID
   - Accepting/final state flag
   - Transitions dictionary mapping symbols to sets of next states (non-deterministic)
   - Support for epsilon transitions using the '\0' character

2. **DfaState Class**: Represents a state in a DFA with:
   - Unique integer ID
   - Accepting/final state flag
   - Transitions dictionary mapping symbols to single next states (deterministic)

3. **Core Algorithm Steps**:

   a) **Epsilon Closure**: For a set of NFA states, compute all states reachable via zero or more epsilon transitions. Uses a depth-first search approach with a stack to ensure all reachable states are discovered.
   
   b) **Move Operation**: Given a set of NFA states and an input symbol, compute the set of all NFA states reachable by that symbol (ignoring epsilon transitions).
   
   c) **Subset Construction**: 
      - Start with the epsilon closure of the NFA's initial state as the DFA's initial state
      - Use a worklist algorithm to process unexplored DFA states
      - For each DFA state and input symbol, compute the next DFA state by:
        * Applying the move operation to the NFA subset
        * Computing the epsilon closure of the result
        * Mapping this to a new (or existing) DFA state
      - A DFA state is accepting if any NFA state in its subset is accepting
      - Skip empty subsets (they represent dead states)

4. **Alphabet Extraction**: Collects all input symbols from the NFA transitions, excluding epsilon transitions.

5. **Subset Equality**: Implements a custom equality comparer for HashSet<NfaState> to use subsets as dictionary keys, preventing duplicate DFA states.

### Key Implementation Details

- **Epsilon Symbol**: Represented by '\0' (null character) for epsilon transitions
- **Worklist Processing**: Ensures all reachable DFA states are discovered through breadth-first enumeration
- **Duplicate Prevention**: Maps NFA subsets to their corresponding DFA states to avoid creating multiple states for the same subset
- **Deterministic Behavior**: Each DFA state has exactly one transition per input symbol (or none for dead states)

## Complexity Analysis

### Time Complexity

- **Epsilon Closure**: O(|S| × |δ|) where |S| is the size of the input state set and |δ| is the number of transitions. In the worst case, O(|Q|²) for |Q| NFA states.
- **Move Operation**: O(|S| × |δ|), typically O(|S|) where we iterate through states and check their transitions.
- **Overall Conversion**: O(2^|Q| × |Σ|) where |Q| is the number of NFA states and |Σ| is the alphabet size. This worst-case occurs when all NFA subsets are reachable. In practice, many NFAs result in polynomial-sized DFAs.
- **Epsilon Closure (per state)**: O(|Q| + |δ_ε|) where |δ_ε| is the number of epsilon transitions.

### Space Complexity

- **DFA States**: O(2^|Q|) in the worst case, as each DFA state corresponds to a unique subset of NFA states.
- **Transition Storage**: O(2^|Q| × |Σ|) for all DFA transitions.
- **Auxiliary Space**: O(|Q| + |Σ|) for the alphabet and state sets during processing.
- **Dictionary Overhead**: O(2^|Q|) for mapping NFA subsets to DFA states.

### Practical Considerations

- Most practical NFAs (e.g., from regular expressions) result in DFAs with significantly fewer states than the theoretical worst case of 2^|Q|
- The algorithm is guaranteed to terminate and produce a minimal DFA in terms of the number of states generated by subset construction
- Further DFA minimization algorithms (e.g., Hopcroft-Karp) can reduce the DFA further if needed