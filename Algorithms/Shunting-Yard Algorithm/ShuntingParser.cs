using System;
using System.Collections.Generic;
using System.Globalization;

public static class ShuntingParser
{
    public static List<string> Parse(string expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        var tokens = Tokenize(expression);
        var output = new List<string>();
        var operators = new Stack<string>();

        foreach (var token in tokens)
        {
            if (IsNumber(token))
            { 
                output.Add(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                bool foundOpenParenthesis = false;
                while (operators.Count > 0)
                {
                    var top = operators.Peek();
                    if (top == "(")
                    {
                        foundOpenParenthesis = true;
                        break;
                    }
                    output.Add(operators.Pop());
                }
                if (!foundOpenParenthesis)
                {
                    throw new InvalidOperationException("Mismatched parentheses: missing '('");
                }
                operators.Pop();
            }
            else
            {
                while (operators.Count > 0 && operators.Peek() != "(")
                {
                    var top = operators.Peek();
                    int topPrec = GetPrecedence(top);
                    int tokenPrec = GetPrecedence(token);

                    if (topPrec > tokenPrec || (topPrec == tokenPrec && !IsRightAssociative(token)))
                    {
                        output.Add(operators.Pop());
                    }
                    else
                    {
                        break;
                    }
                }
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
        {
            var top = operators.Pop();
            if (top == "(" || top == ")")
            {
                throw new InvalidOperationException("Mismatched parentheses");
            }
            output.Add(top);
        }

        return output;
    }

    private static List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        int i = 0;
        while (i < expression.Length)
        { 
            char c = expression[i];
            if (char.IsWhiteSpace(c))
            {
                i++;
                continue;
            }

            if (char.IsDigit(c) || c == '.')
            {
                int start = i;
                bool hasDecimal = c == '.';
                i++;
                while (i < expression.Length && (char.IsDigit(expression[i]) || (!hasDecimal && expression[i] == '.')))
                {
                    if (expression[i] == '.')
                    {
                        hasDecimal = true;
                    }
                    i++;
                }
                tokens.Add(expression.Substring(start, i - start));
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == '(' || c == ')')
            {
                tokens.Add(c.ToString());
                i++;
            }
            else
            {
                throw new InvalidOperationException($"Invalid character in expression: '{c}'");
            }
        }
        return tokens;
    }

    private static bool IsNumber(string token)
    {
        return double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }

    private static int GetPrecedence(string op)
    {
        return op switch
        {
            "^" => 4,
            "*" => 3,
            "/" => 3,
            "+" => 2,
            "-" => 2,
            _ => 0
        };
    }

    private static bool IsRightAssociative(string op)
    {
        return op == "^";
    }
}