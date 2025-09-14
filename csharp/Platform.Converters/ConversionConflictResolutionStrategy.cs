using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>
    /// Defines strategies for resolving conversion conflicts when source values exceed target type limits.
    /// </para>
    /// <para></para>
    /// </summary>
    public enum ConversionConflictResolutionStrategy
    {
        /// <summary>
        /// <para>Throw an exception when conversion conflicts occur.</para>
        /// <para>Бросить исключение при возникновении конфликтов конверсии.</para>
        /// </summary>
        ThrowException,
        
        /// <summary>
        /// <para>Reset value to the target type's default value.</para>
        /// <para>Сбросить значение к значению по умолчанию целевого типа.</para>
        /// </summary>
        ResetToDefault,
        
        /// <summary>
        /// <para>Use target type's maximum value as the closest matching value.</para>
        /// <para>Использовать максимальное значение целевого типа как наиболее близкое соответствие.</para>
        /// </summary>
        ClampToMax,
        
        /// <summary>
        /// <para>Use target type's minimum value as the closest matching value.</para>
        /// <para>Использовать минимальное значение целевого типа как наиболее близкое соответствие.</para>
        /// </summary>
        ClampToMin,
        
        /// <summary>
        /// <para>Clamp to the nearest valid value (min/max) based on the source value.</para>
        /// <para>Ограничить до ближайшего допустимого значения (мин/макс) на основе исходного значения.</para>
        /// </summary>
        ClampToNearest,
        
        /// <summary>
        /// <para>Use unchecked arithmetic (allows overflow/wraparound).</para>
        /// <para>Использовать непроверенную арифметику (допускает переполнение/зацикливание).</para>
        /// </summary>
        AllowOverflow,
        
        /// <summary>
        /// <para>Use System.Convert logic (similar to ThrowException but uses IConvertible).</para>
        /// <para>Использовать логику System.Convert (аналогично ThrowException, но использует IConvertible).</para>
        /// </summary>
        SystemConvert,
        
        /// <summary>
        /// <para>Use explicit IConvertible interface implementations.</para>
        /// <para>Использовать явные реализации интерфейса IConvertible.</para>
        /// </summary>
        UseIConvertible
    }
}