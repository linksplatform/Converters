#pragma once

#include "Converter.h"

namespace Platform::Converters
{
    template<typename TTarget, typename TSource> TTarget To(const TSource& source)
    {
        return Converter<TSource, TTarget>::Convert(source);
    }
}
