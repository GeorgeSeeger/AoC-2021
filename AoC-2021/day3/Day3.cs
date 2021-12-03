using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day3 : IProblem {
        public string Name => "Day3";

        public string Part1() {
            var input = File.ReadAllLines("./day3/input.txt");
            var frequenciesOf1 = FrequenciesOf1(input);

            var gammaRate = Convert.ToInt32(string.Join("", frequenciesOf1.Select(i => i > input.Length / 2 ? "1" : "0")), 2);
            var epsilonRate = Convert.ToInt32(string.Join("", frequenciesOf1.Select(i => i > input.Length / 2 ? "0" : "1")), 2);
            return (gammaRate * epsilonRate).ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day3/input.txt");

            IList<string> oxygenRatings = input.ToList();
            IList<string> co2Ratings = input.ToList();
            for (var i = 0; i < input.First().Length; i++) {
                if (oxygenRatings.Count() > 1) {
                    if (oxygenRatings.Count() == oxygenRatings.Count(l => l[i] == '1') * 2) {
                        oxygenRatings = oxygenRatings.Where(l => l[i] == '1').ToList();
                    } else {
                        oxygenRatings = oxygenRatings.Where(l => l[i] == MostCommonAtPosition(i, oxygenRatings)).ToList();
                    }
                }
                if (co2Ratings.Count() > 1) {
                    if (co2Ratings.Count() == co2Ratings.Count(l => l[i] == '1') * 2) {
                        co2Ratings = co2Ratings.Where(l => l[i] == '0').ToList();
                    } else {
                        co2Ratings = co2Ratings.Where(l => l[i] != MostCommonAtPosition(i, co2Ratings)).ToList();
                    }

                }
            }
            
            return new[] { oxygenRatings, co2Ratings}
                    .Select(l => Convert.ToInt32(l.First(), 2))
                    .Aggregate((i, j) => i * j)
                    .ToString();
        }

        private static char MostCommonAtPosition(int pos, IList<string> input) => FrequenciesOf1(input)[pos] * 2 >= input.Count() ? '1' : '0';

        private static int[] FrequenciesOf1(IList<string> list) => list.Skip(1).Aggregate(
                        list.First().Select(c => c - '0').ToArray(),
                        (acc, line) => {
                            for (var i = 0; i < acc.Length; i++)
                                acc[i] += line[i] - '0';

                            return acc;
                        }).ToArray();
    }
}