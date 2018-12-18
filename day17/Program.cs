using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace day17
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");

            var clay = new List<Point>();
            var walls = new List<Wall>();
            var floors = new List<Floor>();
            foreach (var input in inputs)
            {
                var pieces = input.Split(", ");
                var x = 0;
                var y = 0;
                if (pieces[0].StartsWith("x=")) {
                    // WALL
                    x = int.Parse(pieces[0].Replace("x=", ""));
                    var range = pieces[1].Replace("y=", "");
                    var start = int.Parse(range.Split("..")[0]);
                    var end = int.Parse(range.Split("..")[1]);
                    walls.Add(new Wall { Position = x, Start = Math.Min(start, end), End = Math.Max(start, end)});
                    for (var i = Math.Min(start, end); i <= Math.Max(start, end); i++) {
                        clay.Add(new Point(x, i));
                    }
                } else {
                    // FLOOR
                    y = int.Parse(pieces[0].Replace("y=", ""));
                    var range = pieces[1].Replace("x=", "");
                    var start = int.Parse(range.Split("..")[0]);
                    var end = int.Parse(range.Split("..")[1]);
                    floors.Add(new Floor { Depth = y, Start = Math.Min(start, end), End = Math.Max(start, end)});
                    for (var i = Math.Min(start, end); i <= Math.Max(start, end); i++) {
                        clay.Add(new Point(i, y));
                    }
                }
            }
            var minX = clay.Select(c => c.X).Min();
            var minY = clay.Select(c => c.Y).Min();
            var maxX = clay.Select(c => c.X).Max();
            var maxY = clay.Select(c => c.Y).Max();

            DumpIt(clay);

            Console.WriteLine("Done");
        }

        static void DumpIt(IEnumerable<Point> clay) {

            var minX = clay.Select(c => c.X).Min();
            var minY = clay.Select(c => c.Y).Min();
            var maxX = clay.Select(c => c.X).Max();
            var maxY = clay.Select(c => c.Y).Max();
            for ( var i = 0; i <= maxY; i++) {
                for (var j = minX - 1; j <= maxX + 1; j++ ) {
                   if (clay.Contains(new Point(j, i))) Console.Write("#");
                   else if (i == 0 && j == 500) Console.Write("+");
                   else Console.Write(".");
                }
                System.Console.WriteLine();
            }
        }
    }

    public class Floor {
        public int Depth { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }

    public class Wall {
        public int Position { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
