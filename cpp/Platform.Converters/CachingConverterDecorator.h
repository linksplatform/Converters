#pragma once

#include <functional>
#include <memory>
#include <unordered_map>

namespace Platform::Converters
{
    template <typename TSource, typename TTarget, typename TCache = std::unordered_map<TSource, TTarget>> class Cached
    {
    private: std::function<TTarget(TSource)>& _cachedFunction;

    private: TCache& _cache;

    public: Cached(std::function<TTarget(TSource)>& cachedFunction, std::unordered_map<TSource, TTarget>& cache) : _cachedFunction(cachedFunction), _cache(cache) {};

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
