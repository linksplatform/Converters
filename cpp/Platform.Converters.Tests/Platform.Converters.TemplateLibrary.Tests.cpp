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
	using namespace std;

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

			Assert::AreEqual(string("1"), Convert<int, string>(1));
			Assert::AreEqual(string("1.49"), To<string>(1.49));
			Assert::AreEqual(string("A"), To<string>(A()));
			Assert::AreEqual(string("B"), To<string>(B()));
			Assert::AreEqual(string("C"), To<string>(C()));
			Assert::AreEqual(string("D"), To<string>(D()));
			Assert::AreEqual(string(""), To<string>(string("")));
			Assert::AreEqual(string("instance of class X"), To<string>(x));

			auto pointerToAString = To<string>(aPointer); // pointer <6826744964> to <A>
			Assert::IsTrue(pointerToAString.starts_with("pointer <"));
			Assert::IsTrue(pointerToAString.ends_with("> to <A>"));

			auto pointerToXString = To<string>(xPointer); // pointer <6826744964> to <instanse of class X>
			Assert::IsTrue(pointerToXString.starts_with("pointer <"));
			Assert::IsTrue(pointerToXString.ends_with("> to <instance of class X>"));

			Assert::AreEqual(string("null pointer"), Convert<X *, string>(nullptr));

			Assert::AreEqual(string("null pointer"), To<string>(nullptr));

			Assert::AreEqual(string("A"), Convert<A &, string>(a));

			Assert::AreEqual(string("A"), To<string>(aReference));

			Assert::AreEqual(string("void pointer <10>"), To<string>((void *)10));
		}
	};
}
