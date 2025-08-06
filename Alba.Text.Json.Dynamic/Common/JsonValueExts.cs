using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

internal static class JsonValueExts
{
    extension(JsonNode @this)
    {
        public bool TryGetElementValue(out JsonElement el)
        {
            if ((@this as JsonValue)?.TryGetValue(out el) ?? false)
                return true;
            el = default;
            return false;
        }
    }
}