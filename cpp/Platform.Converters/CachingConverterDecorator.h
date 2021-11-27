#pragma once

#include <functional>
#include <memory>
#include <unordered_map>

namespace Platform::Converters
{
    template <typename ...> class Cached;
    template <typename Function> class Cached<Function>
    {
        private: Function _baseConverter;

        public: Cached(Function baseConverter): _baseConverter(baseConverter) {}

        public: auto operator()(auto&& source)
        {
            static std::unordered_map<std::decay_t<decltype(source)>> cached;
            auto cursor = cached.find(source);
            if (cursor != cached.end())
            {
                return *cursor;
            }
            else 
            {
                return *cached.insert({source, _baseConverter(source)})
            }
        }
    };
}