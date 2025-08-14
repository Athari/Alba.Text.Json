#if JSON10_0_OR_GREATER

using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JArray
{
    private static readonly MethodRef PRemoveAll = MethodRef.Of((JArray o) => o.RemoveAll(null!));

    private static readonly MethodRef PNodeRemoveRange = MethodRef.Of((JsonArray o) => o.RemoveRange(0, 0));

    public int RemoveAll(Func<object?, bool> match) =>
        Node.RemoveAll(n => match(JsonNode.ToJNodeOrValue(n, Options)));

    public void RemoveRange(int index, int count) =>
        Node.RemoveRange(index, count);
}

#endif