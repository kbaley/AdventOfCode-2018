using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");

            var points = inputs.Select(i => new Point3D(i));
            var highestRange = points.Max(p => p.SignalRadius);
            var point = points.First(p => p.SignalRadius == highestRange);
            var inRange = points.Count(p => point.DistanceFrom(p) <= highestRange);

            System.Console.WriteLine(inRange);
            Console.WriteLine("Done");
        }
    }

    public class Point3D {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }
        public long SignalRadius { get; set; }

        public Point3D(string input)
        {
            SignalRadius = long.Parse(input.Split("r=")[1]);
            var coords = input.Split("<")[1].Split(">")[0].Split(",").Select(x => long.Parse(x)).ToArray();
            X = coords[0];
            Y = coords[1];
            Z = coords[2];
        }

        public long DistanceFrom(Point3D point) {
            return Math.Abs(X - point.X) + Math.Abs(Y - point.Y) + Math.Abs(Z - point.Z);
        }
    }
}
