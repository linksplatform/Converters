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
			Assert::AreEqual(std::string("1"), Convert<std::string>(1));
			Assert::AreEqual(std::string("1.49"), Convert<std::string>(1.49));
			Assert::AreEqual(std::string("A"), Convert<std::string>(A()));
			Assert::AreEqual(std::string("B"), Convert<std::string>(B()));
			Assert::AreEqual(std::string("C"), Convert<std::string>(C()));
			Assert::AreEqual(std::string("D"), Convert<std::string>(D()));
			Assert::AreEqual(std::string(""), Convert<std::string>(std::string("")));
			Assert::AreEqual(std::string(""), Convert<std::string>(""));
		}
	};
}
