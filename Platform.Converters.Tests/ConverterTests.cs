using Platform.Diagnostics;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace Platform.Converters.Tests
{
    public class ConverterTests
    {
        private readonly ITestOutputHelper _output;
        private static readonly IConverter<ulong, ulong> _uInt64ToUInt64Converter = Converter<ulong, ulong>.Default;
        private static readonly IConverter<int, ulong> _int32ToUInt64converter = Converter<int, ulong>.Default;

        public ConverterTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void SameTypeTest()
        {
            var result = Converter<ulong, ulong>.Default.Convert(2UL);
            Assert.Equal(2UL, result);
        }

        [Fact]
        public void SameTypePerformanceComparisonTest()
        {
            var N = 10000000;
            var result = 0UL;

            // Warmup
            for (int i = 0; i < N; i++)
            {
                result = _uInt64ToUInt64Converter.Convert(2UL);
            }
            for (int i = 0; i < N; i++)
            {
                result = Converter<ulong, ulong>.Default.Convert(2UL);
            }
            for (int i = 0; i < N; i++)
            {
                result = Convert(2UL);
            }
            for (int i = 0; i < N; i++)
            {
                result = To.UInt64(2UL);
            }
            for (int i = 0; i < N; i++)
            {
                result = System.Convert.ToUInt64(2UL);
            }
            for (int i = 0; i < N; i++)
            {
                result = (ulong)System.Convert.ChangeType(2UL, typeof(ulong));
            }

            var ts1 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = _uInt64ToUInt64Converter.Convert(2UL);
                }
            });
            var ts2 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = Converter<ulong, ulong>.Default.Convert(2UL);
                }
            });
            var ts3 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = Convert(2UL);
                }
            });
            var ts4 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = To.UInt64(2UL);
                }
            });
            var ts5 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = System.Convert.ToUInt64(2UL);
                }
            });
            var ts6 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = (ulong)System.Convert.ChangeType(2UL, typeof(ulong));
                }
            });
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            var ts7 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = ((IConvertible)2UL).ToUInt64(formatProvider);
                }
            });
            var ts8 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = (ulong)((IConvertible)2UL).ToType(typeof(ulong), formatProvider);
                }
            });

            _output.WriteLine($"{ts1} {ts2} {ts3} {ts4} {ts5} {ts6} {ts7} {ts8} {result}");
        }

        [Fact]
        public void Int32ToUInt64Test()
        {
            var result = Converter<int, ulong>.Default.Convert(2);
            Assert.Equal(2UL, result);
        }

        [Fact]
        public void Int32ToUInt64PerformanceComparisonTest()
        {
            var N = 10000000;
            var result = 0UL;

            // Warmup
            for (int i = 0; i < N; i++)
            {
                result = _int32ToUInt64converter.Convert(2);
            }
            for (int i = 0; i < N; i++)
            {
                result = Converter<ulong, ulong>.Default.Convert(2);
            }
            for (int i = 0; i < N; i++)
            {
                result = Convert(2);
            }
            for (int i = 0; i < N; i++)
            {
                result = To.UInt64(2);
            }
            for (int i = 0; i < N; i++)
            {
                result = System.Convert.ToUInt64(2);
            }
            for (int i = 0; i < N; i++)
            {
                result = (ulong)System.Convert.ChangeType(2, typeof(ulong));
            }

            var ts1 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = _int32ToUInt64converter.Convert(2);
                }
            });
            var ts2 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = Converter<ulong, ulong>.Default.Convert(2);
                }
            });
            var ts3 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = Convert(2);
                }
            });
            var ts4 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = To.UInt64(2);
                }
            });
            var ts5 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = System.Convert.ToUInt64(2);
                }
            });
            var ts6 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = (ulong)System.Convert.ChangeType(2, typeof(ulong));
                }
            });
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            var ts7 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = ((IConvertible)2).ToUInt64(formatProvider);
                }
            });
            var ts8 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = (ulong)((IConvertible)2).ToType(typeof(ulong), formatProvider);
                }
            });

            _output.WriteLine($"{ts1} {ts2} {ts3} {ts4} {ts5} {ts6} {ts7} {ts8} {result}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Convert(ulong value) => _uInt64ToUInt64Converter.Convert(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Convert(int value) => _int32ToUInt64converter.Convert(value);
    }
}
