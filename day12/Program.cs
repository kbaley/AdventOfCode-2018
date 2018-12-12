using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            const string INITIAL_STATE = "#.##.##.##.##.......###..####..#....#...#.##...##.#.####...#..##..###...##.#..#.##.#.#.#.#..####..#";
            const long NUM_GENERATIONS = 1000;
            const long TOTAL_GENS = 50000000000;
            var state = StateToIntArray(INITIAL_STATE);
            var inputs = File.ReadAllLines("./input.txt");

            var checks = new bool[32];
            foreach (var input in inputs)
            {
                var check = input.Split(' ')[0];
                var result = input.Split(' ')[2];
                checks[ConvertToInt(check)] = (result == "#");
            }
            var newState = "";
            long startPosition = 0;
            var lastTenDiffs = new long[10];
            long prevSum = GetStateSum(INITIAL_STATE, 0);

            // Based on observation of test input and my input, at some point the difference between the 
            // current state sum and the previous one stabilizes to a single number. I.e. the difference
            // from one step to the next is identical. So we'll run through the exercise 1000 times
            // and see if it's enough such that the last ten differences are the same. If so, this
            // reduces to a mathematical problem
            for ( long i = 0; i < NUM_GENERATIONS; i++) {
                newState = new string(state.Select(s => checks[s]).Select(c => c ? '#' : '.').ToArray());
                startPosition += 2;
                if (newState.StartsWith("....")) {
                    newState = newState.Substring(2);
                    startPosition -= 2;
                }
                if (newState.EndsWith("....")) {
                    newState = newState.Substring(0, newState.Length - 2);
                }
                state = StateToIntArray(newState);
                lastTenDiffs[i % 10] = GetStateSum(newState, startPosition) - prevSum;
                prevSum = GetStateSum(newState, startPosition);
            }

            if (lastTenDiffs.All(x => x == lastTenDiffs[0])) {
                prevSum += (TOTAL_GENS - NUM_GENERATIONS) * lastTenDiffs[0];
                System.Console.WriteLine(prevSum);
            } else {
                System.Console.WriteLine("No pattern; try a bigger value for NUM_GENERATIONS");
            }
            Console.WriteLine("Done");
        }

        private static long GetStateSum(string state, long startPosition) {

            long sum = 0;
            for ( long i = 0; i < state.Length; i++ ) {
                if (state[(int)i] == '#') {
                    sum += i - startPosition;
                }    
            }
            return sum;
        } 

        private static int ConvertToInt(string pattern) {
            return Convert.ToInt32(pattern.Replace('.', '0').Replace('#', '1'), 2);
        }

        private static IEnumerable<int> StateToIntArray(string stateString)
        {
            var state = "...." + stateString + "....";
            var result = new List<int>();
            for ( var i = 0; i < state.Length - 5; i++) {
                result.Add(ConvertToInt(new string(state.Skip(i).Take(5).ToArray())));
            }
            return result;
        }
    }
}
