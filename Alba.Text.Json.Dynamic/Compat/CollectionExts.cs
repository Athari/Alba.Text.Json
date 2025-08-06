namespace System.Collections.Generic;

internal static class CollectionExts
{
  #if !NETCOREAPP2_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
    [SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable", Justification = "No it can't")]
    public static TValue? GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key) =>
        @this.GetValueOrDefault(key, default!);

    public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key, TValue defaultValue) =>
        @this.TryGetValue(key, out TValue? value) ? value : defaultValue;

    public static bool TryDequeue<T>(this Queue<T> @this, [MaybeNullWhen(false)] out T result)
    {
        if (@this.Count == 0) {
            result = default;
            return false;
        }
        result = @this.Dequeue();
        return true;
    }
  #endif
}