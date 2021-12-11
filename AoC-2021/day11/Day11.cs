using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;

namespace AoC_2021 {
    public class Day11 : IProblem {
        public string Name => "Day11";

        public string Part1() {
            var input = Parse("./day11/input.txt");

            var flashed = 0;
            for (var step = 0; step < 100; step++) {
                flashed += Step(input);
            }

            return flashed.ToString();
        }

        private int Step(int[][] input) {
            IncrementAll(input);
            var flashes = new Queue<(int x, int y)>();
            for (var j = 0; j < input.Length; j++) {
                for (var i = 0; i < input[0].Length; i++) {

                    if (input[j][i] >= 10) {
                        flashes.Enqueue((i, j));
                    }
                }
            }

            while (flashes.Any()) {
                var xy = flashes.Dequeue();
                foreach (var p in FlashAt(input, xy).ToArray())
                    flashes.Enqueue(p);
            }

            return ResetFlashed(input);
        }

        private void Print(int[][] input) {
            foreach (var line in input) Console.WriteLine(string.Join("", line.Select(i => i.ToString().PadLeft(3, ' '))));
            Console.WriteLine("");
        }

        private int[][] Parse(string v) {
            return File.ReadAllLines(v).Select(l => l.Select(c => c - '0').ToArray()).ToArray();
        }

        private int ResetFlashed(int[][] octopuses) {
            var flashed = 0;
            for (var y = 0; y < octopuses.Length; y++)
                for (var x = 0; x < octopuses[y].Length; x++)
                    if (octopuses[y][x] >= 10) {
                        octopuses[y][x] = 0;
                        flashed++;
                    } 

            return flashed;
        }

        private List<(int x, int y)> FlashAt(int[][] octopuses, (int x, int y) p) {
            var gonnaFlash = new List<(int x, int y)>();

            for (var j = Math.Max(p.y - 1, 0); j < Math.Min(p.y + 2, octopuses.Length); j++)
                for (var i = Math.Max(p.x - 1, 0); i < Math.Min(p.x + 2, octopuses[p.y].Length); i++)
                    if ((j != p.y || i != p.x) && ++octopuses[j][i] == 10) {
                        gonnaFlash.Add((i, j));
                    }

            return gonnaFlash;
        }

        private void IncrementAll(int[][] octopuses) {
            for (var j = 0; j < octopuses.Length; j++)
                for (var i = 0; i < octopuses[j].Length; i++) 
                    octopuses[j][i]++;
        }

        public string Part2() {
            var input = Parse("./day11/input.txt");

            var steps = 0;
            while (input.SelectMany(i => i).Sum() != 0) {
                Step(input);
                steps++;
            }

            return steps.ToString();
        }
    }
}