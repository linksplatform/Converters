#include <iomanip>
#include <string>

#include <functional>
#include <memory>
#include <unordered_map>


namespace Platform::Converters
{
    template<typename TCache, typename TConverter, typename TSource>
    auto CachedCall(TCache& cache, TConverter&& converter, TSource&& source) 
            -> std::add_lvalue_reference_t<std::decay_t<std::invoke_result_t<TConverter, TSource>>> {
        if (auto cursor = cache.find(source); cursor != cache.end())
        {
            return cursor->second;
        }
        else
        {
            auto target = std::forward<TConverter>(converter)(source);
            return cache.insert({ std::forward<TSource>(source), std::move(target) }).first->second;
        }
    }

    template <typename TSource, typename TTarget, typename TCache = std::unordered_map<TSource, TTarget>> 
    class Cached
    {
        std::function<TTarget(TSource)> _cachedFunction;
        TCache _cache;
    public:
        Cached(std::function<TTarget(TSource)> cachedFunction) 
            : _cachedFunction(std::move(cachedFunction)) {};

        Cached(std::function<TTarget(TSource)> cachedFunction, TCache cache) 
            : _cachedFunction(std::move(cachedFunction)), _cache(std::move(cache)) {};

        const TTarget& operator()(TSource&& source) &
        {
            return CachedCall(_cache, _cachedFunction, std::move(source));
        }

        const TTarget& operator()(const TSource& source) &
        {
            return CachedCall(_cache, _cachedFunction, source);
        }

       TTarget operator()(TSource&& source) &&
        {
            return _cachedFunction(std::move(source));
        }
        
        TTarget operator()(const TSource& source) &&
        {
            return _cachedFunction(source);
        }
    };
}
