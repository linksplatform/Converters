using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    [Obsolete]
    public static class To
    {
        public static readonly char UnknownCharacter = '�';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UInt64(ulong value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Int64(ulong value) => unchecked(value > long.MaxValue ? long.MaxValue : (long)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UInt32(ulong value) => unchecked(value > uint.MaxValue ? uint.MaxValue : (uint)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Int32(ulong value) => unchecked(value > int.MaxValue ? int.MaxValue : (int)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort UInt16(ulong value) => unchecked(value > ushort.MaxValue ? ushort.MaxValue : (ushort)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Int16(ulong value) => unchecked(value > (ulong)short.MaxValue ? short.MaxValue : (short)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Byte(ulong value) => unchecked(value > byte.MaxValue ? byte.MaxValue : (byte)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte SByte(ulong value) => unchecked(value > (ulong)sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Boolean(ulong value) => value > 0UL;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char Char(ulong value) => unchecked(value > char.MaxValue ? UnknownCharacter : (char)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime DateTime(ulong value) => unchecked(value > long.MaxValue ? System.DateTime.MaxValue : new DateTime((long)value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan TimeSpan(ulong value) => unchecked(value > long.MaxValue ? System.TimeSpan.MaxValue : new TimeSpan((long)value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UInt64(long value) => unchecked(value < (long)ulong.MinValue ? ulong.MinValue : (ulong)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UInt64(int value) => unchecked(value < (int)ulong.MinValue ? ulong.MinValue : (ulong)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UInt64(short value) => unchecked(value < (short)ulong.MinValue ? ulong.MinValue : (ulong)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UInt64(sbyte value) => unchecked(value < (sbyte)ulong.MinValue ? ulong.MinValue : (ulong)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UInt64(bool value) => value ? 1UL : 0UL;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UInt64(char value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Signed(ulong value) => unchecked((long)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Signed(uint value) => unchecked((int)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Signed(ushort value) => unchecked((short)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Signed(byte value) => unchecked((sbyte)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Signed<T>(T value) => To<T>.Signed(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Unsigned(long value) => unchecked((ulong)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Unsigned(int value) => unchecked((uint)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Unsigned(short value) => unchecked((ushort)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Unsigned(sbyte value) => unchecked((byte)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Unsigned<T>(T value) => To<T>.Unsigned(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T UnsignedAs<T>(object value) => To<T>.UnsignedAs(value);
    }
}
