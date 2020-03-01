#pragma once

#ifndef Platform_Converters_Convert
#define Platform_Converters_Convert

#include "Converter.h"

namespace Platform
{
    namespace Converters
    {
        template<typename TSource, typename TTarget> TTarget Convert(TSource source)
        {
            return Converter<TSource, TTarget>::Convert(source);
        }
    }
}

#endif // Platform_Converters_Convert