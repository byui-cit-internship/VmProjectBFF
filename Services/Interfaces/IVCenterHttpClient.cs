namespace VmProjectBFF.Services
{
    public interface IVCenterHttpClient : IBffHttpClient, IDisposable
    {
        public void Initialize();
    }
}
