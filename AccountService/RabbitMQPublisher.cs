using FinTransact.Shared.Model;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace AccountService
{
    public class RabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;

        public RabbitMQPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "transaction_exchange", type: ExchangeType.Fanout);
        }

        public void PublishTransaction(Transaction transaction)
        {
            var message = JsonConvert.SerializeObject(transaction);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "transaction_exchange",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
