using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day2_b
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            for (var i = 0; i < inputs.Length - 1; i++)
            {
                for (int j = i + 1; j < inputs.Length; j++)
                {
                    if (inputs[i].IsOffByOne(inputs[j])) {
                        Console.WriteLine(inputs[i]);
                        Console.WriteLine(inputs[j]);
                    }
                }    
            }
            Console.ReadLine();
        }
    }

    public static class StringExtensions
    {
        public static bool IsOffByOne(this string input, string compare) {
            var count = 0;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] != compare[i]) count++;
                if (count > 1) return false;
            }
            return count == 1;
        }
    }
}
