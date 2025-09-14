using System;
using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822 // Mark members as static

namespace Platform.Converters.Benchmarks
{
    public class ConfigurableConverterBenchmarks
    {
        private static readonly IConfigurableConverter<ulong, int> ThrowExceptionConverter = 
            ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.ThrowException);
        private static readonly IConfigurableConverter<ulong, int> ClampToMaxConverter = 
            ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.ClampToMax);
        private static readonly IConfigurableConverter<ulong, int> AllowOverflowConverter = 
            ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.AllowOverflow);
        private static readonly IConfigurableConverter<ulong, int> ResetToDefaultConverter = 
            ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.ResetToDefault);
        
        private const ulong InRangeValue = 1000UL;
        private const ulong OverflowValue = (ulong)int.MaxValue + 1000UL;

        [Benchmark]
        public int DirectCast() => (int)InRangeValue;

        [Benchmark]
        public int UncheckedConverterDefault() => UncheckedConverter<ulong, int>.Default.Convert(InRangeValue);

        [Benchmark]
        public int CheckedConverterDefault() => CheckedConverter<ulong, int>.Default.Convert(InRangeValue);

        [Benchmark]
        public int ConfigurableThrowException() => ThrowExceptionConverter.Convert(InRangeValue);

        [Benchmark]
        public int ConfigurableClampToMax() => ClampToMaxConverter.Convert(InRangeValue);

        [Benchmark]
        public int ConfigurableAllowOverflow() => AllowOverflowConverter.Convert(InRangeValue);

        [Benchmark]
        public int ConfigurableResetToDefault() => ResetToDefaultConverter.Convert(InRangeValue);

        [Benchmark]
        public int SystemConvertToInt32() => Convert.ToInt32(InRangeValue);

        [Benchmark]
        public int ConfigurableDefault() => ConfigurableConverter<ulong, int>.Default.Convert(InRangeValue);
        
        // Benchmark overflow scenarios (when values are in range, to avoid exceptions in benchmark)
        [Benchmark]
        public int ConfigurableClampToMaxWithOverflow() => ClampToMaxConverter.Convert(OverflowValue);

        [Benchmark]
        public int ConfigurableAllowOverflowWithOverflow() => AllowOverflowConverter.Convert(OverflowValue);

        [Benchmark]
        public int ConfigurableResetToDefaultWithOverflow() => ResetToDefaultConverter.Convert(OverflowValue);
    }
}