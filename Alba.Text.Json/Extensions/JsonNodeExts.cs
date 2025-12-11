using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
#if !JSON8_0_OR_GREATER
using Alba.Framework;
#endif
using C = System.TypeCode;

namespace Alba.Text.Json.Dynamic.Extensions;

/// <summary>Extension methods for <see cref="JsonNode"/>.</summary>
[SuppressMessage("Naming", "CA1708: Identifiers should differ by more than case", Justification = "Compiler bug"), SuppressMessage("CodeQuality", "IDE0079")]
public static class JsonNodeExts
{
    ///
    extension(JsonNode @this)
    {
        /// <summary>Gets <see cref="JsonValueKind"/> of the value of <see cref="JsonNode"/>.</summary>
        public JsonValueKind DataValueKind {
            get {
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

        /// <summary>Determines whether the specified <see cref="JsonNode"/> and <see langword="object"/> are considered equal.</summary>
        /// <param name="v1">The first <see langword="object"/> to compare. The object can be <see cref="IJNode"/>, <see cref="JsonNode"/>, <see cref="JsonElement"/>, <see cref="JsonDocument"/>, or anything that can be serialized to <see cref="JsonNode"/>.</param>
        /// <param name="v2">The second <see langword="object"/> to compare. The object can be <see cref="IJNode"/>, <see cref="JsonNode"/>, <see cref="JsonElement"/>, <see cref="JsonDocument"/>, or anything that can be serialized to <see cref="JsonNode"/>.</param>
        /// <param name="equality">Kind of equality comparison.</param>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns><see langword="true"/> if the node and the object are considered equal; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Invalid <paramref name="equality"/> value.</exception>
        /// <remarks>Uses built-in JsonNode.DeepEquals and JsonElement.DeepEquals in deep equality comparison mode, if available.</remarks>
        public static bool Equals(object? v1, object? v2, JEquality equality, JNodeOptions options) =>
            (v1, v2) switch {
                (null, null) => true,
                (JsonNode n1, _) =>
                    JsonNode.EqualsValue(n1, v2, equality, options),
                (_, JsonNode n2) =>
                    JsonNode.EqualsValue(n2, v1, equality, options),
                (IJNode j1, _) =>
                    JsonNode.EqualsValue(j1.Node, v2, equality, options),
                (_, IJNode j2) =>
                    JsonNode.EqualsValue(j2.Node, v1, equality, options),
                (JsonElement or JsonDocument, _) or (_, JsonElement or JsonDocument) =>
                    JsonElement.Equals(v1, v2, equality, options),
                (_, null) =>
                    JNodeOptions.Default.IsNull(v1),
                (null, _) =>
                    JNodeOptions.Default.IsNull(v2),
                // Compare arbitrary types as their JSON representations
                (_, _) =>
                    JsonElement.Equals(v1.ToJsonElement(), v2.ToJsonElement(), equality, options),
            };

        private static bool EqualsValue(JsonNode? n1, object? v2, JEquality equality, JNodeOptions options) =>
            v2 switch {
                JsonNode or null =>
                    JsonNode.Equals(n1, (JsonNode?)v2, equality, options),
                IJNode j2 =>
                    JsonNode.Equals(n1, j2.Node, equality, options),
                JsonElement el2 =>
                    JsonNode.EqualsJsonElement(n1, el2, equality, options),
                JsonDocument doc2 =>
                    JsonNode.EqualsJsonElement(n1, doc2.RootElement, equality, options),
                not null =>
                    JsonNode.Equals(n1, v2.ToJsonNode(n1?.Options ?? options.JsonNodeOptions, false), equality, options),
            };

        private static bool EqualsJsonElement(JsonNode? n1, JsonElement el2, JEquality equality, JNodeOptions options)
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
            return JsonElement.Equals(n1n.ToJsonElement(), el2, equality, options);
        }

        private static bool Equals(JsonNode? n1, JsonNode? n2, JEquality equality, JNodeOptions options)
        {
          #if JSON9_0_OR_GREATER
            // try faster element comparison first
            if ((n1?.TryGetElementValue(out var el1) ?? false) && (n2?.TryGetElementValue(out var el2) ?? false))
                return JsonElement.Equals(el1, el2, equality, options);
          #endif
            return WithNullCheck(
                (n1n, n2n, _) => equality switch {
                    JEquality.Deep => JsonNode.DeepEquals(n1n, n2n, options),
                    JEquality.Shallow => JsonNode.ShallowEquals(n1n, n2n, options),
                    JEquality.Reference => JsonNode.ReferenceEquals(n1n, n2n, options),
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
            n1.TryGetElementValue(out var el1) && n2.TryGetElementValue(out var el2) && JsonElement.Equals(el1, el2, JEquality.Reference, options);

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

        internal bool TryGetElementValue(out JsonElement el)
        {
            if ((@this as JsonValue)?.TryGetValue(out el) ?? false)
                return true;
            el = default;
            return false;
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

        /// <summary>Hash code function which correspeconds to the specified <paramref name="equality"/> kind.</summary>
        /// <param name="v">The <see langword="object"/> whose hash code to calculate. The object can be <see cref="IJNode"/>, <see cref="JsonNode"/>, <see cref="JsonElement"/>, <see cref="JsonDocument"/>, or anything that can be serialized to <see cref="JsonNode"/>.</param>
        /// <param name="equality">Kind of equality comparison.</param>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns>A hash code of the object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Invalid <paramref name="equality"/> value.</exception>
        public static int GetHashCode(object? v, JEquality equality, JNodeOptions options) =>
            v switch {
                IJNode { Node: var n } => n.GetHashCode(equality, options),
                JsonNode n => n.GetHashCode(equality, options),
                JsonElement el => el.GetHashCode(equality, options),
                JsonDocument doc => doc.RootElement.GetHashCode(equality, options),
                _ => v?.ToJsonNode(options.JsonNodeOptions, false)?.GetHashCode(equality, options) ?? 0,
            };

        /// <summary>Hash code function which correspeconds to the specified <paramref name="equality"/> kind.</summary>
        /// <param name="equality">Kind of equality comparison.</param>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns>A hash code of the node.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Invalid <paramref name="equality"/> value.</exception>
        /// <exception cref="InvalidOperationException">Unsupported <see cref="JsonNode"/> type. Should never happen.</exception>
        public int GetHashCode(JEquality equality, JNodeOptions options) =>
            equality switch {
                JEquality.Deep => @this.GetDeepHashCode(options),
                JEquality.Shallow => @this.GetShallowHashCode(options),
                JEquality.Reference => @this.GetReferenceHashCode(),
                _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
            };

        private int GetDeepHashCode(JNodeOptions options)
        {
            var maxCount = options.MaxHashCodeValueCount;
            var maxDepth = options.MaxHashCodeDepth;
            var nameComparer = options.PropertyNameComparer;

            var hash = new HashCode();
            var count = 0;
            AddHashCode(@this, depth: 0);
            return hash.ToHashCode();

            bool Add<T>(T? value, IEqualityComparer<T?>? comparer = null)
            {
                if (count++ >= maxCount)
                    return false;
                hash.Add(value, comparer);
                return true;
            }

            void AddHashCode(JsonNode? n, int depth)
            {
                if (n == null || !Add(n.GetType()))
                    return;
                switch (n) {
                    case JsonArray a:
                        if (depth < maxDepth)
                            foreach (var item in a)
                                AddHashCode(item, depth + 1);
                        else
                            Add(a.Count);
                        break;
                    case JsonObject o:
                        foreach (var (name, value) in o.OrderBy(p => p.Key, nameComparer)) {
                            if (!Add(name, nameComparer))
                                return;
                            if (depth < maxDepth)
                                AddHashCode(value, depth + 1);
                        }
                        break;
                    case JsonValue v:
                        Add(v.ToValue(options));
                        break;
                }
            }
        }

        private int GetShallowHashCode(JNodeOptions options) =>
            @this switch {
                null => 0,
                JsonValue v => v.ToValue(options)?.GetHashCode() ?? 0,
                JsonObject or JsonArray => @this.GetReferenceHashCode(),
                _ => throw new InvalidOperationException($"Unexpected node type: {@this.GetType().Name}"),
            };

        private int GetReferenceHashCode() =>
            RuntimeHelpers.GetHashCode(@this);
    }

    ///
    extension(JsonNode? @this)
    {
        /// <summary>Creates a new instance of the <see cref="JsonNode"/>. All children nodes are recursively cloned.</summary>
        /// <returns>A new cloned instance of the node, or <see langword="null"/> if <see langword="null"/> is passed.</returns>
        /// <remarks>Uses built-in JsonNode.DeepClone, if available.</remarks>
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