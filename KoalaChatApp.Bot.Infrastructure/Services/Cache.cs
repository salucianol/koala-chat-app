using KoalaChatApp.Bot.ApplicationCore.DTOs;
using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace KoalaChatApp.Bot.Infrastructure.Services {
    public class Cache : ICache<Stock> {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<Cache> _logger;
        private readonly IConfiguration _configuration;
        private readonly BotConfigurations _botConfigurations = new BotConfigurations();

        public Cache(IMemoryCache memoryCache, 
                        ILogger<Cache> logger, 
                        IConfiguration configuration) {
            _memoryCache = memoryCache;
            _logger = logger;
            _configuration = configuration;
            _configuration.GetSection("BotConfigurations")
                                .Bind(_botConfigurations);
        }
        public CacheKey<Stock> GetKey(string key) {
            try {
                if (!_memoryCache.TryGetValue(key, out CacheKey<Stock> cacheKey)) {
                    return null;
                }
                return cacheKey;
            } catch (Exception ex) {
                _logger.LogError(ex, "An internal error ocurred, check logs for further details");
                throw;
            }
        }

        public void PutKey(CacheKey<Stock> cacheKey) {
            try {
                _memoryCache.Set<CacheKey<Stock>>(cacheKey.Key, 
                                                    cacheKey, 
                                                    new MemoryCacheEntryOptions {
                                                        SlidingExpiration = 
                                                            TimeSpan.FromSeconds(_botConfigurations.CacheKeyLifetime)
                                                   });
                _logger.LogInformation($"Put key {cacheKey.Key} successfully into cache.");
            } catch (Exception ex) {
                _logger.LogError(ex, "An internal error ocurred, check logs for further details");
                throw;
            }
        }
    }
}
