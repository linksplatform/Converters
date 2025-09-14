using System;
using Xunit;

namespace Platform.Converters.Tests
{
    public static class ConfigurableConverterTests
    {
        [Fact]
        public static void ThrowExceptionStrategyTest()
        {
            var converter = ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.ThrowException);
            
            // Within range should work
            Assert.Equal(100, converter.Convert(100UL));
            
            // Overflow should throw
            Assert.Throws<OverflowException>(() => converter.Convert((ulong)int.MaxValue + 1000));
        }
        
        [Fact]
        public static void ResetToDefaultStrategyTest()
        {
            var converter = ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.ResetToDefault);
            
            // Should always return default value
            Assert.Equal(0, converter.Convert(100UL));
            Assert.Equal(0, converter.Convert((ulong)int.MaxValue + 1000));
        }
        
        [Fact]
        public static void ClampToMaxStrategyTest()
        {
            var converter = ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.ClampToMax);
            
            // Within range should work normally
            Assert.Equal(100, converter.Convert(100UL));
            
            // Overflow should clamp to max
            Assert.Equal(int.MaxValue, converter.Convert((ulong)int.MaxValue + 1000));
        }
        
        [Fact]
        public static void ClampToMinStrategyTest()
        {
            var converter = ConfigurableConverter<int, uint>.Create(ConversionConflictResolutionStrategy.ClampToMin);
            
            // Within range should work normally
            Assert.Equal(100U, converter.Convert(100));
            
            // Negative should clamp to min (0)
            Assert.Equal(0U, converter.Convert(-1000));
        }
        
        [Fact]
        public static void ClampToNearestStrategyTest()
        {
            var converter = ConfigurableConverter<long, int>.Create(ConversionConflictResolutionStrategy.ClampToNearest);
            
            // Within range should work normally
            Assert.Equal(100, converter.Convert(100L));
            
            // Above max should clamp to max
            Assert.Equal(int.MaxValue, converter.Convert(long.MaxValue));
            
            // Below min should clamp to min
            Assert.Equal(int.MinValue, converter.Convert(long.MinValue));
        }
        
        [Fact]
        public static void AllowOverflowStrategyTest()
        {
            var converter = ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.AllowOverflow);
            
            // Should behave like unchecked conversion
            var largeValue = (ulong)int.MaxValue + 1000;
            var expected = UncheckedConverter<ulong, int>.Default.Convert(largeValue);
            Assert.Equal(expected, converter.Convert(largeValue));
        }
        
        [Fact]
        public static void SystemConvertStrategyTest()
        {
            var converter = ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.SystemConvert);
            
            // Within range should work
            Assert.Equal(100, converter.Convert(100UL));
            
            // Overflow should throw (like System.Convert)
            Assert.Throws<OverflowException>(() => converter.Convert((ulong)int.MaxValue + 1000));
        }
        
        [Fact]
        public static void UseIConvertibleStrategyTest()
        {
            var converter = ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.UseIConvertible);
            
            // Within range should work
            Assert.Equal(100, converter.Convert(100UL));
            
            // Overflow should throw (like IConvertible)
            Assert.Throws<OverflowException>(() => converter.Convert((ulong)int.MaxValue + 1000));
        }
        
        [Fact]
        public static void WithStrategyTest()
        {
            var original = ConfigurableConverter<ulong, int>.Create(ConversionConflictResolutionStrategy.ThrowException);
            var modified = original.WithStrategy(ConversionConflictResolutionStrategy.ClampToMax);
            
            Assert.Equal(ConversionConflictResolutionStrategy.ThrowException, original.Strategy);
            Assert.Equal(ConversionConflictResolutionStrategy.ClampToMax, modified.Strategy);
            
            // Test behavior difference
            var testValue = (ulong)int.MaxValue + 1000;
            Assert.Throws<OverflowException>(() => original.Convert(testValue));
            Assert.Equal(int.MaxValue, modified.Convert(testValue));
        }
        
        [Fact]
        public static void DefaultConverterTest()
        {
            var defaultConverter = ConfigurableConverter<ulong, int>.Default;
            
            Assert.Equal(ConversionConflictResolutionStrategy.ThrowException, defaultConverter.Strategy);
            
            // Should behave like ThrowException strategy
            Assert.Equal(100, defaultConverter.Convert(100UL));
            Assert.Throws<OverflowException>(() => defaultConverter.Convert((ulong)int.MaxValue + 1000));
        }
        
        [Fact]
        public static void SameTypeConversionTest()
        {
            var converter = ConfigurableConverter<int, int>.Create(ConversionConflictResolutionStrategy.ClampToMax);
            
            // Same type conversions should always work
            Assert.Equal(100, converter.Convert(100));
            Assert.Equal(-100, converter.Convert(-100));
            Assert.Equal(int.MaxValue, converter.Convert(int.MaxValue));
            Assert.Equal(int.MinValue, converter.Convert(int.MinValue));
        }
    }
}