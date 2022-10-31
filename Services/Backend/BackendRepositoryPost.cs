using Newtonsoft.Json;
using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public VmInstance CreateVmInstance(VmInstance vmInstance)
        {
            _lastResponse = _backendHttpClient.Post("api/v1/CreateVm", vmInstance);
            return JsonConvert.DeserializeObject<VmInstance>(_lastResponse.Response);
        }

        public (User, string) PostToken(AccessToken token)
        {
            _lastResponse = _backendHttpClient.Post("api/v1/token", token.AccessTokenValue);
            return JsonConvert.DeserializeObject<(User, string)>(_lastResponse.Response);
        }

        public User PostUser(User user)
        {
            _lastResponse = _backendHttpClient.Post("api/v2/User", user);
            return JsonConvert.DeserializeObject<User>(_lastResponse.Response);
        }
    }
}