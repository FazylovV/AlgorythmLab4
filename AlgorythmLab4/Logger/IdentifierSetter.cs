namespace AlgorythmLab4.Logger;

public static class IdentifierSetter
{
    private static int _currentIdentifier = 0;

    public static int GetId() => _currentIdentifier++;
}