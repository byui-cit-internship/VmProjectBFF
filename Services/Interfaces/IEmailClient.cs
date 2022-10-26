namespace vmProjectBFF.Services
{
    public interface IEmailClient
    {
        public void SendEmail(string receiverEmail, int code, string subject);
    }
}