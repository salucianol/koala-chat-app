using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace KoalaChatApp.Infrastructure.Services {
    public class MessageParser : IMessageParser {
        private readonly ICommandsHelper commandsHelper;
        public MessageParser(ICommandsHelper commandsHelper, IChatRoomService chatRoomService) {
            this.commandsHelper = commandsHelper;
        }
        public ChatMessage ParseMessage(Guid userId, string message) {
            if (message.StartsWith("/")) {
                Regex regex = new Regex(@"^\/(.*?)=(.*)$");
                MatchCollection matches = regex.Matches(message);
                if (matches.Count != 2) {
                    // TODO: Add some code to get displayed an error message since the command was wrong.
                    return null;
                }
                if (!this.commandsHelper.IsCommandValid(matches[0].Value)) {
                    return null;
                }
                return new ChatMessageCommand(userId, matches[1].Value, ApplicationCore.Enums.ChatMessageType.COMMAND);
            }
            return new ChatMessageText(userId, message);
        }
    }
}
