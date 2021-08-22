using BenchmarkDotNet.Running;

namespace Platform.Converters.Benchmarks
{
    /// <summary>
    /// <para>
    /// Represents the program.
    /// </para>
    /// <para></para>
    /// </summary>
    static class Program
    {
        /// <summary>
        /// <para>
        /// Main.
        /// </para>
        /// <para></para>
        /// </summary>
        static void Main()
        {
            BenchmarkRunner.Run<Int32ToUInt64ConverterBenchmarks>();
            BenchmarkRunner.Run<UInt64ToUInt64ConverterBenchmarks>();
        }
    }
}
