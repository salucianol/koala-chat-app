using KoalaChatApp.ApplicationCore.Entities;
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
        private readonly IConnectionFactory connectionFactory;
        private readonly IConfiguration configuration;
        private readonly IChatHubService chatHubService;
        private readonly IRepository<ChatUser> userRepository;
        private readonly RabbitMqConfigurations rabbitConfigurations = new RabbitMqConfigurations();
        private IConnection connection;
        private IModel model;
        public MessageQueue(IConnectionFactory connectionFactory, IConfiguration configuration, IChatHubService chatHubService, IRepository<ChatUser> userRepository) {
            this.connectionFactory = connectionFactory;
            this.configuration = configuration;
            this.configuration.GetSection("RabbitMqConfigurations").Bind(this.rabbitConfigurations);
            this.chatHubService = chatHubService;
            this.userRepository = userRepository;
        }

        public void Connect() {
            if (this.connection == null) {
                this.connection = this.connectionFactory.CreateConnection();
                if (this.model == null) {
                    this.model = connection.CreateModel();
                }
                model.QueueDeclare(queue: this.rabbitConfigurations.MessageInboundQueue,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                model.QueueDeclare(queue: this.rabbitConfigurations.MessageOutboundQueue,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(this.model);
                eventingBasicConsumer.Received += EventingBasicConsumer_Received;
                model.BasicConsume(this.rabbitConfigurations.MessageOutboundQueue, true, eventingBasicConsumer);
            }
        }

        public void EnqueueMessage(QueueMessageDTO message) {
            byte[] queueMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            this.model = this.connection.CreateModel();
            this.model.BasicPublish(exchange: "",
                                 routingKey: this.rabbitConfigurations.MessageInboundQueue,
                                 basicProperties: null,
                                 body: queueMessage);
        }

        private void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e) {
            QueueMessageDTO queueMessage = JsonConvert.DeserializeObject<QueueMessageDTO>(Encoding.UTF8.GetString(e.Body.ToArray()));
            ChatMessageTextDTO chatMessageText = new ChatMessageTextDTO {
                Date = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm"),
                RoomId = Guid.Parse(queueMessage.RoomId),
                RoomName = string.Empty,
                Text = queueMessage.Quote,
                User = this.userRepository.Get(new UserSpecification("bot@koalaappchat")).FirstOrDefault().UserName
            };
            this.chatHubService.SendMessage(queueMessage.RoomId, chatMessageText);
        }
    }
}
