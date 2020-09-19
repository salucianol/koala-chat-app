using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.Infrastructure.Interfaces;
using KoalaChatApp.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace KoalaChatApp.Web.Services {
    public class ChatHubService : IChatHubService {
        private readonly IHubContext<KoalaChatHub> _koalaChatHubContext;
        public ChatHubService(IHubContext<KoalaChatHub> koalaChatHubContext) {
            this._koalaChatHubContext = koalaChatHubContext;
        }
        public void SendMessage(string chatRoomId, ChatMessageTextDTO message) {
            this._koalaChatHubContext
                    .Clients
                    .All
                    .SendAsync(chatRoomId, 
                                message.User, 
                                message.Date, 
                                message.Text);
        }
    }
}
