# Suffix Automaton (SAM)

## Introduction
A *Suffix Automaton* (SAM) is a highly efficient data structure representing all substrings of a given string. It is a minimal deterministic finite automaton (DFA) that accepts all suffixes of the string. SAM is capable of answering various substring-related queries in linear time relative to the input string length, making it extremely useful for advanced string processing tasks such as substring checking, counting distinct substrings, and finding the longest common substring between strings.

This implementation builds the SAM incrementally by adding characters one-by-one and supports efficient substring existence checks, distinct substring counting, and tracking the longest substring length.

## Usage
You can use the Suffix Automaton like this:

// Initialize for a maximum expected length (e.g., length of your input string)
var sam = new SuffixAutomaton(100000);

// Add characters of the input string sequentially
string input = "ababc";
foreach(char c in input)
{
    sam.AddCharacter(c);
}

// Check if a substring exists
bool exists = sam.Contains("bab"); // true

// Get number of distinct substrings
long distinctSubstrings = sam.CountDistinctSubstrings();

// Get length of the longest substring processed so far
int longestLength = sam.LongestSubstringLength;

## Detailed Explanation
- **States:** Each state corresponds to a set of end positions of substrings in the original string, storing the longest substring length ending at that state.
- **Suffix Links:** Connect states to smaller suffix states ensuring minimal automaton construction.
- **Transitions:** For each state, transitions to next states by characters exist, representing substrings extended by those characters.

Upon adding a character `c`, the SAM extends all suffixes by `c`, creating new states or cloning existing ones to maintain minimal size while preserving all substrings. This approach allows efficient incremental processing.

The number of distinct substrings is calculated using the sum over states of the difference between a state's length and its suffix link's length since each substring corresponds to exactly one such difference.

## Complexity Analysis
- **Building the SAM:** O(n), where n is the length of the input string, since each character addition takes amortized constant time.
- **Substring existence check:** O(m), where m is the length of the substring queried.
- **Counting distinct substrings:** O(n), done once after construction by summing over states.

**Space Complexity:** O(n), proportional to the length of the string, as the number of states is at most 2 * n - 1, and transitions are stored sparsely.

This makes the Suffix Automaton an extremely powerful and efficient tool for advanced string processing problems in many competitive programming and real-world scenarios.