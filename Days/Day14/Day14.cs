using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace AdventOfCode2023.Days;

public static class Day14
{

    static List<List<char>> ReadInput(string[] input)
    {
        List<List<char>> result = [];
        foreach (string line in input)
        {
            result.Add(line.ToCharArray().ToList());
        }
        return result;
    }

    static List<char> GetTilted(List<char> row)
    {
        Queue<int> freeIndices = new Queue<int>();
        for (int i = 0; i < row.Count; i++)
        {
            if (row[i] == '.')
            {
                freeIndices.Enqueue(i);
            }
            else if (row[i] == '#')
            {
                freeIndices.Clear();
            }
            else // Round rock
            {
                if (freeIndices.Count != 0) 
                {
                    row[freeIndices.Dequeue()] = 'O';
                    row[i] = '.';
                    freeIndices.Enqueue(i);
                }
            }
        }
        return row;
    }

    static void TiltNorth(List<List<char>> map)
    {
        for (int columnIdx = 0; columnIdx < map[0].Count; columnIdx++) 
        {
            List<char> column = Enumerable.Range(0, map.Count).Select(idx => map[idx][columnIdx]).ToList();
            List<char> tilted = GetTilted(column);
            for (int idx = 0; idx < tilted.Count; idx++)
            {
                map[idx][columnIdx] = tilted[idx];
            }
        }
    }

    static void TiltEast(List<List<char>> map)
    {
        for (int rowIdx = 0; rowIdx < map.Count; rowIdx++)
        {
            map[rowIdx].Reverse();
            map[rowIdx] = GetTilted(map[rowIdx]);
            map[rowIdx].Reverse();
        }
    }

    static void TiltSouth(List<List<char>> map)
    {
        for (int columnIdx = 0; columnIdx < map[0].Count; columnIdx++)
        {
            List<char> column = Enumerable.Range(0, map.Count).Select(idx => map[idx][columnIdx]).Reverse().ToList();
            List<char> tilted = GetTilted(column);
            tilted.Reverse();
            for (int idx = 0; idx < tilted.Count; idx++)
            {
                map[idx][columnIdx] = tilted[idx];
            }
        }
    }

    static void TiltWest(List<List<char>> map)
    {
        for (int rowIdx = 0; rowIdx < map.Count; rowIdx++)
        {
            map[rowIdx] = GetTilted(map[rowIdx]);
        }
    }

    static int CountNorthLoad(List<List<char>> map)
    {
        int load = 0;
        int rowWeight = map.Count;
        foreach (List<char> row in map)
        {
            load += row.Where(c => c == 'O').Count() * rowWeight;
            rowWeight--;
        }
        return load;
    }

    static void PrintMap(List<List<char>> map)
    {
        foreach (List<char> row in map)
        {
            Console.WriteLine(string.Join("", row));
        }
    }

    static string GetMapString(List<List<char>> map)
    {
        string res = "";
        foreach (List<char> row in map)
        {
            res += string.Join("", row);
        }
        return res;
    }

    public static void Solution(string[] input)
    {
        // Read the map
        List<List<char>> map = ReadInput(input);
        TiltNorth(map);
        int load = CountNorthLoad(map);

        Console.WriteLine($"Part 1: {load}");

        // Refresh map
        map = ReadInput(input);
        int targetCycles = 1000000000;
        Dictionary<string, int> seen = [];
        for (int cycle = 1; cycle <= targetCycles; cycle++)
        {
            TiltNorth(map);
            TiltWest(map);
            TiltSouth(map);
            TiltEast(map);

            // When we encounter a string we've seen - jump ahead to close to the target cycle
            string mapString = GetMapString(map);
            if (seen.ContainsKey(mapString))
            {
                int match = seen[mapString];
                int period = cycle - match;
                cycle += period * (int)((targetCycles - cycle) / period);
            }
            else
            {
                seen[mapString] = cycle;
            }
        }

        load = CountNorthLoad(map);
        Console.WriteLine($"Part 2: {load}");
    }
}