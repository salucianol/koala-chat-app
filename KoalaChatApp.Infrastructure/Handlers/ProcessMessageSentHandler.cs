using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Enums;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KoalaChatApp.Infrastructure.Handlers {
    public class ProcessMessageSentHandler : IRequestHandler<ChatMessageRequestModel, bool> {
        private readonly ILogger<ProcessMessageSentHandler> _logger;
        private readonly IMessageQueue _messageQueue;
        private readonly IChatRoomService _chatRoomService;

        public ProcessMessageSentHandler(ILogger<ProcessMessageSentHandler> logger, 
                                            IMessageQueue messageQueue, 
                                            IChatRoomService chatRoomService) {
            _logger = logger;
            _messageQueue = messageQueue;
            _chatRoomService = chatRoomService;
        }
        public Task<bool> Handle(ChatMessageRequestModel request, CancellationToken cancellationToken) {
            ChatMessage chatMessage = request.ChatMessage;
            if (!cancellationToken.IsCancellationRequested) {
                if (chatMessage.MessageType == ApplicationCore.Enums.ChatMessageType.TEXT) {
                    _chatRoomService
                        .AddChatMessage(Guid.Parse(request.ChatRoomId), 
                                            ((ChatMessageText)chatMessage));
                    
                    _logger.LogInformation($"Chat message of type " +
                        $"{Enum.GetName(typeof(ChatMessageType).GetType(), chatMessage.MessageType)} " +
                        $"added to room with Id {request.ChatRoomId}");

                } else if (chatMessage.MessageType == ApplicationCore.Enums.ChatMessageType.COMMAND) {
                    _messageQueue
                        .EnqueueMessage(new ApplicationCore.DTOs.QueueMessageDTO {
                            Command = ((ChatMessageCommand)chatMessage).Command,
                            RoomId = request.ChatRoomId
                        });
                    _logger.LogInformation($"Chat message of type " +
                        $"{Enum.GetName(typeof(ChatMessageType).GetType(), chatMessage.MessageType)} " +
                        $"enqueued.");
                }
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
