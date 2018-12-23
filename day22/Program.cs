using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace day22
{
    class Program
    {
        static int DEPTH = 510;
        static int TARGET_X = 10;
        static int TARGET_Y = 10;
        static void Main(string[] args)
        {
            var target = new Point(TARGET_X, TARGET_Y);

            var grid = new (char type, int geoIndex, int erosion, int riskLevel)[DEPTH, DEPTH];

            var riskLevel = 0;
            for (var y = 0; y < DEPTH; y++)
            {
                for (var x = 0; x < DEPTH; x++)
                {
                    var geoIndex = 0;
                    if (x == 0 && y != 0)
                    {
                        geoIndex = 48271 * y;
                    }
                    else if (y == 0 && x != 0)
                    {
                        geoIndex = 16807 * x;
                    }
                    else if (x == TARGET_X && y == TARGET_Y)
                    {
                        geoIndex = 0;
                    }
                    else if (x > 0 && y > 0)
                    {
                        geoIndex = grid[x - 1, y].erosion * grid[x, y - 1].erosion;
                    }
                    var erosion = (geoIndex + DEPTH) % 20183;

                    var type = '.'; // ROCKY
                    if (erosion % 3 == 1) type = '='; // WET
                    if (erosion % 3 == 2) type = '|'; // NARROW
                    if (x == 0 & y == 0) type = '.';
                    if (x == TARGET_X && y == TARGET_Y) type = '.';
                    riskLevel += (x <= TARGET_X && y <= TARGET_Y) ? erosion % 3 : 0;
                    grid[x, y] = (type, geoIndex, erosion, erosion % 3);
                }
            }

            System.Console.WriteLine("PART ONE:: " + riskLevel);
            Console.WriteLine("Done");
        }
    }
}
