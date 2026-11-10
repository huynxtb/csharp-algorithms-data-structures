using System.Collections.Generic;

public class FibonacciGenerator
{
    public List<int> GenerateFibonacci(int count)
    {
        List<int> fibonacciNumbers = new List<int>();
        if (count <= 0)
        {
            return fibonacciNumbers;
        }
        if (count >= 1)
        {
            fibonacciNumbers.Add(0);
        }
        if (count >= 2)
        {
            fibonacciNumbers.Add(1);
        }

        for (int i = 2; i < count; i++)
        {
            int nextNumber = fibonacciNumbers[i - 1] + fibonacciNumbers[i - 2];
            fibonacciNumbers.Add(nextNumber);
        }

        return fibonacciNumbers;
    }
}