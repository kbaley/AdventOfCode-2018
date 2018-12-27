using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var points = inputs.Select(i => new Point4D(i));
            var constellations = 0;

            var processed = new List<Point4D>();

            while (true) {
                var pointsToCheck = new Queue<Point4D>();
                var pointToCheck = points.FirstOrDefault(p => !processed.Contains(p));
                var tested = new List<Point4D>();
                if (pointToCheck == null) break;
                pointsToCheck.Enqueue(pointToCheck);
                while (pointsToCheck.Count > 0) {
                    var next = pointsToCheck.Dequeue();
                    if (processed.Contains(next)) {
                        continue;
                    }
                    tested.Add(next);
                    processed.Add(next);
                    foreach (var item in points)
                    {
                        if (!tested.Contains(item) && !processed.Contains(item)) {
                            if (item.Distance(next) <= 3) {
                                pointsToCheck.Enqueue(item);
                            }
                        }
                    }
                }
                if (tested.Count() == 0) {
                    break;
                }
                constellations++;
            }

            System.Console.WriteLine(constellations);
            Console.WriteLine("Done");
        }

        private static HashSet<Point4D> CombineConstellations(IEnumerable<Point4D> pointInConst, List<HashSet<Point4D>> constellations)
        {
            var first = pointInConst.First();
            var constellation = first.Constellation;
            foreach (var item in pointInConst)
            {
                if (item.Constellation != first.Constellation) {
                    foreach (var c in item.Constellation)
                    {
                        constellation.Add(c);   
                    }
                    constellations.Remove(item.Constellation);
                }
            }
            return constellation;
        }
    }

    class Point4D {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int T { get; set; }
        public HashSet<Point4D> Constellation { get; set; }
        public bool IsProcessed { get; set; }

        public Point4D(string input) {
            var p = input.Split(',');
            X = int.Parse(p[0]);
            Y = int.Parse(p[1]);
            Z = int.Parse(p[2]);
            T = int.Parse(p[3]);
        }

        public override int GetHashCode() {
            var hash = 352033288;

            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            hash = hash * 23 + Z.GetHashCode();
            hash = hash * 23 + T.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Point4D);
        }

        public bool Equals(Point4D point) {
            return point != null
                && point.X == X
                && point.Y == Y
                && point.Z == Z
                && point.T == T;
        }

        public override string ToString(){
            return $"{X}, {Y}, {Z}, {T}";
        }

        public int Distance(Point4D point) {
            var distance = Math.Abs(X - point.X) + Math.Abs(Y - point.Y) + Math.Abs(Z - point.Z) + Math.Abs(T - point.T);
            return distance;
        } 
    }
}
