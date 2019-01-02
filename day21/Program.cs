using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day21
{
    class Program
    {
        static void Main(string[] args)
        {

            var inputs = File.ReadAllLines("./input.txt");

            const int IP_REG = 4;
            int ipVal = 0;

            var operations = new Dictionary<string, Func<Input, int>> {
                { "addr", (Input input) => { return input.RegA + input.RegB; }},
                { "addi", (Input input) => { return input.RegA + input.ValB; }},
                { "mulr", (Input input) => { return input.RegA * input.RegB; }},
                { "muli", (Input input) => { return input.RegA * input.ValB; }},
                { "banr", (Input input) => { return input.RegA & input.RegB; }},
                { "bani", (Input input) => { return input.RegA & input.ValB; }},
                { "borr", (Input input) => { return input.RegA | input.RegB; }},
                { "bori", (Input input) => { return input.RegA | input.ValB; }},
                { "setr", (Input input) => { return input.RegA; }},
                { "seti", (Input input) => { return input.ValA; }},
                { "gtir", (Input input) => { return input.ValA > input.RegB ? 1 : 0; }},
                { "gtri", (Input input) => { return input.RegA > input.ValB ? 1 : 0; }},
                { "gtrr", (Input input) => { return input.RegA > input.RegB ? 1 : 0; }},
                { "eqir", (Input input) => { return input.ValA == input.RegB ? 1 : 0; }},
                { "eqri", (Input input) => { return input.RegA == input.ValB ? 1 : 0; }},
                { "eqrr", (Input input) => { return input.RegA == input.RegB ? 1 : 0; }}
            };

            var registers = new int[] { 1, 0, 0, 0, 0, 0 };
            var work = new Input { Before = registers };
            var instructions = new List<(string operation, int[] values)>();
            foreach (var input in inputs)
            {
                var op = input.Split(' ')[0];
                var values = input.Substring(5).Split(' ').Select(x => int.Parse(x)).ToArray();
                instructions.Add((op, values));
            }
            work.Before = registers;
            var visited = new HashSet<int>();
            while (true)
            {
                if (ipVal == 18)
                {
                    work.Before[1] = work.Before[2] / 256;
                    work.Before[5] = 1;
                    ipVal = 26;
                    work.Before[IP_REG] = 26;
                }
                var instruction = instructions.ElementAt((int)ipVal);
                work.Before[IP_REG] = ipVal;
                work.Instructions = instruction.values;
                if (ipVal == 28) {
                    if (!visited.Add(work.Before[3])) {
                        break;
                    }
                }

                work.Before = work.Apply(operations[instruction.operation]);
                ipVal = work.Before[IP_REG] + 1;

                if (ipVal >= instructions.Count)
                {
                    System.Console.WriteLine("Broke");
                    break;
                }
            }

            System.Console.WriteLine("PART ONE: " + visited.First());
            System.Console.WriteLine("PART TWO: " + visited.Last());
            Console.WriteLine("Done");
        }
    }

    public class Input
    {
        public int[] Before { get; set; }
        public int[] Instructions { get; set; }
        public int[] After { get; set; }

        public Input() { }
        public Input(IEnumerable<string> data)
        {
            Before = data.ElementAt(0).Replace("Before: [", "").Replace("]", "").Split(", ").Select(x => int.Parse(x)).ToArray();
            Instructions = data.ElementAt(1).Split(' ').Select(x => int.Parse(x)).ToArray();
            After = data.ElementAt(2).Replace("After:  [", "").Replace("]", "").Split(", ").Select(x => int.Parse(x)).ToArray();
        }

        public int RegA { get { return Before[Instructions[0]]; } }
        public int ValA { get { return Instructions[0]; } }
        public int RegB { get { return Before[Instructions[1]]; } }
        public int ValB { get { return Instructions[1]; } }

        public override string ToString()
        {
            return "Before: [" + string.Join(", ", Before) + "]\n" + string.Join(' ', Instructions) + "\nAfter:  ["
                + string.Join(", ", After) + "]";
        }

    }

    public static class Extensions
    {
        public static int[] Apply(this Input input, Func<Input, int> operation)
        {
            var output = (int[])input.Before.Clone();
            output[input.Instructions[2]] = operation(input);
            return output;
        }

    }
}
