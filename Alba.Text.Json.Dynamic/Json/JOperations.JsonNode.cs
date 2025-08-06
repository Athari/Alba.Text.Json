using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

internal static partial class JOperations
{
    [return: NotNullIfNotNull(nameof(node))]
    public static JsonNode? JsonNodeDeepClone(JsonNode? node)
    {
      #if JSON8_0_OR_GREATER
        return node?.DeepClone();
      #else
        return node.Deserialize<JsonNode?>();
      #endif
    }

    public static bool JsonNodeEquals(JsonNode? n1, object? v2, Equality equality, JNodeOptions options) =>
        v2 switch {
            JsonNode or null => JsonNodeEquals(n1, (JsonNode?)v2, equality, options),
            JNode n2 => JsonNodeEquals(n1, n2.NodeUntyped, equality, options),
            JsonElement el2 => JsonNodeEqualsJsonElement(n1, el2, equality, options),
            JsonDocument doc2 => JsonNodeEqualsJsonElement(n1, doc2.RootElement, equality, options),
            not null => JsonNodeEquals(n1, ValueToNewJsonNode(v2, n1?.Options ?? options.JsonNodeOptions), equality, options),
        };

    private static bool JsonNodeEquals(JsonNode? n1, JsonNode? n2, Equality equality, JNodeOptions options)
    {
      #if JSON9_0_OR_GREATER
        // try faster element comparison first
        if ((n1?.TryGetElementValue(out var el1) ?? false) && (n2?.TryGetElementValue(out var el2) ?? false))
            return JsonElementEquals(el1, el2, equality, options);
      #endif
        return WithNullCheck(
            (n1n, n2n, _) => equality switch {
                Equality.Deep => JsonNodeDeepEquals(n1n, n2n, options),
                Equality.Shallow => JsonNodeShallowEquals(n1n, n2n, options),
                Equality.Reference => JsonNodeReferenceEquals(n1n, n2n, options),
                _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
            },
            n1, n2, options);
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Conditional")]
    private static bool JsonNodeDeepEquals(JsonNode n1, JsonNode n2, JNodeOptions options)
    {
        if (JsonNodeReferenceEquals(n1, n2, options))
            return true;
      #if JSON8_0_OR_GREATER
        return JsonNode.DeepEquals(n1, n2);
      #else
        return (n1, n2) switch {
            (JsonObject o1, JsonObject o2) =>
                o1.Count == o2.Count &&
                o1.All(p => o2.TryGetPropertyValue(p.Key, out var n) && WithNullCheck(JsonNodeDeepEquals, p.Value, n, options)),
            (JsonArray a1, JsonArray a2) =>
                a1.Count == a2.Count &&
                a1.All((n, i) => WithNullCheck(JsonNodeDeepEquals, n, a2[i], options)),
            (JsonValue v1, JsonValue v2) =>
                JsonNodeValueEquals(v1, v2, options),
            _ => false,
        };
      #endif
    }

    private static bool JsonNodeShallowEquals(JsonNode n1, JsonNode n2, JNodeOptions options) =>
        JsonNodeReferenceEquals(n1, n2, options) ||
        (n1, n2) switch {
            (JsonValue v1, JsonValue v2) => JsonNodeValueEquals(v1, v2, options),
            _ => false,
        };

    private static bool JsonNodeReferenceEquals(JsonNode n1, JsonNode n2, JNodeOptions options) =>
        ReferenceEquals(n1, n2) ||
        n1.TryGetElementValue(out var el1) && n2.TryGetElementValue(out var el2) && JsonElementReferenceEquals(el1, el2, options);

    private static bool JsonNodeValueEquals(JsonValue v1, JsonValue v2, JNodeOptions options)
    {
        if (JsonNodeReferenceEquals(v1, v2, options))
            return true;
      #if JSON8_0_OR_GREATER
        return JsonNode.DeepEquals(v1, v2);
      #else
        var (k1, k2) = (v1.GetValueKind(), v2.GetValueKind());
        return k1 == k2 && k1 switch {
            JsonValueKind.Null or JsonValueKind.Undefined or JsonValueKind.True or JsonValueKind.False =>
                true,
            JsonValueKind.String =>
                string.Equals(v1.GetValue<string>(), v2.GetValue<string>(), StringComparison.Ordinal),
            JsonValueKind.Number =>
                Equals(JsonValueToValue(v1, options), JsonValueToValue(v2, options)),
            _ =>
                throw new InvalidOperationException($"Unexpected JsonValueKind of JsonNode: {k1}"),
        };
      #endif
    }

    private static bool JsonNodeIsNull(JsonNode node, JNodeOptions options) =>
        node.GetValueKind() switch {
            JsonValueKind.Null => true,
            JsonValueKind.Undefined => options.UndefinedValue == null,
            _ => false,
        };

    private static bool WithNullCheck(Func<JsonNode, JsonNode, JNodeOptions, bool> equalsFn,
        JsonNode? n1, JsonNode? n2, JNodeOptions options)
    {
        var (n1null, n2null) = (n1 == null || JsonNodeIsNull(n1, options), n2 == null || JsonNodeIsNull(n2, options));
        if (n1null && n2null)
            return true;
        else if (n1null || n2null)
            return false;
        return equalsFn(n1!, n2!, options);
    }

    public static int JsonNodeToHashCode(JsonNode node, Equality equality, JNodeOptions options) =>
        equality switch {
            Equality.Deep => JsonNodeToDeepHashCode(node, options),
            Equality.Shallow => JsonNodeToShallowHashCode(node, options),
            Equality.Reference => JsonNodeToReferenceHashCode(node),
            _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
        };

    private static int JsonNodeToDeepHashCode(JsonNode node, JNodeOptions? options)
    {
        int maxCount = options?.MaxHashCodeValueCount ?? int.MaxValue;
        int maxDepth = options?.MaxHashCodeDepth ?? int.MaxValue;

        var hash = 0;
        var count = 0;
        GetHashCodeProc(node, depth: 0);
        return hash;

        bool Add(object? value)
        {
            if (count >= maxCount)
                return false;
            else
                count++;
            hash = unchecked(hash * 397 ^ (value?.GetHashCode() ?? 0));
            return true;
        }

        void GetHashCodeProc(JsonNode? n, int depth)
        {
            if (n == null || !Add(n.GetType()))
                return;
            switch (n) {
                case JsonArray a:
                    if (depth < maxDepth)
                        foreach (var item in a)
                            GetHashCodeProc(item, depth + 1);
                    else
                        Add(a.Count);
                    break;
                case JsonObject o:
                    foreach (var (name, value) in o.OrderBy(p => p.Key, StringComparer.Ordinal)) {
                        if (!Add(name))
                            return;
                        if (depth < maxDepth)
                            GetHashCodeProc(value, depth + 1);
                    }
                    break;
                case JsonValue value:
                    Add(JsonValueToValue(value, JNodeOptions.Default));
                    break;
            }
        }
    }

    private static int JsonNodeToShallowHashCode(JsonNode node, JNodeOptions options) =>
        node switch {
            null => 0,
            JsonObject v => v.Count.GetHashCode(),
            JsonArray v => v.Count.GetHashCode(),
            JsonValue v => JsonValueToValue(v, options)?.GetHashCode() ?? 0,
            _ => throw new InvalidOperationException($"Unexpected node type: {node.GetType().Name}"),
        };

    private static int JsonNodeToReferenceHashCode(JsonNode node) =>
        RuntimeHelpers.GetHashCode(node);
}