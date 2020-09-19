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
        private readonly IConnectionFactory connectionFactory;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly RabbitMqConfigurations rabbitConfigurations = new RabbitMqConfigurations();
        private IConnection connection;
        private IModel model;
        public MessageQueue(IConnectionFactory connectionFactory, IConfiguration configuration, IMediator mediator) {
            this.connectionFactory = connectionFactory;
            this.configuration = configuration;
            this.configuration.GetSection("RabbitMqConfigurations").Bind(this.rabbitConfigurations);
            this.mediator = mediator;
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
                model.BasicConsume(this.rabbitConfigurations.MessageInboundQueue, true, eventingBasicConsumer);
            }
        }

        public void EnqueueMessage(QueueMessage message) {
            byte[] queueMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            this.model = this.connection.CreateModel();
            this.model.BasicPublish(exchange: "",
                                 routingKey: this.rabbitConfigurations.MessageOutboundQueue,
                                 basicProperties: null,
                                 body: queueMessage);
        }

        private async void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e) {
            QueueMessage queueMessage = JsonConvert.DeserializeObject<QueueMessage>(Encoding.UTF8.GetString(e.Body.ToArray()));
            await this.mediator.Send<bool>(new StockQuoteRequestModel {
                Command = queueMessage.Command,
                RoomId = queueMessage.RoomId
            });
        }
    }
}
