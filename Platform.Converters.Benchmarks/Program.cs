using BenchmarkDotNet.Running;

namespace Platform.Converters.Benchmarks
{
    static class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<Int32ToUInt64ConverterBenchmarks>();
            BenchmarkRunner.Run<UInt64ToUInt64ConverterBenchmarks>();
        }
    }
}
