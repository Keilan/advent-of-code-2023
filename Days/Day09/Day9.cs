using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public static class Day09
{
    static List<List<int>> ReadInput(string[] input)
    {
        List<List<int>> sequences = [];
        foreach (string line in input)
        {
            sequences.Add(line.Split(" ").Select(s => int.Parse(s)).ToList());
        }
        return sequences;
    }

    static List<int> GetDifferences(List<int> sequence)
    {
        List<int> result = [sequence[1] - sequence[0]];
        for (int i = 2; i < sequence.Count; i++)
        {
            result.Add(sequence[i] - sequence[i-1]);
        }
        return result;
    }

    static bool AllZero(List<int> sequence)
    {
        return sequence.All(x => x == 0);
    }

    public static void Solution(string[] input)
    {
        // Read the map
        List<List<int>> sequences = ReadInput(input);

        int lastValueSum = 0;
        foreach(List<int> sequence in sequences)
        {
            List<int> current = sequence;
            List<int> lastValues = [];
            while (!AllZero(current))
            {
                lastValues.Add(current.Last());
                current = GetDifferences(current);
            }
            lastValueSum += lastValues.Sum();
        }

        Console.WriteLine($"Part 1: {lastValueSum}");

        int previousValueSum = 0;
        foreach (List<int> sequence in sequences)
        {
            List<int> current = sequence;
            List<int> firstValues = [];
            while (!AllZero(current))
            {
                firstValues.Add(current.First());
                current = GetDifferences(current);
            }

            int currentDifference = 0;
            firstValues.Reverse();
            foreach(int value in firstValues)
            {
                currentDifference = value - currentDifference;
            }
            previousValueSum += currentDifference;
        }
        Console.WriteLine($"Part 2: {previousValueSum}");
    }
}