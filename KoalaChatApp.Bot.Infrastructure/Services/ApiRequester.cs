using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KoalaChatApp.Bot.Infrastructure.Services {
    public class ApiRequester : IApiRequester {
        private readonly ILogger<ApiRequester> _logger;
        public ApiRequester(ILogger<ApiRequester> logger) {
            _logger = logger;
        }

        public async Task<string> MakeGetRequest(string url) {
            try {
                using HttpClient httpClient = new HttpClient();
                _logger.LogInformation($"Requesting URL: {url}", null);
                using HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode) {
                    _logger.LogInformation($"Request was successful.", null);
                    return await responseMessage.Content.ReadAsStringAsync();
                }
                _logger.LogInformation($"Something went wrong with this request: {url}.", null);
                return string.Empty;
            } catch (Exception ex) {
                _logger.LogError(ex, "An internal error ocurred, check logs for further details.");
                throw;
            }
        }
    }
}
