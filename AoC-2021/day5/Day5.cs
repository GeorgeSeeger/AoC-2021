using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day5 : IProblem {
        public string Name => "Day5";

        public string Part1() {
            var input = File.ReadAllLines("./day5/input.txt");

            var lines = ParseLines(input);
            var map = ProcessNonDiagonals(lines);
            var answer = map.Count(kvp => kvp.Value > 1);

            return answer.ToString();
        }

        private Dictionary<(int x, int y), int> ProcessNonDiagonals((int X1, int Y1, int X2, int Y2)[] lines) {
            var nonDiagonals = lines.Where(line => line.X1 == line.X2 || line.Y1 == line.Y2).ToArray();

            return nonDiagonals.Aggregate(new Dictionary<(int x, int y), int>(), (dict, line) => {
                Action<(int x, int y)> AddOrIncrement = c => {
                    if (!dict.TryAdd(c, 1)) dict[c]++;
                };
                for (var y = Math.Min(line.Y1, line.Y2); y <= Math.Max(line.Y1, line.Y2); y++) 
                    for (var x = Math.Min(line.X1, line.X2); x <= Math.Max(line.X1, line.X2); x++)
                        AddOrIncrement((x, y));
                        
                return dict;
            });
        }

        private (int X1, int Y1, int X2, int Y2)[] ParseLines(string[] input) {
            return input.Select(l => l.Split(" -> ")
                                      .SelectMany(s => s.Split(","))
                                      .Select(i => int.Parse(i))
                                      .ToArray())
            .Select(line => (X1: line[0], Y1: line[1], X2: line[2], Y2: line[3]))
            .ToArray();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day5/input.txt");

            var lines = ParseLines(input);
            var map = ProcessNonDiagonals(lines);
            var diagonals = lines.Where(line => line.X2 + line.Y1 == line.X1 + line.Y2 || line.X1 + line.Y1 == line.X2 + line.Y2).ToArray();

            var answer = diagonals.Aggregate(map, (dict, line) => {
                Action<(int x, int y)> AddOrIncrement = c => {
                    if (!dict.TryAdd(c, 1)) dict[c]++;
                };

                for (var y = Math.Min(line.Y1, line.Y2); y <= Math.Max(line.Y1, line.Y2); y++) {
                    var x = (line.X1 + line.Y1 == line.X2 + line.Y2)
                        ? line.X1 + line.Y1 - y
                        : line.X1 - line.Y1 + y;
                    AddOrIncrement((x, y));
                }

                return dict;
            }).Count(kvp => kvp.Value > 1);
            
            return answer.ToString();
        }
    }
}