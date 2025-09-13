#include <Platform.Converters.h>
#include <gtest/gtest.h>

class A
{
public:
	explicit operator std::string() const
	{
	    return "A";
	}
};

class B
{
	friend std::ostream& operator<<(std::ostream& out, const B& obj)
	{
	    return out << "B";
	}
};

struct X {};

namespace Platform::Converters::Tests
{
	TEST(ConvertersTests, All)
	{
	    A a;
	    A &aReference = a;
	    A *aPointer = &a;
	    X x;
	    X *xPointer = &x;

	    ASSERT_EQ(std::string("1"), (Convert<int, std::string>(1)));
        // TODO: really, you have a magic compiler?
	    // ASSERT_EQ(std::string("1.49"), To<std::string>(1.49));
	    ASSERT_EQ(std::string("A"), To<std::string>(A()));
	    ASSERT_EQ(std::string("B"), To<std::string>(B()));
	    ASSERT_EQ(std::string(""), To<std::string>(std::string("")));
	    // TODO: really, you have a magic way of storing types without compression?
	    // ASSERT_EQ(std::string("instance of class X"), To<std::string>(x));

	    auto pointerToAString = To<std::string>(aPointer); // pointer <6826744964> to <A>
	    ASSERT_TRUE(pointerToAString.starts_with("pointer <"));
	    ASSERT_TRUE(pointerToAString.ends_with("> to <A>"));

	    auto pointerToXString = To<std::string>(xPointer); // pointer <6826744964> to <instanse of class X>
	    ASSERT_TRUE(pointerToXString.starts_with("pointer <"));
        // TODO: really, you have a magic way of storing types without compression?
	    // ASSERT_TRUE(pointerToXString.ends_with("> to <instance of class X>"));

	    ASSERT_EQ(std::string("null pointer"), (Convert<X*, std::string>(nullptr)));
	    ASSERT_EQ(std::string("null pointer"), To<std::string>(nullptr));

	    ASSERT_EQ(std::string("A"), (Convert<A&, std::string>(a)));

	    ASSERT_EQ(std::string("A"), To<std::string>(aReference));
	    ASSERT_EQ(std::string("void pointer <0xa>"), To<std::string>((void *)10));
	};

	TEST(StringConverterTests, U8StringToStringConversion)
	{
	    std::u8string u8str = u8"Hello World";
	    std::string result = To<std::string>(u8str);
	    ASSERT_EQ(std::string("Hello World"), result);
	    
	    // Test UTF-8 characters
	    std::u8string u8utf = u8"Привет мир";
	    std::string resultUtf = To<std::string>(u8utf);
	    ASSERT_EQ(std::string("Привет мир"), resultUtf);
	};

	TEST(StringConverterTests, StringToU8StringConversion)
	{
	    std::string str = "Hello World";
	    std::u8string result = To<std::u8string>(str);
	    ASSERT_EQ(u8"Hello World", result);
	    
	    // Test UTF-8 characters
	    std::string strUtf = "Привет мир";
	    std::u8string resultUtf = To<std::u8string>(strUtf);
	    ASSERT_EQ(u8"Привет мир", resultUtf);
	};

	TEST(StringConverterTests, U16StringToU8StringConversion)
	{
	    std::u16string u16str = u"Hello World";
	    std::u8string result = To<std::u8string>(u16str);
	    ASSERT_EQ(u8"Hello World", result);
	    
	    // Test Unicode characters
	    std::u16string u16unicode = u"Héllo Wörld";
	    std::u8string resultUnicode = To<std::u8string>(u16unicode);
	    ASSERT_EQ(u8"Héllo Wörld", resultUnicode);
	};

	TEST(StringConverterTests, U32StringToU8StringConversion)
	{
	    std::u32string u32str = U"Hello World";
	    std::u8string result = To<std::u8string>(u32str);
	    ASSERT_EQ(u8"Hello World", result);
	    
	    // Test Unicode characters
	    std::u32string u32unicode = U"Héllo Wörld";
	    std::u8string resultUnicode = To<std::u8string>(u32unicode);
	    ASSERT_EQ(u8"Héllo Wörld", resultUnicode);
	    
	    // Test emoji (4-byte UTF-8)
	    std::u32string u32emoji = U"Hello 🌍";
	    std::u8string resultEmoji = To<std::u8string>(u32emoji);
	    ASSERT_EQ(u8"Hello 🌍", resultEmoji);
	};

	TEST(StringConverterTests, U16StringToStringConversion)
	{
	    std::u16string u16str = u"Hello World";
	    std::string result = To<std::string>(u16str);
	    ASSERT_EQ(std::string("Hello World"), result);
	    
	    // Test Unicode characters
	    std::u16string u16unicode = u"Héllo Wörld";
	    std::string resultUnicode = To<std::string>(u16unicode);
	    ASSERT_EQ(std::string("Héllo Wörld"), resultUnicode);
	};

	TEST(StringConverterTests, U32StringToStringConversion)
	{
	    std::u32string u32str = U"Hello World";
	    std::string result = To<std::string>(u32str);
	    ASSERT_EQ(std::string("Hello World"), result);
	    
	    // Test Unicode characters
	    std::u32string u32unicode = U"Héllo Wörld";
	    std::string resultUnicode = To<std::string>(u32unicode);
	    ASSERT_EQ(std::string("Héllo Wörld"), resultUnicode);
	    
	    // Test emoji (4-byte UTF-8)
	    std::u32string u32emoji = U"Hello 🌍";
	    std::string resultEmoji = To<std::string>(u32emoji);
	    ASSERT_EQ(std::string("Hello 🌍"), resultEmoji);
	};
}
