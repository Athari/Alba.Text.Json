#nullable enable

namespace Alba.Text.Json.Dynamic;

#if NET7_0_OR_GREATER
/// <summary>Numeric codes of <see cref="TypeCode"/> with support for <see cref="System.Half"/>, <see cref="System.Int128"/> and <see cref="System.UInt128"/>.</summary>
#elif NET5_0_OR_GREATER
/// <summary>Numeric codes of <see cref="TypeCode"/> with support for <see cref="System.Half"/>, Int128 and UInt128 (.NET 7.0+).</summary>
#else
/// <summary>Numeric codes of <see cref="TypeCode"/> with support for Half (.NET 5.0+), Int128 and UInt128 (.NET 7.0+).</summary>
#endif
public enum NumberType
{
    /// <summary>An integral type representing signed 8-bit integers with values between -128 and 127.</summary>
    SByte = TypeCode.SByte,
    /// <summary>An integral type representing unsigned 8-bit integers with values between 0 and 255.</summary>
    Byte = TypeCode.Byte,
    /// <summary>An integral type representing signed 16-bit integers with values between -32768 and 32767.</summary>
    Int16 = TypeCode.Int16,
    /// <summary>An integral type representing unsigned 16-bit integers with values between 0 and 65535.</summary>
    UInt16 = TypeCode.UInt16,
    /// <summary>An integral type representing signed 32-bit integers with values between -2147483648 and 2147483647.</summary>
    Int32 = TypeCode.Int32,
    /// <summary>An integral type representing unsigned 32-bit integers with values between 0 and 4294967295.</summary>
    UInt32 = TypeCode.UInt32,
    /// <summary>An integral type representing signed 64-bit integers with values between -9223372036854775808 and 9223372036854775807.</summary>
    Int64 = TypeCode.Int64,
    /// <summary>An integral type representing unsigned 64-bit integers with values between 0 and 18446744073709551615.</summary>
    UInt64 = TypeCode.UInt64,
    /// <summary>A floating point type representing values ranging from approximately 1.5×10<sup>-45</sup> to 3.4×10<sup>38</sup> with a precision of 7 digits.</summary>
    Single = TypeCode.Single,
    /// <summary>A floating point type representing values ranging from approximately 5.0×10<sup>-324</sup> to 1.7×10<sup>308</sup> with a precision of 15-16 digits.</summary>
    Double = TypeCode.Double,
    /// <summary>A simple type representing values ranging from 1.0×10<sup>-28</sup> to approximately 7.9×10<sup>28</sup> with 28-29 significant digits.</summary>
    Decimal = TypeCode.Decimal,
  #if NET5_0_OR_GREATER
    /// <summary>A floating point type representing values ranging from approximately 5.96 x 10<sup>-8</sup> to 6.55 x 10<sup>4</sup> with a precision of 3-4 digits.</summary>
    Half = 101,
  #endif
  #if NET7_0_OR_GREATER
    /// <summary>An integral type representing signed 128-bit integers with values between -170141183460469231731687303715884105728 and 170141183460469231731687303715884105727.</summary>
    Int128 = 102,
    /// <summary>An integral type representing unsigned 128-bit integers with values between 0 and 340282366920938463463374607431768211455.</summary>
    UInt128 = 103,
  #endif
}