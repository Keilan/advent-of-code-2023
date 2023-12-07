using System.Diagnostics;
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
            if (args.Length < 1)
            {
                Console.WriteLine("Must pass at least 1 argument!");
                return;
            }
            string day = args[0];
            bool useTestData = args.Length >= 2 && args[1] == "test";

            // Make sure the class for that day exists
            Type? dayType = Type.GetType($"AdventOfCode2023.Days.Day{day}")
                ?? throw new Exception($"No solution exists for day {day}!");

            // Read input
            string inputFile = useTestData ? "test.txt" : "input.txt";
            string inputFilepath = $"Days/Day{day}/{inputFile}";
            string[] input = File.ReadAllLines(inputFilepath);

            // Find the solution method
            MethodInfo solutionMethod = dayType.GetMethod("Solution")
                ?? throw new Exception("Solution method not found!");

            // Call with Timer
            var stopwatch = Stopwatch.StartNew();
            solutionMethod.Invoke(null, new object[] { input });
            double elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("Elapsed Time: " + (elapsed / 1000).ToString("0.000"));
        }
    }
}