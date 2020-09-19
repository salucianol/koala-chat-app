using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Data.Specifications;
using KoalaChatApp.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace KoalaChatApp.Web.Hubs {
    public class KoalaChatHub : Hub {
        private readonly IMessageParser messageParser;
        private readonly IMediator mediator;
        private readonly IRepository<ChatUser> userRepository;
        public KoalaChatHub(IMessageParser messageParser, IMediator mediator, IRepository<ChatUser> userRepository) {
            this.messageParser = messageParser;
            this.mediator = mediator;
            this.userRepository = userRepository;
        }
        public async Task SendMessage(string chatRoomId, string message) {
            ChatUser chatUser = this.userRepository.Get(new UserSpecification(Context.User.Identity.Name)).FirstOrDefault();
            ChatMessage chatMessage = this.messageParser.ParseMessage(chatUser.Id, message);
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
