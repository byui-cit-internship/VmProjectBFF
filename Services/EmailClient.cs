using System.Net.Mail;
using System.Net;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class EmailClient : IEmailClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailClient> _logger;
        private readonly string _senderEmail;
        private readonly string _emailPassword;
        private readonly string _clientHost;
        private readonly string _senderEmailPassword;
        private readonly string _emailHead;
        private readonly SmtpClient _client;

        public EmailClient(
            IConfiguration configuration,
            ILogger<EmailClient> logger)
        {
            _configuration = configuration;
            _senderEmail = _configuration.GetConnectionString("vimaEmail");
            _senderEmailPassword = _configuration.GetConnectionString("vimaEmailPassword");
            _clientHost = _configuration.GetConnectionString("emailClientHost");
            _emailHead = _configuration.GetConnectionString("emailHead");

            _client = new();
            _client.Credentials = new NetworkCredential(_senderEmail, _senderEmailPassword);
            _client.Port = 587;
            _client.Host = _clientHost;
            _client.EnableSsl = true;
            _logger = logger;
        }
        public void SendEmail(
            string receiverEmail,
            int code,
            string subject)
        {
            string body = $"The code is {code.ToString()}";

            MailMessage mail = new MailMessage();
            mail.To.Add(receiverEmail);
            mail.From = new MailAddress(_senderEmail, _emailHead);
            mail.Subject = subject;
            mail.Body = body;
            try
            {
                _client.Send(mail);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }

}