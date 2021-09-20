namespace Platform::Converters
{
    template <typename ...> class CheckedConverter;
    template <typename TSource, typename TTarget> class CheckedConverter<TSource, TTarget> : public ConverterBase<TSource, TTarget>
    {
        public: static CheckedConverter<TSource, TTarget> Default
        {
            get;
        } = CompileCheckedConverter();

        private: static CheckedConverter<TSource, TTarget> CompileCheckedConverter()
        {
            auto type = CreateTypeInheritedFrom<CheckedConverter<TSource, TTarget>>();
            EmitConvertMethod(type, il => il.CheckedConvert<TSource, TTarget>());
            return (CheckedConverter<TSource, TTarget>)Activator.CreateInstance(type.CreateTypeInfo());
        }
    };
}
