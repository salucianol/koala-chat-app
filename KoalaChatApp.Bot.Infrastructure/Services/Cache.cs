using KoalaChatApp.Bot.ApplicationCore.DTOs;
using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace KoalaChatApp.Bot.Infrastructure.Services {
    public class Cache : ICache<Stock> {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<Cache> logger;
        private readonly IConfiguration configuration;
        private BotConfigurations botConfigurations = new BotConfigurations();

        public Cache(IMemoryCache memoryCache, ILogger<Cache> logger, IConfiguration configuration) {
            this.memoryCache = memoryCache;
            this.logger = logger;
            this.configuration = configuration;
            this.configuration.GetSection("BotConfigurations").Bind(this.botConfigurations);
        }
        public CacheKey<Stock> GetKey(string key) {
            try {
                CacheKey<Stock> cacheKey;
                if (!this.memoryCache.TryGetValue(key, out cacheKey)) {
                    return null;
                }
                return cacheKey;
            } catch (Exception) {
                throw;
            }
        }

        public void PutKey(CacheKey<Stock> cacheKey) {
            try {
                this.memoryCache.Set<CacheKey<Stock>>(cacheKey.Key, cacheKey, new MemoryCacheEntryOptions {
                    SlidingExpiration = TimeSpan.FromSeconds(this.botConfigurations.CacheKeyLifetime)
                });
            } catch (Exception) {
                throw;
            }
        }
    }
}
