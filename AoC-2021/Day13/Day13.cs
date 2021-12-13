using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace AoC_2021 {
    public class Day13 : IProblem {
        public string Name => "Day13";

        public string Part1() {
            var input = Parse("./day13/input.txt");

            var coords = Fold(input.DotCoords, input.Folds.First());
            return coords.Length.ToString();
        }

        private (int x, int y)[] Fold(IEnumerable<(int x, int y)> coords, (int x, int y) p) {
            var newCoords =  coords.Select(c => {
                c.y = p.y == 0 || c.y < p.y 
                    ? c.y 
                    : 2 * p.y  - c.y;
                c.x = p.x == 0 || c.x < p.x 
                    ? c.x
                    : 2 * p.x - c.x;
                return c;
            });

            return newCoords.Distinct().ToArray();
        }

        private Instructions Parse(string filename) {
            var input = File.ReadAllLines(filename);
            var instr = new Instructions();
            foreach (var line in input.TakeWhile(l => l != "")) {
                var xy = line.Split(",").Select(int.Parse).ToArray();
                instr.DotCoords.Add((x: xy[0], y: xy[1]));
            }

            foreach (var line in input.SkipWhile(l => l != "").Skip(1)) {
                var rgx = new Regex("([xy])=(\\d+)");
                var res = rgx.Match(line);

                if (res.Groups[1].Value == "x") {
                    instr.Folds.Add((int.Parse(res.Groups[2].Value), y: 0));
                } else {
                    instr.Folds.Add((x: 0, int.Parse(res.Groups[2].Value)));
                }
            }

            return instr;
        }

        public string Part2() {
            var input = Parse("./day13/input.txt");
            var letterCoords = input.Folds.Aggregate(input.DotCoords, Fold).ToHashSet();
            
            var paper = Enumerable.Range(0, letterCoords.Max(c => c.y) + 1)
                .Select(y => Enumerable.Range(0, letterCoords.Max(c => c.x) + 2)
                    .Select(x => letterCoords.Contains((x, y)) ? '#' : '.'));

            return "\n" + string.Join("\n", paper.Select(l => string.Join("", l))) + "\n";
        }

        class Instructions {
            public IList<(int x, int y)> DotCoords { get; set; } = new List<(int x, int y)>();

            public IList<(int x, int y)> Folds { get; set; } = new List<(int x, int y)>();
        }
    }
}