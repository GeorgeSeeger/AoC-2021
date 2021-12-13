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

            var paths = CountPathsToEnd(input, new HashSet<string>(), "", "start");

            return paths.ToString();
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

        public string Part2() {
            var input = Parse("./day12/input.txt");
            
            var pathsCount = CountPathsToEnd(input, new HashSet<string>(), "", "start", allowSmallCavesTwice: true);
            
            return pathsCount.ToString();
        }

        private int CountPathsToEnd(Dictionary<string, List<string>> graph, HashSet<string> previousPaths, string currentPath, string from, bool allowSmallCavesTwice = false) {
            if (!allowSmallCavesTwice 
                && IsSmallCave(from)
                && currentPath.Contains(from))
                return 0;

            var path = currentPath + "," + from;
            if (previousPaths.Contains(path)) return 0;
            previousPaths.Add(path);

            if (from == "end") return 1;
            var allowSmallCavesNow = allowSmallCavesTwice && (!IsSmallCave(from) || !currentPath.Contains(from));
            return graph[from].Where(s => s != "start").Sum(next => CountPathsToEnd(graph, previousPaths, path, next, allowSmallCavesNow));
        }

        private bool IsSmallCave(string from) {
            return from.Length <= 2 && char.IsLower(from[0]);
        }
    }
}