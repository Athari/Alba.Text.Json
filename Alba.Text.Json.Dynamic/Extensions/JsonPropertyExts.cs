#if JSON10_0_OR_GREATER

using System.Runtime.InteropServices;
using System.Text.Json;

namespace Alba.Text.Json.Dynamic.Extensions;

/// <summary>Extension methods for <see cref="JsonProperty"/>.</summary>
public static class JsonPropertyExts
{
    ///
    extension(in JsonProperty @this)
    {
      #if JSON10_0_OR_GREATER
        /// <summary>Gets a <see cref="ReadOnlySpan{T}"/> view over the raw JSON data of the given <see cref="JsonProperty"/> name.</summary>
        /// <returns>The span containing the raw JSON data of the property name. This will not include the enclosing quotes.</returns>
        /// <exception cref="ObjectDisposedException">The underlying <see cref="JsonDocument"/> has been disposed.</exception>
        public ReadOnlySpan<byte> RawNameSpan => JsonMarshal.GetRawUtf8PropertyName(@this);
      #endif
    }
}

#endif