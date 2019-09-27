using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    public class CachingConverterDecorator<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        private readonly IConverter<TSource, TTarget> _baseConverter;
        private readonly IDictionary<TSource, TTarget> _cache;

        public CachingConverterDecorator(IConverter<TSource, TTarget> baseConverter, IDictionary<TSource, TTarget> cache) => (_baseConverter, _cache) = (baseConverter, cache);

        public CachingConverterDecorator(IConverter<TSource, TTarget> baseConverter) : this(baseConverter, new Dictionary<TSource, TTarget>()) { }

        public TTarget Convert(TSource source)
        {
            if (!_cache.TryGetValue(source, out TTarget value))
            {
                value = _baseConverter.Convert(source);
                _cache.Add(source, value);
            }
            return value;
        }
    }
}