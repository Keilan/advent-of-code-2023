using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

public static class Day06
{
    static List<(int time, int record)> ReadInput(string[] input)
    {
        List<(int, int)> races = [];

        List<int> times = input[0].Remove(0, "Time:".Length).Split(" ")
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => int.Parse(s)).ToList();
        List<int> records = input[1].Remove(0, "Distance:".Length).Split(" ")
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => int.Parse(s)).ToList();

        return times.Zip(records, (t, r) => (t, r)).ToList();
    }

    static (double lower, double upper) QuadraticFormula(long time, long record)
    {
        long a = -1;
        long b = time;
        long c = -record;
        double disc = Math.Sqrt(b * b - 4 * a * c);

        // The positive version will be lower because 2a is negative
        return (((-b + disc)/(2*a)), ((-b - disc)/(2*a)));
    }

    public static void Solution(string[] input)
    {
        // Read the cards
        List<(int time, int record)> races = ReadInput(input);

        int product = 1;
        foreach (var (time, record) in races)
        {
            var (lower, upper) = QuadraticFormula(time, record);
            int options = (int)Math.Floor(upper) - (int)Math.Ceiling(lower) + 1;
            product *= options;
        }

        Console.WriteLine($"Part 1: {product}");

        // Join the values
        string joinedTime = "";
        string joinedRecord = "";
        foreach (var (time, record) in races)
        {
            joinedTime += time.ToString();
            joinedRecord += record.ToString();
        }

        long time2 = long.Parse(joinedTime);
        long record2 = long.Parse(joinedRecord);

        var (lower2, upper2) = QuadraticFormula(time2, record2);
        int options2 = (int)Math.Floor(upper2) - (int)Math.Ceiling(lower2) + 1;

        Console.WriteLine($"Part 2: {options2}");
    }
}