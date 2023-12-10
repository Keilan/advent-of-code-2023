using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

public static class Day02
{
    public static void Solution(string[] input)
    {
        int part1Sum = 0;
        int part2Sum = 0;

        foreach (string game in input)
        {
            // Get ID
            string[] firstSplit = game.Split(": ");
            int gameID = int.Parse(firstSplit[0].Split(" ")[1]);

            // Get handfuls
            string[] handfuls = firstSplit[1].Split(";");

            Dictionary<string, int> maxColors = new Dictionary<string, int>()
            {
                { "red", 0 },
                { "blue", 0 },
                { "green", 0 }
            };
            foreach (string handful in handfuls)
            { 
                string[] colors = handful.Trim().Split(",");
                foreach(string colorCount in colors)
                {
                    string[] split = colorCount.Trim().Split(" ");
                    string color = split[1];
                    int number = int.Parse(split[0]);

                    maxColors[color] = Math.Max(maxColors[color], number);
                }
            }

            // Part 1
            if (maxColors["red"] <= 12 && maxColors["green"] <= 13 && maxColors["blue"] <= 14) 
            {
                part1Sum += gameID;
            }

            // Part 2
            int power = maxColors["red"] * maxColors["green"] * maxColors["blue"];
            part2Sum += power;
        }

        Console.WriteLine($"Part 1 Sum: {part1Sum}");
        Console.WriteLine($"Part 2 Sum: {part2Sum}");
    }
}