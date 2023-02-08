using Newtonsoft.Json;
using VmProjectBFF.DTO.VCenter;

namespace VmProjectBFF.Services
{
    public partial class VCenterRepository
    {
        public List<ContentLibrary> GetContentLibraries()
        {
            List<ContentLibrary> contentLibraries = new();
            List<string> contentLibraryIds = GetContentLibraryIds();

            foreach (string contentLibraryId in contentLibraryIds)
            {
                contentLibraries.Add(GetContentLibraryById(contentLibraryId));
            }

            return contentLibraries;
        }

        public List<string> GetContentLibraryIds()
        {
            _lastResponse = _vCenterHttpClient.Get("rest/com/vmware/content/local-library");
            return (JsonConvert.DeserializeObject<ContentLibraryIdList>(_lastResponse.Response)).value;
        }

        public ContentLibrary GetContentLibraryById(string contentLibraryId)
        {
            _lastResponse = _vCenterHttpClient.Get($"rest/com/vmware/content/local-library/id:{contentLibraryId}");
            return (JsonConvert.DeserializeObject<ContentLibraryContainer>(_lastResponse.Response)).value;
        }

        public VmTemplateMetadata GetTemplateMetadata(string itemId)
        {
            _lastResponse = _vCenterHttpClient.Get($"rest/vcenter/vm-template/library-items/{itemId}");
            dynamic metadata = JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response).value;
            VmTemplateMetadata templateMetadata = new();

            templateMetadata.cpuCount = metadata.cpu.count;
            templateMetadata.coresPerSocket = metadata.cpu.cores_per_socket;
            templateMetadata.memory = metadata.memory.size_MiB;
            templateMetadata.os = metadata.guest_OS;
            templateMetadata.storage = metadata.disks;

            return templateMetadata;

        }

        public List<Folder> GetFolders()
        {
            _lastResponse = _vCenterHttpClient.Get("rest/vcenter/folder", new Dictionary<string, dynamic>() { { "filter.type", "VIRTUAL_MACHINE" } });
            return new List<Folder>(JsonConvert.DeserializeObject<FolderContainer>(_lastResponse.Response).value);
        }

        public List<VmTemplate> GetTemplatesByContentLibraryId(string contentLibraryId)
        {
            List<VmTemplate> templates = new();
            List<string> templateIds = GetTemplateIdsInContentLibrary(contentLibraryId);
            foreach (string templateId in templateIds)
            {
                templates.Add(GetTemplateByVCenterId(templateId));
            }
            return templates;
        }

        public List<string> GetTemplateIdsInContentLibrary(string contentLibraryId)
        {
            _lastResponse = _vCenterHttpClient.Get("rest/com/vmware/content/library/item", new Dictionary<string, dynamic>() { { "library_id", contentLibraryId } });
            return (JsonConvert.DeserializeObject<TemplateInLibrary>(_lastResponse.Response).value);
        }

        public VmTemplate GetTemplateByVCenterId(string vCenterId)
        {
            _lastResponse = _vCenterHttpClient.Get($"rest/com/vmware/content/library/item/id:{vCenterId}");
            return (JsonConvert.DeserializeObject<VmTemplateContainer>(_lastResponse.Response).value);
        }

        public List<Pool> GetResourcePools()
        {
            _lastResponse = _vCenterHttpClient.Get("rest/vcenter/resource-pool");
            return new List<Pool>(JsonConvert.DeserializeObject<PoolContainer>(_lastResponse.Response).value);
        }

        public List<VmNetworkInfo> GetVmNetworkInfo(string vmInstanceVcenterId)
        {
            _lastResponse = _vCenterHttpClient.Get($"rest/vcenter/vm/{vmInstanceVcenterId}/guest/networking/interfaces");
            return new List<VmNetworkInfo>(JsonConvert.DeserializeObject<VmNetworkInfoContainer>(_lastResponse.Response).value);
        }
    }
}
