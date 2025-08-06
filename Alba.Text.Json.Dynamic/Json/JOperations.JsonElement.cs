using System.Text.Json;
#if NET5_0_OR_GREATER
using System.Globalization;
#endif

namespace Alba.Text.Json.Dynamic;

internal static partial class JOperations
{
  #if NET5_0_OR_GREATER
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;
  #endif

    private static bool JsonElementEquals(in JsonElement el1, in JsonElement el2, Equality equality, JNodeOptions options) =>
        equality switch {
            Equality.Deep => JsonElementDeepEquals(el1, el2, options),
            Equality.Shallow => JsonElementShallowEquals(el1, el2, options),
            Equality.Reference => JsonElementReferenceEquals(el1, el2, options),
            _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
        };

    [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Conditional")]
    private static bool JsonElementDeepEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options)
    {
        if (JsonElementReferenceEquals(el1, el2, options))
            return true;
      #if JSON9_0_OR_GREATER
        return JsonElement.DeepEquals(el1, el2);
      #else
        var (k1, k2) = (el1.ValueKind, el2.ValueKind);
        if (k1 != k2)
            return false;
        switch (k1) {
            case JsonValueKind.Object: {
                using var it1 = el1.EnumerateObject();
                using var it2 = el2.EnumerateObject();
                while (it1.MoveNext()) {
                    if (!it2.MoveNext())
                        return false;
                    var (prop1, prop2) = (it1.Current, it2.Current);
                    if (!prop1.NameEquals(prop2.Name))
                        return UnorderedObjectDeepEquals(it1, it2, options);
                    if (!JsonElementDeepEquals(prop1.Value, prop2.Value, options))
                        return false;
                }
                return !it2.MoveNext();
            }
            case JsonValueKind.Array: {
                if (el1.GetArrayLength() != el2.GetArrayLength())
                    return false;
                using var it1 = el1.EnumerateArray();
                using var it2 = el2.EnumerateArray();
                while (it1.MoveNext()) {
                    if (!it2.MoveNext() ||
                        !JsonElementDeepEquals(it1.Current, it2.Current, options))
                        return false;
                }
                return !it2.MoveNext();
            }
            default:
                return JsonElementValueEquals(el1, el2, options);
        }

        static bool UnorderedObjectDeepEquals(ObjectEnumerator it1, ObjectEnumerator it2, JNodeOptions options)
        {
            var d2 = new Dictionary<string, ValueQueue<JsonElement>>(StringComparer.Ordinal);
          #if NET6_0_OR_GREATER
            do {
                d2.GetRefOrAdd(it2.Name).Enqueue(it2.Value);
            } while (it2.MoveNext());
            do {
                ref var values = ref d2.GetRefOrNull(it1.Name, out var exists);
                if (!exists ||
                    !values.TryDequeue(out var value) ||
                    !JsonElementDeepEquals(it1.Value, value, options))
                    return false;
            } while (it1.MoveNext());
          #else
            do {
                d2.TryGetValue(it2.Name, out var values);
                values.Enqueue(it2.Value);
                d2[it2.Name] = values;
            } while (it2.MoveNext());
            do {
                if (!d2.TryGetValue(it1.Name, out var values) ||
                    !values.TryDequeue(out var value) ||
                    !JsonElementDeepEquals(it1.Value, value, options))
                    return false;
                d2[it1.Name] = values;
            } while (it1.MoveNext());
          #endif
            return true;
        }
      #endif
    }

    private static bool JsonElementShallowEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options) =>
        JsonElementReferenceEquals(el1, el2, options) ||
        (el1.ValueKind, el2.ValueKind) switch {
            var (k1, k2) => k1 == k2 && JsonElementValueEquals(el1, el2, options),
        };

    [SuppressMessage("Style", "IDE0060:Remove unused parameter"), SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private static bool JsonElementReferenceEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options) =>
        JsonElement.DocumentOffsetEquals(el1, el2);

    private static bool JsonElementValueEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options)
    {
        if (JsonElementReferenceEquals(el1, el2, options))
            return true;
      #if JSON9_0_OR_GREATER
        return JsonElement.DeepEquals(el1, el2);
      #else
        var (k1, k2) = (el1.ValueKind, el2.ValueKind);
        return k1 == k2 && k1 switch {
            JsonValueKind.Null or JsonValueKind.Undefined or JsonValueKind.True or JsonValueKind.False =>
                true,
            JsonValueKind.String =>
                el1.ValueEquals(el2.GetString()),
            JsonValueKind.Number =>
                Equals(JsonElementToValue(el1, options), JsonElementToValue(el2, options)),
            _ =>
                throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {k1}"),
        };
      #endif
    }

    private static bool JsonElementIsNull(in JsonElement el, JNodeOptions options) =>
        el.ValueKind switch {
            JsonValueKind.Null => true,
            JsonValueKind.Undefined => options.UndefinedValue == null,
            _ => false,
        };
}