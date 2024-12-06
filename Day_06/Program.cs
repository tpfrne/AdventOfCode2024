
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Day_06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
        }

        static void Part2()
        {
            var (map, guard) = ParseFile("List1.txt");
            var successfulObstacles = 0;

            for (int y = 0; y < map.Data.GetLength(0); y++)
            {
                for (int x = 0; x < map.Data[y].Length; x++)
                {
                    var clone = map.Clone();
                    var timeTravellingGuard = new Guard { Location = guard.Location };

                    var isGuard = timeTravellingGuard.Location.Equals(new Point { X = x, Y = y }, timeTravellingGuard.Location);
                    if (isGuard || clone.Data[y][x].IsObstruction)
                    {
                        continue;
                    }
                    else
                    {
                        clone.Data[y][x] = new Tile { Symbol = '#' };
                    }

                    while (true)
                    {
                        (bool stillInTheArea, bool stuckInLoop) = timeTravellingGuard.Patrol(clone);

                        if (!stillInTheArea)
                        {
                            break;
                        }

                        if (stuckInLoop)
                        {
                            successfulObstacles++;
                            break;
                        }
                    }
                }
            }



            Console.WriteLine("Timetravelling hacks: " + successfulObstacles);
        }

        static void Part1()
        {
            var (map, guard) = ParseFile("Demo.txt");

            while (guard.Patrol(map).StillInTheArea)
            {
                Visualize(map, guard);
                Thread.Sleep(500);
            }

            Console.WriteLine("Patrolled: " + guard.Visited.Count());
        }

        private static void Visualize(Map map, Guard guard)
        {
            Console.Clear();
            for (int y = 0; y < map.Data.GetLength(0); y++)
            {
                for (int x = 0; x < map.Data[y].Length; x++)
                {
                    var isGuard = guard.Location.Equals(new Point { X = x, Y = y }, guard.Location);
                    Console.Write(isGuard ? '^' : map.Data[y][x].Symbol);
                }
                Console.WriteLine();
            }
        }

        private static (Map, Guard) ParseFile(string v)
        {
            var lines = File.ReadAllLines(v);
            var guard = new Guard();
            var data = new Tile[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                var chars = lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    if (chars[j] == '^')
                    {
                        guard.Location = new Point { X = j, Y = i };
                        chars[j] = '.';
                    }
                }
                data[i] = chars.Select(c => new Tile { Symbol = c }).ToArray();
            }
            var map = new Map() { Data = data };
            return (map, guard);
        }
    }

    public class Map
    {
        public required Tile[][] Data { get; set; }

        public Map Clone()
        {
            var newData = new Tile[Data.GetLength(0)][];
            for (int y = 0; y < Data.GetLength(0); y++)
            {
                newData[y] = new Tile[Data[y].Length];
                for (int x = 0; x < Data[y].Length; x++)
                {
                    newData[y][x] = new Tile { Symbol = Data[y][x].Symbol };
                }
            }

            return new Map { Data = newData };
        }

        public bool OutOfBounds(Point nextPosition)
        {
            if (nextPosition.X < 0 || nextPosition.Y < 0)
            {
                return true;
            }
            if (Data.GetLength(0) - 1 < nextPosition.Y)
            {
                return true;
            }
            if (Data[nextPosition.Y].Length - 1 < nextPosition.X)
            {
                return true;
            }
            return false;
        }
    }

    public class Guard
    {
        private Point? location;

        public Point Location
        {
            get => location!;
            set
            {
                location = value;

                if (Visited.TryGetValue(value, out var count))
                {
                    Visited[value] = count + 1;
                }
                else
                {
                    Visited.Add(value, 1);
                }
            }
        }

        public Direction CurrentDirection { get; set; }

        public Dictionary<Point, int> Visited { get; set; } = new Dictionary<Point, int>(new Point());

        public (bool StillInTheArea, bool StuckInLoop) Patrol(Map map)
        {
            var nextPosition = Location + CurrentDirection;

            if (map.OutOfBounds(nextPosition))
            {
                return (false, false);
            }

            if (IsStuckInLoop())
            {
                return (true, true);
            }

            var tile = map.Data[nextPosition.Y][nextPosition.X];

            if (tile.IsObstruction)
            {
                CurrentDirection = (Direction)(((int)CurrentDirection + 90) % 360);
            }
            else
            {
                Location = nextPosition;
            }

            return (true, false);
        }

        public bool IsStuckInLoop()
        {
            // TODO Ghetto solution
            return Visited.Values.Any(x => x > 10);
        }
    }

    public class Tile
    {
        public char Symbol { get; set; }
        public bool IsObstruction => Symbol == '#';
    }

    [DebuggerDisplay("{Y}, {X}")]
    public class Point : IEqualityComparer<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(Point? x, Point? y)
        {
            if (x == null || y == null) return false;
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode([DisallowNull] Point obj)
        {
            return obj.X.GetHashCode() ^ obj.Y.GetHashCode();
        }

        public static Point operator +(Point pt, Direction dir)
        {
            return dir switch
            {
                Direction.Up => new Point { X = pt.X, Y = pt.Y - 1 },
                Direction.Down => new Point { X = pt.X, Y = pt.Y + 1 },
                Direction.Left => new Point { X = pt.X - 1, Y = pt.Y },
                Direction.Right => new Point { X = pt.X + 1, Y = pt.Y },
                _ => throw new Exception(),
            };
        }
    }

    public enum Direction
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270,
    }
}
