namespace WordleSolver.ClassLibrary;

public enum CharacterValidation
{
    Wrong, // Letter does not exist in targeted word
    Exists, // Letter exists in targeted word, but not in this index
    Correct // Letter both exists and in the correct position
}