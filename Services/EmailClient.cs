using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using VmProjectBFF.Exceptions;
// [Route("api/[controller]")]
// [ApiController]

namespace VmProjectBFF.Services
{

    // public class MailController : IMailController
    public partial class EmailClient : IEmailClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailClient> _logger;

        private readonly SmtpClient _client;
        private readonly string _clientHost;
        private readonly string _emailHead;
        private readonly string _frontendURI;
        private readonly string _senderEmailAddress;
        private readonly MailMessage _mailMessage;

        public EmailClient(
            IConfiguration configuration,
            ILogger<EmailClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _senderEmailAddress = _configuration.GetConnectionString("vimaEmail");
            _clientHost = _configuration.GetConnectionString("emailClientHost");
            _emailHead = _configuration.GetConnectionString("emailHead");
            _frontendURI = _configuration.GetConnectionString("frontendRootUri");

            _client = new()
            {
                Credentials = new NetworkCredential(_senderEmailAddress, _configuration.GetConnectionString("vimaEmailPassword")),
                Port = 587,
                Host = _clientHost,
                EnableSsl = true
            };

            _mailMessage = new();
        }
        public void SendCode(
            string receiverEmail,
            string code,
            string subject)
        {

            string link = _frontendURI + $"verifyemail?code={code}";
            string content = @$"
            <div>
            <body bgcolor=aliceblue>
            <h3><font face=Georgia size=5px background=pink>Hello there!</h3>
            <p><font face=Tahoma color=black size=3px>Your verification code is <b>{code}</b></p>
            <p><font face=Tahoma color=black size=3px width:20px height:20px background-color:red>Or click on this link <a href={link}>{link}</a> </p>
            <br></br>
            </body>
            </div>
            ";
            // <img src=https://media.giphy.com/media/NdKVEei95yvIY/giphy.gif>
            // SEPARATION
            string FilePath = "./templates/vimaemail.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            // MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
            // SEPARATION
            try
            {
                Console.WriteLine(MailText);
                SendEmail(receiverEmail, subject, MailText);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
        public void SendMessage(
            string receiverEmail,
            string subject, 
            string message)
        {
            string content = @$"
            <div>
            <p >{message}</p>
            </div>
            ";
            try
            {
                SendEmail(receiverEmail, subject, content);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private void SendEmail(string receiverEmail, string subject, string content)
        {
            _mailMessage.To.Add(receiverEmail);
            _mailMessage.From = new MailAddress(_senderEmailAddress, _emailHead);
            _mailMessage.Subject = subject;
            _mailMessage.IsBodyHtml = true;
            _mailMessage.AlternateViews.Add(GetEmailView(content, "./Images/LOGO-VIMA.png"));
            try
            {
                _client.Send(_mailMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private AlternateView GetEmailView(string content, string imgFilePath)
        {
            LinkedResource res = new(imgFilePath)
            {
                ContentId = Guid.NewGuid().ToString()
            };

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