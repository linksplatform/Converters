namespace Platform::Converters
{
    template <typename ...> class CachingConverterDecorator;
    template <typename TSource, typename TTarget> class CachingConverterDecorator<TSource, TTarget> : public IConverter<TSource, TTarget>
    {
        private: readonly IConverter<TSource, TTarget> *_baseConverter;
        private: readonly IDictionary<TSource, TTarget> *_cache;

        public: CachingConverterDecorator(IConverter<TSource, TTarget> &baseConverter, IDictionary<TSource, TTarget> &cache) { (_baseConverter, _cache) = (baseConverter, cache); }

        public: CachingConverterDecorator(IConverter<TSource, TTarget> &baseConverter) : this(baseConverter, Dictionary<TSource, TTarget>()) { }

        public: TTarget Convert(TSource source) { return _cache.GetOrAdd(source, _baseConverter.Convert); }
    };
}