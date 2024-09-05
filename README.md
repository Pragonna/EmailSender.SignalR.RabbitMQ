<h3>Sending an email with the information we received from the client</h3>

<li> 1. we receive an email and a message from the client</li> <br>
        
        public void Connect(object model)  // object is User { ToEmail: email@example.com,  Message: "Some Message" }
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


        }

  <br>
<li>2. We serialize the received data and send it to the queue in json format</li><br>
  
            string data = JsonSerializer.Serialize(model);

            channel.BasicPublish(exchange: "",
                                 routingKey: "messagequeue",
                                 body: Encoding.UTF8.GetBytes(data),
                                 basicProperties: null);

<br>
<li>3. Through our consumer, we manage the data in the queue, dequeue it and send the message from the client to the email. [using MailKit]</li><br>
   
           static public void Send(string to, string message)
        {
            ConnectionOptions options = ConnectionOptions.Options;

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(options.SenderName, options.Email));
            email.To.Add(new MailboxAddress("Hi Friend", to));
            email.Subject = "Testing MailKit";
            email.Body = new TextPart("test")
            {
                Text = message
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate(options.Email, options.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

<br>
<li>4. Using SignalR, we invoke our method in the hub and send a notification back to the client</li><br>

    
    consumer.Received += async (s, e) =>
    {
    string data = Encoding.UTF8.GetString(e.Body.Span);
    Email model = JsonSerializer.Deserialize<Email>(data);
    EmailSender.Send(model.ToEmail, model.Message);

    string message = $"Your message has been sent successfully to {model.ToEmail}";

    Console.WriteLine(message);
     await hubConnection.InvokeAsync("SendMessageAsync", message);

    };

 <br>

 <li> Notification Hub </li><br>
 
    public class NotificationHub : Hub<INotificationClient>
    {
        public async Task SendMessageAsync(string message)
        {
            await Clients.Caller.ReceiveMessage(message);
        }
    }



