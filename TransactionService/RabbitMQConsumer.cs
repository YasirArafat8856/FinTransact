using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using FinTransact.Shared.Model;
using Newtonsoft.Json;

namespace TransactionService
{
    public class RabbitMQConsumer : BackgroundService
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
                ProcessTransaction(transaction);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void ProcessTransaction(Transaction transaction)
        {
            // Business logic for processing transaction
            Console.WriteLine($"Processing transaction: {transaction.Id}");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Consumer logic
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
