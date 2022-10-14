using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public dynamic getInstancesByUser(int userId){
            _lastResponse = _backendHttpClient.Get();
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response); 
            
        }
    }
}
