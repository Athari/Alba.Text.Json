#if JSON10_0_OR_GREATER
using System.Runtime.InteropServices;
#endif
using System.Text.Json;

namespace Alba.Text.Json.Dynamic.Extensions;

public static class ObjectEnumeratorExts
{
    extension(ref ObjectEnumerator @this)
    {
        public string Name => @this.Current.Name;
        public JsonElement Value => @this.Current.Value;
      #if JSON10_0_OR_GREATER
        public ReadOnlySpan<byte> RawNameSpan => JsonMarshal.GetRawUtf8PropertyName(@this.Current);
      #endif
    }
}