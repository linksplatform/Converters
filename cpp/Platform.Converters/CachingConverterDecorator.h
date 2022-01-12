#pragma once

#include <functional>
#include <memory>
#include <unordered_map>

namespace Platform::Converters
{
    template <typename TSource, typename TTarget, typename TCache = std::unordered_map<TSource, TTarget>> class Cached
    {
    private: std::function<TTarget(TSource) > _baseConverter;

    private: std::unordered_map<TSource, TTarget> _cache;

    public: Cached(std::function<TTarget(TSource)>& baseConverter, std::unordered_map<TSource, TTarget>& cache) : _baseConverter(baseConverter), _cache(cache) {};

    public: TTarget operator()(TSource&& source)
    {
        if (auto cursor = _cache.find(source); cursor != _cache.end())
        {
            return cursor->second;
        }
        else
        {
            return *_cache.insert({ source, _baseConverter(source) });
        }
    }

    public: TTarget operator()(const TSource& source)
    {
        if (auto cursor = _cache.find(source); cursor != _cache.end())
        {
            return cursor->second;
        }
        else
        {
            return *_cache.insert({ source, _baseConverter(source) });
        }
    }
    };
}
