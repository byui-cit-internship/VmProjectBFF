using Newtonsoft.Json;
using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public User PutUser(User user)
        {
            _lastResponse = _backendHttpClient.Put("api/v2/User", user);
            return JsonConvert.DeserializeObject<User>(_lastResponse.Response);
        }
    }
}