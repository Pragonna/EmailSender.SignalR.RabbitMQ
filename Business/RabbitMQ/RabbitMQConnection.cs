using Business.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Business.RabbitMQ
{
    public class RabbitMQConnection(IOptions<ConnectionOptions> options) : IRabbitMQConnection
    {
        string _Uri = options.Value.Uri;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Connect(object model)
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(_Uri);

            using IConnection connection = factory.CreateConnection();      // 1. Create connection  - RabbitMQ
            using IModel channel = connection.CreateModel();                // 2. Create Channel 

            channel.QueueDeclare(queue: "messagequeue",
                                 durable: false,      //  save all message  when server dumped
                                 exclusive: false,    // delete all message  when disconnect with server
                                 autoDelete: false    // delete all message when finished - last consumer
                                 );

            string data = JsonSerializer.Serialize(model);

            channel.BasicPublish(exchange: "",
                                 routingKey: "messagequeue",
                                 body: Encoding.UTF8.GetBytes(data),
                                 basicProperties: null);

        }
    }
}
