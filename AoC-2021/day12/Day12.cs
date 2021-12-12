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
            return input.SelectMany(line => new[] { line.Split("-"), line.Split("-").Reverse() })
                        .Aggregate(new Dictionary<string, List<string>>(), 
                        (dict, caves) => {
                            if (dict.TryGetValue(caves.First(), out var linked)) linked.Add(caves.Last());
                            else dict.Add(caves.First(), new List<string>{ caves.Last() });

                            return dict;
                        });
        }

        private HashSet<string> FindPathsSmallsOnce(Dictionary<string, List<string>> graph, HashSet<string> previousPaths, string currentPath, string from) {
            if (IsSmallCave.IsMatch(from) && currentPath.Contains(from)) return previousPaths;

            var path = currentPath + "," + from;
            if (previousPaths.Contains(path)) return previousPaths;
            previousPaths.Add(path);

            if (from == "end") return previousPaths;
            foreach (var next in graph[from]) {
                if (next == "start") continue;

                FindPathsSmallsOnce(graph, previousPaths, path, next);
            }

            return previousPaths;
        }

        private Regex IsSmallCave = new Regex("^[a-z]{1,2}$");

        public string Part2() {
            var input = Parse("./day12/input.txt");
            
            var pathsCount = CountPathsSmallsTwice(input, new HashSet<string>(), ("", false), "start");
            return pathsCount.ToString();
        }

        private int CountPathsSmallsTwice(Dictionary<string, List<string>> graph, HashSet<string> previousPaths, (string path, bool doubledUp) currentPath, string from) {
            if (IsSmallCave.IsMatch(from) 
                && currentPath.path.Contains(from)
                && currentPath.doubledUp)
                return 0;

            var path = currentPath.path + "," + from;
            if (previousPaths.Contains(path)) return 0;
            previousPaths.Add(path);

            if (from == "end") return 1;
            var doubledUp = currentPath.doubledUp || IsSmallCave.IsMatch(from) && currentPath.path.Contains(from);
            return graph[from].Where(s => s != "start").Sum(next => CountPathsSmallsTwice(graph, previousPaths, (path, doubledUp), next));
        }
    }
}