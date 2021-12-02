using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AoC_2021 {
    public class Day2 : IProblem {
        public string Name => "Day2";

        public string Part1() {
            var input = File.ReadAllLines("./Day2/input.txt");

            var finalPosition = input.Aggregate((horizontal: 0, depth: 0), (position, line) => {
                var instruction = line.Split(" ");
                var step = int.Parse(instruction.Last());
                switch (instruction.First()) {
                    case "forward":
                        position.horizontal += step;
                        return position;
                    case "up":
                        position.depth -= step;
                        return position;
                    case "down":
                        position.depth += step;
                        return position;
                    default: throw new ArgumentOutOfRangeException("line", instruction.First());
                }
            });

            return (finalPosition.horizontal * finalPosition.depth).ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./Day2/input.txt");
            var finalPosition = input.Aggregate((horizontal: 0, depth: 0, aim: 0), (position, line) => {
                var instruction = line.Split(" ");
                var step = int.Parse(instruction.Last());
                switch (instruction.First()) {
                    case "forward":
                        position.horizontal += step;
                        position.depth += position.aim * step;
                        return position;
                    case "up":
                        position.aim -= step;
                        return position;
                    case "down":
                        position.aim += step;
                        return position;
                    default: throw new ArgumentOutOfRangeException("line", instruction.First());
                }
            });

            return (finalPosition.horizontal * finalPosition.depth).ToString();
        }
    }
}