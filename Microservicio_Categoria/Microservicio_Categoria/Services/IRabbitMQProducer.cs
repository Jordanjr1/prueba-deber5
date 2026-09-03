namespace Microservicio_Categoria.Services
{
    public interface IRabbitMQProducer
    {
        Task EnviarMensajeAsync<T>(T mensaje);
    }
}