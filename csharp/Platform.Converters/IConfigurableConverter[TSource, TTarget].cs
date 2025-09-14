using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>Represents a converter with configurable conflict resolution strategy for converting values from <typeparamref name="TSource"/> type to <typeparamref name="TTarget"/> type.</para>
    /// <para>Представляет конвертер с настраиваемой стратегией разрешения конфликтов для конвертации значений из типа <typeparamref name="TSource"/> в тип <typeparamref name="TTarget"/>.</para>
    /// </summary>
    /// <typeparam name="TSource"><para>Source type of conversion.</para><para>Исходный тип конверсии.</para></typeparam>
    /// <typeparam name="TTarget"><para>Target type of conversion.</para><para>Целевой тип конверсии.</para></typeparam>
    public interface IConfigurableConverter<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        /// <summary>
        /// <para>Gets the strategy used for resolving conversion conflicts.</para>
        /// <para>Получает стратегию, используемую для разрешения конфликтов конверсии.</para>
        /// </summary>
        ConversionConflictResolutionStrategy Strategy { get; }
        
        /// <summary>
        /// <para>Creates a new converter with the specified conflict resolution strategy.</para>
        /// <para>Создает новый конвертер с указанной стратегией разрешения конфликтов.</para>
        /// </summary>
        /// <param name="strategy"><para>The conflict resolution strategy to use.</para><para>Стратегия разрешения конфликтов для использования.</para></param>
        /// <returns><para>A new converter with the specified strategy.</para><para>Новый конвертер с указанной стратегией.</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IConfigurableConverter<TSource, TTarget> WithStrategy(ConversionConflictResolutionStrategy strategy);
    }
}