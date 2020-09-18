using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KoalaChatApp.Web.Controllers {
    [Authorize]
    public class ChatRoomController : Controller {
        public ChatRoomController() {
            
        }
        public IActionResult Index() {
            return View();
        }
    }
}
