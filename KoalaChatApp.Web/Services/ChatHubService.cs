using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.Infrastructure.Interfaces;
using KoalaChatApp.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoalaChatApp.Web.Services {
    public class ChatHubService : IChatHubService {
        private readonly IHubContext<KoalaChatHub> koalaChatHubContext;
        public ChatHubService(IHubContext<KoalaChatHub> koalaChatHubContext) {
            this.koalaChatHubContext = koalaChatHubContext;
        }
        public void SendMessage(string chatRoomId, ChatMessageTextDTO message) {
            this.koalaChatHubContext.Clients.All.SendAsync(chatRoomId, message.User, message.Date, message.Text);
        }
    }
}
