using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

/// <summary>Internal interface. Do not use directly; use concrete types instead. Interface of dynamic adapters for <see cref="JsonNode"/> types with support for dynamic dispatch via <see cref="IDynamicMetaObjectProvider"/>.</summary>
public interface IJNode
{
    /// <summary>The wrapped <see cref="JsonNode"/>.</summary>
    JsonNode Node { get; }

    // The following methods exist to make Visual Studio's parser of XML comments happy.

    /// <inheritdoc cref="object.Equals(object?)"/>
    bool Equals(object? obj);

    /// <inheritdoc cref="object.GetHashCode"/>
    int GetHashCode();
}