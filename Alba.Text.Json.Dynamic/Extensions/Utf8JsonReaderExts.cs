using System.Text.Json;

namespace Alba.Text.Json.Dynamic.Extensions;

/// <summary>Extension methods for <see cref="Utf8JsonReader"/>.</summary>
public static class Utf8JsonReaderExts
{
    ///
    extension(ref Utf8JsonReader @this)
    {
        /// <summary>Gets a raw value at the current reader position: <see langword="null"/>, <see langword="string"/>, <see langword="bool"/> or a numeric type (<see langword="int"/>, <see langword="double"/>, <see langword="decimal"/> etc.).</summary>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns>A raw value at the current position.</returns>
        public object? GetValue(JNodeOptions options) => JsonElement.ParseValue(ref @this).ToValue(options);
    }
}