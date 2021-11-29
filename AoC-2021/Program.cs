using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace AoC_2021 {
    class Program {
        static async Task Main(string[] args) {
            var command = args.Any() ? args.First() : "run";

            switch (command) { 
                case "run":
                    RunProblems(args.Skip(1).FirstOrDefault()); 
                    return;

                case "download":
                    if (!int.TryParse(args.Skip(1).First(), out var day)) {
                        Console.WriteLine("Did you mean to download input for day {DateTime.Now:dd} (y/n)?");
                        if (Console.ReadKey().Key == ConsoleKey.Y) {
                            day = DateTime.Now.Day;
                        } else {
                            throw new ArgumentNullException("args[1]", "Please specify what day");
                        }
                    }

                    await DownloadAsync(day, int.Parse(args.Skip(2).FirstOrDefault()));
                    return;
                default: 
                    throw new ArgumentOutOfRangeException("args[0]", $"Cannot find sub-command {args.FirstOrDefault()}");
            }
        }

        static void RunProblems(string problemName) {
            var problems = typeof(Program)
                                   .Assembly
                                   .GetTypes()
                                   .Where(t => t.GetInterfaces().Any(it => it == typeof(IProblem)))
                                   .Select(t => (IProblem)Activator.CreateInstance(t))
                                   .Where(p => string.IsNullOrWhiteSpace(problemName) ? true : string.Equals(p.Name, problemName, StringComparison.InvariantCultureIgnoreCase))
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

        static async Task DownloadAsync(int day, int? year = null) {
            var inputUrl = $"https://adventofcode.com/{year ?? DateTime.UtcNow.Year}/day/{day}/input";
            var http = new HttpClient();

            Console.WriteLine($"Fetching input from {inputUrl}");
            var response = await http.GetAsync(inputUrl);
            if (response.IsSuccessStatusCode) {
                if (!Directory.Exists($"./day{day}/")) {
                    Directory.CreateDirectory($"./day{day}");
                }
                
                await File.WriteAllTextAsync($"./day{day}/input.txt", await response.Content.ReadAsStringAsync());

                Console.WriteLine($"Input saved at {$"./day{day}/input.txt"}");
            } else {
                Console.WriteLine($"Download from {inputUrl} failed: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        static (string, long elapsedMilliseconds) Time(Func<string> block) {
            try {
                sw.Start();
                var result = block.Invoke();
                sw.Stop();
                return (result, sw.ElapsedMilliseconds);
            }
            catch (Exception e) {
                 return (e.Message, sw.ElapsedMilliseconds);
            }
             finally {
                sw.Reset();
            }
        }

        static Stopwatch sw = new Stopwatch();
    }
}
