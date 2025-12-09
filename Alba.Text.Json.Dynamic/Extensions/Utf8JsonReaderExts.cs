using System.Text.Json;

namespace Alba.Text.Json.Dynamic.Extensions;

public static class Utf8JsonReaderExts
{
    extension(ref Utf8JsonReader @this)
    {
        public object? GetValue(JNodeOptions options) => JsonElement.ParseValue(ref @this).ToValue(options);
    }
}