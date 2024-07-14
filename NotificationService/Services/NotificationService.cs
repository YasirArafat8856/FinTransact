namespace NotificationService.Services
{
    public class NotificationService
    {
        private readonly RabbitMQConsumer _rabbitMqConsumer;

        public NotificationService()
        {
            _rabbitMqConsumer = new RabbitMQConsumer();
        }
    }
}
