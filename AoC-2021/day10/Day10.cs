using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AoC_2021 {
    public class Day10 : IProblem {
        public string Name => "Day10";

        private string[] Parse(string filename) => File.ReadAllLines(filename);

        public string Part1() {
            var input = this.Parse("./day10/input.txt");

            var scores = new Dictionary<char, int> {
                {')', 3},
                {']', 57},
                {'}', 1197},
                {'>', 25137 }
            };
            var answer = input.Select(this.ParseLine).Where(c => c.error.HasValue).Select(c => scores[c.error.Value]).Sum();

            return answer.ToString();
        }

        private (char? error, BracketNode[] nodes) ParseLine(string line) {
            var nodes = new List<BracketNode>();
            var node = new BracketNode(line.First());
            foreach (var c in line.Skip(1)) {
                if (node.IsComplete) {
                    nodes.Add(node);
                    if ("({[<".Contains(c)) {
                        node = new BracketNode(c);
                        continue;
                    }
                }
                if (!node.Accept(c)) {
                    return (error: c, null);
                }
            }
            nodes.Add(node);
            return (null, nodes.ToArray());
        }
        public string Part2() {
            var input = Parse("./day10/input.txt");
            Func<char, int> valueOf = (c) => " )]}>".IndexOf(c);

            var scores = input.Select(l => ParseLine(l))
                              .Where(l => !l.error.HasValue)
                              .Select(i => i.nodes.Last())
                              .Select(b => b.Finish())
                              .Select(s => s.Aggregate(0L, (acc, c) => 5 * acc + valueOf(c)));

            var answer = scores.OrderBy(i => i).Skip(scores.Count() / 2).First();
            return answer.ToString();
        }

        class BracketNode {
            private static Dictionary<char, char> Brackets = "() [] {} <>".Split(' ').ToDictionary(s => s[0], s => s[1]);

            private char closer;

            public bool IsComplete;

            public BracketNode(char opener) {
                this.closer = Brackets[opener];
            }

            private BracketNode Last => this.Nodes.LastOrDefault();

            private List<BracketNode> Nodes = new();

            public bool Accept(char c) {
                if (!this.Last?.IsComplete ?? false)
                    return this.Last.Accept(c);
                if (Brackets.Keys.Contains(c)) {
                    this.Nodes.Add(new BracketNode(c));
                    return true;
                }
                if (this.closer != c) return false;
                this.IsComplete = true;
                return true;
            }

            public string Finish() {
                if (this.IsComplete) return "";

                return (this.Last?.Finish() ?? "") + this.closer;
            }
        }
    }
}