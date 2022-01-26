using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WordleSolver.ClassLibrary;

namespace WordleSolver.Tests;

public class Tests
{
    private Solver _solver;
    private string _word;
    private string[] _words;

    [SetUp]
    public void Setup()
    {
        var words = File.ReadAllLines("FiveLetterWords.txt");
        _words = words.Select(w => w.ToUpperInvariant()).ToArray();
    }

    [TestCase(100, .8)]
    [TestCase(1000, .8)]
    public async Task SolverAccuracyTest(int iterations, double accuracy)
    {
        double wins = 0;
        double solves = 0;

        for (int i = 0; i < iterations; i++)
        {
            _word = _words[Random.Shared.Next(_words.Length)];
            _solver = new Solver(DefaultValidator, 5, 6, _words);

            var result = await _solver.SolveAsync();

            if (result.Win)
                wins++;

            if (result.Success)
                solves++;
        }

        Console.WriteLine($"Win rate: {wins / iterations:P}");
        Console.WriteLine($"Solve rate: {solves / iterations:P}");
        
        Assert.IsTrue(solves / iterations == 1);
        Assert.IsTrue(wins / iterations > accuracy);
    }

    private CharacterValidation DefaultValidator(int idx, char arg)
    {
        if (_word.Contains(char.ToUpperInvariant(arg)))
        {
            return _word[idx] == arg
                ? CharacterValidation.Correct
                : CharacterValidation.Exists;
        }

        return CharacterValidation.Wrong;
    }
}