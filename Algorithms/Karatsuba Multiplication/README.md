# Karatsuba Multiplication Algorithm

### 1. Introduction
The Karatsuba algorithm is a fast multiplication algorithm for large integers. It reduces the multiplication of two $n$-digit numbers to at most three multiplications of $n/2$-digit numbers, compared to the four multiplications required by the classical schoolbook algorithm. This implementation is designed for arbitrary-precision integers represented as strings, bypassing the limits of standard primitive data types without using `System.Numerics.BigInteger`.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        string num1 = "123456789012345678901234567890";
        string num2 = "987654321098765432109876543210";
        
        string result = KaratsubaMultiplier.Multiply(num1, num2);
        Console.WriteLine($"Product: {result}");
    }
}
```

### 3. Detailed Explanation
The algorithm splits the input numbers $x$ and $y$ into two halves:
- $x = a \cdot 10^{m} + b$
- $y = c \cdot 10^{m} + d$

Where $m = n/2$. The product is computed as:
$$xy = 10^{2m}ac + 10^{m}(ad + bc) + bd$$

Karatsuba optimizes the middle term $(ad + bc)$ using only one additional multiplication:
$$ad + bc = (a + b)(c + d) - ac - bd$$

This reduces the recursive steps from 4 multiplications to 3:
1. $z_2 = ac$
2. $z_0 = bd$
3. $z_1 = (a + b)(c + d)$
4. $\text{middle} = z_1 - z_2 - z_0$

The helper methods `Add` and `Subtract` perform digit-by-digit arithmetic on strings to support these operations without overflow.

### 4. Complexity Analysis
- **Time Complexity**: $\mathcal{O}(n^{\log_2 3}) \approx \mathcal{O}(n^{1.585})$, which is faster than the classical $\mathcal{O}(n^2)$ schoolbook multiplication for large $n$.
- **Space Complexity**: $\mathcal{O}(n)$ auxiliary space due to recursion stack depth and string allocations during splitting and arithmetic operations.