using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day4 : IProblem {
        public string Name => "Day4";

        public string Part1() {
            var input = File.ReadAllLines("./day4/input.txt");

            var numbers = ParseNumbers(input);

            var boards = ParseBoards(input);

            foreach (var num in numbers) {
                foreach (var board in boards) {
                    board.Mark(num);
                    if (board.IsWon) {
                        var answer = num * board.NonMarked.Sum();
                        return answer.ToString();
                    }
                }
            }

            throw new Exception("No boards won :(");
        }

        public string Part2() {
            var input = File.ReadAllLines("./day4/input.txt");
            
            var numbers = ParseNumbers(input);
            var boards = ParseBoards(input).ToList();
            var wonBoards = new List<(int num, BingoBoard board)>();
            foreach (var num in numbers) {
                foreach (var board in boards) {
                    if (!board.IsWon) {
                        board.Mark(num);
                        if (board.IsWon) {
                            wonBoards.Add((num, board));
                        }
                    }
                }
            }
            
            var lastWonBoard = wonBoards.Last();
            return $"{lastWonBoard.num * lastWonBoard.board.NonMarked.Sum()}";

            throw new Exception("This should be impossible");
        }

        private int[] ParseNumbers(string[] input) {
            return input.First().Split(",").Select(int.Parse).ToArray();
        }

        private BingoBoard[] ParseBoards(string[] input) {
            return input.Skip(1).Aggregate(new List<List<string>>(), (acc, line) => {
                if (line == string.Empty) acc.Add(new List<string>());
                else acc.Last().Add(line);
                return acc;
            }).Select(lines => new BingoBoard(lines)).ToArray();
        }

        class BingoBoard {
            private Dictionary<int, (int row, int col)> board;

            private List<int> marked = new List<int>();
          
            private readonly int[] rows = new int[5];

            private readonly int[] cols = new int[5];

            private Func<int, bool> isLineWon = i => (i & 0b11111) >= 0b11111;

            public bool IsWon => rows.Any(isLineWon) || cols.Any(isLineWon);

            public BingoBoard(IEnumerable<string> input) {
                this.board = new Dictionary<int, (int row, int col)>(
                    input.SelectMany((line, row) => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select((num, col) => new KeyValuePair<int, (int row, int col)>(int.Parse(num), (row, col)))
                ));
            }

            public void Mark(int num){
                if (!this.board.TryGetValue(num, out var pos)) return;

                this.marked.Add(num);
                this.cols[pos.col] |= (1 << pos.row);
                this.rows[pos.row] |= (1 << pos.col);
            }

            public IEnumerable<int> NonMarked => this.board.Keys.Except(this.marked);
        }
    }
}