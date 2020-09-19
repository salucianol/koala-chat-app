using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoalaChatApp.Web.Hubs {
    public class KoalaChatHub : Hub {
        private readonly IMessageParser messageParser;
        private readonly IMediator mediator;
        private readonly IRequest<bool> chatRequestModel;
        public KoalaChatHub(IMessageParser messageParser, IMediator mediator, IRequest<bool> chatRequestModel) {
            this.messageParser = messageParser;
            this.mediator = mediator;
            this.chatRequestModel = chatRequestModel;
        }
        public async Task SendMessage(string chatRoomId, string message) {
            ChatMessage chatMessage = this.messageParser.ParseMessage(Guid.NewGuid(), message);
            await this.mediator.Send<bool>(new ChatMessageRequestModel {
                ChatMessage = chatMessage,
                ChatRoomId = chatRoomId
            });
            if (chatMessage.MessageType == ApplicationCore.Enums.ChatMessageType.TEXT) {
                await Clients.All.SendAsync(chatRoomId, Context.User.Identity.Name, message);
            }
        }
    }
}
