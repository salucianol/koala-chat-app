using KoalaChatApp.Bot.ApplicationCore.DTOs;
using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using KoalaChatApp.Bot.ApplicationCore.Models;
using KoalaChatApp.Bot.Infrastructure.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;

namespace KoalaChatApp.Bot.Infrastructure.Services {
    public class MessageQueue : IMessageQueue {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly RabbitMqConfigurations _rabbitConfigurations = new RabbitMqConfigurations();
        private IConnection _connection;
        private IModel _model;
        public MessageQueue(IConnectionFactory connectionFactory, 
                                IConfiguration configuration, 
                                IMediator mediator) {
            _connectionFactory = connectionFactory;
            _configuration = configuration;
            _configuration.GetSection("RabbitMqConfigurations")
                            .Bind(_rabbitConfigurations);
            _mediator = mediator;
        }

        public void Connect() {
            if (_connection == null) {
                _connection = _connectionFactory.CreateConnection();
                if (_model == null) {
                    _model = _connection.CreateModel();
                }
                _model.QueueDeclare(queue: _rabbitConfigurations.MessageInboundQueue,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                _model.QueueDeclare(queue: _rabbitConfigurations.MessageOutboundQueue,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(_model);
                eventingBasicConsumer.Received += EventingBasicConsumer_Received;
                _model.BasicConsume(_rabbitConfigurations.MessageInboundQueue, 
                                        true, 
                                        eventingBasicConsumer);
            }
        }

        public void EnqueueMessage(QueueMessage message) {
            byte[] queueMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _model = _connection.CreateModel();
            _model.BasicPublish(exchange: "",
                                 routingKey: _rabbitConfigurations.MessageOutboundQueue,
                                 basicProperties: null,
                                 body: queueMessage);
        }

        private async void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e) {
            QueueMessage queueMessage = JsonConvert
                                            .DeserializeObject<QueueMessage>(Encoding.UTF8.GetString(e.Body.ToArray()));
            await _mediator.Send<bool>(new StockQuoteRequestModel {
                Command = queueMessage.Command,
                RoomId = queueMessage.RoomId
            });
        }
    }
}
