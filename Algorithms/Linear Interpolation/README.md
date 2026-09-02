# Linear Interpolation

## Introduction
Linear interpolation (Lerp) calculates a value between two known endpoints based on a weight factor `weight`. Use Lerp for smooth transitions, animation blending, audio processing, and numeric estimation.

## Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        double start = 10.0;
        double end = 20.0;
        double factor = 0.5;

        double result = InterpolationUtils.Linear(start, end, factor);
        Console.WriteLine($"Interpolated value: {result}"); // Output: 15.0
    }
}
```

## Detailed Explanation
The `InterpolationUtils.Linear` method uses generic math via `System.Numerics.INumber<T>`. It converts the floating-point `weight` factor into type `T` using `T.CreateChecked(weight)`. The intermediate value is evaluated using the formula `val1 + (val2 - val1) * weight`, ensuring accurate linear scaling between start and end bounds.

## Complexity Analysis
- **Time Complexity:** O(1) for scalar arithmetic operations.
- **Space Complexity:** O(1) auxiliary memory space.