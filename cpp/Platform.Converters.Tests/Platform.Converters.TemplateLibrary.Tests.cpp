#include "pch.h"
#include "CppUnitTest.h"

class A
{
public:
	operator std::string() { return "A"; }
};

class B
{
	friend std::ostream& operator << (std::ostream& out, B obj) { return out << "B"; }
};

class C
{
};

namespace std
{
	std::string to_string(C source)
	{
		return "C";
	}
}

class D
{
};

std::string to_string(D source)
{
	return "D";
}

class X
{

};

#include <Platform.Converters.h>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace Platform::Converters;

namespace PlatformConvertersTemplateLibraryTests
{
	TEST_CLASS(PlatformConvertersTemplateLibraryTests)
	{
	public:
		TEST_METHOD(ConversionsTest)
		{
			A a;
			A &aReference = a;
			A *aPointer = &a;
			X x;
			X *xPointer = &x;

			Assert::AreEqual(std::string("1"), Convert<int, std::string>(1));
			Assert::AreEqual(std::string("1.49"), ConvertTo<std::string>(1.49));
			Assert::AreEqual(std::string("A"), ConvertTo<std::string>(A()));
			Assert::AreEqual(std::string("B"), ConvertTo<std::string>(B()));
			Assert::AreEqual(std::string("C"), ConvertTo<std::string>(C()));
			Assert::AreEqual(std::string("D"), ConvertTo<std::string>(D()));
			Assert::AreEqual(std::string(""), ConvertTo<std::string>(std::string("")));
			Assert::AreEqual(std::string("instance of class X"), ConvertTo<std::string>(x));

			auto pointerToAString = ConvertTo<std::string>(aPointer); // pointer <6826744964> to <A>
			Assert::IsTrue(pointerToAString.starts_with("pointer <"));
			Assert::IsTrue(pointerToAString.ends_with("> to <A>"));

			auto pointerToXString = ConvertTo<std::string>(xPointer); // pointer <6826744964> to <instanse of class X>
			Assert::IsTrue(pointerToXString.starts_with("pointer <"));
			Assert::IsTrue(pointerToXString.ends_with("> to <instance of class X>"));

			Assert::AreEqual(std::string("null pointer"), Convert<X *, std::string>(nullptr));

			Assert::AreEqual(std::string("null pointer"), ConvertTo<std::string>(nullptr));

			Assert::AreEqual(std::string("A"), Convert<A &, std::string>(a));

			Assert::AreEqual(std::string("A"), ConvertTo<std::string>(aReference));

			Assert::AreEqual(std::string("void pointer <10>"), ConvertTo<std::string>((void *)10));
		}
	};
}
