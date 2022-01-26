namespace WordleSolver.ClassLibrary;

public sealed class Solver
{
    private readonly Func<int, char, CharacterValidation> _validator;
    private readonly int _maxWordLength;
    private readonly Action<string>? _output;
    private readonly char[] _correctCharacters;
    private readonly HashSet<char> _wrongCharacters = new();
    private readonly LinkedList<string> _possibleWords;
    private readonly double _initialSize;
    private int _tries;

    public Solver(Func<int, char, CharacterValidation> validator,
        int maxWordLength,
        IReadOnlyCollection<string> possibleWords,
        Action<string> output = null)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _maxWordLength = maxWordLength;
        _correctCharacters = new char[_maxWordLength];
        _possibleWords = new LinkedList<string>(possibleWords ?? throw new ArgumentNullException(nameof(possibleWords)));
        _output = output;
        _initialSize = _possibleWords.Count;
    }

    public Task<Result> SolveAsync()
    {
        bool success = true;

        while (true)
        {
            var currentGuess = _possibleWords.First();

            _output?.Invoke($"Guessing: {currentGuess} ({_possibleWords.Count / _initialSize:P} words left)");

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

        EnqueueWordsToPurgeQueueIfInvalid();

        _output?.Invoke($"Pruning {pruneQueue.Count} of {_possibleWords.Count + 1} ({pruneQueue.Count / (double)(_possibleWords.Count + 1):P0})");

        foreach (var pruneWord in pruneQueue)
        {
            _possibleWords.Remove(pruneWord);
        }

        void EnqueueWordsToPurgeQueueIfInvalid()
        {
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
}