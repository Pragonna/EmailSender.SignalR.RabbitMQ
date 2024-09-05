using MessageSenderExample.EmailSenderHelper;
using MessageSenderExample.Models;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var options = ConnectionOptions.Options;

HubConnection hubConnection=new HubConnectionBuilder()
                                .WithUrl("https://localhost:7147/notification-hub")
                                .Build();
await hubConnection.StartAsync();

ConnectionFactory factory = new();
factory.Uri = new Uri(options.Uri);

using IConnection connection = factory.CreateConnection();      // 1. Create connection  - RabbitMQ
using IModel channel = connection.CreateModel();                // 2. Create Channel 

channel.QueueDeclare(queue: "messagequeue",
                     durable: false,      //  save all message  when server dumped
                     exclusive: false,    // delete all message  when disconnect with server
                     autoDelete: false    // delete all message when finished - last consumer
                     );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume("messagequeue", true, consumer);


consumer.Received += async (s, e) =>
{
    string data = Encoding.UTF8.GetString(e.Body.Span);
    Email model = JsonSerializer.Deserialize<Email>(data);
    EmailSender.Send(model.ToEmail, model.Message);

    string message = $"Your message has been sent successfully to {model.ToEmail}";

    Console.WriteLine(message);
   await hubConnection.InvokeAsync("SendMessageAsync", message);

};

Console.Read();