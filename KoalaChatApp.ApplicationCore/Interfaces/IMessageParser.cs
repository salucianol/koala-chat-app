using KoalaChatApp.ApplicationCore.Entities;
using System;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface IMessageParser {
        ChatMessage ParseMessage(Guid userId, string message);
    }
}
