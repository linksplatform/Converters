using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822 // Mark members as static

namespace Platform.Converters.Benchmarks
{
    /// <summary>
    /// <para>
    /// Represents the int 32 to int 64 converter benchmarks.
    /// </para>
    /// <para></para>
    /// </summary>
    [SimpleJob]
    [MemoryDiagnoser]
    public class Int32ToUInt64ConverterBenchmarks
    {
        /// <summary>
        /// <para>
        /// The int 32 to int 64converter.
        /// </para>
        /// <para></para>
        /// </summary>
        private static UncheckedConverter<int, ulong> _int32ToUInt64converter;
        /// <summary>
        /// <para>
        /// The object to int 64 converter.
        /// </para>
        /// <para></para>
        /// </summary>
        private static UncheckedConverter<object, ulong> _objectToUInt64Converter;
        /// <summary>
        /// <para>
        /// The format provider.
        /// </para>
        /// <para></para>
        /// </summary>
        private static IFormatProvider _formatProvider;

        /// <summary>
        /// <para>
        /// Setup.
        /// </para>
        /// <para></para>
        /// </summary>
        [GlobalSetup]
        public static void Setup()
        {
            _int32ToUInt64converter = UncheckedConverter<int, ulong>.Default;
            _objectToUInt64Converter = UncheckedConverter<object, ulong>.Default;
            _formatProvider = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// <para>
        /// Converters the wrapper with no inlining using the specified value.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ulong ConverterWrapperWithNoInlining(int value) => _int32ToUInt64converter.Convert(value);

        /// <summary>
        /// <para>
        /// Converters the wrapper with aggressive inlining using the specified value.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ConverterWrapperWithAggressiveInlining(int value) => _int32ToUInt64converter.Convert(value);

        /// <summary>
        /// <para>
        /// Converters the wrapper using the specified value.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        public static ulong ConverterWrapper(int value) => _int32ToUInt64converter.Convert(value);

        /// <summary>
        /// <para>
        /// Converters the from local static field.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong ConverterFromLocalStaticField() => _int32ToUInt64converter.Convert(2);

        /// <summary>
        /// <para>
        /// Converters the from global static field.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong ConverterFromGlobalStaticField() => UncheckedConverter<int, ulong>.Default.Convert(2);

        /// <summary>
        /// <para>
        /// Systems the convert object to u int 64.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong SystemConvertObjectToUInt64() => Convert.ToUInt64((object)2);

        /// <summary>
        /// <para>
        /// Converts the object to u int 64.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong ConvertObjectToUInt64() => _objectToUInt64Converter.Convert(2);

        /// <summary>
        /// <para>
        /// Systems the convert to u int 64.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong SystemConvertToUInt64() => Convert.ToUInt64(2);

        /// <summary>
        /// <para>
        /// Systems the convert change type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong SystemConvertChangeType() => (ulong)Convert.ChangeType(2, typeof(ulong));

        /// <summary>
        /// <para>
        /// Is the convertible to u int 64.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong IConvertibleToUInt64() => ((IConvertible)2).ToUInt64(_formatProvider);

        /// <summary>
        /// <para>
        /// Is the convertible to type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong IConvertibleToType() => (ulong)((IConvertible)2).ToType(typeof(ulong), _formatProvider);

        /// <summary>
        /// <para>
        /// Statics the function without inlining.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong StaticFunctionWithoutInlining() => ConverterWrapperWithNoInlining(2);

        /// <summary>
        /// <para>
        /// Statics the function with aggressive inlining.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong StaticFunctionWithAggressiveInlining() => ConverterWrapperWithAggressiveInlining(2);

        /// <summary>
        /// <para>
        /// Statics the function.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The ulong</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public ulong StaticFunction() => ConverterWrapper(2);
    }
}
