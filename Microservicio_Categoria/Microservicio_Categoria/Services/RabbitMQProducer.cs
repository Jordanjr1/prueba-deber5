using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Microservicio_Categoria.Services
{
    public interface IRabbitMQProducer
    {
        void EnviarMensaje<T>(T mensaje);
    }

    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConfiguration _configuration;

        public RabbitMQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnviarMensaje<T>(T mensaje)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            using var connection = factory.CreateConnectionAsync().Result;
            using var channel = connection.CreateChannelAsync().Result;

            var queueName = _configuration["RabbitMQ:QueueName"] ?? "categoria-queue";

            channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            ).GetAwaiter().GetResult();

            var jsonString = JsonSerializer.Serialize(mensaje);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body
            ).GetAwaiter().GetResult();
        }
    }
}