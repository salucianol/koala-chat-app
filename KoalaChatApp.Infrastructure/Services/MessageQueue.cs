using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Configurations;
using KoalaChatApp.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace KoalaChatApp.Infrastructure.Services {
    public class MessageQueue : IMessageQueue {
        private readonly IConnectionFactory connectionFactory;
        private readonly IConfiguration configuration;
        private RabbitMqConfigurations rabbitConfigurations = new RabbitMqConfigurations();
        private IConnection connection;
        private IModel model;
        public MessageQueue(IConnectionFactory connectionFactory, IConfiguration configuration) {
            this.connectionFactory = connectionFactory;
            this.configuration = configuration;
            this.configuration.GetSection("RabbitMqConfigurations").Bind(this.rabbitConfigurations);
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

        public void EnqueueMessage(string message) {
            byte[] queueMessage = Encoding.UTF8.GetBytes(message);
            this.model = this.connection.CreateModel();
            this.model.BasicPublish(exchange: "",
                                 routingKey: this.rabbitConfigurations.MessageInboundQueue,
                                 basicProperties: null,
                                 body: queueMessage);
        }

        private void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e) {
            ChatMessageText chatMessageText = new ChatMessageText(Guid.Parse("00000000-0000-0000-0000-000000000000"), Encoding.UTF8.GetString(e.Body.ToArray()));
        }
    }
}
