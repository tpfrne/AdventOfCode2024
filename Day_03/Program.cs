using System.Text.RegularExpressions;

namespace Day_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var demo = ParseFile("List1.txt");
            var enabled = true;
            var mulExpression = @"mul\((?<a>\d+),(?<b>\d+)\)";

            var part1 = demo.SelectMany(line => Regex.Matches(line, mulExpression)).Sum(HandleMatch);
            var part2 = demo.SelectMany(line => Regex.Matches(line, @$"{mulExpression}|do(n't)?\(\)")).Sum(HandleMatch);
            
            int HandleMatch(Match match) {
                if (!match.Success)
                {
                    return 0;
                }
                if (match.Value == "do()")
                {
                    enabled = true;
                    return 0;
                }
                if (match.Value == "don't()")
                {
                    enabled = false;
                    return 0;
                }
                if (!enabled)
                {
                    return 0;
                }
                return int.Parse(match.Groups["a"].Value) * int.Parse(match.Groups["b"].Value);
            }

            Console.WriteLine("Part1: " + part1);
            Console.WriteLine("Part2: " + part2);

            Console.ReadLine();
        }



        private static IEnumerable<string> ParseFile(string v)
        {
            var lines = File.ReadAllLines(v);
            foreach (var line in lines)
            {
                yield return line;
            }
        }
    }
}
