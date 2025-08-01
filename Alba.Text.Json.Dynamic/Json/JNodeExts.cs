using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public static class JNodeExts
{
    public static dynamic? ToDynamic(this JsonNode? @this, JNodeOptions? options = null) =>
        JOperations.NodeToDynamicNodeOrValue(@this, options ?? JNodeOptions.Default);
}