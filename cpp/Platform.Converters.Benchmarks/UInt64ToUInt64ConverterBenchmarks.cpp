namespace Platform::Converters::Benchmarks
{
    class UInt64ToUInt64ConverterBenchmarks
    {
        private: static UncheckedConverter<std::uint64_t, std::uint64_t> _uInt64ToUInt64Converter;
        private: static IFormatProvider *_formatProvider;

        public: static void Setup()
        {
            _uInt64ToUInt64Converter = UncheckedConverter<std::uint64_t, std::uint64_t>.Default;
            _formatProvider = CultureInfo.InvariantCulture;
        }

        public: static std::uint64_t ConverterWrapperWithNoInlining(std::uint64_t value) { return _uInt64ToUInt64Converter.Convert(value); }

        public: static std::uint64_t ConverterWrapperWithAggressiveInlining(std::uint64_t value) { return _uInt64ToUInt64Converter.Convert(value); }

        public: static std::uint64_t ConverterWrapper(std::uint64_t value) { return _uInt64ToUInt64Converter.Convert(value); }

        public: std::uint64_t ConverterFromLocalStaticField() { return _uInt64ToUInt64Converter.Convert(2UL); }

        public: std::uint64_t ConverterFromGlobalStaticField() { return 2UL; }

        public: std::uint64_t SystemConvertToUInt64() { return Convert.ToUInt64(2UL); }

        public: std::uint64_t SystemConvertObjectToUInt64() { return Convert.ToUInt64((void*)2UL); }

        public: std::uint64_t SystemConvertChangeType() { return (std::uint64_t)Convert.ChangeType(2UL, this->typeof(std::uint64_t)); }

        public: std::uint64_t IConvertibleToUInt64() { return ((IConvertible)2UL).ToUInt64(_formatProvider); }

        public: std::uint64_t IConvertibleToType() { return (std::uint64_t)((IConvertible)2UL).ToType(this->typeof(std::uint64_t), _formatProvider); }

        public: std::uint64_t StaticFunctionWithoutInlining() { return this->ConverterWrapperWithNoInlining(2UL); }

        public: std::uint64_t StaticFunctionWithAggressiveInlining() { return this->ConverterWrapperWithAggressiveInlining(2UL); }

        public: std::uint64_t StaticFunction() { return this->ConverterWrapper(2UL); }
    };
}
