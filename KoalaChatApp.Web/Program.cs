using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Data;
using KoalaChatApp.Infrastructure.Interfaces;
using KoalaChatApp.Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KoalaChatApp.Web {
    public class Program {
        public static void Main(string[] args) {
            IHost host = CreateHostBuilder(args).Build();
            IMessageQueue messageQueue = host.Services.GetRequiredService<IMessageQueue>();
            messageQueue.Connect();
            ICommandsHelper commandsHelper = host.Services.GetRequiredService<ICommandsHelper>();
            commandsHelper.AddCommand("stock");
            var userManager = host.Services.GetRequiredService<UserManager<ChatUser>>();
            var logger = host.Services.GetRequiredService<ILogger<DatabaseInitialization>>();
            var chatRoomService = host.Services.GetRequiredService<IChatRoomService>();
            DatabaseInitialization.Initialize(userManager, logger, chatRoomService);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseDefaultServiceProvider(options => options.ValidateScopes = false);
                });
    }
}
