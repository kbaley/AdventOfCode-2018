using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day2_a
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var twos = 0;
            var threes = 0;
            foreach (var input in inputs)
            {
                if (input.ContainsNDuplicates(2)) twos++;
                if (input.ContainsNDuplicates(3)) threes++;
            }
            Console.WriteLine($"Result: {twos*threes}");
            Console.ReadLine();
        }
    }

    public static class StringExtensions
    {
        public static bool ContainsNDuplicates(this string input, int n) {
            var dict = new Dictionary<char, int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (dict.ContainsKey(input[i])) {
                    dict[input[i]]++;
                } else {
                    dict.Add(input[i], 1);
                }
            }
            return dict.Values.Contains(n);
        }
    }
}
