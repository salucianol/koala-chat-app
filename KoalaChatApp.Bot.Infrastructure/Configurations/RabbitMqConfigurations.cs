﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.Infrastructure.Configurations {
    public class RabbitMqConfigurations {
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string MessageInboundQueue { get; set; }
        public string MessageOutboundQueue { get; set; }
        public int Port { get; set; }
    }
}
