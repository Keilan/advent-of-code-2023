using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public static class Day11
{
    static List<List<char>> ReadInput(string[] input, out HashSet<int> expansionRows, out HashSet<int> expansionColumns)
    {
        // Store which columns have a galaxy
        bool[] galaxyInRow = Enumerable.Repeat(false, input.Length).ToArray();
        bool[] galaxyInColumn = Enumerable.Repeat(false, input[0].Length).ToArray();
        
        List<List<char>> image = [];
        for (int i = 0; i < input.Length; i++)
        {
            string line = input[i];
            List<char> characterList = line.ToCharArray().ToList();

            // Mark columns with galaxies
            for (int j = 0; j < characterList.Count; j++)
            {
                if (characterList[j] == '#')
                {
                    galaxyInColumn[j] = true;
                }
            }

            image.Add(characterList);

            // Track which rows have galaxies
            if (line.Contains('#'))
            {
                galaxyInRow[i] = true;
            }
        }

        // Expand columns with no galaxies - note the index offsets as we insert
        expansionRows = galaxyInRow.Select((x, idx) => !x ? idx : -1).Where(i => i != -1).ToHashSet();
        expansionColumns = galaxyInColumn.Select((x, idx) => !x ? idx : -1).Where(i => i != -1).ToHashSet();

        return image;
    }

    public static List<(int x, int y)> FindGalaxies(List<List<char>> image)
    {
        List<(int, int)> galaxies = [];

        for (int x = 0; x < image.Count; x++)
        {
            for (int y = 0;  y < image[x].Count; y++)
            {
                if (image[x][y] == '#')
                {
                    galaxies.Add((x, y));
                }
            }
        }

        return galaxies;
    }

    public static long GetManhattenDistance((int x, int y) a, (int x, int y) b, HashSet<int> expansionRows, HashSet<int> expansionColumns, int expansionFactor)
    {
        // Count how many points to add due to expansion
        long expansionCount = 0;
        for (int x = Math.Min(a.x, b.x); x < Math.Max(a.x, b.x); x++)
        {
            if (expansionRows.Contains(x))
            {
                expansionCount += expansionFactor - 1;
            }
        }
        for (int y = Math.Min(a.y, b.y); y < Math.Max(a.y, b.y); y++)
        {
            if (expansionColumns.Contains(y))
            {
                expansionCount += expansionFactor - 1;
            }
        }
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + expansionCount;
    }

    public static void Solution(string[] input)
    {
        // Read the map
        HashSet<int> expansionRows, expansionColumns;
        List<List<char>> image = ReadInput(input, out expansionRows, out expansionColumns);
        List<(int, int)> galaxies = FindGalaxies(image);

        // Compute sum of distances
        long distanceSum = 0;
        for (int i = 0; i <  galaxies.Count; i++)
        {
            for (int j = i+1; j < galaxies.Count; j++)
            {
                distanceSum += GetManhattenDistance(galaxies[i], galaxies[j], expansionRows, expansionColumns, 1000000);
            }
        }

        Console.WriteLine($"Part 1: {distanceSum}");

        /*        // Print for debugging
                for (int i = 0; i < image.Count; i++)
                {
                    Console.WriteLine(string.Join("", image[i]));
                }*/
    }
}