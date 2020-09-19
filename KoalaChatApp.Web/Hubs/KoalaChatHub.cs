using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Data.Specifications;
using KoalaChatApp.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using KoalaChatApp.Infrastructure.Exceptions;
using System;

namespace KoalaChatApp.Web.Hubs {
    public class KoalaChatHub : Hub {
        private readonly IMessageParser _messageParser;
        private readonly IMediator _mediator;
        private readonly IRepository<ChatUser> _userRepository;
        private readonly ILogger<KoalaChatHub> _logger;

        public KoalaChatHub(IMessageParser messageParser, 
                                IMediator mediator, 
                                IRepository<ChatUser> userRepository, 
                                ILogger<KoalaChatHub> logger) {
            _messageParser = messageParser;
            _mediator = mediator;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task SendMessage(string chatRoomId, string message) {
            string botChatUser = _userRepository
                                        .Get(new UserSpecification("bot@koalaappchat"))
                                        .FirstOrDefault()?.UserName;
            try {
                ChatUser chatUser = _userRepository
                                            .Get(new UserSpecification(Context.User.Identity.Name))
                                            .FirstOrDefault();
                ChatMessage chatMessage = _messageParser.ParseMessage(chatUser.Id, message);
                await _mediator
                        .Send<bool>(new ChatMessageRequestModel {
                            ChatMessage = chatMessage,
                            ChatRoomId = chatRoomId
                        });
                if (chatMessage.MessageType == ApplicationCore.Enums.ChatMessageType.TEXT) {
                    await Clients
                            .All
                            .SendAsync(chatRoomId, 
                                        Context.User.Identity.Name, 
                                        chatMessage.SentDate.ToString("yyyy-MM-dd HH:mm"), 
                                        message);
                }
            } catch (CommandFormatException ex) {
                await Clients
                        .Client(Context.ConnectionId)
                        .SendAsync(chatRoomId,
                                    botChatUser,
                                    DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm"),
                                    $"Command sent ({message}) invalid.");
                _logger.LogError(ex, 
                                $"User {Context.User.Identity.Name} sent an invalid command " +
                                $"to Chat Room {chatRoomId}. Message sent: {message}.");
            } catch (CommandNotFoundException ex) {
                await Clients
                        .Client(Context.ConnectionId)
                        .SendAsync(chatRoomId,
                                    botChatUser,
                                    DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm"),
                                    $"Command sent ({message}) not allowed.");
                _logger.LogError(ex, 
                                $"User {Context.User.Identity.Name} sent not allowed command " +
                                $"to Chat Room {chatRoomId}. Message sent: {message}.");
            } catch (Exception ex) {
                _logger.LogError(ex, 
                                "An internal error occured while processing message sent.");
            }
        }
    }
}
