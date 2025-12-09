using System.Text.Json;

namespace Alba.Text.Json.Dynamic.Extensions;

public static class Utf8JsonWriterExts
{
    extension(Utf8JsonWriter @this)
    {
        public void WriteValue(object? value)
        {
            var node = value.ToJsonNode();
            if (node == null)
                @this.WriteNullValue();
            else
                node.WriteTo(@this);
        }
    }
}