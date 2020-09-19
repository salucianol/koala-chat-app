using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KoalaChatApp.Web.Controllers {
    [Authorize]
    public class ChatRoomController : Controller {
        private readonly IChatRoomService chatRoomService;
        public ChatRoomController(IChatRoomService chatRoomService) {
            this.chatRoomService = chatRoomService;
        }
        public IActionResult Index(Guid id) {
            ChatRoom chatRoom = this.chatRoomService.GetChatRoom(id);
            return View(chatRoom);
        }
    }
}
