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

            var walls = new List<Wall>();
            var floors = new List<Floor>();

            foreach (var input in inputs)
            {
                if (input.StartsWith("x="))
                    walls.Add(new Wall(input));
                else
                    floors.Add(new Floor(input));
            }

            var minWall = walls.Select(w => w.X).Union(floors.Select(f => f.StartX)).Min();
            var maxWall = walls.Select(w => w.X).Union(floors.Select(f => f.EndX)).Max();
            var minFloor = floors.Select(w => w.Y).Union(walls.Select(w => w.StartY)).Min();
            var maxFloor = floors.Select(w => w.Y).Union(walls.Select(w => w.EndY)).Max();
            var xCorrection = minWall - 1;
            var yCorrection = minFloor;
            var grid = new Material[maxWall - xCorrection + 2, maxFloor - yCorrection + 1];
            for (var x = 0; x < maxWall - xCorrection + 2; x++) {
                for (var y = 0; y <= maxFloor - yCorrection; y++) {
                    grid[x,y] = Material.SAND;
                }
            }
            foreach (var wall in walls)
            {
                for ( var y = wall.StartY; y <= wall.EndY; y++ ) {
                    // System.Console.WriteLine(y + ":"  + yCorrection);
                    grid[wall.X - xCorrection, y - yCorrection] = Material.CLAY;
                }
            }

            foreach (var floor in floors)
            {
                for ( var x = floor.StartX; x <= floor.EndX; x++ ) {
                    grid[x - xCorrection, floor.Y - yCorrection] = Material.CLAY;
                }
            }

            grid[500 - xCorrection, 0] = Material.FALLING_WATER;

            var stop = 0;
            while (stop != VolumeOfWater(grid)) {
                stop = VolumeOfWater(grid);
                for (var y = 0; y < grid.GetLength(1); y++) {
                    for (var x = 0; x < grid.GetLength(0); x++) {
                        if (grid[x,y] == Material.FALLING_WATER) {
                            ProcessFallingWater(grid, x, y);
                        }
                    }
                }                
            }

            // DumpIt(grid);
            System.Console.WriteLine("Min floor: " + minFloor);
            System.Console.WriteLine("Part One: " + stop);

            var count = 0;
            for ( var y = 0; y < grid.GetLength(1); y++) {
                for ( var x = 0; x < grid.GetLength(0); x++) {
                    if (grid[x,y] == Material.STANDING_WATER) count++;
                }
            }
            System.Console.WriteLine("PART TWO: " + count);
            Console.WriteLine("Done");
        }

        public static void ProcessFallingWater(Material[,] grid, int x, int y) {
            if (y + 1 == grid.GetLength(1)) return;
            // Falling water below
            if (grid[x,y+1] == Material.FALLING_WATER) return;

            // Sand below
            if (grid[x,y+1] == Material.SAND) {
                grid[x,y+1] = Material.FALLING_WATER;
                return;
            }

            // Standing water or clay below - spread out
            SpreadOut(grid, x, y);

        }

        public static void SpreadOut(Material[,] grid, int x, int y) {
            // Spread outward until you hit a wall or until there's sand below you
            var n = x + 1;
            var hitWallRight = false;
            var hitWallLeft = false;
            while (true) {
                if (grid[n,y] == Material.CLAY) {
                    hitWallRight = true;
                    break;
                } else if (grid[n,y+1] == Material.FALLING_WATER) {
                    break;
                } else if (grid[n, y+1] == Material.SAND) {
                    grid[n,y] = Material.FALLING_WATER;
                    break;
                }
                grid[n,y] = Material.FALLING_WATER;
                n++;
            }
            var m = x - 1;
            while (true) {
                if (grid[m,y] == Material.CLAY) {
                    hitWallLeft = true;
                    break;
                } else if (grid[m,y+1] == Material.FALLING_WATER) {
                    break;
                } else if (grid[m, y+1] == Material.SAND) {
                    grid[m,y] = Material.FALLING_WATER;
                    break;
                }
                grid[m,y] = Material.FALLING_WATER;
                m--;
            }
            if (hitWallLeft && hitWallRight) {
                for (var i = m + 1; i < n; i++) {
                    grid[i,y] = Material.STANDING_WATER;
                }
                return;
            }

            // Otherwise do nothing. The falling water will get processed the next round
        }

        public static int VolumeOfWater(Material[,] grid) {
            var count = 0;
            for ( var y = 0; y < grid.GetLength(1); y++) {
                for ( var x = 0; x < grid.GetLength(0); x++) {
                    if (grid[x,y] == Material.STANDING_WATER || grid[x,y] == Material.FALLING_WATER) count++;
                }
            }
            return count;
        }
        public static void DumpIt(Material[,] grid) {
            for ( var y = 0; y < grid.GetLength(1); y++) {
                for ( var x = 0; x < grid.GetLength(0); x++) {
                    Console.Write((char)grid[x,y]);
                }
                System.Console.WriteLine();
            }
        }
    }

    public enum Material {
        STANDING_WATER = '~',
        CLAY = '#',
        SAND = '.',
        FALLING_WATER = '|'
    }

    class Wall {
        public int X { get; set; }
        public int StartY { get; set; }
        public int EndY { get; set; }

        public override string ToString() {
            return $"x={X}, y={StartY}..{EndY}";
        }

        public Wall(string input)
        {
            X = int.Parse(input.Split(", ")[0].Replace("x=", ""));
            var y = input.Split("y=")[1];
            StartY = int.Parse(y.Split("..")[0]);
            EndY = int.Parse(y.Split("..")[1]);
        }
    }

    class Floor {
        public int StartX { get; set; }
        public int EndX { get; set; }
        public int Y { get; set; }

        public Floor(string input)
        {
            Y = int.Parse(input.Split(", ")[0].Replace("y=", ""));
            var x = input.Split("x=")[1];
            StartX = int.Parse(x.Split("..")[0]);
            EndX = int.Parse(x.Split("..")[1]);
        }

        public override string ToString() {
            return $"y={Y}, x={StartX}..{EndX}";
        }
    }
}
