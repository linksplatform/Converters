#pragma once

#include <iostream>
#include <sstream>
#include "Converter.h"

namespace Platform::Converters
{
    template<typename TSource>
    std::string to_string(const TSource& source)
    {
        static constexpr auto to_hex_pointer = [](auto* self)
        {
            std::ostringstream stream;
            stream << self;
            return stream.str();
        };

        if constexpr (std::same_as<std::nullptr_t, TSource>)
        {
            return "null pointer";
        }

        if constexpr (std::is_pointer_v<TSource>)
        {
            if (source == nullptr)
            {
                return "null pointer";
            }
            else
            {
                if constexpr (std::same_as<TSource, void*>)
                {
                    return std::string("void pointer <")
                        .append(to_hex_pointer(source))
                        .append(1, '>');
                }
                else
                {
                    return std::string("pointer <")
                        .append(to_hex_pointer(source))
                        .append("> to <")
                        .append(to_string(*source))
                        .append(1, '>');
                }
            }
        }

        if constexpr (requires { static_cast<std::string>(source); })
        {
            return static_cast<std::string>(source);
        }

        if constexpr (requires { std::to_string(source); })
        {
            return std::to_string(source);
        }

        if constexpr (requires(std::ostream& stream) { stream << source; })
        {
            std::ostringstream stream;
            stream << source;
            return stream.str();
        }

        // TODO maybe use demangled name
        return std::string("instance of ")
            .append(typeid(TSource).name());
    }


    template<class TSource>
    struct Converter<TSource, std::string>
    {
        static std::string Convert(const TSource& source)
        {
            return to_string(std::forward<decltype(source)>(source));
        }
    };

    // Specialized converters for UTF string types
    template<>
    struct Converter<std::u8string, std::string>
    {
        static std::string Convert(const std::u8string& source)
        {
            return std::string(reinterpret_cast<const char*>(source.c_str()), source.size());
        }
    };

    template<>
    struct Converter<std::string, std::u8string>
    {
        static std::u8string Convert(const std::string& source)
        {
            return std::u8string(reinterpret_cast<const char8_t*>(source.c_str()), source.size());
        }
    };

    template<>
    struct Converter<std::u16string, std::u8string>
    {
        static std::u8string Convert(const std::u16string& source)
        {
            std::u8string result;
            for (char16_t ch : source)
            {
                if (ch < 0x80)
                {
                    result.push_back(static_cast<char8_t>(ch));
                }
                else if (ch < 0x800)
                {
                    result.push_back(static_cast<char8_t>(0xC0 | (ch >> 6)));
                    result.push_back(static_cast<char8_t>(0x80 | (ch & 0x3F)));
                }
                else
                {
                    result.push_back(static_cast<char8_t>(0xE0 | (ch >> 12)));
                    result.push_back(static_cast<char8_t>(0x80 | ((ch >> 6) & 0x3F)));
                    result.push_back(static_cast<char8_t>(0x80 | (ch & 0x3F)));
                }
            }
            return result;
        }
    };

    template<>
    struct Converter<std::u32string, std::u8string>
    {
        static std::u8string Convert(const std::u32string& source)
        {
            std::u8string result;
            for (char32_t ch : source)
            {
                if (ch < 0x80)
                {
                    result.push_back(static_cast<char8_t>(ch));
                }
                else if (ch < 0x800)
                {
                    result.push_back(static_cast<char8_t>(0xC0 | (ch >> 6)));
                    result.push_back(static_cast<char8_t>(0x80 | (ch & 0x3F)));
                }
                else if (ch < 0x10000)
                {
                    result.push_back(static_cast<char8_t>(0xE0 | (ch >> 12)));
                    result.push_back(static_cast<char8_t>(0x80 | ((ch >> 6) & 0x3F)));
                    result.push_back(static_cast<char8_t>(0x80 | (ch & 0x3F)));
                }
                else if (ch < 0x110000)
                {
                    result.push_back(static_cast<char8_t>(0xF0 | (ch >> 18)));
                    result.push_back(static_cast<char8_t>(0x80 | ((ch >> 12) & 0x3F)));
                    result.push_back(static_cast<char8_t>(0x80 | ((ch >> 6) & 0x3F)));
                    result.push_back(static_cast<char8_t>(0x80 | (ch & 0x3F)));
                }
            }
            return result;
        }
    };

    // Chain conversions: u16string -> u8string -> string
    template<>
    struct Converter<std::u16string, std::string>
    {
        static std::string Convert(const std::u16string& source)
        {
            auto u8str = Converter<std::u16string, std::u8string>::Convert(source);
            return Converter<std::u8string, std::string>::Convert(u8str);
        }
    };

    // Chain conversions: u32string -> u8string -> string
    template<>
    struct Converter<std::u32string, std::string>
    {
        static std::string Convert(const std::u32string& source)
        {
            auto u8str = Converter<std::u32string, std::u8string>::Convert(source);
            return Converter<std::u8string, std::string>::Convert(u8str);
        }
    };
}
