using Newtonsoft.Json;
using VmProjectBFF.DTO.VCenter;

namespace VmProjectBFF.Services
{
    public partial class VCenterRepository
    {
        public NewVmInstance NewVmInstanceByTemplateId(
            string vCenterTemplateId,
            DeployContainer deployContainer)
        {
            _lastResponse = _vCenterHttpClient.Post($"rest/vcenter/vm-template/library-items/{vCenterTemplateId}", new() { { "action", "deploy" } }, deployContainer);
            return JsonConvert.DeserializeObject<NewVmInstance>(_lastResponse.Response);
        }
    }
}
