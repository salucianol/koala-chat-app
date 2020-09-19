using System;
using System.Threading;
using System.Threading.Tasks;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KoalaChatApp.Bot {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly BotConfigurations _botConfigurations = new BotConfigurations();

        public Worker(ILogger<Worker> logger, IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;
            _configuration.GetSection("BotConfigurations")
                            .Bind(_botConfigurations);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            if (!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(_botConfigurations.ServiceDelay).Milliseconds, 
                                    stoppingToken);
            }
        }
    }
}
