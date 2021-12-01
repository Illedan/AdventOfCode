using System;
using System.IO;
using System.Linq;

namespace AOC
{
    class Program
    {
        private static string InputLocation = "/Users/erikkvanli/Repos/AdventOfCode/Inputs";
        private static string[] Lines;
        static void Main(string[] args)
        {
            Solve1();
        }

        static void Solve1()
        {
            Setup(1);
            var input = ReadInts;
            var prev = input[0];
            var inc = 0;
            for(var i = 0; i< input.Length; i++)
            {
                if (input[i] > prev) inc++;
                prev = input[i];
            }
            Console.WriteLine(inc);
        }

        static int[] ReadInts => Lines.Select(int.Parse).ToArray();

        static void Setup(int number)
        {
            Lines = File.ReadAllLines(Path.Combine(InputLocation, number + ".txt"));
        }
    }
}
