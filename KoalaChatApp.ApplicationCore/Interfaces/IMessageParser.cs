using KoalaChatApp.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface IMessageParser {
        ChatMessage ParseMessage(Guid userId, string message);
    }
}
