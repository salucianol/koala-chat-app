using KoalaChatApp.Bot.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.ApplicationCore.Interfaces {
    public interface ICache<T> {
        void PutKey(CacheKey<T> cacheKey);
        CacheKey<T> GetKey(string key);
    }
}
