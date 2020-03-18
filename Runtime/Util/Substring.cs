/// <summary>
/// Represents part of a larger string.
/// </summary>
public readonly struct Substring
{
    public string FullString { get; }

    public int Start { get; }
    public int Length { get; }
    public int End => Start + Length;

    public Substring(string fullString, int start, int length)
    {
        FullString = fullString;
        Start = start;
        Length = length;
    }

    public override string ToString() => FullString.Substring(Start, Length);

    public bool Contains(int i)
    {
        return Start <= i && i < End;
    }
}
