using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
#if NET5_0_OR_GREATER
using System.Globalization;
#endif
#if !NET9_0_OR_GREATER
using static System.Text.Json.JsonElement;
#endif

namespace Alba.Text.Json.Dynamic;

internal static class JOperations
{
    private static readonly JsonValue NoJsonValue = Ensure.NotNull(JsonValue.Create("<NOVALUE>"));
  #if NET5_0_OR_GREATER
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;
  #endif

    public static bool ValueToJsonValueNode<T>(T value,
        [NotNullIfNotNull(nameof(value))] out JsonValue? valueNode, JsonNodeOptions? opts)
    {
        valueNode = value switch {
            null => null,
            // already JsonValue
            JsonValue v => v,
            // types stored as JsonValuePrimitive<T> (explicit ctor)
            bool v => JsonValue.Create(v, opts),
            char v => JsonValue.Create(v, opts),
            sbyte v => JsonValue.Create(v, opts),
            byte v => JsonValue.Create(v, opts),
            short v => JsonValue.Create(v, opts),
            ushort v => JsonValue.Create(v, opts),
            int v => JsonValue.Create(v, opts),
            uint v => JsonValue.Create(v, opts),
            long v => JsonValue.Create(v, opts),
            ulong v => JsonValue.Create(v, opts),
            float v => JsonValue.Create(v, opts),
            double v => JsonValue.Create(v, opts),
            decimal v => JsonValue.Create(v, opts),
            DateTime v => JsonValue.Create(v, opts),
            DateTimeOffset v => JsonValue.Create(v, opts),
            Guid v => JsonValue.Create(v, opts),
            // types stored as JsonValuePrimitive<T> (no explicit ctor)
            TimeSpan or Uri or Version => JsonValue.Create(value, opts),
          #if NET5_0_OR_GREATER
            Half => JsonValue.Create(value, opts),
          #endif
          #if NET6_0_OR_GREATER
            DateOnly or TimeOnly => JsonValue.Create(value, opts),
          #endif
          #if NET7_0_OR_GREATER
            Int128 or UInt128 => JsonValue.Create(value, opts),
          #endif
            // convert to JsonValueOfElement if it's a value
            JsonElement el => el.ValueKind is not (JsonValueKind.Object or JsonValueKind.Array)
                ? JsonValue.Create(el, opts)
                : NoJsonValue,
            // everything else isn't a value
            _ => NoJsonValue,
        };
        return !ReferenceEquals(valueNode, NoJsonValue);
    }

    public static JsonNode? ValueToNewJsonNode<T>(T value, JsonNodeOptions? opts) =>
        ValueToJsonValueNode(value, out var valueNode, opts)
            ? valueNode
            : value switch {
                // already JNode, clone if used within another tree
                JNode { NodeUntyped: var v } => v.Parent == null ? v : JsonNodeDeepClone(v),
                // already JsonNode
                JsonNode v => v,
                // element is always stored as JsonValueOfElement
                JsonElement v => JsonValue.Create(v, opts),
                // serialize everything else into node
                JsonDocument v => JsonSerializer.SerializeToNode(v),
                _ => JsonSerializer.SerializeToNode(value),
            };

    [return: NotNullIfNotNull(nameof(node))]
    public static object? JsonNodeToJNodeOrValue(JsonNode? node, JNodeOptions options) =>
        //WriteLine($"{d.GetType().Name}: {node?.GetType().Name} => {o?.GetType().Name}");
        node switch {
            null => null,
            JsonValue v => JsonValueToValue(v, options),
            JsonObject v => new JObject(v, options),
            JsonArray v => new JArray(v, options),
            _ => throw new InvalidOperationException($"Unexpected JsonNode: {node.GetType()}"),
        };

