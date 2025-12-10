#if JSON10_0_OR_GREATER
using System.Runtime.InteropServices;
#endif
using System.Text.Json;

namespace Alba.Text.Json.Dynamic.Extensions;

/// <summary>Extension methods for <see cref="ObjectEnumerator"/>.</summary>
public static class ObjectEnumeratorExts
{
    ///
    extension(ref ObjectEnumerator @this)
    {
        /// <summary>Gets the name of the property.</summary>
        public string Name => @this.Current.Name;
        /// <summary>Gets the value of the property.</summary>
        public JsonElement Value => @this.Current.Value;
      #if JSON10_0_OR_GREATER
        /// <summary>Gets a <see cref="ReadOnlySpan{T}"/> view over the raw JSON data of the given <see cref="JsonProperty"/> name.</summary>
        /// <returns>The span containing the raw JSON data of the property name. This will not include the enclosing quotes.</returns>
        /// <exception cref="ObjectDisposedException">The underlying <see cref="JsonDocument"/> has been disposed.</exception>
        public ReadOnlySpan<byte> RawNameSpan => JsonMarshal.GetRawUtf8PropertyName(@this.Current);
      #endif
    }
}