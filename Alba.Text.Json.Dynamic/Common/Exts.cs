namespace Alba.Text.Json.Dynamic;

internal static class Exts
{
    public static int IndexOf<T>(this IEnumerable<T> @this, Predicate<T> predicate)
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