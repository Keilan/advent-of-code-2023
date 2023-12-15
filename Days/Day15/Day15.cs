using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace AdventOfCode2023.Days;

public static class Day15
{

    static List<string> ReadInput(string[] input)
    {
        return input[0].Split(",").ToList();
    }

    static int GetHash(string s)
    {
        int hash = 0;
        foreach (char c in s)
        {
            hash += (int)c;
            hash *= 17;
            hash %= 256;
        }
        return hash;
    }

    static (string, char, int?) ReadInstruction(string s)
    {
        if (s.IndexOf('-')  != -1)
        {
            s = s.Remove(s.Length - 1);
            return (s, '-', null);
        }
        else
        {
            int focalLength = int.Parse(s.Last().ToString());
            s = s.Remove(s.Length - 2);
            return (s, '=', focalLength);
        }
    }

    public static void Solution(string[] input)
    {
        // Read the map
        List<string> steps = ReadInput(input);

        int sum = 0;
        foreach (string step in steps)
        {
            sum += GetHash(step);
        }
        Console.WriteLine($"Part 1: {sum}");

        // Initialize list of boxes
        List<List<(string label, int focalLength)>> boxes = [];
        for (int i = 0; i < 256; i++)
        {
            boxes.Add(new List<(string label, int focalLength)>());
        }

        // Follow instructions
        foreach (string step in steps)
        {
            (string label, char op, int? focalLength) = ReadInstruction(step);
            int box = GetHash(label);
            
            if (op == '-')
            {
                boxes[box] = boxes[box].Where(lens => lens.label != label).ToList();
            }
            else
            {
                int length = focalLength.Value;
                List<(string label, int focalLength)> currentBox = boxes[box];
                int index = -1;
                for (int i = 0; i < currentBox.Count; i++)
                {
                    if (currentBox[i].label == label)
                    {
                        index = i; break;
                    }
                }

                // If already there, replace it
                if (index != -1)
                {
                    currentBox[index] = (label, length);
                }
                else
                {
                    currentBox.Add((label, length));
                }
            }
        }

        // Sum up lenses
        int focusingPower = 0;
        for (int boxIdx = 0; boxIdx < boxes.Count; boxIdx++){
            for (int lensIdx = 0; lensIdx < boxes[boxIdx].Count; lensIdx++)
            {
                focusingPower += (boxIdx + 1) * (lensIdx + 1) * boxes[boxIdx][lensIdx].focalLength;
            }
        }
        
        Console.WriteLine($"Part 2: {focusingPower}");
    }
}