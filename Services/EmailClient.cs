using System.Net.Mail;
using System.Net;
using VmProjectBFF.Exceptions;

namespace VmProjectBFF.Services
{
    public class EmailClient : IEmailClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailClient> _logger;

        private readonly SmtpClient _client;
        private readonly string _clientHost;
        private readonly string _emailHead;
        private readonly string _senderEmailAddress;

        public EmailClient(
            IConfiguration configuration,
            ILogger<EmailClient> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _senderEmailAddress = _configuration.GetConnectionString("vimaEmail");
            _clientHost = _configuration.GetConnectionString("emailClientHost");
            _emailHead = _configuration.GetConnectionString("emailHead");

            _client = new()
            {
                Credentials = new NetworkCredential(_senderEmailAddress, _configuration.GetConnectionString("vimaEmailPassword")),
                Port = 587,
                Host = _clientHost,
                EnableSsl = true
            };
        }
        public void SendEmail(
            string receiverEmail,
            int code,
            string subject)
        {
            string body = $"The code is {code}";

            MailMessage mail = new();
            mail.To.Add(receiverEmail);
            mail.From = new MailAddress(_senderEmailAddress, _emailHead);
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