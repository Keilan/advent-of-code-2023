using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

public static class Day3
{
    static bool IsPartNumber(int x, int y, List<string> schematic)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (x+i >= 0 && x+i < schematic.Count && y+j >= 0 && y+j < schematic[0].Length)
                {
                    char val = schematic[x+i][y+j];
                    if (!Char.IsNumber(val) && val != '.')
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    static List<(int,int)> GetAdjacentGears(int x, int y, List<string> schematic)
    {
        List<(int, int)> gears = [];
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (x + i >= 0 && x + i < schematic.Count && y + j >= 0 && y + j < schematic[0].Length)
                {
                    char val = schematic[x + i][y + j];
                    if (val == '*')
                    {
                        gears.Add((x + i, y + j));
                    }
                }
            }
        }
        return gears;
    }

    public static void Solution(string[] input)
    {
        // Read the schematic
        List<string> schematic = new List<string>();
        foreach (string line in input)
        {
            schematic.Add(line);
        }

        // Loop through each line, finding the numbers and adding them to the parts list
        List<int> parts = new List<int>();

        // Also track which numbers are adjacent to which gears
        Dictionary<(int, int), List<int>> gearParts = [];
        for (int x = 0; x < schematic.Count; x++)
        {
            string currentNumber = "";
            bool currentIsPart = false;
            HashSet<(int, int)> gears = [];

            for (int y = 0; y < schematic[x].Length; y++)
            {
                if (Char.IsNumber(schematic[x][y])){
                    currentNumber += schematic[x][y];
                    currentIsPart = IsPartNumber(x, y, schematic) ? true : currentIsPart;
                    gears.UnionWith(GetAdjacentGears(x, y, schematic));
                }
                else
                {
                    if (currentNumber.Length > 0 && currentIsPart)
                    {
                        parts.Add(int.Parse(currentNumber));
                        foreach((int, int) gear in gears)
                        {
                            if (!gearParts.ContainsKey(gear))
                            {
                                gearParts[gear] = [];
                            }
                            gearParts[gear].Add(int.Parse(currentNumber));
                        }
                    }
                    currentNumber = "";
                    currentIsPart = false;
                    gears = [];
                }
            }

            // Handle line end
            if (currentNumber.Length > 0 && currentIsPart)
            {
                parts.Add(int.Parse(currentNumber));
                foreach ((int, int) gear in gears)
                {
                    if (!gearParts.ContainsKey(gear))
                    {
                        gearParts[gear] = [];
                    }
                    gearParts[gear].Add(int.Parse(currentNumber));
                }
            }
        }

        Console.WriteLine($"Part 1: {parts.Sum()}");

        int part2Sum = 0;
        foreach(KeyValuePair<(int,int), List<int>> entry in gearParts)
        {
            if (entry.Value.Count == 2)
            {
                part2Sum += entry.Value[0] * entry.Value[1];
            }
        }

        Console.WriteLine($"Part 2: {part2Sum}");
    }
}