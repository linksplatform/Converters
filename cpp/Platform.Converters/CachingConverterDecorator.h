#pragma once
#include <functional>
#include <memory>
#include <unordered_map>

namespace Platform::Converters
{
    template <typename ...> class Cached;
    template <typename TSource, typename TTarget> class Cached<TSource, TTarget>
    {
    private: std::unique_ptr<std::function<TTarget(TSource)>> _baseConverter;

    private: std::unordered_map<TSource, TTarget> _cache;

    public: Cached(std::function<TTarget(TSource)>& baseConverter, std::unordered_map<TSource, TTarget>& cache) : _baseConverter(baseConverter), _cache(cache) {};

    public: Cached(std::function<TTarget(TSource)>& baseConverter) : _baseConverter(baseConverter), _cache() {};

    public: TTarget operator()(const TSource& source) {
        if (auto cursor = _cache.find(source); cursor != _cache.end())
        {
            return *cursor;
        }
        else
        {
            return *_cache.insert({ source, _baseConverter(source) });
        }
    }
    };
}
