using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using FinTransact.Shared.Model;
using Newtonsoft.Json;

namespace AuditService
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

                LogTransaction(transaction);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void LogTransaction(Transaction transaction)
        {
            // Business logic for logging transactions
        }
    }
}
