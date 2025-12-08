using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

public static class JNodeExts
{
    //extension(JsonNode? @this)
    //{
    //    public dynamic? ToDynamic(JNodeOptions? options = null) =>
    //        JsonNode.ToJNodeOrValue(@this, options ?? JNodeOptions.Default);
    //}
    public static dynamic? ToDynamic(this JsonNode? @this, JNodeOptions? options = null) =>
        JsonNode.ToJNodeOrValue(@this, options ?? JNodeOptions.Default);
}