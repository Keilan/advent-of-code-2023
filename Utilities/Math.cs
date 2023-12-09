using System;

namespace AdventOfCode2023.Utilities
{
    public static class MathUtils
    {
        /// <summary>
        /// Computes the greatest common divisor using the Euclidean algorithm.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return System.Math.Abs(a);
        }

        public static long GreatestCommonDivisor(params long[] numbers)
        {
            long gcd = GreatestCommonDivisor(numbers[0], numbers[1]);
            for (int i = 2; i < numbers.Length; i++)
            {
                gcd = GreatestCommonDivisor(gcd, numbers[i]);
            }
            return gcd;
        }

        public static long LeastCommonMultiple(long a, long b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        public static long LeastCommonMultiple(params long[] numbers)
        {
            long lcm = LeastCommonMultiple(numbers[0], numbers[1]);
            for (int i = 2; i < numbers.Length; i++)
            {
                lcm = LeastCommonMultiple(lcm, numbers[i]);
            }
            return lcm;
        }
    }
}
