using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt");

            var grid = new char[inputs[0].Length, inputs.Length];
            var carts = new List<Cart>();
            var directions = new char[] { '^', '>', 'v', '<' };
            System.Console.WriteLine(grid.GetLength(1));
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    switch (inputs[y][x])
                    {
                        // Taking a chance no carts are sitting on a corner or intersection
                        // at the beginning
                        case '^':
                            carts.Add(Cart.NewCart(x, y, Direction.UP));
                            grid[x, y] = '|';
                            break;
                        case '>':
                            carts.Add(Cart.NewCart(x, y, Direction.RIGHT));
                            grid[x, y] = '-';
                            break;
                        case 'v':
                            carts.Add(Cart.NewCart(x, y, Direction.DOWN));
                            grid[x, y] = '|';
                            break;
                        case '<':
                            carts.Add(Cart.NewCart(x, y, Direction.LEFT));
                            grid[x, y] = '-';
                            break;
                        default:
                            grid[x, y] = inputs[y][x];
                            break;
                    }
                }
            }

            var firstCrash = true;
            while(carts.Count > 1) {
                var sortedCarts = carts.OrderBy(c => c.Position.Y).ThenBy(c => c.Position.X);
                var cartsToRemove = new List<Cart>();
                foreach (var cart in sortedCarts)
                {
                    var nextPosition = cart.GetNextPosition();
                    var crashedCarts = carts.Where(c => c.Position == nextPosition && cartsToRemove.All(c2 => c.Position != c2.Position));
                    if (crashedCarts.Count() > 0) {
                        // PART ONE
                        if (firstCrash) {
                            System.Console.WriteLine("::: PART ONE :::" + nextPosition);
                            firstCrash = false;
                        }
                        cartsToRemove.AddRange(crashedCarts);
                        cartsToRemove.Add(cart);
                    }
                    cart.MoveTo(nextPosition, grid);
                }
                foreach (var item in cartsToRemove)
                {
                    carts.Remove(item);    
                }
            }

            System.Console.WriteLine("::: PART TWO :::" + carts.First().Position);
            Console.WriteLine("Done");
        }

        public static void DrawGrid(char[,] grid, List<Cart> carts) {

            for (var y = 0; y < grid.GetLength(1); y++) {
                for (var x = 0; x < grid.GetLength(0); x++) {
                    var theCarts = carts.Where(c => c.Position.X == x && c.Position.Y == y);
                    if (theCarts.Count() > 1) {
                        Console.Write('X');
                    } else if (theCarts.Count() == 1) {
                        Console.Write(theCarts.First().Symbol);
                    } else {
                        Console.Write(grid[x,y]);
                    }
                }
                System.Console.WriteLine();
            }
        }
    }

    public class Cart
    {
        public Point Position { get; set; }
        public Direction Direction { get; set; }
        public Move LastMove { get; set; }

        public static Cart NewCart(int x, int y, Direction direction) {
            return new Cart {
                Position = new Point(x,y),
                Direction = direction,
                LastMove = Move.RIGHT
            };
        }

        public char Symbol {
            get {
                if (Direction == Direction.UP) return '^';
                if (Direction == Direction.RIGHT) return '>';
                if (Direction == Direction.DOWN) return 'v';
                if (Direction == Direction.LEFT) return '<';
                return '.';
            }
        }

        internal Point GetNextPosition()
        {
            switch (Direction) {
                case Direction.UP:
                    return new Point(Position.X, Position.Y - 1);
                case Direction.RIGHT:
                    return new Point(Position.X + 1, Position.Y);
                case Direction.DOWN:
                    return new Point(Position.X, Position.Y + 1);
                case Direction.LEFT:
                    return new Point(Position.X - 1, Position.Y);
            }
            return Position;
        }

        internal void MoveTo(Point nextPosition, char[,] grid)
        {
            switch (grid[nextPosition.X, nextPosition.Y]) {
                case '\\':
                    if (Direction == Direction.UP || Direction == Direction.DOWN) 
                        Direction = Direction.PrevDirection();
                    else
                        Direction = Direction.NextDirection();
                    break;
                case '/':
                    if (Direction == Direction.UP || Direction == Direction.DOWN) 
                        Direction = Direction.NextDirection();
                    else
                        Direction = Direction.PrevDirection();
                    break;
                case '+':
                    if (LastMove == Move.RIGHT)
                        Direction = Direction.PrevDirection();
                    if (LastMove == Move.STRAIGHT)                    
                        Direction = Direction.NextDirection();
                    LastMove = LastMove.NextTurn();

                    break;
            }
            Position = nextPosition;
        }
    }

    public enum Move
    {
        LEFT = 0,
        STRAIGHT = 1,
        RIGHT = 2
    }

    public enum Direction
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

    public static class Extensions
    {

        public static Move NextTurn(this Move turn)
        {
            return (Move)(((int)turn + 1) % 3);
        }

        public static Direction NextDirection(this Direction direction) {
            return (Direction)(((int)direction + 1) % 4);
        }
        public static Direction PrevDirection(this Direction direction) {
            return (Direction)(((int)direction - 1 + 4) % 4);
        }
    }
}