    private static object? JsonValueToValue(JsonValue v, JNodeOptions options) =>
        v.GetValueKind() switch {
            // return primitive values directly
            JsonValueKind.Undefined => options.UndefinedValue,
            JsonValueKind.Null => null,
            JsonValueKind.String => v.GetValue<string>(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            // numbers can be stored as JsonElement pointing to barely parsed binary data
            JsonValueKind.Number => v.TryGetElementValue(out var el) ? JsonElementToNumber(el, options) : v.GetValue<object>(),
            // objects can be JsonElement or an arbitrary user type
            JsonValueKind.Object or JsonValueKind.Array
                or (JsonValueKind)byte.MaxValue => // from JsonNodeExts
                v.TryGetElementValue(out var el)
                    // return value of wrapped JsonElement
                    ? JsonElementToValue(el, options)
                    // return raw objects stored inside
                    : v.GetValue<object>(),
            _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonNode: {v}"),
        };

    private static object? JsonElementToValue(in JsonElement el, JNodeOptions options) =>
        el.ValueKind switch {
            JsonValueKind.Undefined => options.UndefinedValue,
            JsonValueKind.Null => null,
            JsonValueKind.String => el.GetString(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number => JsonElementToNumber(el, options),
            JsonValueKind.Object or JsonValueKind.Array =>
                throw new InvalidOperationException($"{el.ValueKind} JsonElement in JsonValue"),
            _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {el.ValueKind}"),
        };

    private static object JsonElementToNumber(in JsonElement el, JNodeOptions options) =>
        IsFloatingPoint(el.GetRawValueSpan()) ?
            JsonElementToNumberType(el, options.FloatTypes) ??
            throw new InvalidOperationException($"Cannot convert {el} to a floating point number.") :
            JsonElementToNumberType(el, options.IntegerTypes) ??
            throw new InvalidOperationException($"Cannot convert {el} to an integer number.");

    private static object? JsonElementToNumberType(JsonElement el, NumberType[] types) =>
        types.Select(t => JsonElementToNumberType(el, t)).FirstOrDefault(v => v != null);

    [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "Intentional")]
    private static object? JsonElementToNumberType(in JsonElement el, NumberType type) =>
        type switch {
            NumberType.SByte => el.TryGetSByte(out var v) ? v : null,
            NumberType.Byte => el.TryGetByte(out var v) ? v : null,
            NumberType.Int16 => el.TryGetInt16(out var v) ? v : null,
            NumberType.UInt16 => el.TryGetUInt16(out var v) ? v : null,
            NumberType.Int32 => el.TryGetInt32(out var v) ? v : null,
            NumberType.UInt32 => el.TryGetUInt32(out var v) ? v : null,
            NumberType.Int64 => el.TryGetInt64(out var v) ? v : null,
            NumberType.UInt64 => el.TryGetUInt64(out var v) ? v : null,
            NumberType.Single => el.TryGetSingle(out var v) ? v : null,
            NumberType.Double => el.TryGetDouble(out var v) ? v : null,
            NumberType.Decimal => el.TryGetDecimal(out var v) ? v : null,
          #if NET8_0_OR_GREATER
            NumberType.Half => Half.TryParse(el.GetRawValueSpan(), Invariant, out var v) ? v : null,
          #elif NET5_0_OR_GREATER
            NumberType.Half => Half.TryParse(el.GetRawText(), NumberStyles.Float | NumberStyles.AllowThousands, Invariant, out var v) ? v : null,
          #endif
          #if NET8_0_OR_GREATER
            NumberType.Int128 => Int128.TryParse(el.GetRawValueSpan(), Invariant, out var v) ? v : null,
            NumberType.UInt128 => UInt128.TryParse(el.GetRawValueSpan(), Invariant, out var v) ? v : null,
          #elif NET7_0_OR_GREATER
            NumberType.Int128 => Int128.TryParse(el.GetRawText(), Invariant, out var v) ? v : null,
            NumberType.UInt128 => UInt128.TryParse(el.GetRawText(), Invariant, out var v) ? v : null,
          #endif
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"A number or string NumberKind expected, got {type}"),
        };

