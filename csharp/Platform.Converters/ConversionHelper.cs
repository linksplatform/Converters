using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>
    /// Provides helper methods for type conversion with clamping logic.
    /// </para>
    /// <para></para>
    /// </summary>
    internal static class ConversionHelper<TSource, TTarget>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTarget ClampToMax(TSource source)
        {
            return ClampValue(source, useMax: true, useMin: false);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTarget ClampToMin(TSource source)
        {
            return ClampValue(source, useMax: false, useMin: true);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTarget ClampToNearest(TSource source)
        {
            return ClampValue(source, useMax: true, useMin: true);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TTarget ClampValue(TSource source, bool useMax, bool useMin)
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);
            
            // Handle same type conversions
            if (sourceType == targetType)
            {
                return (TTarget)(object)source!;
            }
            
            // Handle numeric type conversions with clamping
            if (IsNumericType(sourceType) && IsNumericType(targetType))
            {
                return ClampNumericValue(source, useMax, useMin);
            }
            
            // For non-numeric types, try direct conversion or default
            try
            {
                return (TTarget)System.Convert.ChangeType(source, targetType)!;
            }
            catch
            {
                return default(TTarget)!;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TTarget ClampNumericValue(TSource source, bool useMax, bool useMin)
        {
            // Convert source to decimal for range checking
            decimal sourceValue = ConvertToDecimal(source);
            decimal targetMin = GetMinValue(typeof(TTarget));
            decimal targetMax = GetMaxValue(typeof(TTarget));
            
            // Apply clamping logic
            if (sourceValue < targetMin)
            {
                if (useMin)
                {
                    return ConvertFromDecimal(targetMin);
                }
                if (useMax)
                {
                    // If source is below min but we can only use max, use max
                    return ConvertFromDecimal(targetMax);
                }
            }
            else if (sourceValue > targetMax)
            {
                if (useMax)
                {
                    return ConvertFromDecimal(targetMax);
                }
                if (useMin)
                {
                    // If source is above max but we can only use min, use min
                    return ConvertFromDecimal(targetMin);
                }
            }
            
            // Value is within range, convert normally
            try
            {
                return (TTarget)System.Convert.ChangeType(source, typeof(TTarget))!;
            }
            catch
            {
                // Fallback to unchecked conversion
                return UncheckedConverter<TSource, TTarget>.Default.Convert(source);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static decimal ConvertToDecimal(TSource value)
        {
            return value switch
            {
                byte b => b,
                sbyte sb => sb,
                short s => s,
                ushort us => us,
                int i => i,
                uint ui => ui,
                long l => l,
                ulong ul => ul,
                float f => (decimal)f,
                double d => (decimal)d,
                decimal dec => dec,
                _ => System.Convert.ToDecimal(value)
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TTarget ConvertFromDecimal(decimal value)
        {
            var targetType = typeof(TTarget);
            
            if (targetType == typeof(byte)) return (TTarget)(object)(byte)value;
            if (targetType == typeof(sbyte)) return (TTarget)(object)(sbyte)value;
            if (targetType == typeof(short)) return (TTarget)(object)(short)value;
            if (targetType == typeof(ushort)) return (TTarget)(object)(ushort)value;
            if (targetType == typeof(int)) return (TTarget)(object)(int)value;
            if (targetType == typeof(uint)) return (TTarget)(object)(uint)value;
            if (targetType == typeof(long)) return (TTarget)(object)(long)value;
            if (targetType == typeof(ulong)) return (TTarget)(object)(ulong)value;
            if (targetType == typeof(float)) return (TTarget)(object)(float)value;
            if (targetType == typeof(double)) return (TTarget)(object)(double)value;
            if (targetType == typeof(decimal)) return (TTarget)(object)value;
            
            return (TTarget)System.Convert.ChangeType(value, targetType)!;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static decimal GetMinValue(Type type)
        {
            if (type == typeof(byte)) return byte.MinValue;
            if (type == typeof(sbyte)) return sbyte.MinValue;
            if (type == typeof(short)) return short.MinValue;
            if (type == typeof(ushort)) return ushort.MinValue;
            if (type == typeof(int)) return int.MinValue;
            if (type == typeof(uint)) return uint.MinValue;
            if (type == typeof(long)) return long.MinValue;
            if (type == typeof(ulong)) return ulong.MinValue;
            if (type == typeof(float)) return decimal.MinValue; // Use decimal limits for float
            if (type == typeof(double)) return decimal.MinValue; // Use decimal limits for double
            if (type == typeof(decimal)) return decimal.MinValue;
            
            return 0; // Default fallback
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static decimal GetMaxValue(Type type)
        {
            if (type == typeof(byte)) return byte.MaxValue;
            if (type == typeof(sbyte)) return sbyte.MaxValue;
            if (type == typeof(short)) return short.MaxValue;
            if (type == typeof(ushort)) return ushort.MaxValue;
            if (type == typeof(int)) return int.MaxValue;
            if (type == typeof(uint)) return uint.MaxValue;
            if (type == typeof(long)) return long.MaxValue;
            if (type == typeof(ulong)) return ulong.MaxValue;
            if (type == typeof(float)) return decimal.MaxValue; // Use decimal limits for float
            if (type == typeof(double)) return decimal.MaxValue; // Use decimal limits for double
            if (type == typeof(decimal)) return decimal.MaxValue;
            
            return decimal.MaxValue; // Default fallback
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }
    }
}