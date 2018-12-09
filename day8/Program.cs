using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day8
{
    class Program
    {
            static int x = 0;
            static int totalMeta = 0;
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");
            var entries = inputs[0].TheDylanSplit().Select(int.Parse).ToList();

            var root = Moo(entries);

            System.Console.WriteLine($"::: PART ONE ::: {totalMeta}");
            System.Console.WriteLine($"::: PART TWO ::: {root.value}");


            Console.WriteLine("Done");
        }

        static Node Moo(List<int> entries) {

            var node = new Node{
                numChildren = entries[x++],
                numMeta = entries[x++]
            };
            for (int i = 0; i < node.numChildren; i++)
            {
                node.children.Add(Moo(entries));
            }
            for (int i = 0; i < node.numMeta; i++)
            {
                var metadata = entries[x++];
                node.metadata.Add(metadata);
                totalMeta += metadata;
            }
            return node;
        }

    }

    public class Node {
        public int numChildren;
        public int numMeta;
        public List<Node> children = new List<Node>();
        public List<int> metadata = new List<int>();

        public int value {
            get {
                if (numChildren == 0) return metadata.Sum();
                var result = 0;
                foreach (var item in metadata)
                {
                    if (item <= children.Count()) {
                        result += children[item - 1].value;
                    }   
                }
                return result;
            }
        }
    }

    static class Extensions {
        public static string[] TheDylanSplit(this string value) {
            return value.Split(' ');
        }
    }
}
