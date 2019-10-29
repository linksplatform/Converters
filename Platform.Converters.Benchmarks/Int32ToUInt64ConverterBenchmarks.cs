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
    public class Int32ToUInt64ConverterBenchmarks
    {
        [Params(1000, 10000, 100000, 1000000, 10000000, 100000000)]
        public int N { get; set; }

        private static UncheckedConverter<int, ulong> _int32ToUInt64converter;
        private static IFormatProvider _formatProvider;

        [GlobalSetup]
        public static void Setup()
        {
            _int32ToUInt64converter = UncheckedConverter<int, ulong>.Default;
            _formatProvider = CultureInfo.InvariantCulture;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ulong ConverterWrapperWithNoInlining(int value) => _int32ToUInt64converter.Convert(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ConverterWrapperWithAggressiveInlining(int value) => _int32ToUInt64converter.Convert(value);

        public static ulong ConverterWrapper(int value) => _int32ToUInt64converter.Convert(value);

        [Benchmark]
        public ulong ConverterFromLocalStaticField() => _int32ToUInt64converter.Convert(2);

        [Benchmark]
        public ulong ConverterFromGlobalStaticField() => UncheckedConverter<int, ulong>.Default.Convert(2);

        [Benchmark]
        public ulong ToUInt64() => To.UInt64(2);

        [Benchmark]
        public ulong SystemConvertToUInt64() => Convert.ToUInt64(2);

        [Benchmark]
        public ulong SystemConvertChangeType() => (ulong)Convert.ChangeType(2, typeof(ulong));

        [Benchmark]
        public ulong IConvertibleToUInt64() => ((IConvertible)2).ToUInt64(_formatProvider);

        [Benchmark]
        public ulong IConvertibleToType() => (ulong)((IConvertible)2).ToType(typeof(ulong), _formatProvider);

        [Benchmark]
        public ulong StaticFunctionWithoutInlining() => ConverterWrapperWithNoInlining(2);

        [Benchmark]
        public ulong StaticFunctionWithAggressiveInlining() => ConverterWrapperWithAggressiveInlining(2);

        [Benchmark]
        public ulong StaticFunction() => ConverterWrapper(2);
    }
}
