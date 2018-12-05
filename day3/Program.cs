using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day3
{
  class Program
  {
    static void Main(string[] args)
    {
      var inputs = File.ReadAllLines("./input.txt");

      FindOverlaps(inputs);
      FindCleanClaim(inputs);

      Console.ReadLine();
    }

    private static void FindCleanClaim(string[] inputs)
    {
      string[] delimiters = { " @ ", ": ", ",", "x" };

      // Create a dictionary with:
      //   - Key = square
      //   - Value = list of claims that touch the square
      var dict = new Dictionary<string, List<string>>();
      var claims = new List<string>();
      foreach (var input in inputs)
      {
        var components = input.Split(delimiters, StringSplitOptions.None);
        var id = components[0];
        var left = int.Parse(components[1]);
        var top = int.Parse(components[2]);
        var width = int.Parse(components[3]);
        var height = int.Parse(components[4]);

        claims.Add(id);
        for (int i = 0; i < width; i++)
        {
          for (int j = 0; j < height; j++)
          {
            var key = (left + i).ToString() + "," + (top + j).ToString();
            if (dict.ContainsKey(key)) {
                dict[key].Add(id);
            } else {
                dict.Add(key, new List<string>());
                dict[key].Add(id);
            }
          }
        }
      }

      var values = dict.Values.Where(v => v.Count > 1).SelectMany( i => i).Distinct();
      var winner = claims.Single(c => !values.Contains(c));
      Console.WriteLine($"Winner: {winner}");
    }

    private static void FindOverlaps(string[] inputs)
    {
      string[] delimiters = { " @ ", ": ", ",", "x" };
      var dict = new Dictionary<string, int>();
      foreach (var input in inputs)
      {
        var components = input.Split(delimiters, StringSplitOptions.None);
        var left = int.Parse(components[1]);
        var top = int.Parse(components[2]);
        var width = int.Parse(components[3]);
        var height = int.Parse(components[4]);

        // Create a dictionary with key = square ID (i.e. coordinates) and
        // value = number of claims hitting it
        for (int i = 0; i < width; i++)
        {
          for (int j = 0; j < height; j++)
          {
            var key = (left + i).ToString() + "," + (top + j).ToString();
            if (dict.ContainsKey(key))
            {
              dict[key]++;
            }
            else
            {
              dict.Add(key, 1);
            }
          }
        }
      }

      var count = dict.Count(d => d.Value > 1);
      Console.WriteLine($"Count: {count}");
    }
  }

}
