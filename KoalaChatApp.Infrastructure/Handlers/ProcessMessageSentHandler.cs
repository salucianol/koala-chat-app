using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KoalaChatApp.Infrastructure.Handlers {
    public class ProcessMessageSentHandler : IRequestHandler<ChatMessageRequestModel, bool> {
        private readonly ILogger<ProcessMessageSentHandler> logger;
        private readonly IMessageQueue messageQueue;
        private readonly IChatRoomService chatRoomService;

        public ProcessMessageSentHandler(ILogger<ProcessMessageSentHandler> logger, IMessageQueue messageQueue, IChatRoomService chatRoomService) {
            this.logger = logger;
            this.messageQueue = messageQueue;
            this.chatRoomService = chatRoomService;
        }
        public Task<bool> Handle(ChatMessageRequestModel request, CancellationToken cancellationToken) {
            ChatMessage chatMessage = request.ChatMessage;
            if (!cancellationToken.IsCancellationRequested) {
                if (chatMessage.MessageType == ApplicationCore.Enums.ChatMessageType.TEXT) {
                    this.chatRoomService.AddChatMessage(Guid.Parse(request.ChatRoomId), ((ChatMessageText)chatMessage));
                } else if (chatMessage.MessageType == ApplicationCore.Enums.ChatMessageType.COMMAND) {
                    this.messageQueue.EnqueueMessage(new ApplicationCore.DTOs.QueueMessageDTO {
                        Command = ((ChatMessageCommand)chatMessage).Command,
                        RoomId = request.ChatRoomId
                    });
                }
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
