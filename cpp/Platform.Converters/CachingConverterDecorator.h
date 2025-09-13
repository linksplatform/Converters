#pragma once

#include <unordered_map>
#include <functional>
#include "IConverter[TSource, TTarget].h"

namespace Platform::Converters
{
    /// <summary>
    /// <para>
    /// Represents the caching converter decorator.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="IConverter{TSource, TTarget}"/>
    template <typename ...> class CachingConverterDecorator;
    template <typename TSource, typename TTarget> 
    class CachingConverterDecorator<TSource, TTarget> : public IConverter<TSource, TTarget>
    {
        private: 
            const IConverter<TSource, TTarget>* _baseConverter;
            mutable std::unordered_map<TSource, TTarget> _cache;

        public: 
            /// <summary>
            /// <para>
            /// Initializes a new <see cref="CachingConverterDecorator"/> instance.
            /// </para>
            /// <para></para>
            /// </summary>
            /// <param name="baseConverter">
            /// <para>A base converter.</para>
            /// <para></para>
            /// </param>
            /// <param name="cache">
            /// <para>A cache.</para>
            /// <para></para>
            /// </param>
            CachingConverterDecorator(const IConverter<TSource, TTarget>* baseConverter, const std::unordered_map<TSource, TTarget>& cache) 
                : _baseConverter(baseConverter), _cache(cache) 
            {
            }

            /// <summary>
            /// <para>
            /// Initializes a new <see cref="CachingConverterDecorator"/> instance.
            /// </para>
            /// <para></para>
            /// </summary>
            /// <param name="baseConverter">
            /// <para>A base converter.</para>
            /// <para></para>
            /// </param>
            CachingConverterDecorator(const IConverter<TSource, TTarget>* baseConverter) 
                : _baseConverter(baseConverter), _cache() 
            {
            }

            /// <summary>
            /// <para>
            /// Converts the source.
            /// </para>
            /// <para></para>
            /// </summary>
            /// <param name="source">
            /// <para>The source.</para>
            /// <para></para>
            /// </param>
            /// <returns>
            /// <para>The target</para>
            /// <para></para>
            /// </returns>
            TTarget Convert(TSource source) override 
            {
                auto it = _cache.find(source);
                if (it != _cache.end()) 
                {
                    return it->second;
                }
                
                TTarget result = _baseConverter->Convert(source);
                _cache[source] = result;
                return result;
            }
    };
}