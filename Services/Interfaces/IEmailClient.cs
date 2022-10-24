namespace vmProjectBFF.Services
{
    public interface IEmailClient
    {
        public void sendEmail(string receiverEmail, int code, string subject);
    }
}