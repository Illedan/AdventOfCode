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
            Solve3();
        }

        static void Solve3()
        {
            Setup(3);
            var bitcount = Lines[0].Length;
            var zero = new int[bitcount];
            var one = new int[bitcount];
            foreach(var l in Lines)
            {
                for(var i = 0; i < bitcount; i++)
                {
                    if (l[i] == '0') zero[i]++;
                    if (l[i] == '1') one[i]++;
                }
            }

            var dominant = "";
            var opposite = "";
            for(var i = 0; i < bitcount; i++)
            {
                if( zero[i] > one[i])
                {

                    dominant += 0;
                    opposite += 1;
                }
                else
                {
                    dominant += 1;
                    opposite += 0;
                }
            }
            var num1 = Convert.ToInt64(dominant, 2);
            var num2 = Convert.ToInt64(opposite,2);
            Console.WriteLine( num1*num2);
        }

        static void Solve2()
        {
            Setup(2);
            var input = Lines.Select(s => s.Split()).Select(l => new KeyValue<int>(l[0], int.Parse(l[1]))).ToArray();
            var x = 0;
            var y = 0;
            var aim = 0;
            foreach(var l in input)
            {
                if (l.Key == "forward")
                {
                    x += l.Value;
                    y += aim * l.Value;
                }
                else if (l.Key == "up") aim -= l.Value;
                else if (l.Key == "down") aim += l.Value;
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
