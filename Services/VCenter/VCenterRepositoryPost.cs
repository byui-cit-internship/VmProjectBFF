using Newtonsoft.Json;
using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public partial class VCenterRepository
    {
        public string NewVmInstanceByTemplateId(
            string vCenterTemplateId,
            Deploy deploy)
        {
            _lastResponse = _vCenterHttpClient.Post($"api/vcenter/vm-template/library-items/{vCenterTemplateId}", new() { { "action", "deploy" } }, deploy);
            return JsonConvert.DeserializeObject<string>(_lastResponse.Response);
        }
    }
}
