using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using System;
using System.Text.RegularExpressions;
using KoalaChatApp.Infrastructure.Interfaces;
using KoalaChatApp.Infrastructure.Exceptions;
using KoalaChatApp.ApplicationCore.Enums;

namespace KoalaChatApp.Infrastructure.Services {
    public class MessageParser : IMessageParser {
        private readonly ICommandsHelper _commandsHelper;
        
        public MessageParser(ICommandsHelper commandsHelper) {
            _commandsHelper = commandsHelper;
        }

        public ChatMessage ParseMessage(Guid userId, string message) {
            if (message.StartsWith("/")) {
                Regex regex = new Regex(@"^\/(.*?)=(.*)$");
                Match match = regex.Match(message);
                if (match.Groups.Count != 3) {
                    throw new CommandFormatException(message);
                }
                if (!_commandsHelper.IsCommandValid(match.Groups[1].Value)) {
                    throw new CommandNotFoundException(match.Groups[1].Value);
                }
                return new ChatMessageCommand(userId, match.Groups[2].Value, ChatMessageType.COMMAND) {
                    SentDate = DateTimeOffset.Now
                };
            }
            return new ChatMessageText(userId, message) {
                SentDate = DateTimeOffset.Now
            };
        }
    }
}
