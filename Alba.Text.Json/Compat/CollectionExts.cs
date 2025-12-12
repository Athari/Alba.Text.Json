namespace System.Collections.Generic;

[ExcludeFromCodeCoverage]
internal static class CollectionExts
{
  #if !NETCOREAPP2_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
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