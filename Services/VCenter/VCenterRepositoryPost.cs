using Newtonsoft.Json;
using VmProjectBFF.DTO.VCenter;

namespace VmProjectBFF.Services
{
    public partial class VCenterRepository
    {
        public string NewVmInstanceByTemplateId(
            string vCenterTemplateId,
            Deploy deploy)
        {
            _lastResponse = _vCenterHttpClient.Post($"rest/vcenter/vm-template/library-items/{vCenterTemplateId}", new() { { "action", "deploy" } }, deploy);
            return JsonConvert.DeserializeObject<string>(_lastResponse.Response);
        }
    }
}
