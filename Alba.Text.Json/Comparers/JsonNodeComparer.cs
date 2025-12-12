using System.Text.Json.Nodes;

namespace Alba.Text.Json;

/// <summary>Container of predefined comparers of <see cref="JsonNode"/> using <see cref="JNodeOptions.Default"/> options.</summary>
public static class JsonNodeComparer
{
    /// <inheritdoc cref="JsonComparer.Reference"/>
    [field: MaybeNull]
    public static JsonComparer<JsonNode> Reference => field ??= new(JEquality.Reference, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Shallow"/>
    [field: MaybeNull]
    public static JsonComparer<JsonNode> Shallow => field ??= new(JEquality.Shallow, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Deep"/>
    [field: MaybeNull]
    public static JsonComparer<JsonNode> Deep => field ??= new(JEquality.Deep, JNodeOptions.Default);
}