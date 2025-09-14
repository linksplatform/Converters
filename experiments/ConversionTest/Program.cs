using System;
using Platform.Converters;

var analyzer = new ConversionAnalysis();
analyzer.Run();

public class ConversionAnalysis
{
    public void Run()
    {
        Console.WriteLine("=== Current Conversion Behavior Analysis ===");
        
        // Test overflow scenarios that would cause conflicts
        Console.WriteLine("\n1. Testing UInt64 to Int32 with overflow (should cause conflict):");
        var largeUInt64 = (ulong)int.MaxValue + 1000;
        Console.WriteLine($"Source: {largeUInt64} (> int.MaxValue: {int.MaxValue})");
        
        try
        {
            var uncheckedResult = UncheckedConverter<ulong, int>.Default.Convert(largeUInt64);
            Console.WriteLine($"UncheckedConverter result: {uncheckedResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UncheckedConverter threw: {ex.GetType().Name}: {ex.Message}");
        }
        
        try
        {
            var checkedResult = CheckedConverter<ulong, int>.Default.Convert(largeUInt64);
            Console.WriteLine($"CheckedConverter result: {checkedResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CheckedConverter threw: {ex.GetType().Name}: {ex.Message}");
        }
        
        Console.WriteLine("\n2. Testing System.Convert behavior for comparison:");
        try
        {
            var systemConvertResult = Convert.ToInt32(largeUInt64);
            Console.WriteLine($"System.Convert result: {systemConvertResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"System.Convert threw: {ex.GetType().Name}: {ex.Message}");
        }
        
        Console.WriteLine("\n3. Testing negative to unsigned conversion:");
        var negativeInt = -1000;
        Console.WriteLine($"Source: {negativeInt}");
        
        try
        {
            var uncheckedResult = UncheckedConverter<int, uint>.Default.Convert(negativeInt);
            Console.WriteLine($"UncheckedConverter result: {uncheckedResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UncheckedConverter threw: {ex.GetType().Name}: {ex.Message}");
        }
        
        try
        {
            var checkedResult = CheckedConverter<int, uint>.Default.Convert(negativeInt);
            Console.WriteLine($"CheckedConverter result: {checkedResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CheckedConverter threw: {ex.GetType().Name}: {ex.Message}");
        }
        
        Console.WriteLine("\n4. Testing IConvertible interface behavior:");
        try
        {
            var iconvertibleResult = ((IConvertible)largeUInt64).ToInt32(null);
            Console.WriteLine($"IConvertible.ToInt32 result: {iconvertibleResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"IConvertible.ToInt32 threw: {ex.GetType().Name}: {ex.Message}");
        }
        
        Console.WriteLine("\n5. Testing direct cast behavior:");
        try
        {
            var directCastResult = (int)largeUInt64;
            Console.WriteLine($"Direct cast result: {directCastResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Direct cast threw: {ex.GetType().Name}: {ex.Message}");
        }
        
        try
        {
            var checkedCastResult = checked((int)largeUInt64);
            Console.WriteLine($"Checked cast result: {checkedCastResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Checked cast threw: {ex.GetType().Name}: {ex.Message}");
        }
    }
}