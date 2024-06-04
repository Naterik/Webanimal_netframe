using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private const string FromEmail = "testemailmvcnet@gmail.com";
    private const string FromPassword = "ybtnwncbfxqusqpg";

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var fromAddress = new MailAddress(FromEmail, "User");
        var toAddress = new MailAddress(toEmail);

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, FromPassword)
        };

        using (var mailMessage = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        })
        {
            await smtp.SendMailAsync(mailMessage);
        }
    }
}
