using System.Diagnostics;

var word = "worse".ToUpperInvariant();
var words = File.ReadAllLines("FiveLetterWords.txt");
var solver = new WordleSolver.WordleSolver(TestValidator,
    words.Select(word => word.ToUpperInvariant()).ToArray(),
    Console.WriteLine);

try
{

    var sw = new Stopwatch();

    sw.Start();
    var result = await solver.SolveAsync();
    sw.Stop();

    Console.WriteLine(result);
    Console.WriteLine($"Duration: {sw.Elapsed}");
}
catch (ApplicationException e)
{
    Console.WriteLine(e.Message);
}

Console.WriteLine($"Word was: {word}");

WordleSolver.WordleSolver.CharacterValidation TestValidator(int idx, char arg)
{
    if (word.Contains(char.ToUpperInvariant(arg)))
    {
        return word[idx] == arg
            ? WordleSolver.WordleSolver.CharacterValidation.Correct
            : WordleSolver.WordleSolver.CharacterValidation.Exists;
    }

    return WordleSolver.WordleSolver.CharacterValidation.Wrong;
}