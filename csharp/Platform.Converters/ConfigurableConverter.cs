using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>
    /// Represents the configurable converter with selectable conflict resolution strategies.
    /// </para>
    /// <para></para>
    /// </summary>
    public sealed class ConfigurableConverter<TSource, TTarget> : IConfigurableConverter<TSource, TTarget>
    {
        private readonly ConversionConflictResolutionStrategy _strategy;
        private readonly Func<TSource, TTarget> _convertFunction;
        
        /// <summary>
        /// <para>
        /// Gets the default configurable converter (uses ThrowException strategy).
        /// </para>
        /// <para></para>
        /// </summary>
        public static IConfigurableConverter<TSource, TTarget> Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = Create(ConversionConflictResolutionStrategy.ThrowException);
        
        /// <summary>
        /// <para>
        /// Gets the strategy used for resolving conversion conflicts.
        /// </para>
        /// <para></para>
        /// </summary>
        public ConversionConflictResolutionStrategy Strategy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _strategy;
        }
        
        private ConfigurableConverter(ConversionConflictResolutionStrategy strategy, Func<TSource, TTarget> convertFunction)
        {
            _strategy = strategy;
            _convertFunction = convertFunction;
        }
        
        /// <summary>
        /// <para>
        /// Creates a new configurable converter with the specified conflict resolution strategy.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="strategy">The conflict resolution strategy to use.</param>
        /// <returns>A new configurable converter with the specified strategy.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IConfigurableConverter<TSource, TTarget> Create(ConversionConflictResolutionStrategy strategy)
        {
            var convertFunction = CreateConvertFunction(strategy);
            return new ConfigurableConverter<TSource, TTarget>(strategy, convertFunction);
        }
        
        /// <summary>
        /// <para>
        /// Creates a new converter with the specified conflict resolution strategy.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="strategy">The conflict resolution strategy to use.</param>
        /// <returns>A new converter with the specified strategy.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IConfigurableConverter<TSource, TTarget> WithStrategy(ConversionConflictResolutionStrategy strategy) => Create(strategy);
        
        /// <summary>
        /// <para>Converts the value of the TSource type to the value of the TTarget type.</para>
        /// <para>Конвертирует значение типа TSource в значение типа TTarget.</para>
        /// </summary>
        /// <param name="source">The TSource type value.</param>
        /// <returns>The converted value of the TTarget type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TTarget Convert(TSource source) => _convertFunction(source);
        
        private static Func<TSource, TTarget> CreateConvertFunction(ConversionConflictResolutionStrategy strategy)
        {
            return strategy switch
            {
                ConversionConflictResolutionStrategy.ThrowException => CreateCheckedConverter(),
                ConversionConflictResolutionStrategy.ResetToDefault => CreateDefaultValueConverter(),
                ConversionConflictResolutionStrategy.ClampToMax => CreateClampToMaxConverter(),
                ConversionConflictResolutionStrategy.ClampToMin => CreateClampToMinConverter(),
                ConversionConflictResolutionStrategy.ClampToNearest => CreateClampToNearestConverter(),
                ConversionConflictResolutionStrategy.AllowOverflow => CreateUncheckedConverter(),
                ConversionConflictResolutionStrategy.SystemConvert => CreateSystemConvertConverter(),
                ConversionConflictResolutionStrategy.UseIConvertible => CreateIConvertibleConverter(),
                _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unknown conversion conflict resolution strategy.")
            };
        }
        
        private static Func<TSource, TTarget> CreateCheckedConverter()
        {
            return source =>
            {
                try
                {
                    return (TTarget)System.Convert.ChangeType(source, typeof(TTarget))!;
                }
                catch (InvalidCastException) when (typeof(TTarget).IsValueType)
                {
                    throw new OverflowException($"Value {source} cannot be converted to {typeof(TTarget).Name}.");
                }
            };
        }
        
        private static Func<TSource, TTarget> CreateDefaultValueConverter()
        {
            return _ => default(TTarget)!;
        }
        
        private static Func<TSource, TTarget> CreateClampToMaxConverter()
        {
            return source => ConversionHelper<TSource, TTarget>.ClampToMax(source);
        }
        
        private static Func<TSource, TTarget> CreateClampToMinConverter()
        {
            return source => ConversionHelper<TSource, TTarget>.ClampToMin(source);
        }
        
        private static Func<TSource, TTarget> CreateClampToNearestConverter()
        {
            return source => ConversionHelper<TSource, TTarget>.ClampToNearest(source);
        }
        
        private static Func<TSource, TTarget> CreateUncheckedConverter()
        {
            return source => UncheckedConverter<TSource, TTarget>.Default.Convert(source);
        }
        
        private static Func<TSource, TTarget> CreateSystemConvertConverter()
        {
            return source => (TTarget)System.Convert.ChangeType(source, typeof(TTarget))!;
        }
        
        private static Func<TSource, TTarget> CreateIConvertibleConverter()
        {
            return source =>
            {
                if (source is IConvertible convertible)
                {
                    return (TTarget)convertible.ToType(typeof(TTarget), null);
                }
                throw new InvalidCastException($"Source type {typeof(TSource).Name} does not implement IConvertible.");
            };
        }
    }
}