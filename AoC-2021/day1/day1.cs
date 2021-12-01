using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace AoC_2021 {

    public class Day1 : IProblem {
        public string Name => "Day1";

        public string Part1() {
            var input = File.ReadAllLines("./day1/input.txt").Select((line) => int.Parse(line)).ToArray();

            var answer = input.Where((num, index) => index == 0 ? false : num > input[index - 1]).Count();

            return answer.ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day1/input.txt").Select((line) => int.Parse(line)).ToArray();

            var slidingWindowInput = input.Skip(2).Select((num, index) => input.Skip(index).Take(3).Sum()).ToArray();
            var answer = slidingWindowInput.Where((num, index) => index == 0 ? false : num > slidingWindowInput[index - 1]).Count();

            return answer.ToString();
        }
    }
}