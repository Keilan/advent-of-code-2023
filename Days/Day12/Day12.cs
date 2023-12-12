using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public static class Day12
{

    static List<(List<char> springs, List<int> groups)> ReadInput(string[] input)
    {
        List<(List<char> springs, List<int> groups)> result = [];
        foreach (string line in input)
        {
            string[] split = line.Split(' ');
            List<int> groups = split[1].Split(',').Select(s => int.Parse(s)).ToList();
            result.Add((split[0].ToCharArray().ToList(), groups));
        }
        return result;
    }

    static bool IsValid(List<char> springs, List<int> groups)
    {
        int groupIdx = 0;
        int contiguous = 0;
        for (int i = 0; i < springs.Count; i++)
        {
            if (springs[i] == '#')
            {
                contiguous++;
            }
            else  // Springs i == '.'
            {
                if (contiguous > 0)
                {
                    if (groupIdx >= groups.Count || groups[groupIdx] != contiguous)
                    {
                        return false;
                    }
                    groupIdx++;
                    contiguous = 0;
                }
            }
        }

        // Final value
        if (contiguous > 0)
        {
            if (groupIdx >= groups.Count || groups[groupIdx] != contiguous)
                return false;
            groupIdx++;
        }

        // Make sure all groups are used
        if (groupIdx != groups.Count)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// This function can be interpreted as returning the number of options satisfying the given springs and groups, only considering
    /// values at springIdx or higher.
    /// </summary>
    /// <returns></returns>
    static long CountOptionsRecursive(List<char> springs, List<int> groups, char prevChar, ref Dictionary<string, long> cache)
    {
        //Console.WriteLine($"Counting {string.Join("", springs)} for {string.Join(",", groups)} - prev {prevChar}");

        // Check cache
        string key = $"{string.Join("", springs)}::{string.Join(",", groups)}::{prevChar}";
        if (cache.ContainsKey(key))
        {
            return cache[key];
        }

        long count = 0;
        // Reached end of list - see if end conditions are satisfied
        if (springs.Count == 0)
        {
            count = (groups.Count == 0 || (groups.Count == 1 && groups[0] == 0)) ? 1 : 0;
        }

        else if (springs[0] == '#')
        {
            // If no more groups - invalid
            if (groups.Count == 0)
            {
                count = 0;
            }

            else
            {
                // Decrement last group - break out early if it's a negative
                groups[0]--;
                if (groups[0] < 0)
                {
                    groups[0]++;
                    return 0;
                }
                else
                {
                    count = CountOptionsRecursive(springs.Skip(1).ToList(), groups, springs[0], ref cache);
                    groups[0]++;
                }
            }

        }
        else if (springs[0] == '.')
        {
            if (groups.Count == 0)
            {
                count = CountOptionsRecursive(springs.Skip(1).ToList(), groups, springs[0], ref cache);
            }

            // Too many contiguous broken springs
            else if (groups[0] < 0)
            {
                count = 0;
            }
            // Group was too small
            else if (groups[0] > 0 && prevChar == '#')
            {
                count = 0;
            }
            // Finished a group
            else if (groups[0] == 0)
            {
                count = CountOptionsRecursive(springs.Skip(1).ToList(), groups.Skip(1).ToList(), springs[0], ref cache);
            }

            // Still going
            else
            {
                count = CountOptionsRecursive(springs.Skip(1).ToList(), groups, springs[0], ref cache);
            }
            
        }

        // Try both
        else
        {
            springs[0] = '#';
            count += CountOptionsRecursive(springs, groups, prevChar, ref cache);
            springs[0] = '.';
            count += CountOptionsRecursive(springs, groups, prevChar, ref cache);
            springs[0] = '?';
        }

        cache[key] = count;
        return count;
    }

    public static List<char> UnfoldSprings(List<char> springs)
    {
        List<char> result = [];
        for (int i = 0; i < 5; i++)
        {
            result.AddRange(springs);
            if (i != 4)
            {
                result.Add('?');
            }
        }
        return result;
    }

    public static List<int> UnfoldGroups(List<int> groups)
    {
        List<int> result = [];
        for (int i = 0; i < 5; i++)
        {
            result.AddRange(groups);
        }
        return result;
    }

    public static void Solution(string[] input)
    {
        // Read the map
        List<(List<char> springs, List<int> groups)> springReport = ReadInput(input);

        long optionSum = 0;
        springReport = ReadInput(input);
        Dictionary<string, long> cache = [];
        foreach (var (springs, groups) in springReport)
        {
            long optionCount = CountOptionsRecursive(springs, groups, '.', ref cache);
            optionSum += optionCount;
        }
        Console.WriteLine($"Part 1 Recursive: {optionSum}");

        optionSum = 0;
        springReport = ReadInput(input); // Refresh input
        foreach (var (springs, groups) in springReport)
        {
            long optionCount = CountOptionsRecursive(UnfoldSprings(springs), UnfoldGroups(groups), '.', ref cache);
            optionSum += optionCount;
        }
        Console.WriteLine($"Part 2 Recursive: {optionSum}");
    }
}