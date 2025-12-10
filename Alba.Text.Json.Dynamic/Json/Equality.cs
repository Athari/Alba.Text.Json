using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

/// <summary>Kind of equality comparison of JSON types.</summary>
public enum Equality
{
    /// <summary>Reference equality. Determines whether nodes reference the same memory address. For <see cref="JsonElement"/>, compares whether elements refer to the same document offset.</summary>
    Reference,
    /// <summary>Shallow eqality. Compares <see cref="JsonValue"/> and value <see cref="JsonElement"/> as values, then falls back to reference equality.</summary>
    /// <remarks><see cref="JsonElement"/> with different representations of the same value are not considered equal. This behavior is consistent with System.Text.Json.</remarks>
    Shallow,
    /// <summary>Deep equality. Determines whether the whole hierarchy of JSON elements is equivalent.</summary>
    /// <remarks><see cref="JsonElement"/> with different representations of the same value are not considered equal. This behavior is consistent with System.Text.Json.</remarks>
    Deep,
}