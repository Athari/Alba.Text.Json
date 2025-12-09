using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
#if !JSON8_0_OR_GREATER
using Alba.Framework;
#endif
using C = System.TypeCode;

namespace Alba.Text.Json.Dynamic.Extensions;

[SuppressMessage("Naming", "CA1708: Identifiers should differ by more than case", Justification = "Compiler bug")]
public static class JsonNodeExts
{
    extension(JsonNode @this)
    {
        public JsonValueKind DataValueKind
        {
            get
            {
                switch (@this) {
                    case null:
                        return JsonValueKind.Null;
                    case JsonObject:
                        return JsonValueKind.Object;
                    case JsonArray:
                        return JsonValueKind.Array;
                    case JsonValue value: {
                        if (value.TryGetValue(out JsonElement el))
                            return el.ValueKind;
                        object obj = value.GetValue<object>();
                        switch (Convert.GetTypeCode(obj)) {
                            case C.Empty:
                                return JsonValueKind.Null;
                            case C.Boolean:
                                return (bool)obj ? JsonValueKind.True : JsonValueKind.False;
                            case C.String:
                                return JsonValueKind.String;
                            case C.SByte or C.Byte
                                or C.Int16 or C.UInt16 or C.Int32 or C.UInt32 or C.Int64 or C.UInt64
                                or C.Single or C.Double or C.Decimal:
                                return JsonValueKind.Number;
                        }
                        switch (obj) {
                          #if NET5_0_OR_GREATER
                            case Half:
                                return JsonValueKind.Number;
                          #endif
                          #if NET7_0_OR_GREATER
                            case Int128 or UInt128:
                                return JsonValueKind.Number;
                          #endif
                            default:
                                break;
                        }
                        // Wrapped object or array
                        return (JsonValueKind)byte.MaxValue;
                    }
                    default:
                        throw new ArgumentException($"Unexpected JsonNode type: {@this.GetType().Name}");
                }
            }
        }

        public bool TryGetElementValue(out JsonElement el)
        {
            if ((@this as JsonValue)?.TryGetValue(out el) ?? false)
                return true;
            el = default;
            return false;
        }

        public static bool EqualsJsonElement(JsonNode? n1, JsonElement el2, Equality equality, JNodeOptions options)
        {
            var el1null = n1 == null || JsonNode.IsNull(n1, options);
            var el2null = el2.IsNull(options);
            if (el1null && el2null)
                return true;
            else if (el1null || el2null)
                return false;

            var n1n = n1!;
            if (n1n.TryGetElementValue(out var el1))
                return JsonElement.Equals(el1, el2, equality, options);

            // Let's hope non-element JsonValue isn't compared with JsonElement often and just deserialize
            var doc1 = n1n.Deserialize<JsonDocument>();
            if (doc1?.RootElement != null)
                return JsonElement.Equals(doc1.RootElement, el2, equality, options);

            // Can doc be null here?
            return el2null;
        }

        public static bool Equals(JsonNode? n1, object? v2, Equality equality, JNodeOptions options) =>
            v2 switch {
                JsonNode or null => JsonNode.Equals(n1, (JsonNode?)v2, equality, options),
                JNode n2 => JsonNode.Equals(n1, n2.NodeUntyped, equality, options),
                JsonElement el2 => JsonNode.EqualsJsonElement(n1, el2, equality, options),
                JsonDocument doc2 => JsonNode.EqualsJsonElement(n1, doc2.RootElement, equality, options),
                not null => JsonNode.Equals(n1, v2.ToJsonNode(n1?.Options ?? options.JsonNodeOptions), equality, options),
            };

