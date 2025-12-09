#if JSON10_0_OR_GREATER

using System.Runtime.InteropServices;
using System.Text.Json;

namespace Alba.Text.Json.Dynamic.Extensions;

public static class JsonPropertyExts
{
    extension(in JsonProperty @this)
    {
      #if JSON10_0_OR_GREATER
        /// <inheritdoc cref="JsonMarshal.GetRawUtf8PropertyName" />
        public ReadOnlySpan<byte> RawNameSpan => JsonMarshal.GetRawUtf8PropertyName(@this);
      #endif
    }
}

#endif