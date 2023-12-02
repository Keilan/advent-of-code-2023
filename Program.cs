using System;
using System.Reflection;

namespace AdventOfCode2023
{
    class Program
    {
        /// <summary>
        /// The main program, expects a single integer representing the day
        /// and it will return an error if it doesn't exist, or run the
        /// solution for that day.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Make sure we passed in a day argument
            if (args.Length != 1)
            {
                Console.WriteLine("Must pass exactly 1 argument!");
                return;
            }
            string day = args[0];

            // Make sure the class for that day exists
            Type? dayType = Type.GetType($"AdventOfCode2023.Days.Day{day}")
                ?? throw new Exception($"No solution exists for day {day}!");

            // Read input
            string inputFilepath = $"Days/Day{day}/input.txt";
            string[] input = File.ReadAllLines(inputFilepath);

            // Call the solution method
            MethodInfo solutionMethod = dayType.GetMethod("Solution")
                ?? throw new Exception("Solution method not found!");
            solutionMethod.Invoke(null, new object[] { input });
        }
    }
}