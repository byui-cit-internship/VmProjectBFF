using vmProjectBFF.DTO;
using Newtonsoft.Json;
using vmProjectBFF.Models;
using Microsoft.Management.Infrastructure;

namespace vmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public VmInstance CreateVmInstance(VmInstance vmInstance)
        {
            _lastResponse = _backendHttpClient.Post("api/v1/CreateVm", vmInstance);
            return(JsonConvert.DeserializeObject<VmInstance>(_lastResponse.Response));
        }
    }
}