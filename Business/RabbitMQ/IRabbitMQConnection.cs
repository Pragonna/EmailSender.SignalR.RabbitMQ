namespace Business.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        void Connect(object message);
    }
}