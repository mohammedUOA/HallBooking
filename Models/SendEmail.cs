using MimeKit;
using MailKit.Net.Smtp;

namespace Continuous_Learning_Booking.Models
{
    
    public class SendEmail
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mohammed", "mohammedalani1991@gmail.com"));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            var htmlTemplate = await File.ReadAllTextAsync("Models/EmailTemplate.html");

            // Replace placeholders with dynamic data
            htmlTemplate = htmlTemplate.Replace("{Name}", "Mohammed Alani")
                                       .Replace("{BookingId}", body);

            bodyBuilder.HtmlBody = htmlTemplate;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("mohammedalani1991@gmail.com", "zkxtplekockixjrp");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

    }
}
