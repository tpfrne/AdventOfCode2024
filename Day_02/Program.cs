
namespace Day_02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var reports = ParseFile("List1.txt");
            var safe = reports.Count(Report.IsSafe);
            var safeV2 = reports.Count(Report.IsSafeWithDampener);

            Console.WriteLine("Part1: " + safe);
            Console.WriteLine("Part2: " + safeV2);

            Console.ReadLine();
        }

        private static IEnumerable<Report> ParseFile(string v)
        {
            var lines = File.ReadAllLines(v);
            foreach (var line in lines)
            {
                yield return new Report { Numbers = line.Split(" ").Select(int.Parse).ToList() };
            }
        }
    }

    internal class Report
    {
        public List<int> Numbers { get; set; } = [];

        public static bool IsSafe(Report report)
        {
            var n1 = report.Numbers[0];
            var n2 = report.Numbers[1];
            var descending = n2 < n1;
            bool op(int a, int b) => descending ? a <= b : a >= b;
            bool diff(int a, int b) => Math.Abs(a - b) > 3;

            for (int i = 0; i < report.Numbers.Count - 1; i++)
            {
                if (op(report.Numbers[i], report.Numbers[i + 1]))
                {
                    return false;
                }
                
                if (diff(report.Numbers[i], report.Numbers[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsSafeWithDampener(Report report)
        {
            if (IsSafe(report))
            {
                return true;
            }

            for (int i = 0; i < report.Numbers.Count; i++)
            {
                var copy = report.Numbers.ToList();
                copy.RemoveAt(i);
                if (IsSafe(new Report { Numbers = copy }))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
