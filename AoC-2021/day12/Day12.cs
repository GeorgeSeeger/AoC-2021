using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace AoC_2021 {
    public class Day12 : IProblem {
        public string Name => "Day12";

        public string Part1() {
            var input = Parse("./day12/input.txt");

            var paths = FindPathsSmallsOnce(input, new HashSet<string>(), "", "start");

            var answer = paths.Count(s => s.EndsWith("end"));

            return answer.ToString();
        }

        private Dictionary<string, List<string>> Parse(string filename) {
            var input = File.ReadAllLines(filename);
            var dict = new Dictionary<string, List<string>>();
            foreach (var line in input) {
                var pair = line.Split("-");
                var caveOne = pair.First();
                var caveTwo = pair.Last();
                if (dict.TryGetValue(caveOne, out var links)) links.Add(caveTwo);
                else dict.Add(caveOne, new List<string>() { caveTwo });
                if (dict.TryGetValue(caveTwo, out var linksTwo)) linksTwo.Add(caveOne);
                else dict.Add(caveTwo, new List<string> { caveOne });
            }
            return dict;
        }

        private HashSet<string> FindPathsSmallsOnce(Dictionary<string, List<string>> graph, HashSet<string> previousPaths, string currentPath, string from) {
            if (IsSmallCave.IsMatch(from) && currentPath.Contains($",{from}")) return previousPaths;

            var path = currentPath + (string.IsNullOrEmpty(currentPath) ? "" : ",") + from;
            if (previousPaths.Contains(path)) return previousPaths;
            previousPaths.Add(path);

            if (from == "end") return previousPaths;
            foreach (var next in graph[from]) {
                if (next == "start") continue;

                FindPathsSmallsOnce(graph, previousPaths, path, next);
            }

            return previousPaths;
        }

        private Regex IsSmallCave = new Regex("^[a-z]+$");

        public string Part2() {
            var input = Parse("./day12/input.txt");
            
            var paths = FindPathsOneSmallTwice(input, new HashSet<string>(), "", "start");

            var answer = paths.Count(s => s.EndsWith("end"));

            return answer.ToString();
        }

        private HashSet<string> FindPathsOneSmallTwice(Dictionary<string, List<string>> graph, HashSet<string> previousPaths, string currentPath, string from) {
            if (IsSmallCave.IsMatch(from) 
                && currentPath.Contains($",{from}")
                && currentPath.Split(",").GroupBy(s => s).Any(g => g.Key.Length <= 2 && IsSmallCave.IsMatch(g.Key) && g.Count() == 2))
                return previousPaths;

            var path = currentPath + (string.IsNullOrEmpty(currentPath) ? "" : ",") + from;
            if (previousPaths.Contains(path)) return previousPaths;
            previousPaths.Add(path);

            if (from == "end") return previousPaths;
            foreach (var next in graph[from]) {
                if (next == "start") continue;

                FindPathsOneSmallTwice(graph, previousPaths, path, next);
            }

            return previousPaths;
        }
    }
}