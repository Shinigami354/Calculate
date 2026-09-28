namespace Calculator.Tests;

internal sealed class SequenceTextReader : TextReader
{
    private readonly Queue<string?> _lines;

    public SequenceTextReader(params string?[] lines)
    {
        _lines = new Queue<string?>(lines);
    }

    public override string? ReadLine()
    {
        return _lines.Count == 0 ? null : _lines.Dequeue();
    }
}
