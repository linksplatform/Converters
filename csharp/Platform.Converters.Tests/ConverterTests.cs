using System;
using System.Collections.Generic;
using Xunit;

namespace Platform.Converters.Tests
{
    public static class ConverterTests
    {
        [Fact]
        public static void SameTypeTest()
        {
            var result = UncheckedConverter<ulong, ulong>.Default.Convert(2UL);
            Assert.Equal(2UL, result);
            result = CheckedConverter<ulong, ulong>.Default.Convert(2UL);
            Assert.Equal(2UL, result);
        }

        [Fact]
        public static void Int32ToUInt64Test()
        {
            var result = UncheckedConverter<int, ulong>.Default.Convert(2);
            Assert.Equal(2UL, result);
            result = CheckedConverter<int, ulong>.Default.Convert(2);
            Assert.Equal(2UL, result);
        }

        [Fact]
        public static void SignExtensionTest()
        {
            var result = UncheckedSignExtendingConverter<byte, long>.Default.Convert(128);
            Assert.Equal(-128L, result);
            result = UncheckedConverter<byte, long>.Default.Convert(128);
            Assert.Equal(128L, result);
        }

        [Fact]
        public static void ObjectTest()
        {
            TestObjectConversion("1");
            TestObjectConversion(DateTime.UtcNow);
            TestObjectConversion(1.0F);
            TestObjectConversion(1.0D);
            TestObjectConversion(1.0M);
            TestObjectConversion(1UL);
            TestObjectConversion(1L);
            TestObjectConversion(1U);
            TestObjectConversion(1);
            TestObjectConversion((char)1);
            TestObjectConversion((ushort)1);
            TestObjectConversion((short)1);
            TestObjectConversion((byte)1);
            TestObjectConversion((sbyte)1);
            TestObjectConversion(true);
        }
        private static void TestObjectConversion<T>(T value) => Assert.Equal(value, UncheckedConverter<object, T>.Default.Convert(value));

        [Fact]
        public static void CachingConverterDecoratorBasicTest()
        {
            var baseConverter = UncheckedConverter<int, long>.Default;
            var cachingConverter = new CachingConverterDecorator<int, long>(baseConverter);
            
            var result1 = cachingConverter.Convert(42);
            var result2 = cachingConverter.Convert(42);
            
            Assert.Equal(42L, result1);
            Assert.Equal(42L, result2);
        }

        [Fact]
        public static void CachingConverterDecoratorWithCustomCacheTest()
        {
            var baseConverter = UncheckedConverter<int, long>.Default;
            var cache = new Dictionary<int, long>();
            var cachingConverter = new CachingConverterDecorator<int, long>(baseConverter, cache);
            
            var result1 = cachingConverter.Convert(123);
            Assert.Equal(123L, result1);
            Assert.True(cache.ContainsKey(123));
            Assert.Equal(123L, cache[123]);
            
            // Modify cache to verify it's being used
            cache[123] = 999L;
            var result2 = cachingConverter.Convert(123);
            Assert.Equal(999L, result2);
        }

        [Fact]
        public static void CachingConverterDecoratorMultipleValuesTest()
        {
            var baseConverter = UncheckedConverter<byte, int>.Default;
            var cachingConverter = new CachingConverterDecorator<byte, int>(baseConverter);
            
            var values = new byte[] { 1, 2, 3, 1, 2, 3 };
            var results = new List<int>();
            
            foreach (var value in values)
            {
                results.Add(cachingConverter.Convert(value));
            }
            
            Assert.Equal(new[] { 1, 2, 3, 1, 2, 3 }, results);
        }

        [Fact]
        public static void CheckedConverterOverflowTest()
        {
            Assert.Throws<OverflowException>(() => 
                CheckedConverter<long, int>.Default.Convert(long.MaxValue));
        }

        [Fact]
        public static void UncheckedConverterOverflowTest()
        {
            // This should not throw, even with overflow
            var result = UncheckedConverter<long, int>.Default.Convert(long.MaxValue);
            Assert.Equal(-1, result); // Expected overflow behavior
        }

        [Fact]
        public static void SignExtensionVariousTypesTest()
        {
            // Test sign extension with various signed types
            Assert.Equal(-1L, UncheckedSignExtendingConverter<sbyte, long>.Default.Convert(-1));
            Assert.Equal(-1L, UncheckedSignExtendingConverter<short, long>.Default.Convert(-1));
            Assert.Equal(-1L, UncheckedSignExtendingConverter<int, long>.Default.Convert(-1));
            
            // Test with positive values
            Assert.Equal(127L, UncheckedSignExtendingConverter<sbyte, long>.Default.Convert(127));
            Assert.Equal(32767L, UncheckedSignExtendingConverter<short, long>.Default.Convert(32767));
        }

        [Fact]
        public static void FloatingPointConversionsTest()
        {
            Assert.Equal(3.14, UncheckedConverter<float, double>.Default.Convert(3.14f), 5);
            Assert.Equal(2.71f, UncheckedConverter<double, float>.Default.Convert(2.71), 2);
            Assert.Equal(42, UncheckedConverter<float, int>.Default.Convert(42.7f));
        }

        [Fact]
        public static void BooleanConversionsTest()
        {
            Assert.Equal(1, UncheckedConverter<bool, int>.Default.Convert(true));
            Assert.Equal(0, UncheckedConverter<bool, int>.Default.Convert(false));
            Assert.Equal(1L, UncheckedConverter<bool, long>.Default.Convert(true));
            Assert.Equal(0L, UncheckedConverter<bool, long>.Default.Convert(false));
        }

