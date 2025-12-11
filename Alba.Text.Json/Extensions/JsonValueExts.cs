using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic.Extensions;

/// <summary>Extension methods for <see cref="JsonValue"/>.</summary>
public static class JsonValueExts
{
    ///
    extension(JsonValue @this)
    {
        /// <summary>Converts a <see cref="JsonValue"/> to a raw value: <see langword="null"/>, <see langword="string"/>, <see langword="bool"/> or a numeric type (<see langword="int"/>, <see langword="double"/>, <see langword="decimal"/> etc.). Conversion of numbers depends on <see cref="JNodeOptions.IntegerTypes"/> and <see cref="JNodeOptions.FloatTypes"/> of <paramref name="options"/>. Conversion of <c>undefined</c> depends on <see cref="JNodeOptions.UndefinedValue"/> of <paramref name="options"/>.</summary>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns>A converted raw value.</returns>
        /// <exception cref="InvalidOperationException">Unsupported <see cref="JsonValueKind"/> value. Should never happen.</exception>
        public object? ToValue(JNodeOptions options) =>
            @this.DataValueKind switch {
                // return primitive values directly
                JsonValueKind.Undefined => options.UndefinedValue,
                JsonValueKind.Null => null,
                JsonValueKind.String => @this.GetValue<string>(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                // numbers can be stored as JsonElement pointing to barely parsed binary data
                JsonValueKind.Number => @this.TryGetElementValue(out var el) ? el.ToNumber(options) : @this.GetValue<object>(),
                // objects can be JsonElement or an arbitrary user type
                JsonValueKind.Object or JsonValueKind.Array
                    or (JsonValueKind)byte.MaxValue => // from JsonNodeExts
                    @this.TryGetElementValue(out var el)
                        // return value of wrapped JsonElement
                        ? el.ToValue(options)
                        // return raw objects stored inside
                        : @this.GetValue<object>(),
                _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonNode: {@this}"),
            };
    }
}