namespace VmProjectBFF.Services
{
    public interface IEmailClient
    {
        public void SendCode(string receiverEmail, string code, string subject);
        public void SendMessage(string receiverEmail, string subject, string message);

    }
}