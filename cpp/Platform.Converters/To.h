#pragma once

#ifndef Platform_Converters_To
#define Platform_Converters_To

#include "Converter.h"

namespace Platform
{
    namespace Converters
    {
        template<typename TTarget, typename TSource> TTarget To(TSource &&source)
        {
            return Converter<TSource, TTarget>::Convert(source);
        }
    }
}

#endif // Platform_Converters_To