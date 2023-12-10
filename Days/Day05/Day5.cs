using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

public static class Day05
{
    static void ReadInput(string[] input, out List<long> seeds, out List<(string, string, List<(long, long, long)>)> almanac)
    {
        seeds = new();
        almanac = new();

        string source = "";
        string dest = "";
        List<(long, long, long)> maps = new();
        foreach (string line in input)
        {
            // Read seeds
            if (line.Contains("seeds:")) {
                string seedList = line.Remove(0, "seeds: ".Length);
                seeds = seedList.Split(' ').Select(s => long.Parse(s)).ToList();
            }

            // Read map
            else if (line.Contains("-to-"))
            {
                string[] names = line.Remove(line.IndexOf(" map:")).Split("-to-");
                source = names[0];
                dest = names[1];
            }

            else if (!string.IsNullOrEmpty(line))
            {
                List<long> values = line.Split(' ').Select(v => long.Parse(v)).ToList();
                maps.Add((values[0], values[1], values[2]));
            }

            // Add map to almanac
            else if (source != "")
            {
                almanac.Add((source, dest, maps.OrderBy(map => map.Item2).ToList()));
                source = "";
                dest = "";
                maps = new();
            }
        }

        // Add final map
        almanac.Add((source, dest, maps.OrderBy(map => map.Item2).ToList()));
    }

    static long GetLocationValue(long seedValue, List<(string, string, List<(long, long, long)>)> almanac)
    {
        string category = "seed";
        long value = seedValue;
        while (category != "location")
        {
            foreach (var (source, dest, maps) in almanac)
            {
                if (source == category)
                {
                    category = dest;
                    foreach (var (destStart, sourceStart, rangeLength) in maps)
                    {
                        if (value >= sourceStart && value < sourceStart + rangeLength)
                        {
                            value = destStart + (value - sourceStart);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        return value;
    }

    /// <summary>
    /// This function works backwards to take a value from any category, and figure out what the seed equivalent is.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="category"></param>
    /// <param name="almanac"></param>
    /// <returns></returns>
    static long GetSeedValue(long value, string category, List<(string, string, List<(long, long, long)>)> almanac) {
        while (category != "seed")
        {
            foreach (var (source, dest, maps) in almanac)
            {
                if (dest == category)
                {
                    category = source;
                    foreach (var (destStart, sourceStart, rangeLength) in maps)
                    {
                        if (value >= destStart && value < destStart + rangeLength)
                        {
                            value = sourceStart + (value - destStart);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        return value;
    }

    public static bool IsValidSeed(long seed, List<(long start, long length)> seedRanges)
    {
        foreach (var (start, length) in seedRanges)
        {
            if (seed >= start && seed < start + length)
            {
                return true;
            }
        }
        return false;
    }

    public static void Solution(string[] input)
    {
        // Read the input
        List<long> seeds;
        List<(string source, string dest, List<(long destStart, long sourceStart, long rangeLength)> maps)> almanac;
        ReadInput(input, out seeds, out almanac);

        long minLocation = long.MaxValue;
        foreach (long seed in seeds)
        {
            long value = GetLocationValue(seed, almanac);

            if (value < minLocation)
            {
                minLocation = value;
            }
        }

        Console.WriteLine($"Part 1 - {minLocation}");

        // Get pairs of seeds
        var seedStarts = seeds.Where((s, i) => i % 2 == 0);
        var rangeLengths = seeds.Where((s, i) => i % 2 == 1);
        List<(long start, long length)> ranges = seedStarts.Zip(rangeLengths, (s, l) => (s, l)).ToList();

        // Build a list of potential lowest values, this will be be the lowest seed, and the lowest
        // seed equivalent location of all the other ranges, or the location right above or below a range,
        // in case the ranges all increase values
        List<long> options = new(seedStarts);
        foreach (var (sourceCategory, destCategory, maps) in almanac)
        {
            foreach (var (destStart, sourceStart, rangeLength) in maps)
            {
                options.AddRange([
                    GetSeedValue(destStart-1, destCategory, almanac),
                    GetSeedValue(destStart, destCategory, almanac),
                    GetSeedValue(destStart+rangeLength-1, destCategory, almanac),
                    GetSeedValue(destStart+rangeLength, destCategory, almanac),
                ]);
            }
        }

        // Remove invalid seeds
        options = options.Distinct().Where(s => IsValidSeed(s, ranges)).ToList();

        // Determine the minimum options
        minLocation = long.MaxValue;
        foreach (long seed in options)
        {
            long value = GetLocationValue(seed, almanac);

            if (value < minLocation)
            {
                minLocation = value;
            }
        }

        Console.WriteLine($"Part 2 - {minLocation}");
    }
}