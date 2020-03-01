#pragma once

#ifndef Platform_Converters_StringConverter
#define Platform_Converters_StringConverter

#include <iostream>
#include <sstream>
#include "Converter.h"
#include "is_detected.h"

namespace Platform
{
    namespace Converters
    {
        // 1 - detecting if T can be sent to an ostringstream

        template<typename T>
        using ostringstream_expression = decltype(std::declval<std::ostringstream&>() << std::declval<T>());

        template<typename T>
        constexpr bool has_ostringstream = is_detected<ostringstream_expression, T>::value;

        // 2 - detecting if to_string is valid on T

        template<typename T>
        using to_string_expression = decltype(to_string(std::declval<T>()));

        template<typename T>
        constexpr bool has_to_string = is_detected<to_string_expression, T>::value;

        template<class TSource>
        class Converter<std::string, TSource>
        {
            public: static std::string Convert(TSource source)
            {
                if constexpr (std::is_convertible<TSource, std::string>::value)
                {
                    return (std::string)source;
                }
                else if constexpr (has_ostringstream<TSource>)
                {
                    std::ostringstream oss;
                    oss << source;
                    return oss.str();
                }
                else if constexpr (has_to_string<TSource>)
                {
                    return to_string(source);
                }
                else
                {
                    return std::to_string(source);
                }
            }
        };
    }
}

#endif // Platform_Converters_StringConverter