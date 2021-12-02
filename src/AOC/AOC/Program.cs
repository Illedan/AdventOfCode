using System;
using System.Collections.Generic;
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
            Solve2();
        }

        static void Solve2()
        {
            Setup(2);
            var input = Lines.Select(s => s.Split()).Select(l => new KeyValue<int>(l[0], int.Parse(l[1]))).ToArray();
            var x = 0;
            var y = 0;
            foreach(var l in input)
            {
                if (l.Key == "forward") x += l.Value;
                else if (l.Key == "up") y -= l.Value;
                else if (l.Key == "down") y += l.Value;
            }
            Console.WriteLine(x + " " + y);
            Console.WriteLine(x * y);
        }

        static void Solve1()
        {
            Setup(1);
            var input = ReadInts;
            var prev = input.Take(3).Sum();
            var inc = 0;
            for(var i = 1; i< input.Length; i++)
            {
                var next = input.Skip(i).Take(3).Sum();
                if (next > prev) inc++;
                prev = next;
            }
            Console.WriteLine(inc);
        }

        static int[] ReadInts => Lines.Select(int.Parse).ToArray();

        static void Setup(int number)
        {
            Lines = File.ReadAllLines(Path.Combine(InputLocation, number + ".txt"));
        }
    }

    public class KeyValue<T>
    {
        public string Key;
        public T Value;
        public KeyValue(string key, T value)
        {
            Key = key;
            Value = value;
        }
    }
}
