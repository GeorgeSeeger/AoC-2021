using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC_2021 {
    public class Day8 : IProblem {
        public string Name => "Day8";

        public string Part1() {
            var input = File.ReadAllLines("./day8/input.txt").Select(l => l.Split(" | ")[1]);

            var oneFourSevenEightLengths = new[] { 2, 4, 3, 7 };
            var answer = input.SelectMany(d => d.Split(" ")).Count(o => oneFourSevenEightLengths.Contains(o.Length));

            return answer.ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day8/input.txt").Select(l => l.Split(" | "));

            var answer = input.Select(line => {
                var charToBitOffset = DiscoverMap(line[0]);
                var digits = line[1].Split(" ").Select(num => Decode(charToBitOffset, num).ToString());
                return int.Parse(string.Join("", digits));
            }).Sum();

            return answer.ToString();
        }

        private static int Decode(Dictionary<char, int> charToBitShift, string display) {
            var digitValue = display.Select(c => charToBitShift[c]).Select(i => 1 << i).Sum();
            return DisplayMap[digitValue];
        }

        private static Dictionary<char, int> DiscoverMap(string str) {
            var digitStrings = str.Split(" ");
            var charPossibles = "abcdefg".ToDictionary(c => c, _ => Enumerable.Range(0, 7).ToList());

            Action<string, IList<int>> Deduce = (chars, vals) => {
                foreach (var pair in charPossibles) {
                    if (pair.Value.Count > 1) {
                        pair.Value.RemoveAll(i => chars.Contains(pair.Key) != vals.Contains(i));
                        if (pair.Value.Count == 1) 
                            foreach (var innerPair in charPossibles.Where(k => k.Key != pair.Key)) 
                                innerPair.Value.Remove(pair.Value.Single());
                    }
                }
            };

            // we know 1,4,7 and 8
            var one = digitStrings.First(d => d.Length == 2);
            Deduce(one, PossibilitiesFrom(1));

            var four = digitStrings.Single(d => d.Length == 4);
            Deduce(four, PossibilitiesFrom(4));

            var seven = digitStrings.Single(d => d.Length == 3);
            Deduce(seven, PossibilitiesFrom(7));

            var eight = digitStrings.Single(d => d.Length == 7);
            Deduce(eight, PossibilitiesFrom(8));

            // then 'f' appears 9 times in 0 - 9
            var letterCounts = digitStrings.SelectMany(c => c).GroupBy(c => c).ToDictionary(g => new string(g.Key, 1), g => g.Count());
            var f = letterCounts.Single(g => g.Value == 9).Key;
            Deduce(f, new[] { 5 });

            // then 'b' appears 6 times in 0 - 9
            var b = letterCounts.Single(g => g.Value == 6).Key;
            Deduce(b, new[] { 1 });

            // then 'e' appears 4 times in 0 - 9
            var e = letterCounts.Single(g => g.Value == 4).Key;
            Deduce(e, new[] { 4 });
            return charPossibles.ToDictionary(k => k.Key, v => v.Value.Single());
        }

        private static Dictionary<int, int> DisplayMap = new() {
            // gfedcba -> x
            { 0b1110111, 0 },
            { 0b0100100, 1 },
            { 0b1011101, 2 },
            { 0b1101101, 3 },
            { 0b0101110, 4 },
            { 0b1101011, 5 },
            { 0b1111011, 6 },
            { 0b0100101, 7 },
            { 0b1111111, 8 },
            { 0b1101111, 9 },
        };

        private static Dictionary<int, int> ReverseDisplayMap = DisplayMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        
        private static IList<int> PossibilitiesFrom(int i) => Enumerable.Range(0, 7).Where(j => ((ReverseDisplayMap[i] >> j) & 1) == 1).ToList();
    }
}