#pragma once

#ifndef Platform_Converters_ConvertTo
#define Platform_Converters_ConvertTo

#include "Converter.h"

namespace Platform
{
    namespace Converters
    {
        template<typename TTarget, typename TSource> TTarget ConvertTo(TSource source)
        {
            return Converter<TSource, TTarget>::Convert(source);
        }
    }
}

#endif // Platform_Converters_ConvertTo