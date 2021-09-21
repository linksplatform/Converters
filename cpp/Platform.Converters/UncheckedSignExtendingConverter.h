namespace Platform::Converters
{
    template <typename ...> class UncheckedSignExtendingConverter;
    template <typename TSource, typename TTarget> class UncheckedSignExtendingConverter<TSource, TTarget> : public ConverterBase<TSource, TTarget>
    {
        public: static UncheckedSignExtendingConverter<TSource, TTarget> Default
        {
            get;
        } = CompileUncheckedConverter();

        private: static UncheckedSignExtendingConverter<TSource, TTarget> CompileUncheckedConverter()
        {
            auto type = CreateTypeInheritedFrom<UncheckedSignExtendingConverter<TSource, TTarget>>();
            EmitConvertMethod(type, il => il.UncheckedConvert<TSource, TTarget>(extendSign: true));
            return (UncheckedSignExtendingConverter<TSource, TTarget>)Activator.CreateInstance(type.CreateTypeInfo());
        }
    };
}
