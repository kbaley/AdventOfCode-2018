using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day20
{
    class Program
    {
        private static int numRooms = 0;
        private static int iteration = 0;
        private const int THRESHOLD = 10;
        private List<string> paths = new List<string>();
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var input = inputs[0];

            var length = GetLength(input).Max();
            System.Console.WriteLine("PART ONE:::" + length);
            System.Console.WriteLine("PART TWO:::" + numRooms);

            Console.WriteLine("Done");
        }

        private static string GetSection(string input, int pos) {
            if (input[pos] != '(') return input;
            var start = 1;
            var posStart = pos;
            pos++;
            while (start > 0) {
                if (input[pos] == ')') start--;
                if (input[pos] == '(') start++;
                pos++;
            }

            return input.Substring(posStart, pos - posStart);
        }

        private static IEnumerable<int> GetLength(string input) {
            var lengths = new List<int>();

            var pos = 0;
            iteration++;
            var s = input.Substring(pos + 1, input.Length - pos - 2);
            var length = 0;
            while (pos < s.Length) {
                switch (s[pos])
                {
                    case 'N':
                    case 'E':
                    case 'W':
                    case 'S':
                        length++;
                        if (iteration == 1 && pos >= THRESHOLD) {
                            numRooms++;
                        }
                        break;
                    case '|':
                        lengths.Add(length);
                        length = 0;
                        break;
                    case '(':
                        var section = GetSection(s, pos);
                        var childLengths = GetLength(section);
                        if (childLengths.All(c => c > 0)) {
                            var max = childLengths.Max();
                            var maxSkipped = false;
                            foreach (var item in childLengths)
                            {
                                if (item == max && !maxSkipped) {
                                    maxSkipped = true;
                                } else {
                                    if (length + item >= THRESHOLD) {
                                        numRooms += (length + item - THRESHOLD);
                                    }   
                                }
                            }
                        }
                        length += childLengths.Min() == 0 ? 0 : childLengths.Max();
                        pos += section.Length - 1;
                        break;
                    case ')':
                        lengths.Add(length);
                        return lengths;
                    default:
                        break;
                }
                pos++;
            }
            lengths.Add(length);
            iteration--;
            return lengths;
        }
    }
}