        private static bool Equals(JsonNode? n1, JsonNode? n2, Equality equality, JNodeOptions options)
        {
          #if JSON9_0_OR_GREATER
            // try faster element comparison first
            if ((n1?.TryGetElementValue(out var el1) ?? false) && (n2?.TryGetElementValue(out var el2) ?? false))
                return JsonElement.Equals(el1, el2, equality, options);
          #endif
            return WithNullCheck(
                (n1n, n2n, _) => equality switch {
                    Equality.Deep => JsonNode.DeepEquals(n1n, n2n, options),
                    Equality.Shallow => JsonNode.ShallowEquals(n1n, n2n, options),
                    Equality.Reference => JsonNode.ReferenceEquals(n1n, n2n, options),
                    _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
                },
                n1, n2, options);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Conditional")]
        private static bool DeepEquals(JsonNode n1, JsonNode n2, JNodeOptions options)
        {
            if (JsonNode.ReferenceEquals(n1, n2, options))
                return true;
          #if JSON8_0_OR_GREATER
            return JsonNode.DeepEquals(n1, n2);
          #else
            return (n1, n2) switch {
                (JsonObject o1, JsonObject o2) =>
                    o1.Count == o2.Count &&
                    o1.All(p => o2.TryGetPropertyValue(p.Key, out var n) && WithNullCheck(JsonNode.DeepEquals, p.Value, n, options)),
                (JsonArray a1, JsonArray a2) =>
                    a1.Count == a2.Count &&
                    a1.All((n, i) => WithNullCheck(JsonNode.DeepEquals, n, a2[i], options)),
                (JsonValue v1, JsonValue v2) =>
                    JsonNode.ValueEquals(v1, v2, options),
                _ => false,
            };
          #endif
        }

        private static bool ShallowEquals(JsonNode n1, JsonNode n2, JNodeOptions options) =>
            JsonNode.ReferenceEquals(n1, n2, options) ||
            (n1, n2) switch {
                (JsonValue v1, JsonValue v2) => JsonNode.ValueEquals(v1, v2, options),
                _ => false,
            };

        private static bool ReferenceEquals(JsonNode n1, JsonNode n2, JNodeOptions options) =>
            ReferenceEquals(n1, n2) ||
            n1.TryGetElementValue(out var el1) && n2.TryGetElementValue(out var el2) && JsonElement.Equals(el1, el2, Equality.Reference, options);

        private static bool ValueEquals(JsonValue v1, JsonValue v2, JNodeOptions options)
        {
            if (JsonNode.ReferenceEquals(v1, v2, options))
                return true;
          #if JSON8_0_OR_GREATER
            return JsonNode.DeepEquals(v1, v2);
          #else
            var (k1, k2) = (v1.DataValueKind, v2.DataValueKind);
            return k1 == k2 && k1 switch {
                JsonValueKind.Null or JsonValueKind.Undefined or JsonValueKind.True or JsonValueKind.False =>
                    true,
                JsonValueKind.String =>
                    string.Equals(v1.GetValue<string>(), v2.GetValue<string>(), StringComparison.Ordinal),
                JsonValueKind.Number =>
                    Equals(v1.ToValue(options), v2.ToValue(options)),
                _ =>
                    throw new InvalidOperationException($"Unexpected JsonValueKind of JsonNode: {k1}"),
            };
          #endif
        }

        private static bool IsNull(JsonNode node, JNodeOptions options) =>
            node.DataValueKind switch {
                JsonValueKind.Null => true,
                JsonValueKind.Undefined => options.UndefinedValue == null,
                _ => false,
            };

        [SuppressMessage("Style", "IDE0051", Justification = "C #14 Bug")]
        private static bool WithNullCheck(Func<JsonNode, JsonNode, JNodeOptions, bool> equalsFn,
            JsonNode? n1, JsonNode? n2, JNodeOptions options)
        {
            var (n1null, n2null) = (n1 == null || JsonNode.IsNull(n1, options), n2 == null || JsonNode.IsNull(n2, options));
            if (n1null && n2null)
                return true;
            else if (n1null || n2null)
                return false;
            return equalsFn(n1!, n2!, options);
        }

        public int GetHashCode(Equality equality, JNodeOptions options) =>
            equality switch {
                Equality.Deep => @this.GetDeepHashCode(options),
                Equality.Shallow => @this.GetShallowHashCode(options),
                Equality.Reference => @this.GetReferenceHashCode(),
                _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
            };

        private int GetDeepHashCode(JNodeOptions? options)
        {
            int maxCount = options?.MaxHashCodeValueCount ?? int.MaxValue;
            int maxDepth = options?.MaxHashCodeDepth ?? int.MaxValue;

            var hash = 0;
            var count = 0;
            GetHashCodeProc(@this, depth: 0);
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
                        Add(value.ToValue(JNodeOptions.Default));
                        break;
                }
            }
        }

        private int GetShallowHashCode(JNodeOptions options) =>
            @this switch {
                null => 0,
                JsonObject v => v.Count.GetHashCode(),
                JsonArray v => v.Count.GetHashCode(),
                JsonValue v => v.ToValue(options)?.GetHashCode() ?? 0,
                _ => throw new InvalidOperationException($"Unexpected node type: {@this.GetType().Name}"),
            };

        private int GetReferenceHashCode() =>
            RuntimeHelpers.GetHashCode(@this);
    }

    extension(JsonNode? @this)
    {
        [return: NotNullIfNotNull(nameof(@this))]
        public JsonNode? DeepClone()
        {
          #if JSON8_0_OR_GREATER
            return @this?.DeepClone();
          #else
            return @this.Deserialize<JsonNode?>();
          #endif
        }
    }
}