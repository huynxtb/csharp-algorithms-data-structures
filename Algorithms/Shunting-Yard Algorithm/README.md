# Shunting-Yard Algorithm

### 1. Introduction
The Shunting-Yard algorithm is a method parsing mathematical expressions specified in infix notation into either Postfix notation (Reverse Polish Notation / RPN) or an Abstract Syntax Tree (AST). Invented by Edsger Dijkstra, it is commonly used in compilers, calculators, and formula evaluators to resolve operator precedence and parentheses correctly before evaluation.

### 2. Usage
```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        string expression = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
        try
        {
            List<string> postfix = ShuntingParser.Parse(expression);
            Console.WriteLine(string.Join(" ", postfix));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Parsing error: {ex.Message}");
        }
    }
}
```

### 3. Detailed Explanation
The implementation processes the input string through two phases:
1. **Tokenization**: The input string is scanned character by character. Multi-digit numbers and decimals are grouped into single tokens. Whitespace is discarded. Operators and parentheses are separated. Invalid characters trigger an `InvalidOperationException`.
2. **Parsing**: The algorithm maintains an output list and an operator stack. 
   - Numbers are immediately appended to the output list.
   - Left parentheses `(` are pushed onto the operator stack.
   - Right parentheses `)` trigger popping operators from the stack to the output list until a matching left parenthesis is found. If the stack empties without finding one, a mismatched parenthesis exception is thrown.
   - Operators are compared with the top of the operator stack. Operators on the stack with higher precedence, or equal precedence (if the current operator is left-associative), are popped to the output list before the current operator is pushed.
   - Finally, remaining operators on the stack are popped to the output list. Any remaining parentheses indicate mismatched input.

### 4. Complexity Analysis
- **Time Complexity**: $\mathcal{O}(N)$ where $N$ is the length of the expression. Each token is read once, pushed to the stack at most once, and popped from the stack at most once.
- **Space Complexity**: $\mathcal{O}(N)$ to store the tokenized representation, the operator stack, and the output list.