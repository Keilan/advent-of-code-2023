using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace AdventOfCode2023.Days;

public static class Day16
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Tile
    {
        public char Symbol { get; set; }
        bool Up = false;
        bool Down = false;
        bool Left = false;
        bool Right = false;

        public Tile(char symbol)
        {
            this.Symbol = symbol;
        }

        public void Clear()
        {
            Up = false;
            Down = false;
            Left = false;
            Right = false;
        }

        public void Energize(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    Up = true; break;
                case Direction.Down:
                    Down = true; break;
                case Direction.Left:
                    Left = true; break;
                default: // Right
                    Right = true; break;
            }
        }

        public bool IsEnergizedInDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Up;
                case Direction.Down:
                    return Down;
                case Direction.Left:
                    return Left;
                default: // Right
                    return Right;
            }
        }

        public bool IsEnergized()
        {
            return Up || Down || Left || Right;
        }

        public int CountDirections()
        {
            return (Up ? 1 : 0)
                + (Down ? 1 : 0)
                + (Left ? 1 : 0)
                + (Right ? 1 : 0);
        }
    }

    static List<List<Tile>> ReadInput(string[] input)
    {
        List<List<Tile>> result = [];
        foreach (string line in input)
        {
            List<Tile> row = [];
            foreach (char symbol in line)
            {
                row.Add(new Tile(symbol));
            }
            result.Add(row);
        }
        return result;
    }

    static (int x, int y) GetNext((int x, int y) pos, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return (pos.x-1, pos.y);
            case Direction.Down:
                return (pos.x+1, pos.y);
            case Direction.Left:
                return (pos.x, pos.y-1);
            default: // Right
                return (pos.x, pos.y+1);
        }
    }

    static void SimulateBeam((int x, int y) startPos, Direction startDirection, ref List<List<Tile>> map)
    {
        // Use a queue for breadth first evaluation of tiles
        Queue<((int x, int y), Direction dir)> queue = [];
        queue.Enqueue((startPos, startDirection));

        while (queue.Count > 0)
        {
            ((int x, int y) pos, Direction dir) = queue.Dequeue();

            // Exit if we've hit a wall
            if (pos.x < 0 || pos.y < 0 || pos.x == map.Count || pos.y == map[0].Count)
            {
                continue;
            }

            Tile tile = map[pos.x][pos.y];
            // Exit if we've already done this
            if (tile.IsEnergizedInDirection(dir))
            {
                continue;
            }

            tile.Energize(dir);

            if (tile.Symbol == '.')
            {
                (int, int) next = GetNext(pos, dir);
                queue.Enqueue((next, dir));
            }
            else if (tile.Symbol == '|')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue((GetNext(pos, Direction.Up), Direction.Up));
                        break;
                    case Direction.Down:
                        queue.Enqueue((GetNext(pos, Direction.Down), Direction.Down));
                        break;
                    case Direction.Left:
                        queue.Enqueue((GetNext(pos, Direction.Up), Direction.Up));
                        queue.Enqueue((GetNext(pos, Direction.Down), Direction.Down));
                        break;
                    default: // Right
                        queue.Enqueue((GetNext(pos, Direction.Up), Direction.Up));
                        queue.Enqueue((GetNext(pos, Direction.Down), Direction.Down));
                        break;
                }
            }
            else if (tile.Symbol == '-')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue((GetNext(pos, Direction.Left), Direction.Left));
                        queue.Enqueue((GetNext(pos, Direction.Right), Direction.Right));
                        break;
                    case Direction.Down:
                        queue.Enqueue((GetNext(pos, Direction.Left), Direction.Left));
                        queue.Enqueue((GetNext(pos, Direction.Right), Direction.Right));
                        break;
                    case Direction.Left:
                        queue.Enqueue((GetNext(pos, Direction.Left), Direction.Left));
                        break;
                    default: // Right
                        queue.Enqueue((GetNext(pos, Direction.Right), Direction.Right));
                        break;
                }
            }
            else if (tile.Symbol == '/')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue((GetNext(pos, Direction.Right), Direction.Right));
                        break;
                    case Direction.Down:
                        queue.Enqueue((GetNext(pos, Direction.Left), Direction.Left));
                        break;
                    case Direction.Left:
                        queue.Enqueue((GetNext(pos, Direction.Down), Direction.Down));
                        break;
                    default: // Right
                        queue.Enqueue((GetNext(pos, Direction.Up), Direction.Up));
                        break;
                }
            }
            else if (tile.Symbol == '\\')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue((GetNext(pos, Direction.Left), Direction.Left));
                        break;
                    case Direction.Down:
                        queue.Enqueue((GetNext(pos, Direction.Right), Direction.Right));
                        break;
                    case Direction.Left:
                        queue.Enqueue((GetNext(pos, Direction.Up), Direction.Up));
                        break;
                    default: // Right
                        queue.Enqueue((GetNext(pos, Direction.Down), Direction.Down));
                        break;
                }
            }
        }
    }

    public static void ClearMap(ref List<List<Tile>> map)
    {
        foreach (var list in map)
        {
            foreach (Tile tile in list)
            {
                tile.Clear();
            }
        }
    }

    public static int CountEnergized(ref List<List<Tile>> map)
    {
        int energized = 0;
        foreach (var list in map)
        {
            foreach (Tile tile in list)
            {
                energized += tile.IsEnergized() ? 1 : 0;
            }
        }
        return energized;
    }

    public static void Solution(string[] input)
    {
        // Read the map
        List<List<Tile>> map = ReadInput(input);

        // Start simulation
        SimulateBeam((0, 0), Direction.Right, ref map);

        // Count energized
        int energized = CountEnergized(ref map);
        Console.WriteLine($"Part 1: {energized}");

        ClearMap(ref map);


        // Try each option - remember y is the horizontal axis
        int maxEnergized = 0;

        // Handle vertical axis going left and right
        for(int x = 0; x < map.Count; x++)
        {
            SimulateBeam((x, 0), Direction.Right, ref map);
            maxEnergized = Math.Max(maxEnergized, CountEnergized(ref map));
            ClearMap(ref map);

            SimulateBeam((x, map[0].Count - 1), Direction.Left, ref map);
            maxEnergized = Math.Max(maxEnergized, CountEnergized(ref map));
            ClearMap(ref map);
        }

        // Vertical axis up and down
        for (int y = 0; y < map[0].Count; y++)
        {
            SimulateBeam((0, y), Direction.Down, ref map);
            maxEnergized = Math.Max(maxEnergized, CountEnergized(ref map));
            ClearMap(ref map);

            SimulateBeam((map.Count - 1, y), Direction.Left, ref map);
            maxEnergized = Math.Max(maxEnergized, CountEnergized(ref map));
            ClearMap(ref map);
        }
        
        Console.WriteLine($"Part 2: {maxEnergized}");
    }
}