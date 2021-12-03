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
            var dominant = Lines.Select(s => s).ToList();
            var least = Lines.Select(s => s).ToList(); 
            var bitcount = Lines[0].Length;
            for(var i = 0; i < bitcount; i++)
            {
                var val = GetMostSignificant(dominant, i, out var one, out var zero);
                var val2 = GetMostSignificant(least, i, out var one2, out var zero2);
                if(dominant.Count > 1)
                dominant.RemoveAll(d => val == -1? d[i]=='0' : d[i] != val + '0');
                if(least.Count > 1)
                least.RemoveAll(d => val2 == -1 ? d[i] == '1' : d[i] == val2 + '0');
            }

            var dominantInt = Convert.ToInt32(dominant[0], 2);
            var leastInt = Convert.ToInt32(least[0], 2);
            Console.WriteLine(dominantInt*leastInt);
        }

        static int GetMostSignificant(List<string> bits, int index, out int one, out int zero)
        {
            one = 0;
            zero = 0;
            foreach(var b in bits)
            {
                if (b[index] == '0') zero++;
                else one++;
            }

            if (one == zero) return -1;
            return one > zero ? 1 : 0;
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
