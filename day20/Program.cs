using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var input = inputs[0];

            var length = GetLength(input).Max();
            System.Console.WriteLine("PART ONE:::" + length);

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
                        break;
                    case '|':
                        lengths.Add(length);
                        length = 0;
                        break;
                    case '(':
                        var section = GetSection(s, pos);
                        var childLengths = GetLength(section);
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
            return lengths;
        }
    }
}
