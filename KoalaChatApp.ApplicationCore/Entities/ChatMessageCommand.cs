using KoalaChatApp.ApplicationCore.Enums;
using System;

namespace KoalaChatApp.ApplicationCore.Entities {
    public class ChatMessageCommand : ChatMessage {
        public string Command { get; private set; }
        public ChatMessageCommand(Guid userId, 
                                    string command, 
                                    ChatMessageType messageType = ChatMessageType.COMMAND) : base(userId, messageType) {
            Command = command;
        }
    }
}
