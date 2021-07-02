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
}
