namespace WordleSolver;

public record Result(bool Success, int Tries, string Guess);

public sealed class WordleSolver
{
    private const int MAX_LENGTH = 5;
    private readonly Func<int, char, CharacterValidation> _validator;
    private readonly Action<string> _output;
    private readonly char[] _correctCharacters = new char[MAX_LENGTH];
    private readonly HashSet<char> _wrongCharacters = new();
    private readonly LinkedList<string> _possibleWords;
    private int _tries;

    public enum CharacterValidation
    {
        Wrong, // Letter does not exist in targeted word
        Exists, // Letter exists in targeted word, but not in this index
        Correct // Letter both exists and in the correct position
    }

    public WordleSolver(Func<int, char, CharacterValidation> validator, IReadOnlyCollection<string> possibleWords, Action<string> output)
    {
        _validator = validator;
        _output = output;
        _possibleWords = new LinkedList<string>(possibleWords);
    }

    public Task<Result> SolveAsync()
    {
        bool success = true;

        while (true)
        {
            var currentGuess = _tries == 0 ? I_AM_CHEATING : _possibleWords.First();

            _output($"Guessing: {currentGuess}");

            EvaluatePossibleWord(currentGuess);

            if (CheckIfCorrect())
                break;
            
            PruneWords(currentGuess);

            if (_possibleWords.Any())
                continue;

            success = false;
            break;
        }

        return Task.FromResult(new Result(success,
            _tries, 
            success
            ? string.Join(string.Empty, _correctCharacters)
            : string.Join(string.Empty, _correctCharacters.Select(c => char.IsLetter(c) ? c : '?'))));
    }

    private bool CheckIfCorrect() => _correctCharacters.All(char.IsLetter);
    
    private void PruneWords(string latestTry)
    {
        var pruneQueue = new Queue<string>();
        pruneQueue.Enqueue(latestTry);

        foreach (var possibleWord in _possibleWords)
        {
            for (int i = 0; i < possibleWord.Length; i++)
            {
                var currentChar = possibleWord[i];

                if (_wrongCharacters.Contains(currentChar)
                    || _correctCharacters[i] != default
                    && _correctCharacters[i] != currentChar)
                {
                    pruneQueue.Enqueue(possibleWord);
                    break;
                }
            }
        }

        _output($"Pruning {pruneQueue.Count} of {_possibleWords.Count + 1} ({pruneQueue.Count / (double)(_possibleWords.Count + 1):P0})");

        foreach (var pruneWord in pruneQueue)
        {
            _possibleWords.Remove(pruneWord);
        }
    }

    private void EvaluatePossibleWord(string currentGuess)
    {
        _tries++;
        var guessAsSpan = currentGuess.AsSpan();

        for (int i = 0; i < guessAsSpan.Length; i++)
        {
            var result = _validator(i, guessAsSpan[i]);

            switch (result)
            {
                case CharacterValidation.Wrong:
                    _wrongCharacters.Add(guessAsSpan[i]);
                    break;
                case CharacterValidation.Exists:
                    // TODO: NOP
                    break;
                case CharacterValidation.Correct:
                    _correctCharacters[i] = guessAsSpan[i];
                    break;
            }
        }
    }

    private const string I_AM_CHEATING = "AEIOU";
}