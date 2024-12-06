
namespace Day_05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var (rules, updates) = ParseFile("List1.txt");
            var comparer = new UpdatesComparer(rules);
            
            bool IsCorrectOrder(int[] updates) => updates.SequenceEqual(updates.Order(comparer));
            
            var part1 = updates.Where(IsCorrectOrder).Sum(x => x[x.Length / 2]);
            var part2 = updates.Where(x => !IsCorrectOrder(x)).Select(x => x.Order(comparer).ToArray()).Sum(x => x[x.Length / 2]);
            
            Console.WriteLine("Part1: " + part1);
            Console.WriteLine("Part2: " + part2);
        }

        private static (HashSet<(int, int)> rules, List<int[]> updates) ParseFile(string v)
        {
            HashSet<(int, int)> rules = [];
            List<int[]> updates = [];
            var parseRules = true;
            var lines = File.ReadAllLines(v);
            foreach (var line in lines) 
            {
                if (line == "")
                {
                    parseRules = false;
                    continue;
                }
                if (parseRules) {
                    var n = line.Split("|");
                    rules.Add((int.Parse(n[0]), int.Parse(n[1])));
                }
                else
                {
                    updates.Add(line.Split(",").Select(int.Parse).ToArray());
                }

            }
            return (rules, updates);
        }
    }

    class UpdatesComparer(HashSet<(int, int)> rules) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (rules.Contains((x, y))) return -1;
            if (rules.Contains((y, x))) return 1;
            return 0;
        }
    }
}