    private static bool IsFloatingPoint(ReadOnlySpan<byte> span) =>
        span.IndexOf((byte)'.') switch {
            -1 => span.IndexOfAny((byte)'e', (byte)'E') switch {
                -1 => false,
                var i => i + 1 < span.Length && span[i + 1] == (byte)'-',
            },
            _ => true,
        };

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

    public static bool JsonNodeEquals(JsonNode? n1, JsonNode? n2, Equality equality, JNodeOptions options)
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

    public static bool JsonElementEquals(in JsonElement el1, in JsonElement el2, Equality equality, JNodeOptions options) =>
        equality switch {
            Equality.Deep => JsonElementDeepEquals(el1, el2, options),
            Equality.Shallow => JsonElementShallowEquals(el1, el2, options),
            Equality.Reference => JsonElementReferenceEquals(el1, el2, options),
            _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
        };

    [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Conditional")]
    private static bool JsonElementDeepEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options)
    {
        if (JsonElementReferenceEquals(el1, el2, options))
            return true;
      #if JSON9_0_OR_GREATER
        return JsonElement.DeepEquals(el1, el2);
      #else
        var (k1, k2) = (el1.ValueKind, el2.ValueKind);
        if (k1 != k2)
            return false;
        switch (k1) {
            case JsonValueKind.Object: {
                var it1 = el1.EnumerateObject();
                var it2 = el2.EnumerateObject();
                while (it1.MoveNext()) {
                    if (!it2.MoveNext())
                        return false;
                    var (prop1, prop2) = (it1.Current, it2.Current);
                    if (!prop1.NameEquals(prop2.Name))
                        return UnorderedObjectDeepEquals(it1, it2, options);
                    if (!JsonElementDeepEquals(prop1.Value, prop2.Value, options))
                        return false;
                }
                return !it2.MoveNext();
            }
            case JsonValueKind.Array: {
                if (el1.GetArrayLength() != el2.GetArrayLength())
                    return false;
                var it2 = el2.EnumerateArray();
                foreach (JsonElement item in el1.EnumerateArray())
                    if (!it2.MoveNext() ||
                        !JsonElementDeepEquals(item, it2.Current, options))
                        return false;
                return !it2.MoveNext();
            }
            default:
                return JsonElementValueEquals(el1, el2, options);
        }

        static bool UnorderedObjectDeepEquals(ObjectEnumerator it1, ObjectEnumerator it2, JNodeOptions options)
        {
            var d2 = new Dictionary<string, ValueQueue<JsonElement>>(StringComparer.Ordinal);
          #if NET6_0_OR_GREATER
            do {
                var prop2 = it2.Current;
                d2.GetValueRefOrAddDefault(prop2.Name).Enqueue(prop2.Value);
            } while (it2.MoveNext());
            do {
                var prop1 = it1.Current;
                ref var values = ref d2.GetValueRefOrNullRef(prop1.Name, out var exists);
                if (!exists ||
                    !values.TryDequeue(out var value) ||
                    !JsonElementDeepEquals(prop1.Value, value, options))
                    return false;
            } while (it1.MoveNext());
          #else
            do {
                var prop2 = it2.Current;
                d2.TryGetValue(prop2.Name, out var values);
                values.Enqueue(prop2.Value);
                d2[prop2.Name] = values;
            } while (it2.MoveNext());
            do {
                var prop1 = it1.Current;
                if (!d2.TryGetValue(prop1.Name, out var values) ||
                    !values.TryDequeue(out var value) ||
                    !JsonElementDeepEquals(prop1.Value, value, options))
                    return false;
                d2[prop1.Name] = values;
            } while (it1.MoveNext());
          #endif
            return true;
        }
      #endif
    }

