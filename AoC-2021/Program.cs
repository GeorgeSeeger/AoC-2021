using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC_2021 {
    class Program {
        static void Main(string[] args) {
            var problemName = args.Any() ? args.First() : string.Empty;

            var problems = typeof(Program)
                                   .Assembly
                                   .GetTypes()
                                   .Where(t => t.GetInterfaces().Any(it => it == typeof(IProblem)))
                                   .Select(t => (IProblem)Activator.CreateInstance(t))
                                   .Where(p => string.IsNullOrWhiteSpace(problemName) ? true : string.Equals(p.Name, problemName, StringComparison.InvariantCultureIgnoreCase))
                                   .OrderBy(p => int.Parse(new Regex("\\D").Replace(p.Name, "")))
                                   .ToArray();

            foreach (var problem in problems) {
                var (firstPart, elapsedMillisecondsPt1) = Time(() => problem.Part1());
                if (!string.IsNullOrWhiteSpace(firstPart))
                    Console.WriteLine($"{problem.Name} Part 1: {firstPart} ({elapsedMillisecondsPt1}ms)");

                var (secondPart, elapsedMillisecondsPt2) = Time(() => problem.Part2());
                if (!string.IsNullOrWhiteSpace(secondPart))
                    Console.WriteLine($"{problem.Name} Part 2: {secondPart} ({elapsedMillisecondsPt2}ms)");
            }
        }

        static (string result, long elapsedMilliseconds) Time<T>(Func<T> block) {
            try {
                sw.Start();
                var result = block.Invoke();
                sw.Stop();
                return (result.ToString(), sw.ElapsedMilliseconds);
            }
            catch (Exception e) {
                if (sw.IsRunning) sw.Stop();
                return (e.Message, sw.ElapsedMilliseconds);
            }
            finally {
                sw.Reset();
            }
        }

        static Stopwatch sw = new Stopwatch();
    }
}
