using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day9 : IProblem {
        public string Name => "Day9";

        public string Part1() {
            var input = File.ReadAllLines("./day9/input.txt").Select(l => l.Select(c => c - '0').ToArray()).ToArray();
            var xMax = input.First().Length;
            var yMax = input.Length;
            var localMins = input.SelectMany((l, y) => l.Where((i, x) => 
                    ((y == 0 || input[y - 1][x] > i)
                    && (y == yMax - 1 || input[y + 1][x] > i)
                    && (x == 0 || input[y][x - 1] > i)
                    && (x == xMax - 1 || input[y][x + 1] > i))));

            var answer = localMins.Select(i => i + 1).Sum();
            return answer.ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day9/input.txt").Select(l => l.Select(c => c - '0').ToArray()).ToArray();

            var basinSizes = new List<int>();

            var yMax = input.Length;
            var xMax = input.First().Length;
            for (var y = 0; y < yMax; y++) {
                for (var x = 0; x < xMax; x++) {
                    if (input[y][x] != 9)
                        basinSizes.Add(CountByFloodFill(x, y));
                }
            }

            var answer = basinSizes.OrderByDescending(i => i).Take(3).Aggregate((acc, s) => acc * s);
            return answer.ToString();

            int CountByFloodFill(int x, int y) {
                if (y < 0 || y >= yMax|| x < 0 || x >= xMax || input[y][x] == 9)
                    return 0;

                input[y][x] = 9;

                return 1 
                + CountByFloodFill(x + 1, y)
                + CountByFloodFill(x - 1, y)
                + CountByFloodFill(x,     y + 1)
                + CountByFloodFill(x,     y - 1);
            }
        }
    }
}