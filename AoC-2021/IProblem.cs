namespace AoC_2021 {
    public interface IProblem {
        string Name { get; }

        string Part1();

        string Part2();
    }
}