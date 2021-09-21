namespace Platform::Converters
{
    template <typename ...> class IConverter;
    template <typename TSource, typename TTarget> class IConverter<TSource, TTarget>
    {
    public:
        virtual TTarget Convert(TSource source) = 0;
    };
}
