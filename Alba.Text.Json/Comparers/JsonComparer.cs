namespace Alba.Text.Json;

/// <summary>Container of predefined comparers of JSON objects using <see cref="JNodeOptions.Default"/> options.</summary>
public static class JsonComparer
{
    /// <summary>Gets a comparer instance that compares objects by <see cref="JEquality.Reference"/> equality using <see cref="JNodeOptions.Default"/> options.</summary>
    [field: MaybeNull]
    public static JsonComparer<object> Reference => field ??= new(JEquality.Reference, JNodeOptions.Default);

    /// <summary>Gets a comparer instance that compares objects by <see cref="JEquality.Shallow"/> equality using <see cref="JNodeOptions.Default"/> options.</summary>
    [field: MaybeNull]
    public static JsonComparer<object> Shallow => field ??= new(JEquality.Shallow, JNodeOptions.Default);

    /// <summary>Gets a comparer instance that compares objects by <see cref="JEquality.Deep"/> equality using <see cref="JNodeOptions.Default"/> options.</summary>
    [field: MaybeNull]
    public static JsonComparer<object> Deep => field ??= new(JEquality.Deep, JNodeOptions.Default);
}