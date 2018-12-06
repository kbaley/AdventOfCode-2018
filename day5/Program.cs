using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            var replacements = new List<string>();
            foreach (var c in alphabet)
            {
                replacements.Add(c.ToString() + c.ToString().ToUpper());    
                replacements.Add(c.ToString().ToUpper() + c.ToString());
            }
            foreach (var input in inputs)
            {
                Console.WriteLine(":::Part 1:::" + React(input, replacements).Length);
            }

            // PART 2
            var smallest = int.MaxValue;
            foreach (var input in inputs)
            {
                foreach (var c in alphabet)
                {
                    var str = input.Replace(c.ToString(), "").Replace(c.ToString().ToUpper(), "");
                    str = React(str, replacements);
                    if (str.Length < smallest) smallest = str.Length;
                }
            }
            Console.WriteLine(":::Part 2:::" + smallest);

            Console.WriteLine("Done");
        }

        static string React(string input, List<string> replacements) {

            while (replacements.Any(r => input.Contains(r)))
            {
                replacements.ForEach(r => input = input.Replace(r, ""));
            }    
            return input;
        }
    }
}
