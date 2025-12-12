using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Extensions;

/// <summary>Extension methods for <see cref="Utf8JsonWriter"/>.</summary>
public static class Utf8JsonWriterExts
{
    ///
    extension(Utf8JsonWriter @this)
    {
        /// <summary>Writes a value at the current writer position. The value can be of any type serializable to JSON.</summary>
        /// <param name="value">A value to write.</param>
        public void WriteValue(object? value)
        {
            var json = value.ToJsonElementOrNode();
            switch (json) {
                case null:
                    @this.WriteNullValue();
                    break;
                case JsonElement el:
                    el.WriteTo(@this);
                    break;
                case JsonNode n:
                    n.WriteTo(@this);
                    break;
            }
        }
    }
}