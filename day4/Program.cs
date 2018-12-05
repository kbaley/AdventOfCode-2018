using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt").OrderBy(s => s).ToArray();

            // Maps guard number to an array of minutes indicating how many times was asleep in that minute
            var sleepTimes = new Dictionary<string, int[]>();
            var currentGuard = "";
            var minute = 0;
            foreach (var input in inputs)
            {
                string[] delimiters = { "[", "] " };
                var components = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                var timeValue = components[0];
                var action = components[1];
                if (action.StartsWith("guard #", StringComparison.CurrentCultureIgnoreCase))
                {
                    currentGuard = action.Split(" begins ")[0];
                    if (!sleepTimes.ContainsKey(currentGuard)) sleepTimes.Add(currentGuard, new int[60]);
                    minute = 0;
                }
                else if (action.Equals("falls asleep", StringComparison.CurrentCultureIgnoreCase))
                {
                    minute = DateTime.ParseExact(timeValue, "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture).Minute;
                }
                else if (action.Equals("wakes up", StringComparison.CurrentCultureIgnoreCase))
                {
                    var lastMinute = minute;
                    minute = DateTime.ParseExact(timeValue, "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture).Minute;
                    for (int i = lastMinute; i < minute; i++)
                    {
                        sleepTimes[currentGuard][i]++;
                    }
                }
            }

            var guardWithTheMost = sleepTimes.Aggregate((l, r) => l.Value.Sum() > r.Value.Sum() ? l : r).Key;
            var indexWithTheMost = Array.FindIndex(sleepTimes[guardWithTheMost], t => t == sleepTimes[guardWithTheMost].Max());
            var guardId = int.Parse(guardWithTheMost.Replace("Guard #", ""));
            Console.WriteLine();
            Console.WriteLine($"Guard with the most: {guardId}");
            Console.WriteLine($"Index of highest number: {indexWithTheMost}");
            Console.WriteLine($":::: Answer: {indexWithTheMost * guardId}");

            // Part 2
            var guardMostAsleep = sleepTimes.Aggregate((l, r) => l.Value.Max() > r.Value.Max() ? l : r).Key;
            var indexAsleep = Array.FindIndex(sleepTimes[guardMostAsleep], t => t == sleepTimes[guardMostAsleep].Max());
            guardId = int.Parse(guardMostAsleep.Replace("Guard #", ""));
            Console.WriteLine($"Guard most asleep on the same minute: ${guardMostAsleep}");
            Console.WriteLine($"Index of highest minute: {indexAsleep}");
            Console.WriteLine($":::: Answer: {indexAsleep * guardId}");
            Console.WriteLine("Done");
            Console.ReadLine();
        }

    }
}
