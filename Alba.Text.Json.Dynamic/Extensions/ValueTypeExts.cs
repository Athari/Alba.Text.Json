using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Framework;

namespace Alba.Text.Json.Dynamic.Extensions;

/// <summary>Extension methods for <see langword="object"/> related to <see cref="JsonNode"/>.</summary>
public static class ValueTypeExts
{
    private static readonly JsonValue NoJsonValue = Ensure.NotNull(JsonValue.Create("<NOVALUE>"));

    ///
    extension<T>(T @this)
    {
        /// <summary>
        ///   Converts a value to a <see cref="JsonNode"/>. Attempts to reuse the existing value. If isolation is requested (<paramref name="isolated"/> is <see langword="true"/> by default), it creates a new node or clones the existing node if necessary. The result depends on the type of the value:
        ///   <list type="table">
        ///     <listheader><term>Type</term><term>Result</term></listheader>
        ///     <item><description><see langword="null"/></description>
        ///           <description><see langword="null"/></description></item>
        ///     <item><description>A primitive type †</description>
        ///           <description><see cref="JsonValue"/> wrapping the value</description></item>
        ///     <item><description><see cref="JNode"/></description>
        ///           <description>Wrapped <see cref="JsonNode"/>; deep cloned if it has a parent and isolation is requested</description></item>
        ///     <item><description><see cref="JsonValue"/></description>
        ///           <description><see cref="JsonValue"/> itself</description></item>
        ///     <item><description><see cref="JsonArray"/> or <see cref="JsonObject"/></description>
        ///           <description><see cref="JsonNode"/> itself; deep cloned if it has a parent and isolation is requested</description></item>
        ///     <item><description><see cref="JsonElement"/></description>
        ///           <description><see cref="JsonValue"/> wrapping the <see cref="JsonElement"/></description></item>
        ///     <item><description><see cref="JsonDocument"/></description>
        ///           <description><see cref="JsonNode"/> serialization of the <see cref="JsonDocument"/></description></item>
        ///     <item><description>Everything else</description>
        ///           <description><see cref="JsonNode"/> serialization of the value</description></item>
        ///   </list>
        /// </summary>
        /// <param name="options">Options to control the behavior.</param>
        /// <param name="isolated">Isolate the node for reuse in a separate node hierarchy. Causes the node to be cloned if it has a parent.</param>
        /// <returns>An isolated <see cref="JsonNode"/></returns>
        /// <remarks>† The list of primitive types depends on .NET and System.Text.Json version, but in general it includes all built-in types (<see langword="bool"/>, <see langword="int"/>, <see langword="char"/> etc.), plus <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Uri"/>, <see cref="Version"/>, <see cref="Guid"/>.</remarks>
        public JsonNode? ToJsonNode(JsonNodeOptions? options = null, bool isolated = true) =>
            @this.ToJsonValue(out var valueNode, options)
                ? valueNode
                : @this switch {
                    // already JNode, clone if used within another tree
                    JNode { NodeUntyped: var n } => n.ToJsonNode(options, isolated),
                    // already JsonNode
                    JsonNode v => v.Parent == null ? v : v.DeepClone(),
                    // element is always stored as JsonValueOfElement
                    JsonElement v => JsonValue.Create(v, options),
                    // serialize everything else into node
                    JsonDocument v => JsonSerializer.SerializeToNode(v),
                    _ => JsonSerializer.SerializeToNode(@this),
                };

        /// <summary>
        ///   Converts a value to a primitive <see cref="JsonValue"/>. A return value indicates whether the conversion succeeded. The result <paramref name="valueNode"/> depends on the type of the value:
        ///   <list type="table">
        ///     <listheader><term>Type</term><term>Result</term></listheader>
        ///     <item><description><see langword="null"/></description>
        ///           <description><see langword="null"/></description></item>
        ///     <item><description>A primitive type †</description>
        ///           <description><see cref="JsonValue"/> wrapping the value</description></item>
        ///     <item><description><see cref="JsonValue"/></description>
        ///           <description><see cref="JsonValue"/> itself</description></item>
        ///     <item><description><see cref="JsonElement"/></description>
        ///           <description><see cref="JsonValue"/> wrapping the <see cref="JsonElement"/>, unless an array or an object is wrapped</description></item>
        ///     <item><description>Everything else</description>
        ///           <description><see langword="null"/> (failure)</description></item>
        ///   </list>
        /// </summary>
        /// <param name="valueNode">A converted <see cref="JsonValue"/>.</param>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns><see langword="true"/> if the value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <remarks>† The list of primitive types depends on .NET and System.Text.Json version, but in general it includes all built-in types (<see langword="bool"/>, <see langword="int"/>, <see langword="char"/> etc.), plus <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Uri"/>, <see cref="Version"/>, <see cref="Guid"/>.</remarks>
        public bool ToJsonValue([NotNullIfNotNull(nameof(@this))] out JsonValue? valueNode, JsonNodeOptions? options = null)
        {
            valueNode = @this switch {
                null => null,
                // already JsonValue
                JsonValue v => v,
                // types stored as JsonValuePrimitive<T> (explicit ctor)
                bool v => JsonValue.Create(v, options),
                char v => JsonValue.Create(v, options),
                sbyte v => JsonValue.Create(v, options),
                byte v => JsonValue.Create(v, options),
                short v => JsonValue.Create(v, options),
                ushort v => JsonValue.Create(v, options),
                int v => JsonValue.Create(v, options),
                uint v => JsonValue.Create(v, options),
                long v => JsonValue.Create(v, options),
                ulong v => JsonValue.Create(v, options),
                float v => JsonValue.Create(v, options),
                double v => JsonValue.Create(v, options),
                decimal v => JsonValue.Create(v, options),
                DateTime v => JsonValue.Create(v, options),
                DateTimeOffset v => JsonValue.Create(v, options),
                Guid v => JsonValue.Create(v, options),
                // types stored as JsonValuePrimitive<T> (no explicit ctor)
                TimeSpan or Uri or Version => JsonValue.Create(@this, options),
              #if NET5_0_OR_GREATER
                Half => JsonValue.Create(@this, options),
              #endif
              #if NET6_0_OR_GREATER
                DateOnly or TimeOnly => JsonValue.Create(@this, options),
              #endif
              #if NET7_0_OR_GREATER
                Int128 or UInt128 => JsonValue.Create(@this, options),
              #endif
                // convert to JsonValueOfElement if it's a value
                JsonElement el => el.ValueKind is not (JsonValueKind.Object or JsonValueKind.Array)
                    ? JsonValue.Create(el, options)
                    : NoJsonValue,
                // everything else isn't a value
                _ => NoJsonValue,
            };
            var isNone = ReferenceEquals(valueNode, NoJsonValue);
            if (isNone)
                valueNode = null;
            return !isNone;
        }
    }
}