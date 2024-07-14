using FinTransact.Shared.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService
{
    public class RabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "transaction_exchange", type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: "transaction_exchange",
                              routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var transaction = JsonConvert.DeserializeObject<Transaction>(message);

                SendNotification(transaction);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void SendNotification(Transaction transaction)
        {
            // Business logic for sending notifications (e.g., SignalR)
        }
    }
}
