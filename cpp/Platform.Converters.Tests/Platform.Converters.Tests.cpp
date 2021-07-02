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
}
