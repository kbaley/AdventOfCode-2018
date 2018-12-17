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

            var operations = new List<Func<Input, int>> {
            (Input input) => { return input.RegA + input.RegB; },
            (Input input) => { return input.RegA + input.ValB; },
            (Input input) => { return input.RegA * input.RegB; },
            (Input input) => { return input.RegA * input.ValB; },
            (Input input) => { return input.RegA & input.RegB; },
            (Input input) => { return input.RegA & input.ValB; },
            (Input input) => { return input.RegA | input.RegB; },
            (Input input) => { return input.RegA | input.ValB; },
            (Input input) => { return input.RegA; },
            (Input input) => { return input.ValA; },
            (Input input) => { return input.ValA > input.RegB ? 1 : 0; },
            (Input input) => { return input.RegA > input.ValB ? 1 : 0; },
            (Input input) => { return input.RegA > input.RegB ? 1 : 0; },
            (Input input) => { return input.ValA == input.RegB ? 1 : 0; },
            (Input input) => { return input.RegA == input.ValB ? 1 : 0; },
            (Input input) => { return input.RegA == input.RegB ? 1 : 0; }
            };

            var result = 0;

            // PART ONE
            foreach (var item in inputs)
            {
                if (operations.Count(o => item.Apply(o).SequenceEqual(item.After)) >= 3) {
                    result++;
                }
            }
            System.Console.WriteLine("PART ONE: " + result);

            // PART TWO
            // Process of elimination; all opcodes could be all operations
            var opcodes = new Dictionary<int, List<Func<Input, int>>>();
            for(var n = 0; n < 16; n++) {
                opcodes.Add(n, new List<Func<Input, int>>());
                foreach (var op in operations)
                {
                    opcodes[n].Add(op);
                }
            }

            // Start eliminating any that don't produce the desired result
            foreach (var item in inputs)
            {
                var opcode = item.Instructions[0];
                foreach (var op in operations)
                {
                    if (opcodes[opcode].Contains(op) && !item.Apply(op).SequenceEqual(item.After)) {
                        opcodes[opcode].Remove(op);
                    }
                }                
            }

            // Still not done. Build a list of final codes starting with ones we know for sure
            // and eliminating them from the running until every opcode has an assignment
            var finalOpcodes = new List<(int opcode, Func<Input, int> op)>();
            while (finalOpcodes.Count < 16) {

                var fixedOpcodes = opcodes.Where(o => o.Value.Count == 1);
                foreach (var item in fixedOpcodes)
                {
                    var op = item.Value.ElementAt(0);
                    finalOpcodes.Add((item.Key, op));
                    opcodes.Values.ToList().ForEach(o => o.Remove(op));
                }
            }

            // Now run the program
            var program = File.ReadAllLines("./input2.txt");
            var start = new[] { 0,0,0,0};
            var finalInput = new Input {
                Before = start
            };
            foreach (var item in program)
            {
                finalInput.Instructions = item.Split(' ').Select(x => int.Parse(x)).ToArray();
                finalInput.Before = finalInput.Apply(finalOpcodes.Single(o => o.opcode == finalInput.Instructions[0]).op);
            }

            System.Console.WriteLine("PART TWO: " + finalInput.Before[0]);

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

        public Input() { }
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
