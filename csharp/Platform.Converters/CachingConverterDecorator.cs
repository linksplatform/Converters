using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform.Collections;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>
    /// Represents the caching converter decorator.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="IConverter{TSource, TTarget}"/>
    public class CachingConverterDecorator<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        /// <summary>
        /// <para>
        /// The base converter.
        /// </para>
        /// <para></para>
        /// </summary>
        private readonly IConverter<TSource, TTarget> _baseConverter;
        /// <summary>
        /// <para>
        /// The cache.
        /// </para>
        /// <para></para>
        /// </summary>
        private readonly IDictionary<TSource, TTarget> _cache;

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="CachingConverterDecorator"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="baseConverter">
        /// <para>A base converter.</para>
        /// <para></para>
        /// </param>
        /// <param name="cache">
        /// <para>A cache.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CachingConverterDecorator(IConverter<TSource, TTarget> baseConverter, IDictionary<TSource, TTarget> cache) => (_baseConverter, _cache) = (baseConverter, cache);

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="CachingConverterDecorator"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="baseConverter">
        /// <para>A base converter.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CachingConverterDecorator(IConverter<TSource, TTarget> baseConverter) : this(baseConverter, new Dictionary<TSource, TTarget>()) { }

        /// <summary>
        /// <para>
        /// Converts the source.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="source">
        /// <para>The source.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The target</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TTarget Convert(TSource source) => _cache.GetOrAdd(source, _baseConverter.Convert);
    }
}