        [Fact]
        public static void CharConversionsTest()
        {
            Assert.Equal(65, UncheckedConverter<char, int>.Default.Convert('A'));
            Assert.Equal(65L, UncheckedConverter<char, long>.Default.Convert('A'));
            Assert.Equal('A', UncheckedConverter<int, char>.Default.Convert(65));
        }

        [Fact]
        public static void NullObjectConversionTest()
        {
            // Test null object conversion - should return default values
            Assert.Equal(0, UncheckedConverter<object, int>.Default.Convert(null));
            Assert.Equal(0L, UncheckedConverter<object, long>.Default.Convert(null));
            Assert.False(UncheckedConverter<object, bool>.Default.Convert(null));
            Assert.Equal(string.Empty, UncheckedConverter<object, string>.Default.Convert(null));
            Assert.Equal(DateTime.MinValue, UncheckedConverter<object, DateTime>.Default.Convert(null));
            Assert.Equal(decimal.Zero, UncheckedConverter<object, decimal>.Default.Convert(null));
        }

        [Fact]
        public static void UnsupportedObjectConversionTest()
        {
            // Test conversion to unsupported type should throw TypeInitializationException (which wraps NotSupportedException)
            Assert.Throws<TypeInitializationException>(() => 
            {
                var converter = UncheckedConverter<object, System.Drawing.Point>.Default;
                converter.Convert(new System.Drawing.Point(1, 2));
            });
        }

        [Fact]
        public static void BasicTypeConversionsTest()
        {
            // Test basic type conversions that are guaranteed to work
            Assert.Equal(100L, UncheckedConverter<int, long>.Default.Convert(100));
            Assert.Equal((short)255, UncheckedConverter<int, short>.Default.Convert(255));
            Assert.Equal(-50L, UncheckedConverter<short, long>.Default.Convert(-50));
        }

        [Fact]
        public static void BoundaryValueTests()
        {
            // Test boundary values for various numeric types
            Assert.Equal(byte.MaxValue, UncheckedConverter<byte, byte>.Default.Convert(byte.MaxValue));
            Assert.Equal(byte.MinValue, UncheckedConverter<byte, byte>.Default.Convert(byte.MinValue));
            
            Assert.Equal(short.MaxValue, UncheckedConverter<short, short>.Default.Convert(short.MaxValue));
            Assert.Equal(short.MinValue, UncheckedConverter<short, short>.Default.Convert(short.MinValue));
            
            Assert.Equal(int.MaxValue, UncheckedConverter<int, int>.Default.Convert(int.MaxValue));
            Assert.Equal(int.MinValue, UncheckedConverter<int, int>.Default.Convert(int.MinValue));
        }

        [Fact]
        public static void CachingConverterDecoratorPerformanceTest()
        {
            var baseConverter = UncheckedConverter<int, long>.Default;
            var cachingConverter = new CachingConverterDecorator<int, long>(baseConverter);
            
            // Convert the same value multiple times - should be fast after first conversion
            const int value = 42;
            const int iterations = 1000;
            
            for (int i = 0; i < iterations; i++)
            {
                var result = cachingConverter.Convert(value);
                Assert.Equal(42L, result);
            }
        }

        [Fact]
        public static void CachingConverterDecoratorStringHandlingTest()
        {
            var baseConverter = UncheckedConverter<string, string>.Default;
            var cachingConverter = new CachingConverterDecorator<string, string>(baseConverter);
            
            // Test string handling (avoid null keys since Dictionary doesn't support them)
            var result1 = cachingConverter.Convert("test");
            var result2 = cachingConverter.Convert("test");
            
            Assert.Equal("test", result1);
            Assert.Equal(result1, result2);
        }

        [Fact]
        public static void MultipleConverterInstancesTest()
        {
            // Test that different converter instances work independently
            var converter1 = UncheckedConverter<int, long>.Default;
            var converter2 = CheckedConverter<int, long>.Default;
            
            Assert.Equal(100L, converter1.Convert(100));
            Assert.Equal(100L, converter2.Convert(100));
            
            // They should be different instances but produce same results for valid conversions
            Assert.NotSame(converter1, converter2);
        }

        [Fact]
        public static void NegativeNumberConversionsTest()
        {
            // Test negative number conversions
            Assert.Equal(-1, UncheckedConverter<int, int>.Default.Convert(-1));
            Assert.Equal(-100L, UncheckedConverter<int, long>.Default.Convert(-100));
            Assert.Equal(-50.5f, UncheckedConverter<double, float>.Default.Convert(-50.5), 1);
        }

        [Fact]
        public static void ZeroValueConversionsTest()
        {
            // Test zero value conversions across different types
            Assert.Equal(0, UncheckedConverter<int, int>.Default.Convert(0));
            Assert.Equal(0L, UncheckedConverter<int, long>.Default.Convert(0));
            Assert.Equal(0.0f, UncheckedConverter<int, float>.Default.Convert(0));
            Assert.Equal(0.0, UncheckedConverter<int, double>.Default.Convert(0));
            Assert.Equal((byte)0, UncheckedConverter<int, byte>.Default.Convert(0));
        }

        [Fact]
        public static void LargeNumberConversionsTest()
        {
            // Test conversions with large numbers
            var largeInt = 1000000;
            Assert.Equal(1000000L, UncheckedConverter<int, long>.Default.Convert(largeInt));
            Assert.Equal(1000000.0, UncheckedConverter<int, double>.Default.Convert(largeInt));
        }
    }
}
