using System.Diagnostics;
using WordleSolver.ClassLibrary;

Console.Title = "Wordle Solver";
string word;
var words = File.ReadAllLines("FiveLetterWords.txt");

Console.WriteLine($"Loaded {words.Length:N0} words");

while (true)
{
    Console.Write("Give me a 5-letter word: ");
    word = Console.ReadLine()?.ToUpperInvariant();

    if (word?.Length != 5) continue;

    var solver = new Solver(DefaultValidator,
        5,
        words.Select(w => w.ToUpperInvariant()).ToArray(),
        Console.WriteLine);

    try
    {

        var sw = new Stopwatch();

        sw.Start();
        var result = await solver.SolveAsync();
        sw.Stop();

        Console.ForegroundColor = result.Success ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(result);
        Console.ResetColor();

        Console.WriteLine($"Duration: {sw.Elapsed}");
    }
    catch (ApplicationException e)
    {
        Console.WriteLine(e.Message);
    }

    Console.WriteLine($"Word was: {word}");
}

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