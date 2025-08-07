using System.Runtime.InteropServices;
#if !NET7_0_OR_GREATER && (NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNSAFE)
using System.Runtime.CompilerServices;
#endif

namespace Alba.Text.Json.Dynamic;

#if UNSAFE
internal static unsafe class MemoryMarshalExts
#else
internal static class MemoryMarshalExts
#endif
{
    extension(MemoryMarshal)
    {
        public static Span<byte> CreateByteSpan<T>(ref T value) where T : struct =>
          #if NET7_0_OR_GREATER
            MemoryMarshal.AsBytes(new Span<T>(ref value));
          #elif NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in value), 1));
          #elif UNSAFE
            MemoryMarshal.AsBytes(new Span<T>(Unsafe.AsPointer(ref value), Marshal.SizeOf<T>()));
          #else
            MemoryMarshal.AsBytes(new Span<T>([ value ]));
          #endif

        public static ReadOnlySpan<byte> CreateReadOnlyByteSpan<T>(in T value) where T : struct =>
          #if NET7_0_OR_GREATER
            MemoryMarshal.AsBytes(new ReadOnlySpan<T>(in value));
          #elif NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in value), 1));
          #elif UNSAFE
            MemoryMarshal.AsBytes(new ReadOnlySpan<T>(Unsafe.AsPointer(ref Unsafe.AsRef(value)), Marshal.SizeOf<T>()));
          #else
            MemoryMarshal.AsBytes(new ReadOnlySpan<T>([ value ]));
          #endif
    }
}