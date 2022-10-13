namespace vmProjectBFF.Services
{
    public interface IBackendRepository
    {
        public IBackendHttpClient BackendHttpClient { get; }
    }
}
