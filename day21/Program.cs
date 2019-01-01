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
            var i = 0;
            work.Before = registers;
            var it = 0;
            while (true)
            {
                it++;
                var iteration = 0; ;
                work.Before[0] = i++;
                while (true && iteration++ < 100000)
                {
                    var instruction = instructions.ElementAt((int)ipVal);
                    work.Before[IP_REG] = ipVal;
                    work.Instructions = instruction.values;
                    // work.Before = work.Apply(operations[instruction.operation]);
                    work.Before = work.DoOperation(ipVal); 
                    System.Console.WriteLine("Iteration: " + it + " :: " + string.Join(' ', work.Before) + "  : " + ipVal + "  : " + instruction.operation + " -- " + string.Join(',', instruction.values) + " || " + i);
                    ipVal = work.Before[IP_REG] + 1;

                    if (ipVal >= instructions.Count)
                    {
                        System.Console.WriteLine(work.Before[0]);
                        break;
                    }
                    // if (ipVal == 28)
                        // System.Console.WriteLine("Iteration: " + it + " :: " + string.Join(' ', work.Before) + "  : " + ipVal + "  : " + instruction.operation + " -- " + string.Join(',', instruction.values) + " || " + i);

                }
            }

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

        public int[] DoOperation(int i)
        {
            var clone = (int[])Before.Clone();
            switch (i)
            {
                case 0:
                    clone[3] = 123;
                    break;
                case 1:
                    clone[3] = clone[3] & 456;
                    break;
                case 2:
                    clone[3] = clone[3] == 72 ? 1 : 0;
                    break;
                case 3:
                    clone[4] += clone[3];
                    break;
                case 4:
                    clone[4] = 0;
                    break;
                case 5:
                    clone[3] = 0;
                    break;
                case 6:
                    clone[2] = clone[3] | 65536;
                    break;
                case 7:
                    clone[3] = 7637914;
                    break;
                case 8:
                    clone[1] = clone[2] & 255;
                    break;
                case 9:
                    clone[3] += clone[1];
                    break;
                case 10:
                    clone[3] = clone[3] & 16777215;
                    break;
                case 11:
                    clone[3] *= 65899;
                    break;
                case 12:
                    clone[3] = clone[3] & 16777215;
                    break;
                case 13:
                    clone[1] = 256 > clone[2] ? 1 : 0;
                    break;
                case 14:
                    clone[4] += clone[1];
                    break;
                case 15:
                    clone[4]++;
                    break;
                case 16:
                    clone[4] = 27;
                    break;
                case 17:
                    clone[1] = 0;
                    break;
                case 18:
                    clone[5] = clone[1] + 1;
                    break;
                case 19:
                    clone[5] *= 256;
                    break;
                case 20:
                    clone[5] = clone[5] > clone[2] ? 1 : 0;
                    break;
                case 21:
                    clone[4] += clone[5];
                    break;
                case 22:
                    clone[4]++;
                    break;
                case 23:
                    clone[4] = 25;
                    break;
                case 24:
                    clone[1]++;
                    break;
                case 25:
                    clone[4] = 17;
                    break;
                case 26:
                    clone[2] = clone[1];
                    break;
                case 27:
                    clone[4] = 7;
                    break;
                case 28:
                    clone[1] = clone[3] == clone[0] ? 1 : 0;
                    break;
                case 29:
                    clone[4] += clone[1];
                    break;
                case 30:
                    clone[4] = 5;
                    break;
            }
            return clone;
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
