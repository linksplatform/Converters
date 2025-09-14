using System;
using Platform.Converters;

var tester = new ConfigurableConverterTester();
tester.Run();

public class ConfigurableConverterTester
{
    public void Run()
    {
        Console.WriteLine("=== Configurable Converter Test ===");
        
        var largeUInt64 = (ulong)int.MaxValue + 1000; // 2147484647
        var negativeInt = -1000;
        
        Console.WriteLine($"\nTesting with large UInt64: {largeUInt64} (target: int, max={int.MaxValue})");
        TestAllStrategies<ulong, int>(largeUInt64);
        
        Console.WriteLine($"\nTesting with negative int: {negativeInt} (target: uint, min={uint.MinValue}, max={uint.MaxValue})");
        TestAllStrategies<int, uint>(negativeInt);
        
        Console.WriteLine($"\nTesting with large long: {long.MaxValue} (target: int, max={int.MaxValue})");
        TestAllStrategies<long, int>(long.MaxValue);
        
        Console.WriteLine($"\nTesting within range: 100 (ulong -> int)");
        TestAllStrategies<ulong, int>(100);
    }
    
    private void TestAllStrategies<TSource, TTarget>(TSource value)
    {
        var strategies = Enum.GetValues<ConversionConflictResolutionStrategy>();
        
        foreach (var strategy in strategies)
        {
            try
            {
                var converter = ConfigurableConverter<TSource, TTarget>.Create(strategy);
                var result = converter.Convert(value);
                Console.WriteLine($"  {strategy,-20}: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  {strategy,-20}: Exception - {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}