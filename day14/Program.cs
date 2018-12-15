using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "37";
            var numRecipes = 306281;
            var elf1 = 0;
            var elf2 = 1;

            PartOne(input, numRecipes, elf1, elf2);

            PartTwo(input, numRecipes.ToString());

            Console.WriteLine("Done");
        }

        private static void PartTwo(string input, string numRecipes)
        {
            var list = new LinkedList<int>();
            var elf1 = list.AddFirst(3);
            var elf2 = list.AddLast(7);
            var last10 = input;
            while (!last10.Contains(numRecipes))
            {
                var sum = (elf1.Value + elf2.Value).ToString();
                last10 += sum;
                if (last10.Length > 10) {
                    last10 = last10.Substring(last10.Length - 10);
                }
                list.AddLast(int.Parse(sum[0].ToString()));
                if (sum.Length > 1) list.AddLast(int.Parse(sum[1].ToString()));
                elf1 = elf1.NextNInCircle(elf1.Value + 1);
                elf2 = elf2.NextNInCircle(elf2.Value + 1);
            }
            System.Console.WriteLine(list.Count - 10 + list.Get10Back().IndexOf(numRecipes));
        }

        private static void PartOne(string input, int numRecipes, int elf1, int elf2)
        {
            while (input.Length <= numRecipes + 10)
            {
                var sum = int.Parse(input[elf1].ToString()) + int.Parse(input[elf2].ToString());
                input += sum.ToString();
                elf1 = (elf1 + 1 + int.Parse(input[elf1].ToString())) % input.Length;
                elf2 = (elf2 + 1 + int.Parse(input[elf2].ToString())) % input.Length;
            }

            var scores = input.Substring(numRecipes, 10);
            System.Console.WriteLine(scores);
        }

        static void DumpIt(string input, int elf1, int elf2) {
            for ( var i = 0; i < input.Length; i++ ) {
                if (i == elf1) 
                    Console.Write( $" ({input[i]})");
                else if (i == elf2)
                    Console.Write( $" [{input[i]}]");
                else Console.Write(" " + input[i]);
            }
            System.Console.WriteLine();
        }
    }
    public static class Extensions {
        public static LinkedListNode<int> NextInCircle(this LinkedListNode<int> node) {
            return node.Next ?? node.List.First;
        }

        public static LinkedListNode<int> PrevInCircle(this LinkedListNode<int> node) {
            return node.Previous ?? node.List.Last;
        }
        
        public static LinkedListNode<int> NextNInCircle(this LinkedListNode<int> node, int n) {
            for (int i = 0; i < n; i++)
            {
                node = node.NextInCircle();     
            }
            return node;
        }

        public static string Get10Back(this LinkedList<int> list) {
            var posNode = list.Last;
            var checkStr = "";
            for(var i = 0; i < 10; i++) {
                checkStr += posNode.Value.ToString();
                posNode = posNode.Previous;
            }
            return new string(checkStr.Select(x => x).Reverse().ToArray());
        }
    }

}
