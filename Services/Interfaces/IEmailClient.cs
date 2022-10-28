namespace vmProjectBFF.Services
{
    public interface IEmailClient
    {
        public void SendEmailCode(string receiverEmail, string code, string subject);
        public void SendEmail(string receiverEmail, string code, string subject);

    }
}