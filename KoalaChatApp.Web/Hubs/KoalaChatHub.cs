using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoalaChatApp.Web.Hubs {
    public class KoalaChatHub : Hub {
        public async Task SendMessage(string message) {
            // Parse the message to determine whether is a COMMAND or a TEXT message
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }
    }
}
