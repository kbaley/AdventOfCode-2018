using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            const int NUM_ITERATIONS = 10_000;
            const long NUM_MINUTES = 1_000_000_000;

            var grid = inputs.ToGrid();
            var lastSums = new List<int>();
            var lastIndex = 0;


            for ( var i = 0; i < NUM_ITERATIONS; i++) {
                var snapshot = (char[,])grid.Clone();
                for ( var y = 0; y < grid.GetLength(1); y++) {
                    for ( var x = 0; x < grid.GetLength(0); x++ ) {
                        grid[x,y] = ProcessSquare(snapshot, x, y);
                    }
                }

                if (i == 9) System.Console.WriteLine("PART ONE: " + GetValue(grid));
                if ( i % 100 == 99 && i >= 1000) {
                    var val = GetValue(grid);
                    if (!lastSums.Contains(val)) {
                        lastSums.Add(val);
                    } else {
                        lastSums.Add(val);
                        lastIndex = i;
                        break;
                    }
                }
            }

            // Find the looping pattern
            var lastSum = lastSums.Last();
            var firstIndex = lastSums.IndexOf(lastSum);
            lastSums = lastSums.Skip(firstIndex).ToList();
            lastSums.Remove(lastSums.First());

            // Figure out which one will be hit
            var numIterations = (NUM_MINUTES - lastIndex) / 100 - 1;
            var numMoves = numIterations % lastSums.Count;
            System.Console.WriteLine("PART TWO: " + lastSums.ElementAt((int)numMoves));

            Console.WriteLine("Done");
        }

        static int GetValue(char[,] grid) {

            var trees = 0;
            var lumber = 0;
            for ( var y = 0; y < grid.GetLength(1); y++) {
                for ( var x = 0; x < grid.GetLength(0); x++ ) {
                    trees += grid[x,y] == '|' ? 1 : 0;
                    lumber += grid[x,y] == '#' ? 1 : 0;
                }
            }
            return trees * lumber;
        }

        static char ProcessSquare(char[,] grid, int x, int y) {
            switch (grid[x,y]) {
                case '.':
                    return grid.AdjacentSquares(x, y).Count(s => s == '|') >= 3 ? '|' : '.';
                case '|':
                    return grid.AdjacentSquares(x, y).Count(s => s == '#') >= 3 ? '#' : '|';
                case '#':
                    var adjacents = grid.AdjacentSquares(x, y);
                    return adjacents.Any(s => s == '|') && adjacents.Any(s => s == '#') ? '#' : '.';
            }

            throw new Exception("Do not come here");
        }
    }

    public static class Extensions {
        public static char[,] ToGrid(this string[] input) {
            var grid = new char[input[0].Length, input.Length];
            for (var y = 0; y < input.Length; y++) {
                for (var x = 0; x < input[0].Length; x++) {
                    grid[x,y] = input[y][x];
                }
            }
            return grid;
        }

        public static void DumpIt(this char[,] grid) {
            for ( var y = 0; y < grid.GetLength(1); y++) {
                for ( var x = 0; x < grid.GetLength(0); x++ ) {
                    Console.Write(grid[x,y]);
                }
                System.Console.WriteLine();
            }
        }

        public static IEnumerable<char> AdjacentSquares(this char[,] grid, int x, int y) {
            for (var n = x - 1; n <= x + 1; n++) {
                if (n >= 0 && n < grid.GetLength(0)) {
                    for ( var m = y - 1; m <= y + 1; m++) {
                        if (m >= 0 && m < grid.GetLength(1) && !(n == x && m == y)) {
                            yield return grid[n,m];
                        }
                    }
                }
            }
        }
    }
}
