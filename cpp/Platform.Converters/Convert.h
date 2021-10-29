#pragma once

#include "Converter.h"

namespace Platform::Converters
{
    template <typename TSource, typename TTarget> TTarget Convert(const TSource& source)
    {
        return Converter<std::remove_cvref_t<decltype(source)>, TTarget>::Convert(source);
    }
}