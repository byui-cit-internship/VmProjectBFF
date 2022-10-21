using Newtonsoft.Json;
using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public interface IVCenterRepository
    {
        public IVCenterHttpClient VCenterHttpClient { get; }
        public List<ContentLibrary> GetContentLibraries();
        public List<string> GetContentLibraryIds();
        public ContentLibrary GetContentLibraryById(string contentLibraryId);
        public List<OldFolder> GetFolders();
        public List<Template> GetTemplatesByContentLibraryId(string contentLibraryId);
        public List<string> GetTemplateIdsInContentLibrary(string contentLibraryId);
        public Template GetTemplateByVCenterId(string vCenterId);
        public List<Pool> GetResourceGroups();

        // POST's
        public string NewVmInstanceByTemplateId(
            string vCenterTemplateId,
            Deploy deploy);
    }
}
