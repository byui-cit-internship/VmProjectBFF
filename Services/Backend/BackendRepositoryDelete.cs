namespace VmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public void DeleteToken()
        {
            _lastResponse = _backendHttpClient.Delete("api/v1/token", null);
        }
    }
}