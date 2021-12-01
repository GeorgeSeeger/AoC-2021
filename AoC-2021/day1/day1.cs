using System.IO;
using System.Linq;

namespace AoC_2021 {

    public class Day1 : IProblem {
        public string Name => "Day1";

        public string Part1() {
            var input = File.ReadAllLines("./day1/input.txt").Select(int.Parse).ToArray();

            var answer = input.Skip(1).Where((num, index) => num > input[index]).Count();

            return answer.ToString();
        }

        public string Part2() {
            var input = File.ReadAllLines("./day1/input.txt").Select(int.Parse).ToArray();

            var sumOfPreviousThree = input.Skip(2).Select((num, index) => input.Skip(index).Take(3).Sum()).ToArray();
            var answer = sumOfPreviousThree.Skip(1).Where((num, index) => num > sumOfPreviousThree[index]).Count();

            return answer.ToString();
        }
    }
}