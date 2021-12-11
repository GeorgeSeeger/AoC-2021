using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day11 : IProblem {
        public string Name => "Day11";

        public string Part1() {
            var input = Parse("./day11/input.txt");

            var flashed = Enumerable.Range(0, 100).Sum(i => {
                return Step(input).Sum(l => l.Count(i => i == 0));
            });

            return flashed.ToString();
        }

        public string Part2() {
            var input = Parse("./day11/input.txt");

            Func<bool> allFlashingSyncd = () => input.SelectMany(i => i).Sum() == 0;
            int steps = 0;
            while (!allFlashingSyncd()) {
                Step(input);
                ++steps;
            }

            return steps.ToString();
        }

        private int[][] Step(int[][] input) {
            IncrementAll(input);

            var flashes = new List<(int j, int i)>();
            input.ToEach((j, i) => {
                if (input[j][i] >= 10) {
                    flashes.Add((i, j));
                }
            });

            while (flashes.Any()) {
                var p = flashes.First();
                flashes.AddRange(FlashAt(input, p));
                flashes.RemoveAt(0);
            }

            ResetFlashed(input);
            return input;
        }

        private int[][] Parse(string filename) {
            return File.ReadAllLines(filename).Select(l => l.Select(c => c - '0').ToArray()).ToArray();
        }

        private void ResetFlashed(int[][] octopuses) {
            octopuses.ToEach((j, i) => {
                if (octopuses[j][i] >= 10) {
                    octopuses[j][i] = 0;
                }
            });
        }

        private List<(int x, int y)> FlashAt(int[][] octopuses, (int x, int y) p) {
            var gonnaFlash = new List<(int x, int y)>();

            for (var j = Math.Max(p.y - 1, 0); j < Math.Min(p.y + 2, octopuses.Length); j++)
                for (var i = Math.Max(p.x - 1, 0); i < Math.Min(p.x + 2, octopuses[p.y].Length); i++)
                    if ((j != p.y || i != p.x) && ++octopuses[j][i] == 10)
                        gonnaFlash.Add((i, j));

            return gonnaFlash;
        }

        private void IncrementAll(int[][] octopuses) {
            octopuses.ToEach((j, i) => octopuses[j][i]++);
        }
    }
}