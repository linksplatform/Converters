using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CS0612 // Type or member is obsolete

namespace Platform.Converters.Benchmarks
{
    [SimpleJob]
    [MemoryDiagnoser]
    public class UInt64ToUInt64ConverterBenchmarks
    {
        [Params(1000, 10000, 100000, 1000000, 10000000, 100000000)]
        public int N { get; set; }

        private static UncheckedConverter<ulong, ulong> _uInt64ToUInt64Converter;
        private static IFormatProvider _formatProvider;

        [GlobalSetup]
        public static void Setup()
        {
            _uInt64ToUInt64Converter = UncheckedConverter<ulong, ulong>.Default;
            _formatProvider = CultureInfo.InvariantCulture;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ulong ConverterWrapperWithNoInlining(ulong value) => _uInt64ToUInt64Converter.Convert(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ConverterWrapperWithAggressiveInlining(ulong value) => _uInt64ToUInt64Converter.Convert(value);

        public static ulong ConverterWrapper(ulong value) => _uInt64ToUInt64Converter.Convert(value);

        [Benchmark]
        public ulong ConverterFromLocalStaticField() => _uInt64ToUInt64Converter.Convert(2UL);

        [Benchmark]
        public ulong ConverterFromGlobalStaticField() => UncheckedConverter<ulong, ulong>.Default.Convert(2UL);

        [Benchmark]
        public ulong ToUInt64() => To.UInt64(2UL);

        [Benchmark]
        public ulong SystemConvertToUInt64() => Convert.ToUInt64(2UL);

        [Benchmark]
        public ulong SystemConvertChangeType() => (ulong)Convert.ChangeType(2UL, typeof(ulong));

        [Benchmark]
        public ulong IConvertibleToUInt64() => ((IConvertible)2UL).ToUInt64(_formatProvider);

        [Benchmark]
        public ulong IConvertibleToType() => (ulong)((IConvertible)2UL).ToType(typeof(ulong), _formatProvider);

        [Benchmark]
        public ulong StaticFunctionWithoutInlining() => ConverterWrapperWithNoInlining(2UL);

        [Benchmark]
        public ulong StaticFunctionWithAggressiveInlining() => ConverterWrapperWithAggressiveInlining(2UL);

        [Benchmark]
        public ulong StaticFunction() => ConverterWrapper(2UL);
    }
}
