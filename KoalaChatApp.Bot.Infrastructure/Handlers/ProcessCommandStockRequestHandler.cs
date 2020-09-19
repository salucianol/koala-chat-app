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
        private readonly ILogger<ProcessCommandStockRequestHandler> logger;
        private readonly ICache<Stock> cacheStock;
        private readonly IApiRequester apiRequester;
        private readonly ITextParser textParser;
        private readonly IMessageQueue messageQueue;
        private readonly IConfiguration configuration;
        private readonly StockApiConfig stockApiConfig = new StockApiConfig();

        public ProcessCommandStockRequestHandler(ILogger<ProcessCommandStockRequestHandler> logger, ICache<Stock> cacheStock, IApiRequester apiRequester, ITextParser textParser, IMessageQueue messageQueue, IConfiguration configuration) {
            this.logger = logger;
            this.cacheStock = cacheStock;
            this.apiRequester = apiRequester;
            this.textParser = textParser;
            this.messageQueue = messageQueue;
            this.configuration = configuration;
            this.configuration.GetSection("StockApiConfig").Bind(this.stockApiConfig);
        }
        public async Task<bool> Handle(StockQuoteRequestModel request, CancellationToken cancellationToken) {
            if (!cancellationToken.IsCancellationRequested) {
                if (request.Command != string.Empty && request.Command?.Length > 0) {
                    Stock stock;
                    CacheKey<Stock> cacheStockKey = this.cacheStock.GetKey(request.Command);
                    if (cacheStockKey == null) {
                        string stockInfo = await this.apiRequester.MakeGetRequest($"{this.stockApiConfig.Url}&s={request.Command}");
                        stock = this.textParser.ParseText(stockInfo);
                        this.cacheStock.PutKey(new CacheKey<Stock> {
                            Key = request.Command,
                            Message = stock
                        });
                    } else {
                        stock = cacheStockKey.Message;
                    }
                    this.messageQueue.EnqueueMessage(new QueueMessage {
                        Command = request.Command,
                        RoomId = request.RoomId,
                        Quote = $"{stock.Symbol} quote is {stock.Open} per share."
                    });
                    return true;
                }
            }
            return false;
        }
    }
}
