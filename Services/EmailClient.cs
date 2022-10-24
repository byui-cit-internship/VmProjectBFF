using System.Net.Mail;
using System.Net;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class EmailClient : IEmailClient
    {
        IConfiguration _configuration;
        private readonly string _senderEmail;
        private readonly string _emailPassword;
        private readonly string _clientHost;
        private readonly string _senderEmailPassword;
        private readonly string _emailHead;

        public EmailClient(
            IConfiguration configuration)
        {
            _configuration = configuration;
            _senderEmail = _configuration.GetConnectionString("vimaEmail");
            _senderEmailPassword = _configuration.GetConnectionString("emailClientHost");
            _clientHost = _configuration.GetConnectionString("smtp.outlook.com");
            _emailHead = _configuration.GetConnectionString("GetConnectionString");
        }
        public void sendEmail(
            string receiverEmail,
            int code,
            string Subject)
        {
            string body = "The code is" + code.ToString();

            MailMessage mail = new MailMessage();
            mail.To.Add(receiverEmail);
            mail.From = new MailAddress(_senderEmail, _emailHead);
            mail.Subject = Subject;
            mail.Body = body;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(_senderEmail, _senderEmailPassword);
            client.Port = 587;
            client.Host = _clientHost;
            client.EnableSsl = true;

            try
            {
                client.Send(mail);
            }
            catch (BffHttpException be)
            {
                Console.WriteLine(be);
            }
        }
    }

}