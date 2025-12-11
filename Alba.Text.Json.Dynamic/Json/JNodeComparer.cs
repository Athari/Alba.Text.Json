namespace Alba.Text.Json.Dynamic;

/// <summary>Container of predefined comparers of <see cref="JNode"/> using <see cref="JNodeOptions.Default"/> options.</summary>
public static class JNodeComparer
{
    /// <inheritdoc cref="JsonComparer.Reference"/>
    [field: MaybeNull]
    public static JsonComparer<JNode> Reference => field ??= new(JEquality.Reference, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Shallow"/>
    [field: MaybeNull]
    public static JsonComparer<JNode> Shallow => field ??= new(JEquality.Shallow, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Deep"/>
    [field: MaybeNull]
    public static JsonComparer<JNode> Deep => field ??= new(JEquality.Deep, JNodeOptions.Default);
}