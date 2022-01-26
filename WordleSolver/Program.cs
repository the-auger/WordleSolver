using System.Diagnostics;
using WordleSolver.ClassLibrary;

Console.Title = "Wordle Solver";

Console.Write("Give me a 5-letter word: ");
var word = Console.ReadLine()!.ToUpperInvariant();

var words = File.ReadAllLines("FiveLetterWords.txt");

var solver = new Solver(DefaultValidator,
    words.Select(w => w.ToUpperInvariant()).ToArray(),
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

CharacterValidation DefaultValidator(int idx, char arg)
{
    if (word.Contains(char.ToUpperInvariant(arg)))
    {
        return word[idx] == arg
            ? CharacterValidation.Correct
            : CharacterValidation.Exists;
    }

    return CharacterValidation.Wrong;
}