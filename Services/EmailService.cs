using System.Net.Mail;
using System.Net;
using System.Text;

namespace vmProjectBFF.Services
{
    public class EmailClient
    {
        
    }
}

// MailMessage mail = new MailMessage();
// mail.To.Add("dadelapema@gmail.com");
// mail.From = new MailAddress("fakeuser1995@outlook.com", "Email head", Encoding.UTF8);
// mail.Subject = "This mail is send from asp.net application";
// //mail.SubjectEncoding = System.Text.Encoding.UTF8;
// mail.Body = "This is Email Body Text";
// //mail.BodyEncoding = System.Text.Encoding.UTF8;
// //mail.IsBodyHtml = true;
// mail.Priority = MailPriority.High;

// SmtpClient client = new SmtpClient();
// client.Credentials = new NetworkCredential("fakeuser1995@outlook.com", "fakepassword123");
// client.Port = 587;
// client.Host = "smtp.outlook.com";
// client.EnableSsl = true;
// try
// {
//     client.Send(mail);
// }
// catch (Exception ex)
// {
//     Exception ex2 = ex;
//     string errorMessage = string.Empty;
//     while (ex2 != null)
//     {
//         errorMessage += ex2.ToString();
//         ex2 = ex2.InnerException;
//     }
// }