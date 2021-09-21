namespace Platform::Converters::Benchmarks
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<Int32ToUInt64ConverterBenchmarks>();
            BenchmarkRunner.Run<UInt64ToUInt64ConverterBenchmarks>();
        }
    };
}
