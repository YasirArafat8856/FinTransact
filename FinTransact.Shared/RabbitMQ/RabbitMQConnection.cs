using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace FinTransact.Shared.RabbitMQ
{
    public class RabbitMQConnection : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConnection(RabbitMQConfig config)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: config.ExchangeName, type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: config.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: config.QueueName, exchange: config.ExchangeName, routingKey: config.RoutingKey);
        }

        public void Publish(string message, string routingKey)
        {
            var body = System.Text.Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "transaction_exchange", routingKey: routingKey, basicProperties: null, body: body);
        }

        public IModel GetChannel() => _channel;

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
