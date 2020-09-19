﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Web.Models;
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
            List<ChatMessageModelResponse> chatMessages = this.chatRoomService.GetChatRoomMessages(id).Select(cm => new ChatMessageModelResponse {
                Text = cm.Text,
                Date = cm.Date,
                User = cm.User,
                RoomName = cm.RoomName,
                RoomId = cm.RoomId
            }).ToList();
            return View(chatMessages);
        }
    }
}
