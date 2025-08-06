using System.Text.Json.Nodes;
using static Alba.Text.Json.Dynamic.JOperations;

namespace Alba.Text.Json.Dynamic;

public static class JNodeExts
{
    //extension(JsonNode? @this)
    //{
    //    public dynamic? ToDynamic(JNodeOptions? options = null) =>
    //        JsonNodeToJNodeOrValue(@this, options ?? JNodeOptions.Default);
    //}
    public static dynamic? ToDynamic(this JsonNode? @this, JNodeOptions? options = null) =>
        JsonNodeToJNodeOrValue(@this, options ?? JNodeOptions.Default);
}