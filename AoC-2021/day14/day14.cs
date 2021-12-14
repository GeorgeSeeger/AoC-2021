using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AoC_2021 {
    public class Day14 : IProblem {
        public string Name => "Day14";

        public string Part1() {
            var input = Parse("./day14/input.txt");

            var polymer = input.Seed;
            for (var i = 0; i < 10; i++) {
                polymer = SimulateStep(polymer, input);
            }

            var counts = polymer.GroupBy(c => c).OrderBy(g => g.Count());
            return (counts.Last().Count() - counts.First().Count()).ToString();
        }

        private string SimulateStep(string polymer, Polymeriser input) {
            var stepped = polymer.Skip(1)
            .Select((c, i) => input.PairToString[(polymer[i], c)])
            .Prepend(polymer[0].ToString());
            return string.Join("", stepped);
        }

        public string Part2() {
            var input = Parse("./day14/input.txt");

            var polymer = input.SeedCount;
            for (var i = 0; i < 40; i++) {
                polymer = StepCounts(polymer, input);
            }

            var letterCounts = polymer
            .Select(kv => (letter: kv.Key.Item1, value: kv.Value ))
            .Aggregate(new Dictionary<char, long>(), (dict, kv) => {
                if (!dict.TryAdd(kv.letter, kv.value)) dict[kv.letter] += kv.value;
                return dict;
            })
            .Select(kv => kv.Key == input.Seed.Last() ? kv.Value + 1 : kv.Value)
            .OrderBy(v => v);

            return (letterCounts.Last() - letterCounts.First()).ToString();
        }

        private Dictionary<(char, char), long> StepCounts(Dictionary<(char, char), long> polymer, Polymeriser input) {
            var res = new Dictionary<(char, char), long>();
            foreach (var pair in polymer) {
                var next = input.PairToNext[pair.Key];
                foreach (var c in next) {
                    if (!res.TryAdd(c, pair.Value)) res[c] += pair.Value;
                }
            }
            return res;
        }

        private static Polymeriser Parse(string filename) {
            var input = File.ReadAllLines(filename);

            return new Polymeriser {
                Seed = input.First(),
                PairToString = input.Skip(2)
                .Select(line => line.Split(" -> "))
                .ToDictionary(l => (l[0][0], l[0][1]), l => new string(new[] { l[1][0], l[0][1]})),
                PairToNext = input.Skip(2)
                .Select(l => l.Split(" -> "))
                .ToDictionary(l => (l[0][0], l[0][1]), l => new[] { (l[0][0], l[1][0]), (l[1][0], l[0][1]) })
            };
        }

        class Polymeriser {
            public string Seed { get; set; }

            public Dictionary<(char, char), long> SeedCount => new(this.Seed.Skip(1)
            .Select((c, i) => KeyValuePair.Create((this.Seed[i], c), 1L)));

            public Dictionary<(char, char), string> PairToString { get; set; }

            public Dictionary<(char, char), (char, char)[]> PairToNext { get; set; }
        }
    }
}