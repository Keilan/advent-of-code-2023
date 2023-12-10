using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.Days;

public static class Day08
{


    static void ReadInput(string[] input, out List<char> directions, out Dictionary<string, (string left, string right)> map)
    {
        directions = [];
        map = [];
        foreach (string line in input)
        {
            if (string.IsNullOrEmpty(line)) 
            { 
                continue; 
            }


            if (line.Contains(" = "))
            {
                List<string> split = line.Split(" = ").ToList();
                string position = split[0];

                List<string> paths = split[1].Replace("(", "").Replace(")", "").Split(", ").ToList();
                map[position] = (paths[0], paths[1]);

            }

            // Directions
            else
            {
                directions = line.ToCharArray().ToList();
            }
        }
    }

    public static void Solution(string[] input)
    {
        // Read the map
        List<char> directions;
        Dictionary<string, (string left, string right)> map;
        ReadInput(input, out directions, out map);

        int steps = 0;
        string position = "AAA";
        int directionIndex = 0;
        while (position != "ZZZ")
        {
            steps++;
            (string left, string right) paths = map[position];

            if (directions[directionIndex] == 'L')
            {
                position = paths.left;
            }
            else
            {
                position = paths.right;
            }

            directionIndex++;
            directionIndex %= directions.Count;
        }

        Console.WriteLine($"Part 1: {steps}");

        // Get starting nodes
        List<string> starts = map.Keys.Where(k => k.Last() == 'A').ToList();
        List<int> cycleLengths = [];

        foreach (string start in starts) 
        {
            steps = 0;
            position = start;
            directionIndex = 0;
            while (position.Last() != 'Z')
            {
                steps++;
                (string left, string right) paths = map[position];

                if (directions[directionIndex] == 'L')
                {
                    position = paths.left;
                }
                else
                {
                    position = paths.right;
                }

                directionIndex++;
                directionIndex %= directions.Count;
            }
            cycleLengths.Add(steps);
        }

        long lcm = MathUtils.LeastCommonMultiple(cycleLengths.Select(l => (long)l).ToArray());
        Console.WriteLine($"Part 2: {lcm}");
    }
}