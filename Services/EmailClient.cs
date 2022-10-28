using System.Net.Mail;
using System.Net;
using System.Net.Mime;
namespace vmProjectBFF.Services

{
    public partial class EmailClient : IEmailClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailClient> _logger;
        private readonly string _senderEmail;
        private readonly string _emailPassword;
        private readonly string _clientHost;
        private readonly string _senderEmailPassword;
        private readonly string _emailHead;
        private readonly string _frontendURI;
        private readonly SmtpClient _client;
        private readonly MailMessage _mailMessage;
        private readonly Stream _reader;

        public EmailClient(
            IConfiguration configuration,
            ILogger<EmailClient> logger)
        {
            _configuration = configuration;
            _senderEmail = _configuration.GetConnectionString("vimaEmail");
            _senderEmailPassword = _configuration.GetConnectionString("vimaEmailPassword");
            _clientHost = _configuration.GetConnectionString("emailClientHost");
            _emailHead = _configuration.GetConnectionString("emailHead");
            _frontendURI = _configuration.GetConnectionString("FrontendRootUri");

            _client = new();
            _client.Credentials = new NetworkCredential(_senderEmail, _senderEmailPassword);
            _client.Port = 587;
            _client.Host = _clientHost;
            _client.EnableSsl = true;
            _logger = logger;

            _mailMessage = new();
        }
        public void SendEmailCode(
            string receiverEmail,
            string code,
            string subject)
        {
            string link = _frontendURI + $"verifyemail?code={code}";
            _mailMessage.To.Add(receiverEmail);
            _mailMessage.From = new MailAddress(_senderEmail, _emailHead);
            _mailMessage.Subject = subject;
            _mailMessage.IsBodyHtml = true;
            _mailMessage.AlternateViews.Add(GetCodeEmail("./Images/LOGO-VIMA2.png", code, link));

            try
            {
                _client.Send(_mailMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public void SendEmail(
            string receiverEmail,
            string message,
            string subject)
        {
            try
            {
                // TO-DO    
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private AlternateView GetCodeEmail(string imgFilePath, string code, string link)
        {
            string content = @$"
            <div>
            <p>The code is {code}</p>
            <p>Or Click on this link: </p>
            <a href={link}>{link}</a>
            </div>
            ";

            LinkedResource res = new LinkedResource(imgFilePath);
            res.ContentId = Guid.NewGuid().ToString();
            string htmlBody = @"
            <div>
            <img src='cid:" + res.ContentId + @$"'/>
            {content}
            </div>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
    }
}