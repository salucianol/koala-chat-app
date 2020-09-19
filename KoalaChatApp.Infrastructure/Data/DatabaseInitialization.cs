using KoalaChatApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace KoalaChatApp.Infrastructure.Data {
    public static class DatabaseInitialization {
        public static async void Initialize(UserManager<ChatUser> _userManager) {
            await _userManager.CreateAsync(new ChatUser {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Email = "bot@koalacreativesoftware.com",
                UserName = "bot@koalaappchat"
            });
        }
    }
}
