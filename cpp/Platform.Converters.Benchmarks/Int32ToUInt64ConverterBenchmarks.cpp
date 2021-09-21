namespace Platform::Converters::Benchmarks
{
    class Int32ToUInt64ConverterBenchmarks
    {
        private: static UncheckedConverter<std::int32_t, std::uint64_t> _int32ToUInt64converter;
        private: static UncheckedConverter<void*, std::uint64_t> _objectToUInt64Converter;
        private: static IFormatProvider *_formatProvider;

        public: static void Setup()
        {
            _int32ToUInt64converter = UncheckedConverter<std::int32_t, std::uint64_t>.Default;
            _objectToUInt64Converter = UncheckedConverter<void*, std::uint64_t>.Default;
            _formatProvider = CultureInfo.InvariantCulture;
        }

        public: static std::uint64_t ConverterWrapperWithNoInlining(std::int32_t value) { return _int32ToUInt64converter.Convert(value); }

        public: static std::uint64_t ConverterWrapperWithAggressiveInlining(std::int32_t value) { return _int32ToUInt64converter.Convert(value); }

        public: static std::uint64_t ConverterWrapper(std::int32_t value) { return _int32ToUInt64converter.Convert(value); }

        public: std::uint64_t ConverterFromLocalStaticField() { return _int32ToUInt64converter.Convert(2); }

        public: std::uint64_t ConverterFromGlobalStaticField() { return 2; }

        public: std::uint64_t SystemConvertObjectToUInt64() { return Convert.ToUInt64((void*)2); }

        public: std::uint64_t ConvertObjectToUInt64() { return _objectToUInt64Converter.Convert(2); }

        public: std::uint64_t SystemConvertToUInt64() { return Convert.ToUInt64(2); }

        public: std::uint64_t SystemConvertChangeType() { return (std::uint64_t)Convert.ChangeType(2, this->typeof(std::uint64_t)); }

        public: std::uint64_t IConvertibleToUInt64() { return ((IConvertible)2).ToUInt64(_formatProvider); }

        public: std::uint64_t IConvertibleToType() { return (std::uint64_t)((IConvertible)2).ToType(this->typeof(std::uint64_t), _formatProvider); }

        public: std::uint64_t StaticFunctionWithoutInlining() { return this->ConverterWrapperWithNoInlining(2); }

        public: std::uint64_t StaticFunctionWithAggressiveInlining() { return this->ConverterWrapperWithAggressiveInlining(2); }

        public: std::uint64_t StaticFunction() { return this->ConverterWrapper(2); }
    };
}
