namespace Platform::Converters
{
    template <typename ...> class IConverter;
    template <typename T> class IConverter<T> : public IConverter<T, T>
    {
    public:
    };
}
