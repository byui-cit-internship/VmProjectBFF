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

        public List<Folder> GetFolders()
        {
            _lastResponse = _vCenterHttpClient.Get("rest/vcenter/folder", new() { { "filter.type", "VIRTUAL_MACHINE" } });
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
            _lastResponse = _vCenterHttpClient.Get("rest/com/vmware/content/library/item", new() { { "library_id", contentLibraryId } });
            return JsonConvert.DeserializeObject<List<string>>(_lastResponse.Response);
        }

        public VmTemplate GetTemplateByVCenterId(string vCenterId)
        {
            _lastResponse = _vCenterHttpClient.Get($"api/content/library/item/{vCenterId}");
            return JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response);
        }

        public List<Pool> GetResourceGroups()
        {
            _lastResponse = _vCenterHttpClient.Get("rest/vcenter/resource-pool");
            return new List<Pool>(JsonConvert.DeserializeObject<PoolContainer>(_lastResponse.Response).value);
        }
    }
}
