
namespace Day_01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lists = ParseFile("List1.txt");
            
            var distance = lists.left.Select((x, i) => Math.Abs(x - lists.right[i])).Sum();
            var similarity = lists.left.Select(l => l * lists.right.Count(r => r == l)).Sum();

            Console.WriteLine("Part1: " + distance);
            Console.WriteLine("Part2: " + similarity);

            Console.ReadLine();
        }

        private static (List<int> left, List<int> right) ParseFile(string path)
        {
            var left = new List<int>();
            var right = new List<int>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines) 
            {
                var l = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                left.Add(int.Parse(l[0]));
                right.Add(int.Parse(l[1]));
            }
            return (left.OrderBy(x => x).ToList(), right.OrderBy(x => x).ToList());
        }
    }
}
