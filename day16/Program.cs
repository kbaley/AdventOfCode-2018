using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("./input.txt").Take(4).ToArray();
            var inputs = new List<Input>();
            inputs.Add(new Input(lines));
            var i = 1;
            while (lines.Any() && lines[0].StartsWith("Before"))
            {
                lines = File.ReadLines("./input.txt").Skip(4 * i++).Take(4).ToArray();
                if (lines.Any() && lines[0].StartsWith("Before"))
                    inputs.Add(new Input(lines));
            }

            Func<Input, int> addr = (Input input) => { return input.RegA + input.RegB; };
            Func<Input, int> addi = (Input input) => { return input.RegA + input.ValB; };
            Func<Input, int> mulr = (Input input) => { return input.RegA * input.RegB; };
            Func<Input, int> muli = (Input input) => { return input.RegA * input.ValB; };
            Func<Input, int> banr = (Input input) => { return input.RegA & input.RegB; };
            Func<Input, int> bani = (Input input) => { return input.RegA & input.ValB; };
            Func<Input, int> borr = (Input input) => { return input.RegA | input.RegB; };
            Func<Input, int> bori = (Input input) => { return input.RegA | input.ValB; };
            Func<Input, int> setr = (Input input) => { return input.RegA; };
            Func<Input, int> seti = (Input input) => { return input.ValA; };
            Func<Input, int> gtir = (Input input) => { return input.ValA > input.RegB ? 1 : 0; };
            Func<Input, int> gtri = (Input input) => { return input.RegA > input.ValB ? 1 : 0; };
            Func<Input, int> gtrr = (Input input) => { return input.RegA > input.RegB ? 1 : 0; };
            Func<Input, int> eqir = (Input input) => { return input.ValA == input.RegB ? 1 : 0; };
            Func<Input, int> eqri = (Input input) => { return input.RegA == input.ValB ? 1 : 0; };
            Func<Input, int> eqrr = (Input input) => { return input.RegA == input.RegB ? 1 : 0; };

            var operations = new List<Func<Input, int>>();
            operations.AddRange(new[] {addr, addi, mulr, muli, banr, bani, borr, bori, setr, seti, gtir, gtri, gtrr, eqir, eqri, eqrr});

            var result = 0;
            foreach (var item in inputs)
            {
                if (operations.Count(o => item.Apply(o).SequenceEqual(item.After)) >= 3) {
                    result++;
                }
            }

            System.Console.WriteLine("PART ONE: " + result);
            Console.WriteLine("Done");
        }
    }

    public class Instruction
    {
        public int Opcode { get; set; }
        public int InputA { get; set; }
        public int InputB { get; set; }
        public int OutputC { get; set; }
    }

    public class Input
    {
        public int[] Before { get; set; }
        public int[] Instructions { get; set; }
        public int[] After { get; set; }

        public Input(IEnumerable<string> data)
        {
            Before = data.ElementAt(0).Replace("Before: [", "").Replace("]", "").Split(", ").Select(x => int.Parse(x)).ToArray();
            Instructions = data.ElementAt(1).Split(' ').Select(x => int.Parse(x)).ToArray();
            After = data.ElementAt(2).Replace("After:  [", "").Replace("]", "").Split(", ").Select(x => int.Parse(x)).ToArray();
        }

        public int RegA { get { return Before[Instructions[1]]; } }
        public int ValA { get { return Instructions[1]; } }
        public int RegB { get { return Before[Instructions[2]]; } }
        public int ValB { get { return Instructions[2]; } }

        public override string ToString() {
            return "Before: [" + string.Join(", ", Before) + "]\n" + string.Join(' ', Instructions) + "\nAfter:  ["
                + string.Join(", ", After) + "]";
        }

    }

    public static class Extensions {
        public static int[] Apply(this Input input, Func<Input, int> operation) {
            var output = (int[])input.Before.Clone();
            output[input.Instructions[3]] = operation(input);
            return output; 
        }

    }
}
