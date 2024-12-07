

namespace Day_07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var calibrations = ParseFile("List1.txt");

            var part1 = calibrations.Where(x => x.Solvable(["+", "*"])).Sum(x => x.Target);
            var part2 = calibrations.Where(x => x.Solvable(["+", "*", "||"])).Sum(x => x.Target);


            Console.WriteLine("Part1: " + part1);
            Console.WriteLine("Part2: " + part2);
        }

        private static IEnumerable<Calibration> ParseFile(string v)
        {
            var f = File.ReadAllLines(v);
            foreach (var line in f)
            {
                var parts = line.Split(':');
                yield return new Calibration(long.Parse(parts[0]), parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            }
        }
    }

    internal class Calibration(long target, int[] values)
    {
        public long Target { get; } = target;
        public int[] Values { get; } = values;

        internal bool Solvable(string[] ops)
        {
            return SolvableImpl(ops, 0, 0L);
        }

        private bool SolvableImpl(string[] ops, int valueIndex, long sum)
        {
            if (sum > Target)
            {
                return false;
            }
            if (valueIndex > Values.Length - 1)
            {
                return false;
            }

            foreach (var op in ops)
            {
                var tmpSum = op switch
                {
                    "+" => sum + Values[valueIndex],
                    "*" => sum * Values[valueIndex],
                    "||" => Convert.ToInt64($"{sum}{Values[valueIndex]}"),
                    _ => throw new NotImplementedException()
                };
                if (tmpSum == Target && valueIndex == Values.Length - 1)
                {
                    return true;
                }
                if (SolvableImpl(ops, valueIndex + 1, tmpSum))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
