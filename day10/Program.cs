using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var pointList = new List<(Point point, Point velocity)>();

            foreach (var input in inputs)
            {
                var data = input
                    .Replace("position=<", "")
                    .Replace("> velocity=<", "~~")
                    .Replace(">", "")
                    .Split("~~");
                pointList.Add((data[0].InputToPoint(), data[1].InputToPoint()));
            }
            var points = pointList.ToArray();

            var lastBounds = long.MaxValue;
            var bounds = Bounds(points.Select(p => p.point));
            var secs = 0;
            while (bounds < lastBounds)
            {
                lastBounds = bounds;
                for (var i = 0; i < points.Length; i++)
                {
                    points[i].point.X += points[i].velocity.X;
                    points[i].point.Y += points[i].velocity.Y;
                }
                bounds = Bounds(points.Select(p => p.point));
                secs++;
            }
            for (var i = 0; i < points.Length; i++)
            {
                    points[i].point.X -= points[i].velocity.X;
                    points[i].point.Y -= points[i].velocity.Y;
            }

            ShowGrid(points.Select(p => p.point));
            System.Console.WriteLine($"{secs - 1} seconds");

            Console.WriteLine("Done");
        }

        static void ShowGrid(IEnumerable<Point> points)
        {

            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);
            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);
            for (var j = minY; j <= maxY; j++)
            {
                for (var i = minX; i <= maxX; i++)
                {
                    Console.Write(points.Any(p => p.X == i && p.Y == j) ? "#" : " ");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
        }

        static long Bounds(IEnumerable<Point> points)
        {
            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);
            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);
            return (long)(maxX - minX) * (long)(maxY - minY);
        }
    }

    public static class Extensions
    {
        public static Point InputToPoint(this string input)
        {
            return new Point(int.Parse(input.Split(',')[0]), int.Parse(input.Split(',')[1]));
        }
    }
}
