using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");

            var order = "";
            var stepNames = "";
            var steps = new List<Steps>();
            foreach (var input in inputs)
            {
                var firstStep = input.Substring("Step ".Length)[0];
                var secondStep = input.Substring("Step A must be finished before step ".Length)[0];
                if (!stepNames.Contains(firstStep)) stepNames += firstStep;
                if (!stepNames.Contains(secondStep)) stepNames += secondStep;
                steps.Add(new Steps{First = firstStep, Second = secondStep});
            }

            var numberOfSteps = stepNames.Length;
            while (order.Length != numberOfSteps) {
                var available = "";
                foreach (var stepName in stepNames)
                {
                    if (stepName.CanBeCompleted(order, steps)) available += stepName;
                }
                var current = available.OrderBy(x => x).First();
                order += current;
                stepNames = stepNames.Replace(current.ToString(), string.Empty);
            }

            System.Console.WriteLine($"::: PART 1 ::: {order}");

            // PART 2 //

            stepNames = order;
            order = "";
            const int NUM_ELVES = 5;
            var t = 0;
            var availableElves = NUM_ELVES;
            var wip = new Dictionary<char, int>();
            while (order.Length != numberOfSteps) {

                if (wip.Count > 0) {
                    foreach (var item in wip.Keys.ToArray())
                    {
                        wip[item]--;
                        if (wip[item] == 0) {
                            order += item;
                            stepNames = stepNames.Replace(item.ToString(), "");
                            availableElves++;
                        }
                    }
                    wip = wip.Where(w => w.Value > 0).ToDictionary( w => w.Key, w => w.Value);
                }

                if (availableElves > 0) {
                    var available = "";
                    foreach (var stepName in stepNames)
                    {
                        if (stepName.CanBeCompleted(order, steps)) available += stepName;
                    }
                    foreach (var item in wip.Keys)
                    {
                        available = available.Replace(item.ToString(), "");
                    }
                    var ableToWorkOn = available.OrderBy(x => x).Take(availableElves);
                    availableElves -= ableToWorkOn.Count();
                    foreach (var item in ableToWorkOn)
                    {
                        wip.Add(item, TimeToComplete(item));   
                    }
                }

                t++;
            }

            Console.WriteLine($"::: PART 2 ::: {t-1}");

            Console.WriteLine("Done");
        }

        static int TimeToComplete(char c) {
            return 60 + (c - 'A' + 1);
        }

    }
    public class Steps {
        public char First;
        public char Second;
    }


    public static class Extensions
    {
        public static bool CanBeCompleted(this char step, string completedSteps, IEnumerable<Steps> steps) {
            var prereqs = steps.Where(s => s.Second == step).Select(s => s.First);
            return prereqs.All(s => completedSteps.Contains(s));
        }
    }
}
