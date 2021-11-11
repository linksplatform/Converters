using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822 // Mark members as static

namespace Platform.Converters.Benchmarks
{
    /// <summary>
    /// <para>
    /// Represents the int 64 to int 64 converter benchmarks.
    /// </para>
    /// <para></para>
    /// </summary>
    [SimpleJob]
    [MemoryDiagnoser]
    public class UInt64ToUInt64ConverterBenchmarks
    {
        /// <summary>
        /// <para>
        /// The int 64 to int 64 converter.
        /// </para>
        /// <para></para>
        /// </summary>
        private static UncheckedConverter<ulong, ulong> _uInt64ToUInt64Converter;
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
            _uInt64ToUInt64Converter = UncheckedConverter<ulong, ulong>.Default;
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
        public static ulong ConverterWrapperWithNoInlining(ulong value) => _uInt64ToUInt64Converter.Convert(value);

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
        public static ulong ConverterWrapperWithAggressiveInlining(ulong value) => _uInt64ToUInt64Converter.Convert(value);

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
        public static ulong ConverterWrapper(ulong value) => _uInt64ToUInt64Converter.Convert(value);

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
        public ulong ConverterFromLocalStaticField() => _uInt64ToUInt64Converter.Convert(2UL);

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
        public ulong ConverterFromGlobalStaticField() => UncheckedConverter<ulong, ulong>.Default.Convert(2UL);

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
        public ulong SystemConvertToUInt64() => Convert.ToUInt64(2UL);

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
        public ulong SystemConvertObjectToUInt64() => Convert.ToUInt64((object)2UL);

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
        public ulong SystemConvertChangeType() => (ulong)Convert.ChangeType(2UL, typeof(ulong));

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
        public ulong IConvertibleToUInt64() => ((IConvertible)2UL).ToUInt64(_formatProvider);

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
        public ulong IConvertibleToType() => (ulong)((IConvertible)2UL).ToType(typeof(ulong), _formatProvider);

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
        public ulong StaticFunctionWithoutInlining() => ConverterWrapperWithNoInlining(2UL);

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
        public ulong StaticFunctionWithAggressiveInlining() => ConverterWrapperWithAggressiveInlining(2UL);

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
        public ulong StaticFunction() => ConverterWrapper(2UL);
    }
}
