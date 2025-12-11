using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

internal interface IJNode
{
    JsonNode NodeUntyped { get; }
    bool Equals(object? obj);
    int GetHashCode();
}