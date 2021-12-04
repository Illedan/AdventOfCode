using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AOC
{
    class Program
    {
        private static string InputLocation = "/Users/erikkvanli/Repos/AdventOfCode/Inputs";
        private static string[] Lines;
        static void Main(string[] args)
        {
            Solve4();
        }

        static void Solve4()
        {
            Setup(4);
            var boards = new List<int[][]>();
            var drops = Lines[0].Split(',').Select(int.Parse).ToList();
            for(var i = 2; i < Lines.Length; i+=6)
            {
                var next = new int[5][];
                boards.Add(next);
                for (var j = 0; j < 5; j++)
                {
                    next[j] = Lines[i + j].Split().Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToArray();
                }
            }

            void Drop(int[][] board, int piece)
            {
                for(var i = 0; i < 5; i++)
                {
                    for(var j = 0; j < 5; j++)
                    {
                        if (board[i][j] == piece) board[i][j] = -1;
                    }
                }
            }

            bool IsWonDir(int[][] board, int x, int y, int dx, int dy)
            {
                for(var i = 0; i < 5; i++)
                {
                    if (board[x + dx * i][y + dy * i] != -1) return false;
                }

                return true;
            }

            bool IsWon(int[][] board)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (IsWonDir(board, i, 0, 0, 1)) return true;
                    if (IsWonDir(board, 0, i, 1, 0)) return true;
                }

                return false;
            }

            int CountNumbers(int[][] board)
            {
                var sum = 0;
                for (var i = 0; i < 5; i++)
                {
                    for (var j = 0; j < 5; j++)
                    {
                        if (board[i][j] != -1) sum += board[i][j];
                    }
                }
                return sum;
            }

            var dropIndex = 0;
            while(boards.All(b => !IsWon(b)))
            {
                foreach (var b in boards)
                {
                    Drop(b, drops[dropIndex]);
                }
                dropIndex++;
            }

            var lastDrop = drops[dropIndex - 1];
            var wonBoard = boards.First(b => IsWon(b));
            Console.WriteLine(CountNumbers(wonBoard) * lastDrop);
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
        static int[] ReadBitsAsInt => Lines.Select(l => Convert.ToInt32(l, 2)).ToArray();

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
