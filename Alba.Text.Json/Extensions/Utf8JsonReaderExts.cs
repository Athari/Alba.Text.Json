using System.Text.Json;

namespace Alba.Text.Json.Extensions;

/// <summary>Extension methods for <see cref="Utf8JsonReader"/>.</summary>
public static class Utf8JsonReaderExts
{
    ///
    extension(ref Utf8JsonReader @this)
    {
        /// <summary>Gets a raw value at the current reader position: <see langword="null"/>, <see cref="string"/>, <see cref="bool"/> or a numeric type (<see cref="int"/>, <see cref="double"/>, <see cref="decimal"/> etc.).</summary>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns>A raw value at the current position.</returns>
        public object? GetValue(JNodeOptions options) => JsonElement.ParseValue(ref @this).ToValue(options);
    }
}