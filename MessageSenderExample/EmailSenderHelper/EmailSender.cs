using MimeKit;
using MailKit.Net.Smtp;
using MessageSenderExample.Models;

namespace MessageSenderExample.EmailSenderHelper
{
    static public class EmailSender
    {
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


            //SmtpClient smtpClient = new()
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    EnableSsl = true,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(userName: " email ",
            //                                        password: " password")
            //};

            //MailAddress from = new("email");
            //MailAddress _to = new(to);

            //using MailMessage mail = new(from, _to)
            //{
            //    Subject = "test",
            //    Body = message,
            //    IsBodyHtml = true,
            //    Sender = from
            //};
            //try
            //{
            //    smtpClient.Send(mail);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Exception Stack Trace:      {ex.StackTrace}");
            //    Console.WriteLine($"Exception message:          {ex.Message}");
            //    Console.WriteLine($"HelpLink:                   {ex.HelpLink}");
            //    Console.WriteLine($"HResult:                    {ex.HResult}");
            //}





        }
    }
}