    private static bool JsonElementShallowEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options) =>
        JsonElementReferenceEquals(el1, el2, options) ||
        (el1.ValueKind, el2.ValueKind) switch {
            var (k1, k2) => k1 == k2 && JsonElementValueEquals(el1, el2, options),
        };

    [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Consistency")]
    private static bool JsonElementReferenceEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options) =>
        JsonElementExts.DocumentOffsetEquals(el1, el2);

    private static bool JsonElementValueEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options)
    {
        if (JsonElementReferenceEquals(el1, el2, options))
            return true;
      #if JSON9_0_OR_GREATER
        return JsonElement.DeepEquals(el1, el2);
      #else
        var (k1, k2) = (el1.ValueKind, el2.ValueKind);
        return k1 == k2 && k1 switch {
            JsonValueKind.Null or JsonValueKind.Undefined or JsonValueKind.True or JsonValueKind.False =>
                true,
            JsonValueKind.String =>
                el1.ValueEquals(el2.GetString()),
            JsonValueKind.Number =>
                Equals(JsonElementToValue(el1, options), JsonElementToValue(el2, options)),
            _ =>
                throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {k1}"),
        };
      #endif
    }

    private static bool JsonElementIsNull(in JsonElement el, JNodeOptions options) =>
        el.ValueKind switch {
            JsonValueKind.Null => true,
            JsonValueKind.Undefined => options.UndefinedValue == null,
            _ => false,
        };

    public static bool JsonNodeEqualsJsonElement(JsonNode? n1, JsonElement el2, Equality equality, JNodeOptions options)
    {
        var el1null = n1 == null || JsonNodeIsNull(n1, options);
        var el2null = JsonElementIsNull(el2, options);
        if (el1null && el2null)
            return true;
        else if (el1null || el2null)
            return false;

        var n1n = n1!;
        if (n1n.TryGetElementValue(out var el1))
            return JsonElementEquals(el1, el2, equality, options);

        // Let's hope non-element JsonValue isn't compared with JsonElement often and just deserialize
        var doc1 = n1n.Deserialize<JsonDocument>();
        if (doc1?.RootElement != null)
            return JsonElementEquals(doc1.RootElement, el2, equality, options);

        // Can doc be null here?
        return el2null;
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

    public static int JsonNodeListIndexOf<T>(IEnumerable<JsonNode?> nodes, T value, JNodeOptions options) =>
        value switch {
            JsonNode node => JsonNodeListIndexOfJsonNode(nodes, node, options),
            JNode node => JsonNodeListIndexOfJsonNode(nodes, node.NodeUntyped, options),
            JsonElement el => JsonNodeListIndexOfJsonElement(nodes, el, options),
            JsonDocument doc => JsonNodeListIndexOfJsonElement(nodes, doc.RootElement, options),
            _ => ValueToJsonValueNode(value, out var node, options.JsonNodeOptions)
                ? JsonNodeListIndexOfJsonNode(nodes, node, options) : -1,
        };

    private static int JsonNodeListIndexOfJsonNode(IEnumerable<JsonNode?> nodes, JsonNode? node, JNodeOptions options) =>
        nodes.IndexOf(n => JsonNodeEquals(n, node, options.SearchEquality, options));

    private static int JsonNodeListIndexOfJsonElement(IEnumerable<JsonNode?> nodes, JsonElement el, JNodeOptions options) =>
        nodes.IndexOf(n => JsonNodeEqualsJsonElement(n, el, options.SearchEquality, options));

    public static bool JsonNodeListContains<T>(IEnumerable<JsonNode?> nodes, T value, JNodeOptions options) =>
        JsonNodeListIndexOf(nodes, value, options) != -1;

    public static bool JsonNodeListRemove<T>(IList<JsonNode?> nodes, T value, JNodeOptions options)
    {
        var index = JsonNodeListIndexOf(nodes, value, options);
        if (index == -1)
            return false;
        nodes.RemoveAt(index);
        return true;
    }
}