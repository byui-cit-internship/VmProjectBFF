using Newtonsoft.Json;
using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
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
            _lastResponse = _vCenterHttpClient.Get("api/content/local-library");
            return JsonConvert.DeserializeObject<List<string>>(_lastResponse.Response);
        }

        public ContentLibrary GetContentLibraryById(string contentLibraryId)
        {
            _lastResponse = _vCenterHttpClient.Get($"api/content/local-library/{contentLibraryId}");
            return JsonConvert.DeserializeObject<ContentLibrary>(_lastResponse.Response);
        }

        public List<OldFolder> GetFolders()
        {
            _lastResponse = _vCenterHttpClient.Get("rest/vcenter/folder", new() { { "filter.type", "VIRTUAL_MACHINE" } });
            return new List<OldFolder>(JsonConvert.DeserializeObject<FolderResponse>(_lastResponse.Response).value);
        }

        public List<Template> GetTemplatesByContentLibraryId(string contentLibraryId)
        {
            List<Template> templates = new();
            List<string> templateIds = GetTemplateIdsInContentLibrary(contentLibraryId);
            foreach (string templateId in templateIds)
            {
                templates.Add(GetTemplateByVCenterId(templateId));
            }
            return templates;
        }

        public List<string> GetTemplateIdsInContentLibrary(string contentLibraryId)
        {
            _lastResponse = _vCenterHttpClient.Get("api/content/library/item", new() { { "library_id", contentLibraryId } });
            return JsonConvert.DeserializeObject<List<string>>(_lastResponse.Response);
        }

        public Template GetTemplateByVCenterId(string vCenterId)
        {
            _lastResponse = _vCenterHttpClient.Get($"api/content/library/item/{vCenterId}");
            return JsonConvert.DeserializeObject<Template>(_lastResponse.Response);
        }

        public List<Pool> GetResourceGroups()
        {
            _lastResponse = _vCenterHttpClient.Get("api/vcenter/resource-pool");
            return JsonConvert.DeserializeObject<List<Pool>>(_lastResponse.Response);
        }
    }
}
