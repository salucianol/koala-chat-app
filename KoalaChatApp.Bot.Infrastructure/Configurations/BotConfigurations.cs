using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.Infrastructure.Configurations {
    public class BotConfigurations {
        public short ServiceDelay { get; set; }
        public short CacheKeyLifetime { get; set; }
    }
}
