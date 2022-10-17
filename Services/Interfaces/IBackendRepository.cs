namespace vmProjectBFF.Services
{
    public interface IBackendRepository
    {
        public IBackendHttpClient BackendHttpClient { get; }
        public dynamic getInstancesByUser(int userId);
    }
}
