#pragma once

#ifndef Platform_Converters_Convert
#define Platform_Converters_Convert

#include "Converter.h"

namespace Platform
{
    namespace Converters
    {
        template<typename TTarget, typename TSource> TTarget Convert(TSource source)
        {
            return Converter<TTarget, TSource>::Convert(source);
        }
    }
}

#endif // Platform_Converters_Convert