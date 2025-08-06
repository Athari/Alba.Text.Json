#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace Alba.Text.Json.Dynamic;

internal static class Exts
{
    public static T? GetOrDefault<TKey, T>(this Dictionary<TKey, T> @this, TKey key) where TKey : notnull =>
        @this.GetValueOrDefault(key, default!);

    public static T GetOrDefault<TKey, T>(this Dictionary<TKey, T> @this, TKey key, T def) where TKey : notnull =>
        @this.GetValueOrDefault(key, def);

    public static bool All<T>(this IEnumerable<T> @this, Func<T, int, bool> predicate)
    {
        int i = 0;
        using var it = @this.GetEnumerator();
        while (it.MoveNext())
            if (!predicate(it.Current, i++))
                return false;
        return true;
    }

    public static int IndexOf<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
    {
        int i = 0;
        using var it = @this.GetEnumerator();
        while (it.MoveNext()) {
            if (predicate(it.Current))
                return i;
            i++;
        }
        return -1;
    }

  #if NET6_0_OR_GREATER
    [SuppressMessage("ReSharper", "NotAccessedField.Local", Justification = "Discard `out _` doesn't work")]
    private static bool _discardExists;

    public static ref T? GetValueRefOrAddDefault<TKey, T>(this Dictionary<TKey, T> @this, TKey key, [UnscopedRef] out bool exists)
        where TKey : notnull
        => ref CollectionsMarshal.GetValueRefOrAddDefault(@this, key, out exists);

    public static ref T? GetValueRefOrAddDefault<TKey, T>(this Dictionary<TKey, T> @this, TKey key)
        where TKey : notnull
        => ref @this.GetValueRefOrAddDefault(key, out _discardExists);

    public static ref T GetValueRefOrNullRef<TKey, T>(this Dictionary<TKey, T> @this, TKey key)
        where TKey : notnull =>
        ref CollectionsMarshal.GetValueRefOrNullRef(@this, key);

    public static ref T GetValueRefOrNullRef<TKey, T>(this Dictionary<TKey, T> @this, TKey key, out bool exists)
        where TKey : notnull
    {
        ref var value = ref @this.GetValueRefOrNullRef(key);
        exists = Unsafe.IsNullRef(ref value);
        return ref value!;
    }
  #endif
}