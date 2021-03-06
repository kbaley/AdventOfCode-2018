﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day19
{
    class Program
    {
        static void Main(string[] args)
        {

            var inputs = File.ReadAllLines("./input.txt");

            const int IP_REG = 5;
            int ipVal = 0;
            var registers = new int[] { 0, 0, 0, 0, 0, 0 };

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

            var work = new Input{Before = registers};
            var instructions = new List<(string operation, int[] values)>();
            foreach (var input in inputs)
            {
                var op = input.Split(' ')[0];
                var values = input.Substring(5).Split(' ').Select(x => int.Parse(x)).ToArray();
                instructions.Add((op, values));
            }
            // var y = 0;
            // var lastReg0 = work.Before[0];
            // var lastReg4 = work.Before[4];
            while(true) {
                var instruction = instructions.ElementAt((int)ipVal);
                work.Before[IP_REG] = ipVal;
                work.Instructions = instruction.values;
                work.Before = work.Apply(operations[instruction.operation]);
                ipVal = work.Before[IP_REG] + 1;

                if (ipVal >= instructions.Count) {
                    System.Console.WriteLine(work.Before[0]);
                    break;
                }

                // y++;
                // if (work.Before[0] != lastReg0) {
                //     Console.Write("---- ");
                // }
                // if (work.Before[0] != lastReg0 || work.Before[4] != lastReg4) {
                //     System.Console.WriteLine(string.Join(' ', work.Before) + "  : " + ipVal + "  : " + instruction.operation + " -- " + string.Join(',', instruction.values) + " || " + y);
                //     lastReg0 = work.Before[0];
                //     lastReg4 = work.Before[4];
                // }

            }
            
            System.Console.WriteLine("PART TWO: Go to Wolfram and enter: sum(divisors 10551430)");
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
        public static int[] Apply(this Input input, Func<Input, int> operation) {
            var output = (int[])input.Before.Clone();
            output[input.Instructions[2]] = operation(input);
            return output; 
        }

    }
}
