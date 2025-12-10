#if JSON10_0_OR_GREATER

using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JArray
{
    private static readonly MethodRef PRemoveAll = MethodRef.Of((JArray o) => o.RemoveAll(null!));

    private static readonly MethodRef PNodeRemoveRange = MethodRef.Of((JsonArray o) => o.RemoveRange(0, 0));

    /// <summary>Removes all the values that match the conditions defined by the specified predicate.</summary>
    /// <param name="match">The predicate that defines the conditions of the values to remove.</param>
    /// <returns>The number of values removed from the <see cref="JsonArray"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
    public int RemoveAll(Func<object?, bool> match) =>
        Node.RemoveAll(n => match(n.ToJNodeOrValue(Options)));

    /// <summary>Removes a range of values from the <see cref="JsonArray"/>.</summary>
    /// <param name="index">The zero-based starting index of the range of values to remove.</param>
    /// <param name="count">The number of values to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than 0.</exception>
    /// <exception cref="ArgumentException"><paramref name="index"/> and <paramref name="count"/> do not denote a valid range of values in the <see cref="JsonArray"/>.</exception>
    public void RemoveRange(int index, int count) =>
        Node.RemoveRange(index, count);

    /// <summary>Removes a range of values from the <see cref="JsonArray"/>.</summary>
    /// <param name="index">The starting index of the range of values to remove, which is either from the beginning or the end of the array.</param>
    /// <param name="count">The number of values to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than 0.</exception>
    /// <exception cref="ArgumentException"><paramref name="index"/> and <paramref name="count"/> do not denote a valid range of values in the <see cref="JsonArray"/>.</exception>
    public void RemoveRange(Index index, int count) =>
        Node.RemoveRange(index.GetOffset(Node.Count), count);
}

#endif