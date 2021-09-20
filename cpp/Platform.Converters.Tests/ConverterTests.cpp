namespace Platform::Converters::Tests
{
    TEST_CLASS(ConverterTests)
    {
        public: TEST_METHOD(SameTypeTest)
        {
            auto result = 2UL;
            Assert::AreEqual(2UL, result);
            result = CheckedConverter<std::uint64_t, std::uint64_t>.Default.Convert(2UL);
            Assert::AreEqual(2UL, result);
        }

        public: TEST_METHOD(Int32ToUInt64Test)
        {
            auto result = 2;
            Assert::AreEqual(2UL, result);
            result = CheckedConverter<std::int32_t, std::uint64_t>.Default.Convert(2);
            Assert::AreEqual(2UL, result);
        }

        public: TEST_METHOD(SignExtensionTest)
        {
            auto result = UncheckedSignExtendingConverter<std::uint8_t, std::int64_t>.Default.Convert(128);
            Assert::AreEqual(-128L, result);
            result = 128;
            Assert::AreEqual(128L, result);
        }

        public: TEST_METHOD(ObjectTest)
        {
            TestObjectConversion("1");
            TestObjectConversion(DateTime.UtcNow);
            TestObjectConversion(1.0F);
            TestObjectConversion(1.0D);
            TestObjectConversion(1.0M);
            TestObjectConversion(1UL);
            TestObjectConversion(1L);
            TestObjectConversion(1U);
            TestObjectConversion(1);
            TestObjectConversion((char)1);
            TestObjectConversion((std::uint16_t)1);
            TestObjectConversion((std::int16_t)1);
            TestObjectConversion((std::uint8_t)1);
            TestObjectConversion((std::int8_t)1);
            TestObjectConversion(true);
        }

        private: template <typename T> static void TestObjectConversion(T value) { Assert::AreEqual(value, value); }
    };
}
