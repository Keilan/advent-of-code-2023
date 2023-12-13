using AdventOfCode2023.Utilities;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public static class Day13
{
    public class Pattern
    {
        public List<string> rows { get; set; }

        public Pattern(List<string> rows)
        {
            this.rows = rows;
        }
        
        public List<string> GetRows() 
        { 
            return rows; 
        }

        public List<string> GetColumns()
        {
            List<string> columns = [];

            return Enumerable.Range(0, rows[1].Count())
                .Select(column => string.Join("", Enumerable.Range(0, rows.Count()).Select(row => rows[row][column])))
                .ToList();
        }

        public static int GetStringDifferenceCount(int lowIdx, int highIdx, List<string> strings)
        {
            int distance = 0;
            while (lowIdx >= 0 && highIdx < strings.Count)
            {
                string s1 = strings[lowIdx];
                string s2 = strings[highIdx];

                for (int i = 0; i < s1.Length; i++)
                {
                    distance += s1[i] == s2[i] ? 0 : 1;
                }

                lowIdx--;
                highIdx++;
            }
            return distance;
        }

        public (string, int) FindReflection(int offset)
        {
            // Check horizontal lines
            for (int idx = 0; idx < rows.Count - 1; idx++)
            {
                int distance = GetStringDifferenceCount(idx, idx + 1, rows);

                if (distance == offset)
                {
                    return ("horizontal", idx);
                }
            }

            // Check vertical lines
            List<string> columns = GetColumns();
            for (int idx = 0; idx < columns.Count - 1; idx++)
            {
                int distance = GetStringDifferenceCount(idx, idx + 1, columns);

                if (distance == offset)
                {
                    return ("vertical", idx);
                }
            }

            throw new Exception("No reflection found");
        }

        public void Print()
        {
            foreach (var row in GetRows())
            {
                Console.WriteLine(row);
            }
        }

    }
        
    static List<Pattern> ReadInput(string[] input)
    {
        List<Pattern> result = [];

        List<string> rows = [];
        foreach (string line in input)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                rows.Add(line);
            }
            else
            {
                result.Add(new Pattern(rows));
                rows = [];
            }
        }

        result.Add(new Pattern(rows));
        rows = [];
        return result;
    }

    public static void Solution(string[] input)
    {
        // Read the map
        List<Pattern> patterns = ReadInput(input);

        int sum = 0;
        foreach (Pattern pattern in patterns)
        {
            (string orientation, int position) = pattern.FindReflection(offset: 0);

            if (orientation == "vertical") 
            {
                sum += position + 1;
            }
            else
            {
                sum += (position + 1) * 100;
            }
        }
        Console.WriteLine($"Part 1: {sum}");

        sum = 0;
        foreach (Pattern pattern in patterns)
        {
            (string orientation, int position) = pattern.FindReflection(offset: 1);

            if (orientation == "vertical")
            {
                sum += position + 1;
            }
            else
            {
                sum += (position + 1) * 100;
            }
        }
        Console.WriteLine($"Part 2: {sum}");
    }
}