using System.Net.Mail;
using System.Net;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class EmailClient
    {
        public void sendEmail(
            string receiverEmail,
            int code,
            string emailHead,
            string Subject        )
        {
            // var senderEmail = configuration.GetConnectionString("vimaEmail");
            // var senderEmailPassword = configuration.GetConnectionString("vimaEmailPassword");
            // var clientHost = configuration.GetConnectionString("emailClientHost");
            var senderEmail = "fakeuser1995@outlook.com";
            var senderEmailPassword = "fakepassword123";

            string body = "The code is" + code.ToString();

            MailMessage mail = new MailMessage();
            mail.To.Add(receiverEmail);
            mail.From = new MailAddress(senderEmail, emailHead);
            mail.Subject = Subject;
            mail.Body = body;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(senderEmail, senderEmailPassword);
            client.Port = 587;
            client.Host = "smtp.outlook.com";
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