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
        private readonly IMessageParser messageParser;
        private readonly IMediator mediator;
        private readonly IRepository<ChatUser> userRepository;
        private readonly ILogger<KoalaChatHub> logger;

        public KoalaChatHub(IMessageParser messageParser, IMediator mediator, IRepository<ChatUser> userRepository, ILogger<KoalaChatHub> logger) {
            this.messageParser = messageParser;
            this.mediator = mediator;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task SendMessage(string chatRoomId, string message) {
            string botChatUser = this.userRepository
                                        .Get(new UserSpecification("bot@koalaappchat"))
                                        .FirstOrDefault()?.UserName;
            try {
                ChatUser chatUser = this.userRepository
                                            .Get(new UserSpecification(Context.User.Identity.Name))
                                            .FirstOrDefault();
                ChatMessage chatMessage = this.messageParser.ParseMessage(chatUser.Id, message);
                await this.mediator
                        .Send<bool>(new ChatMessageRequestModel {
                            ChatMessage = chatMessage,
                            ChatRoomId = chatRoomId
                        });
                if (chatMessage.MessageType == ApplicationCore.Enums.ChatMessageType.TEXT) {
                    await Clients.All.SendAsync(chatRoomId, 
                                                Context.User.Identity.Name, 
                                                chatMessage.SentDate.ToString("yyyy-MM-dd HH:mm"), 
                                                message);
                }
            } catch (CommandFormatException ex) {
                await Clients.Client(Context.ConnectionId).SendAsync(chatRoomId,
                                                                botChatUser,
                                                                DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm"),
                                                                $"Command sent ({message}) invalid.");
                logger.LogError(ex, 
                                $"User {Context.User.Identity.Name} sent an invalid command to Chat Room {chatRoomId}. Message sent: {message}.");
            } catch (CommandNotFoundException ex) {
                await Clients.Client(Context.ConnectionId).SendAsync(chatRoomId,
                                                                botChatUser,
                                                                DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm"),
                                                                $"Command sent ({message}) not allowed.");
                logger.LogError(ex, 
                                $"User {Context.User.Identity.Name} sent not allowed command to Chat Room {chatRoomId}. Message sent: {message}.");
            } catch (Exception ex) {
                logger.LogError(ex, 
                                "An internal error occured while processing message sent.");
            }
        }
    }
}
