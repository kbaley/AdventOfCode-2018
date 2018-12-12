using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace day11
{
    class Program
    {
        static void Main(string[] args)
        {
            const int SERIAL_NUMBER  = 8199;
            (var width, var height) = (300, 300);
            var grid = new long[width, height];

            for(var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    grid[x,y] = PowerLevel(x, y, SERIAL_NUMBER);
                }
            }

            Point maxPoint = default(Point);
            long maxValue = long.MinValue;
            int maxSize = int.MinValue;
            for (var x = 0; x < width; x++ ) {
                for (var y = 0; y < height; y++) {
                    for ( var i = 0; i < GetMaxSquare(x, y); i++ ) {
                        var totalPower = TotalPower(x, y, i, grid);
                        if (totalPower > maxValue) {
                            maxPoint = new Point(x, y);
                            maxValue = totalPower;
                            maxSize = i;
                        }
                    }
                }
            }
            System.Console.WriteLine(maxPoint.ToString());
            System.Console.WriteLine(maxSize);
            Console.WriteLine("Done");
        }

        private static int GetMaxSquare(int x, int y)
        {
            return Math.Min(300 - x + 1, 300 - y + 1);
        }

        private static long TotalPower(int x, int y, int size, long[,] grid)
        {
            if (x + size >= 300) return long.MinValue;
            if (y + size >= 300) return long.MinValue;
            var totalPower = 0L;
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    totalPower += grid[x + i, y + j];
                }
            }
            return totalPower;
        }

        static long PowerLevel(int x, int y, int serial) {
            var rackId = x + 10;
            var powerLevel = (rackId * y + serial) * rackId;
            powerLevel = (int)Math.Abs(powerLevel/100%10) - 5;
            return powerLevel;
        }
    }
}
