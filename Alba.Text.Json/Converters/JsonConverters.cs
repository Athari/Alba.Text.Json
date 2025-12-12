using System.Text.Json.Serialization;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
using System.Drawing;
#endif
#if NET7_0_OR_GREATER
using System.Numerics;
using System.Runtime.InteropServices;
#endif
#if NET8_0_OR_GREATER
using System.Net;
#endif

namespace Alba.Text.Json.Converters;

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

/// <summary>A <see cref="JsonConverter"/> using <see cref="ColorConverter"/> to serialize <see cref="Color"/> as a <see cref="string"/>.</summary>
public class DrawingColorJsonConverter : TypeJsonConverter<Color, string, ColorConverter>;

/// <summary>A <see cref="JsonConverter"/> using <see cref="PointConverter"/> to serialize <see cref="Point"/> as a <see cref="string"/>.</summary>
public class DrawingPointJsonConverter : TypeJsonConverter<Point, string, PointConverter>;

/// <summary>A <see cref="JsonConverter"/> using <see cref="RectangleConverter"/> to serialize <see cref="Rectangle"/> as a <see cref="string"/>.</summary>
public class DrawingRectangleJsonConverter : TypeJsonConverter<Rectangle, string, RectangleConverter>;

/// <summary>A <see cref="JsonConverter"/> using <see cref="SizeConverter"/> to serialize <see cref="Size"/> as a <see cref="string"/>.</summary>
public class DrawingSizeJsonConverter : TypeJsonConverter<Size, string, SizeConverter>;

/// <summary>A <see cref="JsonConverter"/> using <see cref="SizeFConverter"/> to serialize <see cref="SizeF"/> as a <see cref="string"/>.</summary>
public class DrawingSizeFJsonConverter : TypeJsonConverter<SizeF, string, SizeFConverter>;

#endif

#if NET8_0_OR_GREATER

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="BigInteger"/> as a <see cref="string"/>.</summary>
public class BigIntegerJsonConverter : Utf8ParsableJsonConverter<BigInteger>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="Complex"/> as a <see cref="string"/>.</summary>
public class ComplexJsonConverter : Utf8ParsableJsonConverter<Complex>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="NFloat"/> as a <see cref="string"/>.</summary>
public class NFloatJsonConverter : Utf8ParsableJsonConverter<NFloat>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="IntPtr"/> as a <see cref="string"/>.</summary>
public class IntPtrJsonConverter : Utf8ParsableJsonConverter<IntPtr>;

#elif NET7_0_OR_GREATER

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="BigInteger"/> as a <see cref="string"/>.</summary>
public class BigIntegerJsonConverter : ParsableJsonConverter<BigInteger>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="Complex"/> as a <see cref="string"/>.</summary>
public class ComplexJsonConverter : ParsableJsonConverter<Complex>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="NFloat"/> as a <see cref="string"/>.</summary>
public class NFloatJsonConverter : ParsableJsonConverter<NFloat>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="IntPtr"/> as a <see cref="string"/>.</summary>
public class IntPtrJsonConverter : ParsableJsonConverter<IntPtr>;

#endif

#if NET10_0_OR_GREATER

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="IPAddress"/> as a <see cref="string"/>.</summary>
public class IPAddressJsonConverter : Utf8ParsableJsonConverter<IPAddress>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="IPNetwork"/> as a <see cref="string"/>.</summary>
public class IPNetworkJsonConverter : Utf8ParsableJsonConverter<IPNetwork>;

#elif NET8_0_OR_GREATER

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="IPAddress"/> as a <see cref="string"/>.</summary>
public class IPAddressJsonConverter : ParsableJsonConverter<IPAddress>;

/// <summary>A <see cref="JsonConverter"/> serializing <see cref="IPNetwork"/> as a <see cref="string"/>.</summary>
public class IPNetworkJsonConverter : ParsableJsonConverter<IPNetwork>;

#endif