using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic.Extensions;

/// <summary>Extension methods for <see cref="Utf8JsonWriter"/>.</summary>
public static class Utf8JsonWriterExts
{
    ///
    extension(Utf8JsonWriter @this)
    {
        /// <summary>Writes a value at the current writer position. The value can be of any type serializable to <see cref="JsonNode"/>.</summary>
        /// <param name="value">A value to write.</param>
        public void WriteValue(object? value)
        {
            var node = value.ToJsonNode(isolated: false);
            if (node == null)
                @this.WriteNullValue();
            else
                node.WriteTo(@this);
        }
    }
}