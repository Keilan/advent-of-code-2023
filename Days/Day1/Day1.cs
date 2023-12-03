using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

public static class Day1
{
    public static void Solution(string[] input)
    {
        // Solve Part 1
        int calibrationSum = 0;
        foreach (string line in input)
        {
            List<int> numbers = [];
            foreach (char c in line)
            {
                if (char.IsNumber(c))
                {
                    numbers.Add(int.Parse(c.ToString()));
                }
            }
            calibrationSum += numbers.First() * 10 + numbers.Last();
        }

        Console.WriteLine($"Part 1: {calibrationSum}");

        // Solve Part 2 - do the same thing but track indices of written numbers
        List<string> writtenNumbers = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        int calibrationSum2 = 0;
        foreach (string line in input)
        {
            Dictionary<int, int> toInsert = [];
            for (int numberIdx = 0; numberIdx < writtenNumbers.Count; numberIdx++) 
            {
                string number = writtenNumbers[numberIdx];
                int index = line.IndexOf(number);
                while (index != -1)
                {
                    toInsert[index] = numberIdx+1;
                    index = line.IndexOf(number, index + number.Length);
                }
            }

            List<int> numbers = [];
            for (int idx = 0; idx < line.Length; idx++)
            {
                if (toInsert.ContainsKey(idx))
                {
                    numbers.Add(toInsert[idx]);
                }
                if (char.IsNumber(line[idx]))
                {
                    numbers.Add(int.Parse(line[idx].ToString()));
                }
            }
            calibrationSum2 += numbers.First() * 10 + numbers.Last();
        }

        Console.WriteLine($"Part 2: {calibrationSum2}");
    }
}