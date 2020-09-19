using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace KoalaChatApp.Infrastructure.Data {
    public class DatabaseInitialization {
        public static async void Initialize(UserManager<ChatUser> userManager, ILogger<DatabaseInitialization> logger, IChatRoomService chatRoomService) {
            try {
                if (!chatRoomService.Exists("Default Chat Room")) {
                    chatRoomService.AddChatRoom(new ApplicationCore.Entities.ChatRoom() {
                        Name = "Default Chat Room",
                        MaxCharactersCount = 150,
                        MaxUsersAllowed = 10,
                        MaxMessagesCount = 100
                    });
                }
                var user = await userManager.FindByEmailAsync("bot@koalacreativesoftware.com");
                if (user?.Id != Guid.Empty) {
                    return;
                }
                await userManager.CreateAsync(new ChatUser {
                    Email = "bot@koalacreativesoftware.com",
                    UserName = "bot@koalaappchat",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                });
            } catch (Exception ex) {
                logger.LogError(ex, "Error trying to create the default bot user.");
            }
        }
    }
}
