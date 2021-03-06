﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace day15
{
    class Program
    {
        public static bool log = false;
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var grid = new char[inputs[0].Length,inputs.Length];

            var pieces = new List<Piece>();

            for (int y = 0; y < inputs.Length; y++)
            {
                for ( var x = 0; x < inputs[0].Length; x++) {
                    grid[x,y] = inputs[y][x];
                    if (inputs[y][x] == 'G') {
                        pieces.Add(new Piece(new Point(x, y), 200, 'G'));
                    } else if (inputs[y][x] == 'E') {
                        pieces.Add(new Piece(new Point(x, y), 200, 'E'));
                    } else { 
                    }
                }    
            }
            var round = 0;
            while(pieces.Any(p => p.type == 'G') && pieces.Any(p => p.type == 'E')) {
                var sortedPieces = pieces.OrderBy(p => p.SortFactor);
                foreach (var piece in sortedPieces)
                {
                    Log("Start turn: " + piece + " at " + DateTime.Now.ToLongTimeString());
                    if (piece.IsAlive) {
                        var targets = pieces.Where(p => p.type == piece.Enemy && p.IsAlive);
                        if (targets.Count() == 0) {
                            System.Console.WriteLine(round * pieces.Sum(p => p.hp));
                            return;
                        }

                        // If no targets adjacent, move first
                        if (!piece.location.Adjacent().Any( p => targets.Select(t => t.location).Contains(p))) {
                            piece.Move(pieces, grid);
                        }
                        var adjacentSquares = piece.location.Adjacent();
                        var enemyToAttack = targets
                            .Where(t => t.IsAlive && adjacentSquares.Contains(t.location))
                            .OrderBy(t => t.hp)
                            .ThenBy(t => t.SortFactor)
                            .FirstOrDefault();
                        
                        if (enemyToAttack != null) {
                            // System.Console.WriteLine("Attacking: " + enemyToAttack);
                            enemyToAttack.hp -= 3;
                            if (!enemyToAttack.IsAlive) grid[enemyToAttack.location.X, enemyToAttack.location.Y] = '.';
                        }
                    }

                }
                pieces.RemoveAll(p => p.hp <= 0);
                round++;
            }
            System.Console.WriteLine(round);
            System.Console.WriteLine(round * pieces.Sum(p => p.hp));
            Console.WriteLine("Done");
        }

        public static void DumpIt(char[,] grid, IEnumerable<Piece> pieces) {
            for( var y = 0; y < grid.GetLength(1); y++) {
                for (var x = 0; x < grid.GetLength(0); x++) {
                    Console.Write(grid[x,y]);
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine();
            foreach (var item in pieces.OrderBy(p => p.SortFactor))
            {
                System.Console.WriteLine(item);
            }
            System.Console.WriteLine();
        }

        public static void Log(string Message) {
            if (!log) return;
            System.Console.WriteLine(Message);
        }
    }

    public class Piece {
        public Point location { get; set; }
        public int hp { get; set; }
        public char type { get; set; }

        public Piece(Point p, int h, char t)
        {
            location = p;
            hp = h;
            type = t;    
        }

        internal void Move(List<Piece> pieces, char[,] grid)
        {
            var nextMove = GetNextMove(grid);
            if (nextMove != default(Point)) {
                Program.Log("NEXT MOVE: " + nextMove + " : " + DateTime.Now.ToLongTimeString());
                grid[location.X, location.Y] = '.';
                location = nextMove;
                grid[location.X, location.Y] = type;
            }
        }

        internal Point GetNextMove(char[,] grid) {
            var shortestDistance = int.MaxValue;
            var nextMove = default(Point);
            var currentPoint = new Point(location.X, location.Y - 1);
            if (grid[currentPoint.X, currentPoint.Y] == '.') {
                var distance = DistanceToEnemy(currentPoint, grid);
                if (distance < shortestDistance) {
                    nextMove = new Point(currentPoint.X, currentPoint.Y);
                    shortestDistance = distance;
                }
            }

            currentPoint = new Point(location.X - 1, location.Y);
            if (grid[currentPoint.X, currentPoint.Y] == '.') {
                var distance = DistanceToEnemy(currentPoint, grid);
                if (distance < shortestDistance) {
                    nextMove = new Point(currentPoint.X, currentPoint.Y);
                    shortestDistance = distance;
                }
            }

            currentPoint = new Point(location.X + 1, location.Y);
            if (grid[currentPoint.X, currentPoint.Y] == '.') {
                var distance = DistanceToEnemy(currentPoint, grid);
                if (distance < shortestDistance) {
                    nextMove = new Point(currentPoint.X, currentPoint.Y);
                    shortestDistance = distance;
                }
            }

            currentPoint = new Point(location.X, location.Y + 1);
            if (grid[currentPoint.X, currentPoint.Y] == '.') {
                var distance = DistanceToEnemy(currentPoint, grid);
                if (distance < shortestDistance) {
                    nextMove = new Point(currentPoint.X, currentPoint.Y);
                    shortestDistance = distance;
                }
            }

            return nextMove;
        }

        internal int DistanceToEnemy(Point start, char[,] grid) {
            var i = 1;
            var previousVisited = 0;
            var visited = new List<Point>{start};
            var newPoints = new HashSet<Point>{start};
            while (previousVisited < visited.Count && !visited.Any(p => grid[p.X, p.Y]== Enemy)) {
                previousVisited = visited.Count;
                var nextPoints = new HashSet<Point>();
                foreach (var item in newPoints)
                {
                    var adjacents = item.Adjacent().Where(a => !visited.Contains(a) && (grid[a.X, a.Y] == '.' || grid[a.X, a.Y] == Enemy));
                    nextPoints.UnionWith(adjacents);
                }
                visited.AddRange(nextPoints);
                newPoints = nextPoints;
                i++;
            }
            if (visited.Any(p => grid[p.X, p.Y] == Enemy)) {
                return i - 1;
            }
            return int.MaxValue;
        }

        internal char Enemy {
            get {
                if (type == 'E') return 'G';
                return 'E';
            }
        }

        internal bool IsAlive {
            get { return hp > 0;}
        }

        internal int SortFactor {
            get { return location.Y * 1000 + location.X;}
        }

        public override string ToString() {
            return $"location: {location}  type: {type}  hp: {hp}";
        }
    }

    public static class Extensions {
        public static List<Point> Adjacent(this Point p) {
            var points = new List<Point>();
            points.Add(new Point(p.X, p.Y-1));
            points.Add(new Point(p.X-1, p.Y));
            points.Add(new Point(p.X+1, p.Y));
            points.Add(new Point(p.X, p.Y+1));
            return points;
        }
    }
}
