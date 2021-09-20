namespace Platform::Converters
{
    template <typename ...> class UncheckedConverter;
    template <typename TSource, typename TTarget> class UncheckedConverter<TSource, TTarget> : public ConverterBase<TSource, TTarget>
    {
        public: static UncheckedConverter<TSource, TTarget> Default
        {
            get;
        } = CompileUncheckedConverter();

        private: static UncheckedConverter<TSource, TTarget> CompileUncheckedConverter()
        {
            auto type = CreateTypeInheritedFrom<UncheckedConverter<TSource, TTarget>>();
            EmitConvertMethod(type, il => il.UncheckedConvert<TSource, TTarget>());
            return (UncheckedConverter<TSource, TTarget>)Activator.CreateInstance(type.CreateTypeInfo());
        }
    };
}
