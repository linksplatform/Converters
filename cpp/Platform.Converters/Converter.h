#pragma once

namespace Platform::Converters
{
    template<typename TSource, typename TTarget>
    struct Converter
    {
        static TTarget Convert(const TSource& source)
        {
            return (TTarget)source;
        }
    };
}
