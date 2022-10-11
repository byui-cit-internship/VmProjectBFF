namespace vmProjectBFF.Services
{
    public interface IVCenterHttpClient : IBffHttpClient, IDisposable
    {
        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
    }
}
