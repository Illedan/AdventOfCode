using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AOC
{
    class Program
    {
        private static Random rnd = new Random();
        private static string InputLocation = "/Users/erikkvanli/Repos/AdventOfCode/Inputs";
        private static string[] Lines;
        static void Main(string[] args)
        {
            Solve8();
        }

        static void Solve10()
        {
            Setup(10);
            var dict = new Dictionary<char, char>
            {
                {'(',')'},
                {'{','}'},
                {'[',']'},
                {'<','>'}
            };
            var points = new Dictionary<char, int>
            {
                {')',1},
                {']',2},
                {'}',3},
                {'>',4}
            };

            bool IsValid(string expression, out List<char> ending)
            {
                var b = ending = new List<char>();
                foreach (var c in expression)
                {
                    if (!"(){}[]<>".Contains(c))
                    {
                        continue;
                    }
                    if (dict.ContainsKey(c))
                    {
                        b.Add(dict[c]);
                    }
                    else if (b.Count > 0 && b[b.Count - 1] == c)
                    {
                        b.RemoveAt(b.Count - 1);
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            var score = new List<long>();
            foreach(var expression in Lines)
            {
                if (!IsValid(expression, out var ending)) continue;
                var localScore = 0L;
                ending.Reverse();
                foreach (var c in ending)
                {
                    localScore *= 5;
                    localScore += points[c];
                }
                score.Add(localScore);
            }

            Console.WriteLine(score.OrderBy(s => s).ToList()[score.Count/2]);
        }

        static void Solve9()
        {
            Setup(9);

            var board = new Board(Lines[0].Length, Lines.Length);
            var counter = 1;
            var basins = new int[board.Width, board.Height];
            for(var x = 0; x < board.Width; x++)
            {
                for(var y = 0; y < board.Height; y++)
                {
                    board.Cells[x, y] = (int)(Lines[y][x] - '0');
                }
            }

            bool IsLowPoint(int x, int y)
            {
                var value = board.Cells[x, y];
                foreach(var adjacent in Vector.Directions)
                {
                    var x2 = x + adjacent.X;
                    var y2 = y + adjacent.Y;
                    if (!board.OnBoard(x2, y2)) continue;
                    if (board.Cells[x2, y2] <= value) return false;
                }

                return true;
            }

            void UpdateAll(int newValue, int prevValue)
            {
                for (var x = 0; x < board.Width; x++)
                {
                    for (var y = 0; y < board.Height; y++)
                    {
                        if (basins[x, y] == prevValue) basins[x, y] = newValue;
                    }
                }
            }

            void FloodFill(int x, int y)
            {
                if (board.Cells[x, y] == 9) return;
                if (basins[x, y] == 0) basins[x, y] = counter++;
                var value = board.Cells[x, y];
                var basinValue = basins[x, y];
                foreach (var adjacent in Vector.Directions)
                {
                    var x2 = x + adjacent.X;
                    var y2 = y + adjacent.Y;
                    if (!board.OnBoard(x2, y2)) continue;
                    if (board.Cells[x2, y2] > value) continue;
                    if (board.Cells[x2, y2] == 9) continue;
                    if (basins[x2, y2] != 0) UpdateAll(basinValue, basins[x2, y2]);
                    else basins[x2, y2] = basinValue;
                }
            }

            List<Vector> CountBasin(int value)
            {

                var vals = new List<Vector>();
                for (var x = 0; x < board.Width; x++)
                {
                    for (var y = 0; y < board.Height; y++)
                    {
                        if (basins[x, y] == value) vals.Add(new Vector(x, y));
                    }
                }

                return vals;
            }

            for (var x = 0; x < board.Width; x++)
            {
                for (var y = 0; y < board.Height; y++)
                {
                    FloodFill(x, y);
                }
            }

            var sizes = new List<List<Vector>>();
            for(var i = 1; i < counter; i++)
            {
                var vects = CountBasin(i);
                sizes.Add(vects);
            }
            var top3 = sizes.OrderByDescending(s => s.Count).Take(3).ToList();
            Console.WriteLine(top3[0].Count*top3[1].Count*top3[2].Count);
        }

        static void Solve8() 
        {
            Setup(8);
            var connections = new List<string>
            {
                "abcefg", "cf", "acdeg", "acdfg", "bcdf", "abdfg", "abdefg", "acf", "abcdefg", "abcdfg"
            };

            var sum = 0;
            foreach (var input in Lines.Select(s => s.Split('|').Select(c => c.Trim()).ToArray()))
            {
                var newNeeded = Enumerable.Range(0, int.MaxValue) // Montecarlo
                    .Select(i => string.Join("", "abcdefg".OrderBy(c => rnd.NextDouble())))
                    .Select(randomized => connections.Select(n => string.Join("", n.Select(c => randomized[c - 'a']).OrderBy(c => c))).ToList())
                    .First(newConnections => input[0].Split().Select(s => string.Join("", s.OrderBy(c => c))).All(newConnections.Contains));

                sum += int.Parse(string.Join("", input[1].Split().Select(s => string.Join("", s.OrderBy(c => c))).Select(s => newNeeded.IndexOf(s))));
            }

            Console.WriteLine(sum); // 1063760
        }

        static void Solve7()
        {
            Setup(7);
            var numbers = Lines[0].Split(',').Select(int.Parse).ToList();
            var min = numbers.Min();
            var max = numbers.Max();
            var needed = new long[max + 1];
            var amount = new long[max + 1];
            foreach(var n in numbers)
            {
                amount[n]++;
            }
            var cost = new int[needed.Length];
            for(var i = 1; i < cost.Length; i++)
            {
                cost[i] = cost[i - 1] + i;
            }
            for(var i = 0; i < needed.Length; i++)
            {
                for(var j = 0; j < amount.Length; j++)
                {
                    needed[i] += cost[Math.Abs(j - i)] * amount[j];
                }
            }

            Console.WriteLine(needed.Min());
        }

        static void Solve6()
        {
            Setup(6);
            var fishes = Lines[0].Split(',').Select(int.Parse).ToArray();
            var counts = new long[9];
            foreach(var f in fishes)
            {
                counts[f]++;
            }

            var next = new long[9];
            for(var i = 0; i < 256; i++)
            {
                for(var j = 1; j < 9; j++)
                {
                    next[j - 1] = counts[j];
                }
                next[8] = counts[0];
                next[6] += counts[0];
                var t = next;
                next = counts;
                counts = t;
            }

            Console.WriteLine(counts.Sum(c => c));
        }

        static void Solve5()
        {
            Setup(5);
            // 576,67 -> 801,67
            var lines = new List<Line>();
            foreach(var l in Lines)
            {
                var splitted = l.Split(" -> ");
                var first = splitted[0].Split(',').Select(int.Parse).ToArray();
                var last = splitted[1].Split(',').Select(int.Parse).ToArray();
                lines.Add(new Line(new Vector(first[0], first[1]), new Vector(last[0], last[1])));
            }
            int GetHash(Vector v)
            {
                return v.X + v.Y * 10000;
            }

            var seen = new int[10000000];
            foreach(var l in lines)
            {
               // if (!l.IsHorizontal()) continue;
                var dx = Clamp(-1, l.V2.X - l.V1.X, 1);
                var dy = Clamp(-1, l.V2.Y - l.V1.Y, 1);
                var current = l.V1;
                while(!current.Equals(l.V2))
                {
                    seen[GetHash(current)]++;
                    current = new Vector(current.X + dx, current.Y + dy);
                }
                seen[GetHash(current)]++;
            }

            Console.WriteLine(seen.Count(s => s > 1));
        }

        static int Clamp(int min, int num, int max)
        {
            if (num < min) return min;
            if (num > max) return max;
            return num;
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
            while(true)
            {
                boards.RemoveAll(b => IsWon(b));
                foreach (var b in boards)
                {
                    Drop(b, drops[dropIndex]);
                }
                dropIndex++;
                if (boards.Count == 1 && IsWon(boards.First())) break;
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
    public class Line
    {
        public Vector V1, V2;
        public Line(Vector v1, Vector v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public bool IsHorizontal() => V1.X == V2.X || V1.Y == V2.Y;
    }

    public class Board
    {
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new int[Width, Height];
        }

        public int Width { get; }
        public int Height { get; }
        public int[,] Cells;

        public bool OnBoard(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
    }

    public struct Vector : IEquatable<Vector>
    {
        public static List<Vector> Directions = new List<Vector>
        {
            new Vector(0,1),
            new Vector(1,0),
            new Vector(0,-1),
            new Vector(-1, 0),
        };
        public int X, Y;
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vector other)
        {
            return other.X == X && other.Y == Y;
        }

        public override string ToString()
        {
            return X + " " + Y;
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
