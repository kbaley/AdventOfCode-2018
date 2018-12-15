using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace day14
{
    class Program
    {
        static long i = 0;
        static void Main(string[] args)
        {
            var input = "37";
            var numRecipes = 306281; //306281;
            var elf1 = 0;
            var elf2 = 1;

            // PartOne(input, numRecipes, elf1, elf2);

            // PART TWO
            PartTwo(input, numRecipes.ToString(), elf1, elf2);

            Console.WriteLine("Done");
        }

        private static void PartTwo(string input, string numRecipes, int e1, int e2)
        {
            var list = new LinkedList<int>();
            var elf1 = list.AddFirst(3);
            var elf2 = list.AddLast(7);
            while (!Contains(list, numRecipes))
            {
                var sum = (elf1.Value + elf2.Value).ToString();
                list.AddLast(int.Parse(sum[0].ToString()));
                if (sum.Length > 1) list.AddLast(int.Parse(sum[1].ToString()));
                var elf1Value = elf1.Value;
                var elf2Value = elf2.Value;
                for (int i = 0; i < 1 + elf1Value; i++)
                {
                    elf1 = elf1.NextInCircle();    
                }
                for (int i = 0; i < 1 + elf2Value; i++)
                {
                    elf2 = elf2.NextInCircle();    
                }
                if (i++ % 1000000 == 1000) System.Console.WriteLine("end: " + i + ": " + DateTime.Now);
            }
            var newInput = string.Join("", list.Select(x => x.ToString()).ToArray());
            System.Console.WriteLine(newInput.IndexOf(numRecipes));
        }

        private static bool Contains(LinkedList<int> list, string check)
        {
            if (list.Count < 12) return false;
            var posNode = list.Last;
            var checkStr = "";
            for(var i = 0; i < 10; i++) {
                checkStr += posNode.Value.ToString();
                posNode = posNode.Previous;
            }
            checkStr = new string(checkStr.Select(x => x).Reverse().ToArray());
            return checkStr.Contains(check);
        }

        private static void PartOne(string input, int numRecipes, int elf1, int elf2)
        {
            while (input.Length <= numRecipes + 10)
            {
                var sum = int.Parse(input[elf1].ToString()) + int.Parse(input[elf2].ToString());
                input += sum.ToString();
                elf1 = (elf1 + 1 + int.Parse(input[elf1].ToString())) % input.Length;
                elf2 = (elf2 + 1 + int.Parse(input[elf2].ToString())) % input.Length;
                // DumpIt(input, elf1, elf2);
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
    }

}
