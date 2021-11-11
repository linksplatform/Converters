using System;
using Xunit;

namespace Platform.Converters.Tests
{
    /// <summary>
    /// <para>
    /// Represents the converter tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class ConverterTests
    {
        /// <summary>
        /// <para>
        /// Tests that same type test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public static void SameTypeTest()
        {
            var result = UncheckedConverter<ulong, ulong>.Default.Convert(2UL);
            Assert.Equal(2UL, result);
            result = CheckedConverter<ulong, ulong>.Default.Convert(2UL);
            Assert.Equal(2UL, result);
        }

        /// <summary>
        /// <para>
        /// Tests that int 32 to u int 64 test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public static void Int32ToUInt64Test()
        {
            var result = UncheckedConverter<int, ulong>.Default.Convert(2);
            Assert.Equal(2UL, result);
            result = CheckedConverter<int, ulong>.Default.Convert(2);
            Assert.Equal(2UL, result);
        }

        /// <summary>
        /// <para>
        /// Tests that sign extension test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public static void SignExtensionTest()
        {
            var result = UncheckedSignExtendingConverter<byte, long>.Default.Convert(128);
            Assert.Equal(-128L, result);
            result = UncheckedConverter<byte, long>.Default.Convert(128);
            Assert.Equal(128L, result);
        }

        /// <summary>
        /// <para>
        /// Tests that object test.
        /// </para>
        /// <para></para>
        /// </summary>
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
    }
}
