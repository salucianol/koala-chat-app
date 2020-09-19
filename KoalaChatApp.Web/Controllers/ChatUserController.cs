using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoalaChatApp.Web.Controllers {
    [Authorize]
    public class ChatUserController : Controller {
        private readonly IChatRoomService _chatRoomService;

        public ChatUserController(IChatRoomService chatRoomService) {
            _chatRoomService = chatRoomService;
        }
        
        public IActionResult Index() {
            IEnumerable<ChatRoom> chatRooms = _chatRoomService.GetChatRooms();
            return View(chatRooms);
        }
    }
}
