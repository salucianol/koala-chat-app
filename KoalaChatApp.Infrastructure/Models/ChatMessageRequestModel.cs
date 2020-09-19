using KoalaChatApp.ApplicationCore.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Infrastructure.Models {
    public class ChatMessageRequestModel : IRequest<bool> {
        public ChatMessage ChatMessage { get; set; }
        public string ChatRoomId { get; set; }
    }
}
