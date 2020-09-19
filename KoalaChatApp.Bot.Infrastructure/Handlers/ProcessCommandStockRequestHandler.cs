using KoalaChatApp.Bot.ApplicationCore.DTOs;
using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using KoalaChatApp.Bot.ApplicationCore.Models;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace KoalaChatApp.Bot.Infrastructure.Handlers {
    public class ProcessCommandStockRequestHandler : IRequestHandler<StockQuoteRequestModel, bool> {
        private readonly ILogger<ProcessCommandStockRequestHandler> _logger;
        private readonly ICache<Stock> _cacheStock;
        private readonly IApiRequester _apiRequester;
        private readonly ITextParser _textParser;
        private readonly IMessageQueue _messageQueue;
        private readonly IConfiguration _configuration;
        private readonly StockApiConfig _stockApiConfig = new StockApiConfig();

        public ProcessCommandStockRequestHandler(ILogger<ProcessCommandStockRequestHandler> logger, 
                                                    ICache<Stock> cacheStock, 
                                                    IApiRequester apiRequester, 
                                                    ITextParser textParser, 
                                                    IMessageQueue messageQueue, 
                                                    IConfiguration configuration) {
            _logger = logger;
            _cacheStock = cacheStock;
            _apiRequester = apiRequester;
            _textParser = textParser;
            _messageQueue = messageQueue;
            _configuration = configuration;
            _configuration.GetSection("StockApiConfig").Bind(_stockApiConfig);
        }

        public async Task<bool> Handle(StockQuoteRequestModel request, CancellationToken cancellationToken) {
            if (!cancellationToken.IsCancellationRequested) {
                if (request.Command != string.Empty && request.Command?.Length > 0) {
                    Stock stock;
                    CacheKey<Stock> cacheStockKey = _cacheStock.GetKey(request.Command);
                    if (cacheStockKey == null) {
                        string stockInfo = await _apiRequester
                                                        .MakeGetRequest($"{_stockApiConfig.Url}&s={request.Command}");
                        stock = _textParser.ParseText(stockInfo);
                        _cacheStock.PutKey(new CacheKey<Stock> {
                            Key = request.Command,
                            Message = stock
                        });
                        _logger.LogInformation($"Command ({request.Command}) not found in cache. Caching it...");
                    } else {
                        _logger.LogInformation($"Command ({request.Command}) found in cache.");
                        stock = cacheStockKey.Message;
                    }
                    _messageQueue.EnqueueMessage(new QueueMessage {
                        Command = request.Command,
                        RoomId = request.RoomId,
                        Quote = $"{stock.Symbol} quote is {stock.Open} per share."
                    });
                    return true;
                }
                _logger.LogInformation($"Command ({request.Command}) was, either empty or length lesser than 1.");
            }
            return false;
        }
    }
}
