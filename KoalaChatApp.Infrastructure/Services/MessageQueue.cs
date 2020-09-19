using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Configurations;
using KoalaChatApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.Infrastructure.Models;
using KoalaChatApp.Infrastructure.Data.Specifications;
using System.Linq;

namespace KoalaChatApp.Infrastructure.Services {
    public class MessageQueue : IMessageQueue {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly IChatHubService _chatHubService;
        private readonly IRepository<ChatUser> _userRepository;
        private readonly RabbitMqConfigurations _rabbitConfigurations = new RabbitMqConfigurations();
        private IConnection _connection;
        private IModel _model;

        public MessageQueue(IConnectionFactory connectionFactory, 
                                IConfiguration configuration, 
                                IChatHubService chatHubService, 
                                IRepository<ChatUser> userRepository) {
            _connectionFactory = connectionFactory;
            _configuration = configuration;
            _configuration.GetSection("RabbitMqConfigurations")
                            .Bind(_rabbitConfigurations);
            _chatHubService = chatHubService;
            _userRepository = userRepository;
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
                _model.BasicConsume(_rabbitConfigurations.MessageOutboundQueue, true, eventingBasicConsumer);
            }
        }

        public void EnqueueMessage(QueueMessageDTO message) {
            byte[] queueMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _model = _connection.CreateModel();
            _model.BasicPublish(exchange: "",
                                 routingKey: _rabbitConfigurations.MessageInboundQueue,
                                 basicProperties: null,
                                 body: queueMessage);
        }

        private void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e) {
            QueueMessageDTO queueMessage = JsonConvert
                                            .DeserializeObject<QueueMessageDTO>(Encoding.UTF8
                                                                                            .GetString(e.Body.ToArray()));
            ChatMessageTextDTO chatMessageText = new ChatMessageTextDTO {
                Date = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm"),
                RoomId = Guid.Parse(queueMessage.RoomId),
                RoomName = string.Empty,
                Text = queueMessage.Quote,
                User = _userRepository
                        .Get(new UserSpecification("bot@koalaappchat"))
                        .FirstOrDefault()
                        .UserName
            };
            _chatHubService.SendMessage(queueMessage.RoomId, chatMessageText);
        }
    }
}
