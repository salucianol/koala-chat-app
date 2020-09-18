using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KoalaChatApp.Bot.Infrastructure.Services {
    public class ApiRequester : IApiRequester {
        private readonly ILogger<ApiRequester> logger;
        public ApiRequester(ILogger<ApiRequester> logger) {
            this.logger = logger;
        }

        public async Task<string> MakeGetRequest(string url) {
            try {
                using (HttpClient httpClient = new HttpClient()) {
                    using (HttpResponseMessage responseMessage = await httpClient.GetAsync(url)) {
                        this.logger.LogInformation($"Requesting URL: {url}", null);
                        if (responseMessage.IsSuccessStatusCode) {
                            return await responseMessage.Content.ReadAsStringAsync();
                        }
                        return string.Empty;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
