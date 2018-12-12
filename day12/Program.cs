using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            const string INITIAL_STATE = "#.##.##.##.##.......###..####..#....#...#.##...##.#.####...#..##..###...##.#..#.##.#.#.#.#..####..#";
            const long NUM_GENERATIONS = 20;
            var state = StateToIntArray(INITIAL_STATE);
            var inputs = File.ReadAllLines("./input.txt");

            var rules = new bool[32];
            foreach (var input in inputs)
            {
                var check = input.Split(' ')[0];
                var result = input.Split(' ')[2];
                rules[ConvertToInt(check)] = (result == "#");
            }
            var newState = "";
            for ( long i = 0; i < NUM_GENERATIONS; i++) {
                newState = new string(state.Select(s => rules[s]).Select(c => c ? '#' : '.').ToArray());
                // System.Console.WriteLine(newState);
                state = StateToIntArray(newState);
            }
            
            var startPosition = NUM_GENERATIONS * 2;
            long sum = 0;
            for ( var i = 0; i < newState.Length; i++ ) {
                if (newState[i] == '#') {
                    sum += i - startPosition;
                }    
            }
            System.Console.WriteLine(sum);
            Console.WriteLine("Done");
        }

        private static int ConvertToInt(string pattern) {
            return Convert.ToInt32(pattern.Replace('.', '0').Replace('#', '1'), 2);
        }

        private static IEnumerable<long> StateToIntArray(string stateString)
        {
            var state = "...." + stateString + "....";
            var result = new List<long>();
            for ( long i = 0; i < state.Length - 5; i++) {
                result.Add(ConvertToInt(new string(state.Skip((int)i).Take(5).ToArray())));
            }
            return result;
        }
    }
}
