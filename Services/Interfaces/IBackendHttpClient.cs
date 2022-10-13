namespace vmProjectBFF.Services
{
    public interface IBackendHttpClient : IBffHttpClient
    {
        public string Cookie { get; set; }
    }
}
