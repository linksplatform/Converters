#pragma once

#ifndef Platform_Converters_Converter
#define Platform_Converters_Converter

namespace Platform
{
    namespace Converters
    {
        template<typename TSource, typename TTarget>
        class Converter
        {
        public:
            static TTarget Convert(TSource source)
            {
                return (TTarget)source;
            }
        };
    }
}

#endif // Platform_Converters_Converter