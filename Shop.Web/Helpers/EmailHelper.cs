using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;


namespace Shop.Web.Helpers
{
    public class EmailHelper :IEmailHelper
    {
        private readonly IConfiguration configuration;

        public EmailHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMail(string to, string subject, string body)
        {
            string from = configuration["Mail:From"];
            string smtp = configuration["Mail:Smtp"];
            string port = configuration["Mail:Port"];
            string password = configuration["Mail:Password"];

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(from));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;
            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(smtp, int.Parse(port), false);
                client.Authenticate(from, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }

    }
}
