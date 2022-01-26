using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using WordleSolver.ClassLibrary;

_ = BenchmarkRunner.Run<Benchy>();

[MemoryDiagnoser]
public class Benchy
{
    [Params("AALST", "WORST", "ABAMA", "PEPSI", "ZYNGA")]
    public string Word;

    private Solver _solver;
    private string[] possibleWords;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var words = File.ReadAllLines("FiveLetterWords.txt");
        possibleWords = words.Select(w => w.ToUpperInvariant()).ToArray();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _solver = new Solver(DefaultValidator, 5, possibleWords);
    }

    [Benchmark(Baseline = true)]
    public Task<Result> Solve() => _solver.SolveAsync();

    /// <summary>
    /// Character validation was made a dependency in case I wire it up to the Wordle website
    /// </summary>
    CharacterValidation DefaultValidator(int idx, char arg)
    {
        if (Word.Contains(char.ToUpperInvariant(arg)))
        {
            return Word[idx] == arg
                ? CharacterValidation.Correct
                : CharacterValidation.Exists;
        }

        return CharacterValidation.Wrong;
    }
}