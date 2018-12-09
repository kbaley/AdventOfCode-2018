using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day9
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NUM_PLAYERS = 464;
            const Int64 LAST_MARBLE_POINTS = 70918;

            DoPart(NUM_PLAYERS, LAST_MARBLE_POINTS);
            DoPart(NUM_PLAYERS, LAST_MARBLE_POINTS * 100);
            Console.WriteLine("Done");
        }

        static void DoPart(int numPlayers, Int64 lastMarblePoints) {

            var circle = new LinkedList<int>();
            var marble = 0;
            var player = 0;
            var scores = new Int64[numPlayers];
            circle.AddFirst(marble++);
            circle.AddLast(marble++);
            var node = circle.AddAfter(circle.First, marble++);
            while (marble < lastMarblePoints) {
                if (marble % 23 != 0) {
                    node = circle.AddAfter(node.NextInCircle(), marble++);
                } else {
                    for ( var i = 0; i < 7; i++ ) {
                        node = node.PrevInCircle();
                    }
                    scores[player] += marble + node.Value;
                    node = node.NextInCircle();
                    circle.Remove(node.PrevInCircle());
                    marble++;
                    // PrintScores(scores);
                }
                // WriteCircle(circle, currentMarble - 1);
                player = marble % numPlayers;
            }
            System.Console.WriteLine(scores.Max());
        }

        static void PrintScores(int[] scores) {
            for(int i = 0; i <= scores.GetUpperBound(0); i++) {
                Console.Write($"{i}:{scores[i]}  ");
            }
            System.Console.WriteLine();
        }

        static void WriteCircle(LinkedList<int> circle, int currentMarble) {
            foreach (var item in circle)
            {
                Console.Write((item == currentMarble) ? "(" + item + ")" : item + " ");
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
