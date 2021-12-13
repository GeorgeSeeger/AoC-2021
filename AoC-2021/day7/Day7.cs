using System;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day7 : IProblem {
        public string Name => "Day7";

        public string Part1() {
            var input = File.ReadAllLines("./day7/input.txt").First().Split(",").Select(int.Parse).ToArray();

            var fuelCosts = input.Select((_, dest) => input.Select(pos => Math.Abs(dest - pos)).Sum());

            return fuelCosts.Min().ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day7/input.txt").First().Split(",").Select(int.Parse).ToArray();
            var fuelCosts = input.Select((_, dest) => input.Select(pos => {
                var distance = Math.Abs(dest - pos);
                return distance * (distance + 1) / 2;
            }).Sum());

            return fuelCosts.Min().ToString();
        }
    }
}