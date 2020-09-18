using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.ApplicationCore.DTOs {
    public class CacheKey<T> {
        public string Key { get; set; }
        public T Message { get; set; }
    }
}
