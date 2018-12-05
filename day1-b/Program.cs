using System;
using System.Collections.Generic;
using System.IO;

namespace day1_b
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var set = new HashSet<int>();
            var value = 0;
            var i = 0;
            while (!set.Contains(value)) {
                set.Add(value);
                value = value + int.Parse(inputs[i++]);
                if (i >= inputs.Length) i = 0;
            }
            Console.WriteLine($"value: {value}");
            Console.ReadLine();
        }
    }
}
