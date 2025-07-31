using System.Buffers;
using CommunityToolkit.Diagnostics;

namespace Alba.Text.Json.Dynamic;

internal sealed class FixedArrayBufferWriter<T> : IBufferWriter<T>
{
    private readonly T[] _buffer;

    public int Index { get; private set; }

    public FixedArrayBufferWriter(int size)
    {
        Guard.IsGreaterThanOrEqualTo(size, 0);
        _buffer = new T[size];
    }

    public ReadOnlyMemory<T> WrittenMemory => _buffer.AsMemory(0, Index);
    public ReadOnlySpan<T> WrittenSpan => _buffer.AsSpan(0, Index);

    public int Size => _buffer.Length;
    public int Free => _buffer.Length - Index;

    public void Clear()
    {
        _buffer.AsSpan(0, Index).Clear();
        ResetIndex();
    }

    public void ResetIndex()
    {
        Index = 0;
    }

    public void Advance(int count)
    {
        Guard.IsGreaterThanOrEqualTo(count, 0);
        if (Index > Size - count)
            throw new InvalidOperationException($"Buffer advanced past its size of {Size}.");
        Index += count;
    }

    public Memory<T> GetMemory(int sizeHint = 0)
    {
        Guard.IsLessThanOrEqualTo(sizeHint, Size - Index);
        return _buffer.AsMemory(Index);
    }

    public Span<T> GetSpan(int sizeHint = 0)
    {
        Guard.IsLessThanOrEqualTo(sizeHint, Size - Index);
        return _buffer.AsSpan(Index);
    }
}