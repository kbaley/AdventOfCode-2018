using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");

            var points = inputs.Select(i => new Point(int.Parse(i.Split(", ")[0]), int.Parse(i.Split(", ")[1]))).ToList();
            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);
            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);
            var grid = new Point[maxX + 100, maxY+100];
            for (int i = 0; i < maxX + 50; i++)
            {
                for (int ij = 0; ij < maxY + 50; ij++)
                {
                    grid[i, ij] = FindClosestPoint(i, ij, points);    
                }
            }

            var bounds = new List<Point>();
            for (int i = 0; i < maxX + 50; i++)
            {
                bounds.Add(grid[i, 0]);
                bounds.Add(grid[i, maxY]);
            }

            for (int i = 0; i < maxY + 50; i++)
            {
                bounds.Add(grid[0, i]);
                bounds.Add(grid[maxX, i]);
            }

            var boundedPoints = points.Where(p => !bounds.Contains(p));

            var listOfPoints = new List<Point>();
            for (int i = minX; i <= maxX; i++)
            {
                for (int ij = minY; ij <= maxY; ij++)
                {
                    listOfPoints.Add(grid[i,ij]);    
                }    
            }
            var resultCounts = listOfPoints
                .GroupBy(p => p)
                .Where(p => boundedPoints.Contains(p.Key));
            
            var result = resultCounts.Select(p => p.Count()).OrderBy(p => p).Last();
            Console.WriteLine($":::PART 1::: {result}");

            // Attempt 1 before I realized I messed up the algorithm for determining the bounded points
            // Works but slow

            // var dict = new Dictionary<Point, int>();
            // for (int x = minX; x <= maxX; x++)
            // {
            //     for (int y = minY; y <= maxY; y++)
            //     {
            //         var closestPoint = FindClosestPoint(x, y, points);                    
            //         if (closestPoint != default(Point)) {
            //             var boundedPoint = boundedPoints.SingleOrDefault(p => p == closestPoint);
            //             if (boundedPoint != default(Point)) {
            //                 if (!dict.Keys.Contains(boundedPoint)) {
            //                     dict.Add(boundedPoint, 0);
            //                 }
            //                 dict[boundedPoint]++;
            //             }
            //         }
            //     }
            // }

            // foreach (var key in dict.Keys)
            // {
            //     Console.WriteLine($"{key.X},{key.Y}:{dict[key]}");   
            // }

            // Console.WriteLine($":::PART 1::: {dict.Values.Max()}");

            // PART 2
            const int THRESHOLD = 10000;
            var count = 0;

            for (int i = 0; i < maxX; i++)
            {
                for (int ij = 0; ij < maxY; ij++)
                {
                    var sum = points.Select(p => Distance(i, ij, p)).Sum();
                    if (sum < THRESHOLD) {
                        count++;
                    }
                }
            }

            System.Console.WriteLine("::: PART 2 ::: " + count);
            Console.WriteLine("Done");
        }

        public static Point FindClosestPoint(int x, int y, List<Point> points) {
            var min = points.Select(p => Distance(x, y, p)).Min();
            var closest = points.Where(p => Distance(x, y, p) == min);
            if (closest.Count() > 1) return default(Point);
            return closest.ElementAt(0);
        }

        public static int Distance(int x, int y, Point point) {
            return Math.Abs(x - point.X) + Math.Abs(y - point.Y);
        }
    }
}
