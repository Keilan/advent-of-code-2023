using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public static class Day10
{
    static List<List<char>> ReadInput(string[] input)
    {
        List<List<char>> map = [];
        foreach (string line in input)
        {
            map.Add(line.ToCharArray().ToList());
        }
        return map;
    }

    public static (int, int) FindStart(List<List<char>> map)
    {
        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0;  y < map[x].Count; y++)
            {
                if (map[x][y] == 'S')
                {
                    return (x, y);
                }
            }
        }
        throw new Exception("No starting position present.");
    }

    public static void ReplaceStart((int x, int y) start, ref List<List<char>> map)
    {
        // Only one pipe shape will result in 2 adjacent pipes for start
        foreach (char pipe in "F-7|JL")
        {
            List<(int, int)> adjacentPipes = FindAdjacent(pipe, start, map);
            if (adjacentPipes.Count == 2)
            {
                Console.WriteLine("Start is " + pipe.ToString());
                map[start.x][start.y] = pipe;
                return;
            }
        }
        throw new Exception("Unable to determine starting pipe shape.");
    }

    public static List<(int, int)> FindAdjacent(char pipe, (int x, int y) position, List<List<char>> map)
    {
        List<(int, int)> adjacentPipes = [];

        // Check Left
        if ("-7J".Contains(pipe) && position.y != 0 && "-FL".Contains(map[position.x][position.y - 1]))
        {
            adjacentPipes.Add((position.x, position.y - 1));
        }

        // Check Right
        if ("-FL".Contains(pipe) && position.y != map[0].Count - 1 && "-J7".Contains(map[position.x][position.y + 1]))
        {
            adjacentPipes.Add((position.x, position.y + 1));
        }

        // Check Up
        if ("|LJ".Contains(pipe) && position.x != 0 && "|7F".Contains(map[position.x - 1][position.y]))
        {
            adjacentPipes.Add((position.x - 1, position.y));
        }

        // Check Down
        if ("|F7".Contains(pipe) && position.x != map.Count - 1 && "|LJ".Contains(map[position.x + 1][position.y]))
        {
            adjacentPipes.Add((position.x + 1, position.y));
        }

        return adjacentPipes;
    }

    public static int FindMaximumDistance((int x, int y) start, List<List<char>> map, out List<List<int>> distances)
    {
        int maximumDistance = 0;

        // Create map of distances
        distances = [];
        for (int i = 0; i < map.Count; i++)
        {
            List<int> temp = [];
            for (int j = 0; j < map[i].Count; j++)
            {
                temp.Add(-1);
            }
            distances.Add(temp);
        }
        distances[start.x][start.y] = 0;
        
        // Breadth first search
        Queue<(int x, int y)> queue = new();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            (int x, int y) current = queue.Dequeue();
            char pipe = map[current.x][current.y];
            int currentValue = distances[current.x][current.y];

            List<(int, int)> adjacentPositions = FindAdjacent(pipe, current, map);
            foreach ((int x, int y) adjacent in adjacentPositions)
            {
                // Only consider visited locations
                if (distances[adjacent.x][adjacent.y] == -1)
                {
                    if (currentValue + 1 > maximumDistance)
                    {
                        maximumDistance = currentValue + 1;
                    }

                    distances[adjacent.x][adjacent.y] = currentValue + 1;
                    queue.Enqueue(adjacent);
                }
            }
        }

        return maximumDistance;
    }

    public static int CountEnclosedTiles(List<List<char>> map, List<List<int>> distances)
    {
        // Replace junk pipes with ground
        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[x].Count; y++)
            {
                if (distances[x][y] == -1)
                {
                    map[x][y] = '.';
                }
            }
        }
        int count = 0;

        for (int x = 0; x < distances.Count; x++)
        {
            // Replace all edges that should be counted for point in poly with |
            string row = string.Join("", map[x]);
            row = row.Replace("-", "");
            row = row.Replace("F7", "").Replace("LJ", "");
            row = row.Replace("L7", "|").Replace("FJ", "|");
            

            bool inside = false;
            foreach (char c in row)
            {
                if (c == '|')
                {
                    inside = !inside;
                }
                else if (inside)
                {
                    count++;
                }
            }
        }

        return count;
    }


    public static void Solution(string[] input)
    {
        // Read the map
        List<List<char>> map = ReadInput(input);
        (int x, int y) start = FindStart(map);

        // Replace start with correct symbol
        ReplaceStart(start, ref map);

        // Do part 1, store distances for later
        List<List<int>> distances;
        int maximumDistance = FindMaximumDistance(start, map, out distances);
        Console.WriteLine($"Part 1: {maximumDistance}");

        int enclosedCount = CountEnclosedTiles(map, distances);
        Console.WriteLine($"Part 2: {enclosedCount}");
    }
}