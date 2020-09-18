using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KoalaChatApp.Bot.ApplicationCore.DTOs;
using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using KoalaChatApp.Bot.ApplicationCore.Models;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using KoalaChatApp.Bot.Infrastructure.Handlers;
using KoalaChatApp.Bot.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace KoalaChatApp.Bot {
    public class Program {
        public static void Main(string[] args) {
            IHost host = CreateHostBuilder(args).Build();
            IMessageQueue messageQueue = host.Services.GetRequiredService<IMessageQueue>();
            messageQueue.Connect();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggerFactory => loggerFactory.AddConsole())
                .ConfigureServices((hostContext, services) => {
                    RabbitMqConfigurations rabbitConfigurations = new RabbitMqConfigurations();
                    hostContext.Configuration.GetSection("RabbitMqConfigurations").Bind(rabbitConfigurations);
                    services.AddMediatR(Assembly.GetExecutingAssembly());
                    services.AddLogging(configure => configure.AddConsole());
                    services.AddMemoryCache();
                    services.AddSingleton<IConnectionFactory>(new ConnectionFactory {
                        HostName = rabbitConfigurations.Hostname,
                        Port = rabbitConfigurations.Port,
                        UserName = rabbitConfigurations.Username,
                        Password = rabbitConfigurations.Password
                    });
                    services.AddSingleton<ICache<Stock>, Cache>();
                    services.AddSingleton<IApiRequester, ApiRequester>();
                    services.AddSingleton<IMessageQueue, MessageQueue>();
                    services.AddSingleton<ITextParser, CsvTextParser>();
                    services.AddSingleton<IRequestHandler<StockQuoteRequestModel, bool>, ProcessCommandStockRequestHandler>();
                    services.AddHostedService<Worker>();
                });
    }
}
