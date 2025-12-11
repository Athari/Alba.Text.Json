namespace Alba.Framework;

internal static class EquatableArrayExts
{
    public static RefEquatableArray<T> AsRefEquatable<T>(this T[]? @this) =>
        @this != null ? new(@this) : RefEquatableArray<T>.Empty;
}