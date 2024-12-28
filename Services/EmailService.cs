using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ChildGuard.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _senderEmail = "amritsyangtan1@gmail.com"; // Replace with your email
        private readonly string _senderPassword = "bdcx ycpy cbrr szpm"; // Replace with your email password
        private readonly Dictionary<string, string> _incidentTypeToEmail;

        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_senderEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(recipientEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
        public EmailService()
        {
            _incidentTypeToEmail = new Dictionary<string, string>
            {
                { "Accident", "amreitsyanf@gmail.com" },
                { "Crime", "crime-authority@example.com" },
                { "Fire", "fire-department@example.com" },
                { "Other", "general-authority@example.com" }
            };
        }

        public string GetRecipientEmail(string incidentType)
        {
            return _incidentTypeToEmail.TryGetValue(incidentType, out var email) ? email : "amreitsyanf@gmail.com";
        }
    }
}
