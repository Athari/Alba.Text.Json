#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace Alba.Text.Json.Dynamic;

[InternalAPI]
internal static class Exts
{
  #pragma warning disable // Discard `out _` doesn't work
    private static bool _discardExists;
  #pragma warning restore

    extension<TKey, T>(Dictionary<TKey, T> @this) where TKey : notnull
    {
        [SuppressMessage("ReSharper", "CanSimplifyDictionaryTryGetValueWithGetValueOrDefault")]
        public T? GetOrDefault(TKey key) => @this.TryGetValue(key, out var value) ? value : default;

        public T GetOrDefault(TKey key, T def) => @this.GetValueOrDefault(key, def);

      #if NET6_0_OR_GREATER
        public ref T? GetRefOrAdd(TKey key, [UnscopedRef] out bool exists) =>
            ref CollectionsMarshal.GetValueRefOrAddDefault(@this, key, out exists);

        public ref T? GetRefOrAdd(TKey key) =>
            ref @this.GetRefOrAdd(key, out _discardExists);

        public ref T GetRefOrNull(TKey key) =>
            ref CollectionsMarshal.GetValueRefOrNullRef(@this, key);

        public ref T GetRefOrNull(TKey key, out bool exists)
        {
            ref var value = ref @this.GetRefOrNull(key);
            exists = Unsafe.IsNullRef(ref value);
            return ref value!;
        }
      #endif
    }

    extension<T>(IEnumerable<T> @this)
    {
        public bool All(Func<T, int, bool> predicate)
        {
            int i = 0;
            using var it = @this.GetEnumerator();
            while (it.MoveNext())
                if (!predicate(it.Current, i++))
                    return false;
            return true;
        }

        public int IndexOf(Func<T, bool> predicate)
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
    }
}