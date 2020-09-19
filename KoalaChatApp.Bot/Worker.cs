using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KoalaChatApp.Bot.ApplicationCore.DTOs;
using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KoalaChatApp.Bot {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> logger;
        private readonly IConfiguration configuration;
        private readonly BotConfigurations botConfigurations = new BotConfigurations();

        public Worker(ILogger<Worker> logger, IConfiguration configuration) {
            this.logger = logger;
            this.configuration = configuration;
            this.configuration.GetSection("BotConfigurations").Bind(this.botConfigurations);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            if (!stoppingToken.IsCancellationRequested) {
                this.logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(this.botConfigurations.ServiceDelay).Milliseconds, stoppingToken);
            }
        }
    }
}
