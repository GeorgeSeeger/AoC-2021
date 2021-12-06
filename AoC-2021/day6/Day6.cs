using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day6 : IProblem {
        public string Name => "Day6";

        public string Part1() {
            var input = File.ReadAllLines("./day6/input.txt").First().Split(",").Select(int.Parse);
            var fishList = input.ToList();
            return SimulateForNDays(fishList, 80).Count().ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day6/input.txt").First().Split(",").Select(int.Parse);
            var answer = input.Select(i => CountForNDays(i, 256)).Sum();
            return answer.ToString();
        }

        private static IList<int> SimulateForNDays(List<int> fishList, int nDays) {
            for (var i = 0; i < nDays; i++) {
                var newFish = 0;
                foreach (var idx in Enumerable.Range(0, fishList.Count)) {
                    if (fishList[idx]-- == 0) {
                        ++newFish;
                        fishList[idx] = 6;
                    }
                }
                fishList.AddRange(Enumerable.Range(0, newFish).Select(j => 8));
                newFish = 0;
            }

            return fishList;
        }

        private static Dictionary<int, long> memo = new();

        private static long CountForNDays(int daysLeft, int nDays) {
            if (daysLeft > 0) return CountForNDays(0, nDays - daysLeft);

            if (nDays <= 0) return 1;
            if (nDays <= 7) return 2;
            if (memo.TryGetValue(nDays, out var count)) return count;
            count = CountForNDays(0, nDays - 7) + CountForNDays(2, nDays - 7);
            memo.Add(nDays, count);
            return count;
        }
    }
}